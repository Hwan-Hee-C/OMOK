using OMOK.Game;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OMOK
{
    public partial class LobbyForm : Form
    {
        // 포트는 한 곳에서 고정 관리
        private const int _port = 7777;

        // Host 리슨 IP: 로컬 테스트는 0.0.0.0 권장(모든 NIC 바인딩)
        private const string _hostBindIp = "0.0.0.0";

        private bool _busy = false;

        public LobbyForm()
        {
            InitializeComponent();
            lblStatus.Text = "대기 중";
        }

        private async void OnClickCreateRoomBtn(object sender, EventArgs e)
        {
            if (_busy) return;

            string myName = (txtPlayerName.Text ?? "").Trim();
            if (string.IsNullOrWhiteSpace(myName))
            {
                MessageBox.Show(this, "Player 이름을 입력하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPlayerName.Focus();
                return;
            }

            SetBusy(true, "방 생성(호스트) 준비 중...");

            try
            {
                var game = new Game.Game(isHost: true, name: myName);

                await game.HostOpenAsync(_hostBindIp, _port);

                OpenGameForm(game);
            }
            catch (Exception ex)
            {
                SetBusy(false, $"방 만들기 실패: {ex.Message}");
                MessageBox.Show(this, $"방 만들기 실패:\n{ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void OnClickJoinRoomBtn(object sender, EventArgs e)
        {
            if (_busy) return;

            string myName = (txtPlayerName.Text ?? "").Trim();
            string ip = (txtHostIp.Text ?? "").Trim();

            if (string.IsNullOrWhiteSpace(myName))
            {
                MessageBox.Show(this, "Player 이름을 입력하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPlayerName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(ip))
            {
                MessageBox.Show(this, "Host IP를 입력하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHostIp.Focus();
                return;
            }

            if (!IPAddress.TryParse(ip, out _))
            {
                MessageBox.Show(this, "Host IP 형식이 올바르지 않습니다. 예) 127.0.0.1", "오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHostIp.Focus();
                txtHostIp.SelectAll();
                return;
            }

            SetBusy(true, "접속 중...");

            try
            {
                var game = new Game.Game(isHost: false, name: myName);

                await game.GuestConnectAsync(ip, _port);

                OpenGameForm(game);
            }
            catch (Exception ex)
            {
                SetBusy(false, $"접속 실패: {ex.Message}");
                MessageBox.Show(this, $"접속 실패:\n{ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenGameForm(Game.Game game)
        {
            Hide();

            var gameForm = new GameForm(game);
            gameForm.FormClosed += (_, __) =>
            {
                Show();
                SetBusy(false, "대기 중");
            };

            gameForm.Show(this);
        }

        private void SetBusy(bool busy, string status)
        {
            _busy = busy;
            btnCreateRoom.Enabled = !busy;
            btnJoinRoom.Enabled = !busy;
            txtPlayerName.Enabled = !busy;
            txtHostIp.Enabled = !busy;

            lblStatus.Text = status;
        }
    }
}
