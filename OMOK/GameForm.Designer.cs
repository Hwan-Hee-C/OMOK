namespace OMOK
{
    partial class GameForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            MainSplit = new SplitContainer();
            LeftRootPanel = new Panel();
            OmokAreaPanel = new PictureBox();
            BoardTopBar = new Panel();
            PutStoneBtn = new Button();
            SelectedPosLabel = new Label();
            TurnLabel = new Label();
            RightRootPanel = new Panel();
            LeaveGameBtn = new Button();
            ChatGroup = new GroupBox();
            ChatBtn = new Button();
            ChatInput = new TextBox();
            ChatBox = new RichTextBox();
            RoomGroup = new GroupBox();
            OtherNameLabel = new Label();
            MyNameLabel = new Label();
            RoomStatusLabel = new Label();
            StartBtn = new Button();
            ReadyBtn = new Button();
            ((System.ComponentModel.ISupportInitialize)MainSplit).BeginInit();
            MainSplit.Panel1.SuspendLayout();
            MainSplit.Panel2.SuspendLayout();
            MainSplit.SuspendLayout();
            LeftRootPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)OmokAreaPanel).BeginInit();
            BoardTopBar.SuspendLayout();
            RightRootPanel.SuspendLayout();
            ChatGroup.SuspendLayout();
            RoomGroup.SuspendLayout();
            SuspendLayout();
            // 
            // MainSplit
            // 
            MainSplit.Dock = DockStyle.Fill;
            MainSplit.FixedPanel = FixedPanel.Panel1;
            MainSplit.IsSplitterFixed = true;
            MainSplit.Location = new Point(0, 0);
            MainSplit.Name = "MainSplit";
            // 
            // MainSplit.Panel1
            // 
            MainSplit.Panel1.Controls.Add(LeftRootPanel);
            // 
            // MainSplit.Panel2
            // 
            MainSplit.Panel2.Controls.Add(RightRootPanel);
            MainSplit.Size = new Size(800, 600);
            MainSplit.SplitterDistance = 510;
            MainSplit.TabIndex = 0;
            // 
            // LeftRootPanel
            // 
            LeftRootPanel.BackColor = SystemColors.Control;
            LeftRootPanel.Controls.Add(OmokAreaPanel);
            LeftRootPanel.Controls.Add(BoardTopBar);
            LeftRootPanel.Dock = DockStyle.Fill;
            LeftRootPanel.Location = new Point(0, 0);
            LeftRootPanel.Name = "LeftRootPanel";
            LeftRootPanel.Size = new Size(510, 600);
            LeftRootPanel.TabIndex = 0;
            // 
            // OmokAreaPanel
            // 
            OmokAreaPanel.BackColor = Color.FromArgb(244, 176, 77);
            OmokAreaPanel.Location = new Point(5, 95);
            OmokAreaPanel.Margin = new Padding(0);
            OmokAreaPanel.Name = "OmokAreaPanel";
            OmokAreaPanel.Size = new Size(500, 500);
            OmokAreaPanel.TabIndex = 1;
            OmokAreaPanel.TabStop = false;
            OmokAreaPanel.Paint += OmokArea_Paint;
            OmokAreaPanel.MouseDown += OmokArea_MouseDown;
            // 
            // BoardTopBar
            // 
            BoardTopBar.BackColor = SystemColors.ControlLight;
            BoardTopBar.Controls.Add(PutStoneBtn);
            BoardTopBar.Controls.Add(SelectedPosLabel);
            BoardTopBar.Controls.Add(TurnLabel);
            BoardTopBar.Dock = DockStyle.Top;
            BoardTopBar.Location = new Point(0, 0);
            BoardTopBar.Name = "BoardTopBar";
            BoardTopBar.Size = new Size(510, 90);
            BoardTopBar.TabIndex = 0;
            // 
            // PutStoneBtn
            // 
            PutStoneBtn.Location = new Point(380, 45);
            PutStoneBtn.Name = "PutStoneBtn";
            PutStoneBtn.Size = new Size(120, 36);
            PutStoneBtn.TabIndex = 3;
            PutStoneBtn.Text = "착수";
            PutStoneBtn.UseVisualStyleBackColor = true;
            PutStoneBtn.Click += PutStoneBtn_Click;
            // 
            // SelectedPosLabel
            // 
            SelectedPosLabel.Font = new Font("맑은 고딕", 12F, FontStyle.Regular, GraphicsUnit.Point, 129);
            SelectedPosLabel.Location = new Point(12, 50);
            SelectedPosLabel.Name = "SelectedPosLabel";
            SelectedPosLabel.Size = new Size(180, 28);
            SelectedPosLabel.TabIndex = 2;
            SelectedPosLabel.Text = "선택: -";
            SelectedPosLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // TurnLabel
            // 
            TurnLabel.Font = new Font("맑은 고딕", 18F, FontStyle.Bold, GraphicsUnit.Point, 129);
            TurnLabel.Location = new Point(176, 8);
            TurnLabel.Name = "TurnLabel";
            TurnLabel.Size = new Size(140, 32);
            TurnLabel.TabIndex = 0;
            TurnLabel.Text = "-";
            TurnLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // RightRootPanel
            // 
            RightRootPanel.Controls.Add(LeaveGameBtn);
            RightRootPanel.Controls.Add(ChatGroup);
            RightRootPanel.Controls.Add(RoomGroup);
            RightRootPanel.Dock = DockStyle.Fill;
            RightRootPanel.Location = new Point(0, 0);
            RightRootPanel.Name = "RightRootPanel";
            RightRootPanel.Padding = new Padding(8);
            RightRootPanel.Size = new Size(286, 600);
            RightRootPanel.TabIndex = 0;
            // 
            // LeaveGameBtn
            // 
            LeaveGameBtn.Dock = DockStyle.Bottom;
            LeaveGameBtn.Location = new Point(8, 557);
            LeaveGameBtn.Name = "LeaveGameBtn";
            LeaveGameBtn.Size = new Size(270, 35);
            LeaveGameBtn.TabIndex = 2;
            LeaveGameBtn.Text = "나가기";
            LeaveGameBtn.UseVisualStyleBackColor = true;
            LeaveGameBtn.Click += LeaveGameBtn_Click;
            // 
            // ChatGroup
            // 
            ChatGroup.Controls.Add(ChatBtn);
            ChatGroup.Controls.Add(ChatInput);
            ChatGroup.Controls.Add(ChatBox);
            ChatGroup.Dock = DockStyle.Top;
            ChatGroup.Location = new Point(8, 178);
            ChatGroup.Name = "ChatGroup";
            ChatGroup.Size = new Size(270, 360);
            ChatGroup.TabIndex = 1;
            ChatGroup.TabStop = false;
            ChatGroup.Text = "채팅";
            // 
            // ChatBtn
            // 
            ChatBtn.Location = new Point(192, 324);
            ChatBtn.Name = "ChatBtn";
            ChatBtn.Size = new Size(66, 28);
            ChatBtn.TabIndex = 2;
            ChatBtn.Text = "보내기";
            ChatBtn.UseVisualStyleBackColor = true;
            ChatBtn.Click += ChatBtn_Click;
            // 
            // ChatInput
            // 
            ChatInput.Location = new Point(12, 326);
            ChatInput.Name = "ChatInput";
            ChatInput.Size = new Size(174, 23);
            ChatInput.TabIndex = 1;
            ChatInput.KeyDown += ChatInput_KeyDown;
            // 
            // ChatBox
            // 
            ChatBox.Location = new Point(12, 24);
            ChatBox.Name = "ChatBox";
            ChatBox.ReadOnly = true;
            ChatBox.Size = new Size(246, 292);
            ChatBox.TabIndex = 0;
            ChatBox.Text = "";
            // 
            // RoomGroup
            // 
            RoomGroup.Controls.Add(OtherNameLabel);
            RoomGroup.Controls.Add(MyNameLabel);
            RoomGroup.Controls.Add(RoomStatusLabel);
            RoomGroup.Controls.Add(StartBtn);
            RoomGroup.Controls.Add(ReadyBtn);
            RoomGroup.Dock = DockStyle.Top;
            RoomGroup.Location = new Point(8, 8);
            RoomGroup.Name = "RoomGroup";
            RoomGroup.Size = new Size(270, 170);
            RoomGroup.TabIndex = 0;
            RoomGroup.TabStop = false;
            RoomGroup.Text = "방 상태";
            // 
            // OtherNameLabel
            // 
            OtherNameLabel.AutoSize = true;
            OtherNameLabel.Location = new Point(12, 78);
            OtherNameLabel.Name = "OtherNameLabel";
            OtherNameLabel.Size = new Size(70, 15);
            OtherNameLabel.TabIndex = 4;
            OtherNameLabel.Text = "상대: (없음)";
            // 
            // MyNameLabel
            // 
            MyNameLabel.AutoSize = true;
            MyNameLabel.Location = new Point(12, 54);
            MyNameLabel.Name = "MyNameLabel";
            MyNameLabel.Size = new Size(58, 15);
            MyNameLabel.TabIndex = 3;
            MyNameLabel.Text = "나: Player";
            // 
            // RoomStatusLabel
            // 
            RoomStatusLabel.BorderStyle = BorderStyle.FixedSingle;
            RoomStatusLabel.Location = new Point(12, 24);
            RoomStatusLabel.Name = "RoomStatusLabel";
            RoomStatusLabel.Size = new Size(246, 22);
            RoomStatusLabel.TabIndex = 2;
            RoomStatusLabel.Text = "상태: Waiting";
            RoomStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // StartBtn
            // 
            StartBtn.Location = new Point(138, 112);
            StartBtn.Name = "StartBtn";
            StartBtn.Size = new Size(120, 40);
            StartBtn.TabIndex = 1;
            StartBtn.Text = "START";
            StartBtn.UseVisualStyleBackColor = true;
            StartBtn.Click += StartBtn_Click;
            // 
            // ReadyBtn
            // 
            ReadyBtn.Location = new Point(12, 112);
            ReadyBtn.Name = "ReadyBtn";
            ReadyBtn.Size = new Size(120, 40);
            ReadyBtn.TabIndex = 0;
            ReadyBtn.Text = "READY";
            ReadyBtn.UseVisualStyleBackColor = true;
            ReadyBtn.Click += ReadyBtn_Click;
            // 
            // GameForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 600);
            Controls.Add(MainSplit);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "GameForm";
            Text = "OMOK";
            MainSplit.Panel1.ResumeLayout(false);
            MainSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)MainSplit).EndInit();
            MainSplit.ResumeLayout(false);
            LeftRootPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)OmokAreaPanel).EndInit();
            BoardTopBar.ResumeLayout(false);
            RightRootPanel.ResumeLayout(false);
            ChatGroup.ResumeLayout(false);
            ChatGroup.PerformLayout();
            RoomGroup.ResumeLayout(false);
            RoomGroup.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer MainSplit;

        private Panel LeftRootPanel;
        private Panel BoardTopBar;
        private PictureBox OmokAreaPanel;

        private Label TurnLabel;
        private Label SelectedPosLabel;
        private Button PutStoneBtn;

        private Panel RightRootPanel;
        private GroupBox RoomGroup;
        private Label RoomStatusLabel;
        private Label MyNameLabel;
        private Label OtherNameLabel;
        private Button ReadyBtn;
        private Button StartBtn;

        private GroupBox ChatGroup;
        private RichTextBox ChatBox;
        private TextBox ChatInput;
        private Button ChatBtn;

        private Button LeaveGameBtn;
    }
}
