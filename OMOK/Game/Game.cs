using System;
using System.Net;
using System.Threading.Tasks;
using OMOK.Net;

namespace OMOK.Game
{
    public enum RoomState { Waiting, Ready, Game, End }
    public enum ChatType { Info, Debug, Chat }

    public sealed class Game : IDisposable
    {
        private readonly bool _isHost;
        private RoomState _state;

        private bool _ready;
        private bool _peerConnected;
        private bool _peerReady;   
        public bool IsReady => _ready;

        private StoneType _myStoneType;
        private StoneType _otherStoneType;
        private StoneType _currentTurn;

        private const int _turnTime = 60;
        private int _remainingSeconds;

        private readonly StoneType[,] _board = new StoneType[15, 15];

        private readonly GameNetwork _net;

        // ===== UI 이벤트 =====
        public event Action<RoomState>? StateChanged;
        public event Action<int>? TimerChanged;
        public event Action<Stone>? StonePlaced;
        public event Action<ChatType, string>? ChatMessageReceived;
        public event Action<StoneType>? TurnChanged;
        public event Action<string>? GameEnded;
        public event Action? AllPlayerReady;
        public event Action<string?>? PeerNameChanged;

        public event Action? RoomDestroyed; 
        public event Action? RoomReset;     

        public bool IsHost => _isHost;
        public RoomState State => _state;
        public bool IsMyTurn => _currentTurn == _myStoneType;
        public int RemainingSeconds => _remainingSeconds;

        public string Name { get; private set; } = "Player";
        public string PeerName { get; private set; } = "Peer";


        public Game(bool isHost, string name)
        {
            _isHost = isHost;
            _net = new GameNetwork(isHost);
            _net.SetMyName(name);
            Name = name;

            // 네트워크 이벤트 연결
            _net.Connected += () =>
            {
                OnPeerConnected();
                Chat(ChatType.Debug, "네트워크 연결 완료");
            };

            _net.HelloReceived += name =>
            {
                PeerName = name;
                Chat(ChatType.Info, $"{PeerName}님이 입장하였습니다.");
                PeerNameChanged?.Invoke( PeerName );
            };

            _net.Disconnected += reason =>
            {
                Chat(ChatType.Info, $"연결 종료: {reason}");

                ApplyExit(exiterIsHost: !_isHost ? true : false, isLocal: false, message: "연결이 끊겼습니다.");
            };

            _net.ReadyReceived += ready =>
            {
                OnPeerReady(ready);
            };

            _net.StartReceived += () =>
            {
                // 게스트가 방장 Start 수신 시 게임 시작
                OnGuestStartReceived();
            };

            _net.MoveReceived += pos =>
            {
                // 상대 착수 수신
                TryPutStone(pos, isMine: false);
            };

            _net.LeaveReceived += remoteIsHost =>
            {
                // 상대가 나갔음
                ApplyExit(exiterIsHost: remoteIsHost, isLocal: false,
                    message: remoteIsHost ? "방장이 나가서 방이 종료됩니다." : "상대가 나갔습니다.");
            };

            _net.ChatReceived += text =>
            {
                Chat(ChatType.Chat, text);
            };

            ResetToWaiting(initial: true);
        }

        // =======================
        // 네트워크 시작/접속 API
        // =======================
        public Task HostOpenAsync(string ip, int port)
        {
            if (!_isHost) throw new InvalidOperationException("Guest는 HostOpenAsync를 호출할 수 없습니다.");
            return _net.StartHostAsync(IPAddress.Parse(ip), port);
        }

        public Task GuestConnectAsync(string host, int port)
        {
            if (_isHost) throw new InvalidOperationException("Host는 GuestConnectAsync를 호출할 수 없습니다.");
            return _net.ConnectAsync(host, port);
        }

        // =======================
        // Ready/Start/Move/Leave (UI가 호출)
        // =======================
        public Task SendReadyAsync(bool ready)
        {
            if (_state == RoomState.End)
                SetState(RoomState.Ready);

            if (_state != RoomState.Ready)
                return Task.CompletedTask;

            _ready = ready;

            if (_ready && _peerReady)
                AllPlayerReady?.Invoke();

            return _net.SendReadyAsync(ready);
        }

        public async Task HostStartAsync()
        {
            if (!_isHost) return;

            if (!CanHostStart())
            {
                Chat(ChatType.Info, "시작 조건이 충족되지 않았습니다. (상대 입장/상대 Ready 필요)");
                return;
            }

            StartGameInternal();
            await _net.SendStartAsync();
        }

        public async Task PutMineAsync(Coord pos)
        {
            if (TryPutStone(pos, isMine: true))
                await _net.SendMoveAsync(pos);
        }

        public async Task LeaveAsync()
        {
            await _net.SendLeaveAsync(_isHost);
            ApplyExit(exiterIsHost: _isHost, isLocal: true,
                message: _isHost ? "방장이 방을 종료했습니다." : "게임에서 나갔습니다.");
        }

        public Task SendChatAsync(string text) => _net.SendChatAsync(text);

        // =======================
        // 상태 전이(내부)
        // =======================
        private void OnPeerConnected()
        {
            if (_state != RoomState.Waiting) return;

            _peerConnected = true;
            SetState(RoomState.Ready);
        }

        private void OnPeerReady(bool ready)
        {
            if (_state == RoomState.End)
                SetState(RoomState.Ready);

            if (_state != RoomState.Ready) return;

            _peerReady = ready;
            Chat(ChatType.Info, ready ? "상대가 준비했습니다." : "상대가 준비를 해제했습니다.");

            if (_ready && _peerReady)
                AllPlayerReady?.Invoke();
        }

        private void OnGuestStartReceived()
        {
            if (_isHost) return;

            if (_state != RoomState.Ready && _state != RoomState.End) return;

            StartGameInternal();
        }

        private bool CanHostStart()
        {
            return _isHost
                   && _state == RoomState.Ready
                   && _peerConnected
                   && _peerReady;
        }

        private void StartGameInternal()
        {
            Array.Clear(_board, 0, _board.Length);

            _myStoneType = _isHost ? StoneType.Black : StoneType.White;
            _otherStoneType = _isHost ? StoneType.White : StoneType.Black;

            _currentTurn = StoneType.Black;
            _remainingSeconds = _turnTime;

            SetState(RoomState.Game);
            TurnChanged?.Invoke(_currentTurn);
            TimerChanged?.Invoke(_remainingSeconds);
            Chat(ChatType.Info, "게임 시작");
        }

        // =======================
        // 착수/승리/타이머
        // =======================
        public bool TryPutStone(in Coord pos, bool isMine)
        {
            if (_state != RoomState.Game) return false;

            StoneType stoneType = isMine ? _myStoneType : _otherStoneType;

            // 턴 체크
            if (stoneType != _currentTurn) return false;

            // 범위/빈칸 체크
            if ((uint)pos.X >= 15 || (uint)pos.Y >= 15) return false;
            if (_board[pos.X, pos.Y] != StoneType.None) return false;

            // 적용
            _board[pos.X, pos.Y] = stoneType;
            var stone = new Stone(stoneType, pos);
            StonePlaced?.Invoke(stone);

            // 승리 체크
            if (CheckWin(stone))
            {
                _ready = false;        
                _peerReady = false;  

                string msg = $"{stoneType} 승리";
                GameEnded?.Invoke(msg);
                Chat(ChatType.Info, msg);
                SetState(RoomState.End);
                return true;
            }

            // 턴 전환 + 타이머 리셋
            _currentTurn = (stoneType == StoneType.Black) ? StoneType.White : StoneType.Black;
            _remainingSeconds = _turnTime;

            TurnChanged?.Invoke(_currentTurn);
            TimerChanged?.Invoke(_remainingSeconds);
            return true;
        }


        // =======================
        // 나가기 정책
        // =======================
        private void ApplyExit(bool exiterIsHost, bool isLocal, string message)
        {
            if (_state == RoomState.End) return;

            Chat(ChatType.Info, message);

            if (_state == RoomState.Game)
                GameEnded?.Invoke(message);

            if (exiterIsHost)
            {
                // Host가 나감 -> 방 파괴(양쪽 종료)
                SetState(RoomState.End);
                RoomDestroyed?.Invoke();
                return;
            }

            // Guest가 나감
            if (_isHost)
            {
                // Host는 방 유지 + 초기화 후 Waiting 복귀
                ResetToWaiting(initial: false);
                Chat(ChatType.Info, "게임이 초기화되었습니다. 새로운 상대를 기다립니다.");
                PeerNameChanged?.Invoke(null);
                RoomReset?.Invoke();
            }
            else
            {
                // Guest는 종료
                SetState(RoomState.End);
                RoomDestroyed?.Invoke();
            }
        }

        private void ResetToWaiting(bool initial)
        {
            Array.Clear(_board, 0, _board.Length);

            _peerConnected = false;
            _peerReady = false;
            _ready = false;

            _myStoneType = StoneType.None;
            _otherStoneType = StoneType.None;
            _currentTurn = StoneType.None;

            _remainingSeconds = 0;

            SetState(RoomState.Waiting);

            if (!initial)
                TimerChanged?.Invoke(_remainingSeconds);
        }

        // =======================
        // 승리 판정
        // =======================
        private bool CheckWin(in Stone lastStone)
        {
            StoneType s = lastStone.Type;
            int x = lastStone.Pos.X;
            int y = lastStone.Pos.Y;

            return IsFiveOrMore(x, y, 1, 0, s) ||
                   IsFiveOrMore(x, y, 0, 1, s) ||
                   IsFiveOrMore(x, y, 1, 1, s) ||
                   IsFiveOrMore(x, y, 1, -1, s);
        }

        private bool IsFiveOrMore(int x, int y, int dx, int dy, StoneType s)
        {
            int count = 1;
            count += CountDirection(x, y, dx, dy, s);
            count += CountDirection(x, y, -dx, -dy, s);
            return count >= 5;
        }

        private int CountDirection(int x, int y, int dx, int dy, StoneType s)
        {
            int cnt = 0;
            int cx = x + dx;
            int cy = y + dy;

            while (InBoard(cx, cy) && _board[cx, cy] == s)
            {
                cnt++;
                cx += dx;
                cy += dy;
            }
            return cnt;
        }

        public static bool InBoard(int x, int y) => (uint)x < 15 && (uint)y < 15;

        // =======================
        // 공통 유틸
        // =======================
        private void SetState(RoomState state)
        {
            _state = state;
            StateChanged?.Invoke(state);
        }

        private void Chat(ChatType type, string msg) => ChatMessageReceived?.Invoke(type, msg);

        public void Dispose()
        {
            _net.Dispose();
        }
    }
}
