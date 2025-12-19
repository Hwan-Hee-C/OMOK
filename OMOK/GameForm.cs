using OMOK.Game;
using static System.Windows.Forms.AxHost;

namespace OMOK
{

    public partial class GameForm : Form
    {
        private readonly Game.Game _game;
        private List<Stone> _stones = new List<Stone>();
        private Coord? _selected;


        public GameForm(Game.Game game)
        {
            InitializeComponent();

            _game = game;
            BindGameEvents();
            UpdateUI(_game.State);
            MyNameLabel.Text = $"나 : {_game.Name}";
            FormClosed += (_, __) =>
            {
                _game.Dispose();
            };
        }

        private void UpdateUI(RoomState state)
        {
            if (RoomStatusLabel != null)
                RoomStatusLabel.Text = $"상태: {state}";

            if (state == RoomState.Waiting)
            {
                ReadyBtn.Enabled = false;
                StartBtn.Enabled = false;
                PutStoneBtn.Enabled = false;
            }

            else if (state == RoomState.Ready)
            {
                ReadyBtn.Enabled = !_game.IsReady;
                StartBtn.Enabled = false;
                PutStoneBtn.Enabled = false;
            }
            else if (state == RoomState.Game)
            {
                _stones.Clear();
                _selected = null;

                OmokAreaPanel.Invalidate();

                ReadyBtn.Enabled = false;
                StartBtn.Enabled = false;
                PutStoneBtn.Enabled = _selected.HasValue;
            }
            else if (state == RoomState.End)
            {
                ReadyBtn.Enabled = true;
            }
        }

        private void BindGameEvents()
        {
            _game.StateChanged += state =>
            {
                BeginInvoke(() =>
                {
                    UpdateUI(state);
                });
            };

            _game.PeerNameChanged += (name) =>
            {
                if (name == null) OtherNameLabel.Text = "상대 : (없음)";
                else OtherNameLabel.Text = $"상대 : {name}";
            };

            _game.AllPlayerReady += () =>
            {
                StartBtn.Enabled = _game.IsHost;
            };

            // 턴 표시
            _game.TurnChanged += turn =>
            {
                BeginInvoke(() =>
                {
                    TurnLabel.Text = _game.IsMyTurn ? "내 턴" : "상대 턴";
                });
            };

            _game.StonePlaced += stone =>
            {
                BeginInvoke(() =>
                {
                    _stones.Add(stone);
                    OmokAreaPanel.Invalidate();
                });
            };

            // 채팅/로그
            _game.ChatMessageReceived += (type, msg) =>
            {
                BeginInvoke(() =>
                {
                    AppendChat($"[{type}] {msg}");
                });
            };

            // 게임 종료
            _game.GameEnded += msg =>
            {
                BeginInvoke(() =>
                {
                    AppendChat($"[END] {msg}");
                });
            };

            // 방 파괴/초기화
            _game.RoomDestroyed += () =>
            {
                BeginInvoke(() =>
                {
                    AppendChat("[Info] 방이 종료되었습니다.");
                    Close();
                });
            };

            _game.RoomReset += () =>
            {
                BeginInvoke(() =>
                {
                    AppendChat("[Info] 방이 초기화되었습니다.");
                    _stones.Clear();
                    _selected = null;
                    if (SelectedPosLabel != null) SelectedPosLabel.Text = "선택: -";
                    PutStoneBtn.Enabled = false;
                    OmokAreaPanel.Invalidate();
                });
            };
        }


        private void OmokArea_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            DrawArea(g);

            if (_selected.HasValue)
            {
                DrawSelection(g, _selected.Value);
            }

            foreach (Stone? s in _stones)
            {
                if (!s.HasValue) continue;
                DrawStone(g, s.Value);
            }
        }

        private void OmokArea_MouseDown(object sender, MouseEventArgs e)
        {
            if (_game.State != RoomState.Game) return;
            if (!_game.IsMyTurn) return;

            if (e.Button != MouseButtons.Left)
            {
                SetSelected(null);
                OmokAreaPanel.Invalidate();

                return;
            }

            Coord selectedCoord = Coord.ScreenPointToCoord(e.Location);
            if (!Game.Game.InBoard(selectedCoord.X, selectedCoord.Y)) return;

            SetSelected(selectedCoord);

            OmokAreaPanel.Invalidate();
        }

        private void SetSelected(Coord? selected)
        {
            _selected = selected;
            PutStoneBtn.Enabled = true;

            const string sel = "선택: ";
            string pos = "";
            if (_selected == null) pos = "-";
            else
            {
                string y = Convert.ToChar('A' + _selected?.Y).ToString();
                pos = $"{_selected?.X}, {_selected?.Y}";
            }
            string msg = sel + pos;
            SelectedPosLabel.Text = msg;

            PutStoneBtn.Enabled = _selected.HasValue;
        }

        private void DrawArea(Graphics g)
        {
            Pen pen = new Pen(Color.Black, 1);
            Font font = new Font("Arial", 7);
            SolidBrush brush = new SolidBrush(Color.Black);
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            int edge = Constants.BoardEdge;
            int margin = Constants.BoardMargin;
            int size = Constants.BoardInterval;
            int len = Constants.BoardSize;

            for (int i = 0; i < edge; i++)
            {
                float pos = margin + i * size;
                g.DrawLine(pen, margin, pos, margin + len, pos);
                g.DrawLine(pen, pos, margin, pos, margin + len);

                g.DrawString((i + 1).ToString(), font, brush, margin - 15, pos, stringFormat);
                g.DrawString(Convert.ToChar('A' + i).ToString(), font, brush, pos, margin - 15, stringFormat);
            }

            float r = 8.5f;
            g.FillEllipse(brush, margin + size * 3 - (r / 2), margin + size * 3 - (r / 2), r, r);
            g.FillEllipse(brush, margin + size * 3 - (r / 2), margin + size * 11 - (r / 2), r, r);
            g.FillEllipse(brush, margin + size * 11 - (r / 2), margin + size * 3 - (r / 2), r, r);
            g.FillEllipse(brush, margin + size * 11 - (r / 2), margin + size * 11 - (r / 2), r, r);
            g.FillEllipse(brush, margin + size * 7 - (r / 2), margin + size * 7 - (r / 2), r, r);


            pen.Dispose();
            brush.Dispose();
            font.Dispose();
            stringFormat.Dispose();
        }

        private void DrawStone(Graphics g, in Stone stone)
        {
            Color color = stone.Type == Game.StoneType.Black ? Color.Black : Color.White;
            SolidBrush brush = new SolidBrush(color);

            g.FillEllipse(brush, stone.Rect);

        }

        private void DrawSelection(Graphics g, in Coord c)
        {
            int margin = Constants.BoardMargin;
            int size = Constants.BoardInterval;

            int x = margin + c.X * size;
            int y = margin + c.Y * size;

            using Pen pen = new Pen(Color.Red, 2);
            g.DrawEllipse(pen, x - 14, y - 14, 28, 28);
        }

        private async void ReadyBtn_Click(object sender, EventArgs e)
        {
            try
            {
                await _game.SendReadyAsync(true);
                ReadyBtn.Enabled = false;
                AppendChat("[Info] Ready 전송");
            }
            catch (Exception ex)
            {
                AppendChat($"[Debug] Ready 실패: {ex.Message}");
            }
        }

        private async void StartBtn_Click(object sender, EventArgs e)
        {
            try
            {
                await _game.HostStartAsync();
            }
            catch (Exception ex)
            {
                AppendChat($"[Debug] Start 실패: {ex.Message}");
            }
        }

        private async void LeaveGameBtn_Click(object sender, EventArgs e)
        {
            try
            {
                await _game.LeaveAsync();
            }
            catch { }
            Close();
        }

        private async void ChatInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                await SendChatAsync();
            }
        }

        private async void ChatBtn_Click(object sender, EventArgs e)
        {
            await SendChatAsync();
        }

        private async Task SendChatAsync()
        {
            string text = (ChatInput?.Text ?? "").Trim();
            if (string.IsNullOrEmpty(text)) return;

            ChatInput?.Clear();

            try
            {
                await _game.SendChatAsync(text);

                AppendChat($"[Me] {text}");
            }
            catch (Exception ex)
            {
                AppendChat($"[Debug] 채팅 실패: {ex.Message}");
            }
        }

        private void AppendChat(string line)
        {
            ChatBox.AppendText(line + Environment.NewLine);
            ChatBox.ScrollToCaret();
        }

        private async void PutStoneBtn_Click(object sender, EventArgs e)
        {
            if (!_selected.HasValue) return;

            try
            {
                await _game.PutMineAsync(_selected.Value);
            }
            catch (Exception ex)
            {
                AppendChat($"[Debug] 착수 실패: {ex.Message}");
                return;
            }

            SetSelected(null);
        }
    }
}
