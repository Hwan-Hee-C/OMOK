using OMOK.Game;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace OMOK.Net
{
    public enum PacketId : ushort
    {
        Hello  = 0,
        Ready    = 1,
        Start    = 2,
        Move     = 3,
        Leave    = 4,
        Chat     = 5,
    }

    public class GameNetwork : IDisposable
    {
        public bool IsHost { get; }

        private TcpListener? _listener;
        private Session? _session;
        private readonly CancellationTokenSource _cts = new();
        private Task? _acceptTask;
        private Task? _recvTask;

        public bool IsConnected => _session != null;

        // 연결 상태
        public event Action? Connected;
        public event Action<string>? Disconnected;

        // 게임용 수신 이벤트
        public event Action<bool>? ReadyReceived;
        public event Action? StartReceived;
        public event Action<Coord>? MoveReceived;
        public event Action<bool>? LeaveReceived;
        public event Action<string>? ChatReceived;
        public event Action<string>? HelloReceived;

        private string _myName = "Player"; 
        public void SetMyName(string name) => _myName = (name ?? "Player").Trim();

        public GameNetwork(bool isHost)
        {
            IsHost = isHost;
        }

        public Task StartHostAsync(IPAddress ip, int port)
        {
            if (!IsHost) throw new InvalidOperationException("Guest는 StartHostAsync를 호출할 수 없습니다.");
            if (_listener != null) throw new InvalidOperationException("이미 리스너가 시작되어 있습니다.");

            _listener = new TcpListener(ip, port);
            _listener.Start();

            _acceptTask = Task.Run(() => AcceptLoopAsync(_cts.Token), _cts.Token);
            return Task.CompletedTask;
        }

        private async Task AcceptLoopAsync(CancellationToken ct)
        {
            try
            {
                while (!ct.IsCancellationRequested)
                {
                    // 이미 세션이 있으면(게스트 접속중) 새 접속은 받지 않음
                    if (_session != null)
                    {
                        await Task.Delay(100, ct);
                        continue;
                    }

                    TcpClient client = await _listener!.AcceptTcpClientAsync(ct);

                    // accept 직후 세션이 생겼다면(레이스) 그냥 끊어버림
                    if (_session != null)
                    {
                        client.Close();
                        continue;
                    }

                    AttachSession(client);
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                Disconnected?.Invoke($"AcceptLoop 종료: {ex.Message}");
            }
        }

        public async Task ConnectAsync(string host, int port)
        {
            if (IsHost) throw new InvalidOperationException("Host는 ConnectAsync를 호출할 수 없습니다.");
            if (_session != null) throw new InvalidOperationException("이미 연결되어 있습니다.");

            try
            {
                var client = new TcpClient();
                await client.ConnectAsync(host, port, _cts.Token);
                AttachSession(client);
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                Disconnected?.Invoke($"Connect 실패: {ex.Message}");
            }
        }

        private void AttachSession(TcpClient client)
        {
            _session = new Session(client);

            _session.OnRecv += OnRecvCallback;

            _session.OnDisconnect += OnDisconnectCallback;

            _recvTask = Task.Run(() => _session.RecvAsync(_cts.Token), _cts.Token);

            Connected?.Invoke();
            _ = SendHelloAsync(_myName);
        }

        private void OnRecvCallback(ushort id, ReadOnlyMemory<byte> payload)
        {
            HandlePacket(id, payload.Span);
        }

        private void OnDisconnectCallback()
        {
            DetachSession("연결 종료");
        }

        private void DetachSession(string reason)
        {
            try
            {
                var s = _session;
                if (s != null)
                {
                    s.OnRecv -= OnRecvCallback;
                    s.OnDisconnect -= OnDisconnectCallback;
                    s.DisConnect();
                }
            }
            catch { }
            finally
            {
                _session = null;
                Disconnected?.Invoke(reason);
            }
        }

        public Task SendHelloAsync(string name)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(name ?? "");
            return SendPacketAsync(PacketId.Hello, bytes);
        }

        public Task SendReadyAsync(bool ready) =>
            SendPacketAsync(PacketId.Ready, [ready ? (byte)1 : (byte)0]);

        public Task SendStartAsync() =>
            SendPacketAsync(PacketId.Start, ReadOnlySpan<byte>.Empty);

        public Task SendMoveAsync(Coord pos)
        {
            Span<byte> p = stackalloc byte[2];
            p[0] = (byte)pos.X;
            p[1] = (byte)pos.Y;
            return SendPacketAsync(PacketId.Move, p);
        }

        public Task SendLeaveAsync(bool isHost)
        {
            Span<byte> p = stackalloc byte[1];
            p[0] = isHost ? (byte)1 : (byte)0;
            return SendPacketAsync(PacketId.Leave, p);
        }

        public Task SendChatAsync(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text ?? "");
            return SendPacketAsync(PacketId.Chat, bytes);
        }

        private Task SendPacketAsync(PacketId id, ReadOnlySpan<byte> payload)
        {
            if (_session == null) return Task.CompletedTask;
            return _session.SendPacketAsync((ushort)id, payload, _cts.Token);
        }

        private void HandlePacket(ushort id, ReadOnlySpan<byte> payload)
        {
            switch ((PacketId)id)
            {
                case PacketId.Ready:
                    if (payload.Length >= 1)
                        ReadyReceived?.Invoke(payload[0] != 0);
                    break;

                case PacketId.Start:
                    StartReceived?.Invoke();
                    break;

                case PacketId.Move:
                    if (payload.Length >= 2)
                    {
                        int x = payload[0];
                        int y = payload[1];
                        MoveReceived?.Invoke(new Coord(x, y));
                    }
                    break;

                case PacketId.Leave:
                    if (payload.Length >= 1)
                        LeaveReceived?.Invoke(payload[0] != 0);
                    break;

                case PacketId.Chat:
                    ChatReceived?.Invoke(Encoding.UTF8.GetString(payload));
                    break;

                case PacketId.Hello:
                    string name = Encoding.UTF8.GetString(payload);
                    HelloReceived?.Invoke(name);
                    break;
            }
        }

        public void Dispose()
        {
            _cts.Cancel();
            try { _listener?.Stop(); } catch { }
            try { _session?.DisConnect(); } catch { }
            _cts.Dispose();
        }
    }
}
