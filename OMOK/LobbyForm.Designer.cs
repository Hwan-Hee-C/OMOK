namespace OMOK
{
    partial class LobbyForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblTitle = new Label();
            grpCommon = new GroupBox();
            lblPlayerName = new Label();
            txtPlayerName = new TextBox();
            grpJoin = new GroupBox();
            lblHostIp = new Label();
            txtHostIp = new TextBox();
            btnCreateRoom = new Button();
            btnJoinRoom = new Button();
            lblStatus = new Label();
            grpCommon.SuspendLayout();
            grpJoin.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Font = new Font("맑은 고딕", 14F, FontStyle.Bold);
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(455, 45);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "OMOK - Lobby";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // grpCommon
            // 
            grpCommon.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpCommon.Controls.Add(lblPlayerName);
            grpCommon.Controls.Add(txtPlayerName);
            grpCommon.Location = new Point(14, 52);
            grpCommon.Margin = new Padding(3, 2, 3, 2);
            grpCommon.Name = "grpCommon";
            grpCommon.Padding = new Padding(3, 2, 3, 2);
            grpCommon.Size = new Size(427, 56);
            grpCommon.TabIndex = 1;
            grpCommon.TabStop = false;
            grpCommon.Text = "Player";
            // 
            // lblPlayerName
            // 
            lblPlayerName.AutoSize = true;
            lblPlayerName.Location = new Point(12, 24);
            lblPlayerName.Name = "lblPlayerName";
            lblPlayerName.Size = new Size(34, 15);
            lblPlayerName.TabIndex = 0;
            lblPlayerName.Text = "이름:";
            // 
            // txtPlayerName
            // 
            txtPlayerName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtPlayerName.Location = new Point(72, 21);
            txtPlayerName.Margin = new Padding(3, 2, 3, 2);
            txtPlayerName.MaxLength = 16;
            txtPlayerName.Name = "txtPlayerName";
            txtPlayerName.Size = new Size(344, 23);
            txtPlayerName.TabIndex = 1;
            // 
            // grpJoin
            // 
            grpJoin.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpJoin.Controls.Add(lblHostIp);
            grpJoin.Controls.Add(txtHostIp);
            grpJoin.Location = new Point(14, 114);
            grpJoin.Margin = new Padding(3, 2, 3, 2);
            grpJoin.Name = "grpJoin";
            grpJoin.Padding = new Padding(3, 2, 3, 2);
            grpJoin.Size = new Size(427, 56);
            grpJoin.TabIndex = 2;
            grpJoin.TabStop = false;
            grpJoin.Text = "방 입장 정보";
            // 
            // lblHostIp
            // 
            lblHostIp.AutoSize = true;
            lblHostIp.Location = new Point(12, 24);
            lblHostIp.Name = "lblHostIp";
            lblHostIp.Size = new Size(49, 15);
            lblHostIp.TabIndex = 0;
            lblHostIp.Text = "Host IP:";
            // 
            // txtHostIp
            // 
            txtHostIp.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtHostIp.Location = new Point(72, 21);
            txtHostIp.Margin = new Padding(3, 2, 3, 2);
            txtHostIp.Name = "txtHostIp";
            txtHostIp.Size = new Size(344, 23);
            txtHostIp.TabIndex = 1;
            txtHostIp.Text = "127.0.0.1";
            // 
            // btnCreateRoom
            // 
            btnCreateRoom.Location = new Point(14, 178);
            btnCreateRoom.Margin = new Padding(3, 2, 3, 2);
            btnCreateRoom.Name = "btnCreateRoom";
            btnCreateRoom.Size = new Size(210, 33);
            btnCreateRoom.TabIndex = 3;
            btnCreateRoom.Text = "방 만들기";
            btnCreateRoom.UseVisualStyleBackColor = true;
            btnCreateRoom.Click += OnClickCreateRoomBtn;
            // 
            // btnJoinRoom
            // 
            btnJoinRoom.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnJoinRoom.Location = new Point(231, 178);
            btnJoinRoom.Margin = new Padding(3, 2, 3, 2);
            btnJoinRoom.Name = "btnJoinRoom";
            btnJoinRoom.Size = new Size(210, 33);
            btnJoinRoom.TabIndex = 4;
            btnJoinRoom.Text = "방 입장";
            btnJoinRoom.UseVisualStyleBackColor = true;
            btnJoinRoom.Click += OnClickJoinRoomBtn;
            // 
            // lblStatus
            // 
            lblStatus.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblStatus.BorderStyle = BorderStyle.FixedSingle;
            lblStatus.Location = new Point(14, 222);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(427, 26);
            lblStatus.TabIndex = 5;
            lblStatus.Text = "대기 중";
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // LobbyForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(455, 258);
            Controls.Add(lblStatus);
            Controls.Add(btnJoinRoom);
            Controls.Add(btnCreateRoom);
            Controls.Add(grpJoin);
            Controls.Add(grpCommon);
            Controls.Add(lblTitle);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LobbyForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "OMOK - Lobby";
            grpCommon.ResumeLayout(false);
            grpCommon.PerformLayout();
            grpJoin.ResumeLayout(false);
            grpJoin.PerformLayout();
            ResumeLayout(false);

        }

        #endregion


        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox grpCommon;
        private System.Windows.Forms.Label lblPlayerName;
        private System.Windows.Forms.TextBox txtPlayerName;

        private System.Windows.Forms.GroupBox grpJoin;
        private System.Windows.Forms.Label lblHostIp;
        private System.Windows.Forms.TextBox txtHostIp;

        private System.Windows.Forms.Button btnCreateRoom;
        private System.Windows.Forms.Button btnJoinRoom;

        private System.Windows.Forms.Label lblStatus;
    }
}