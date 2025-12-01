namespace VisionInspectionSystem.Forms
{
    partial class CommunicationForm
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
            this.groupMode = new System.Windows.Forms.GroupBox();
            this.rdoClient = new System.Windows.Forms.RadioButton();
            this.rdoServer = new System.Windows.Forms.RadioButton();
            this.groupLocal = new System.Windows.Forms.GroupBox();
            this.cboLocalIP = new System.Windows.Forms.ComboBox();
            this.txtLocalPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupRemote = new System.Windows.Forms.GroupBox();
            this.txtRemotePort = new System.Windows.Forms.TextBox();
            this.txtRemoteIP = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblStatusText = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.groupSend = new System.Windows.Forms.GroupBox();
            this.chkClearAfterSend = new System.Windows.Forms.CheckBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtSend = new System.Windows.Forms.TextBox();
            this.groupReceive = new System.Windows.Forms.GroupBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.txtReceive = new System.Windows.Forms.TextBox();
            this.lblTip = new System.Windows.Forms.Label();
            this.groupMode.SuspendLayout();
            this.groupLocal.SuspendLayout();
            this.groupRemote.SuspendLayout();
            this.groupSend.SuspendLayout();
            this.groupReceive.SuspendLayout();
            this.SuspendLayout();
            //
            // groupMode
            //
            this.groupMode.Controls.Add(this.rdoClient);
            this.groupMode.Controls.Add(this.rdoServer);
            this.groupMode.Location = new System.Drawing.Point(12, 12);
            this.groupMode.Name = "groupMode";
            this.groupMode.Size = new System.Drawing.Size(200, 55);
            this.groupMode.TabIndex = 0;
            this.groupMode.TabStop = false;
            this.groupMode.Text = "通讯模式";
            //
            // rdoServer
            //
            this.rdoServer.AutoSize = true;
            this.rdoServer.Location = new System.Drawing.Point(15, 22);
            this.rdoServer.Name = "rdoServer";
            this.rdoServer.Size = new System.Drawing.Size(59, 16);
            this.rdoServer.TabIndex = 0;
            this.rdoServer.Text = "服务端";
            this.rdoServer.UseVisualStyleBackColor = true;
            this.rdoServer.CheckedChanged += new System.EventHandler(this.rdoServer_CheckedChanged);
            //
            // rdoClient
            //
            this.rdoClient.AutoSize = true;
            this.rdoClient.Checked = true;
            this.rdoClient.Location = new System.Drawing.Point(100, 22);
            this.rdoClient.Name = "rdoClient";
            this.rdoClient.Size = new System.Drawing.Size(59, 16);
            this.rdoClient.TabIndex = 1;
            this.rdoClient.TabStop = true;
            this.rdoClient.Text = "客户端";
            this.rdoClient.UseVisualStyleBackColor = true;
            this.rdoClient.CheckedChanged += new System.EventHandler(this.rdoClient_CheckedChanged);
            //
            // groupLocal
            //
            this.groupLocal.Controls.Add(this.cboLocalIP);
            this.groupLocal.Controls.Add(this.txtLocalPort);
            this.groupLocal.Controls.Add(this.label2);
            this.groupLocal.Controls.Add(this.label1);
            this.groupLocal.Location = new System.Drawing.Point(12, 75);
            this.groupLocal.Name = "groupLocal";
            this.groupLocal.Size = new System.Drawing.Size(200, 85);
            this.groupLocal.TabIndex = 1;
            this.groupLocal.TabStop = false;
            this.groupLocal.Text = "本地设置";
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP:";
            //
            // cboLocalIP
            //
            this.cboLocalIP.FormattingEnabled = true;
            this.cboLocalIP.Location = new System.Drawing.Point(55, 25);
            this.cboLocalIP.Name = "cboLocalIP";
            this.cboLocalIP.Size = new System.Drawing.Size(135, 20);
            this.cboLocalIP.TabIndex = 1;
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "端口:";
            //
            // txtLocalPort
            //
            this.txtLocalPort.Location = new System.Drawing.Point(55, 52);
            this.txtLocalPort.Name = "txtLocalPort";
            this.txtLocalPort.Size = new System.Drawing.Size(80, 21);
            this.txtLocalPort.TabIndex = 3;
            this.txtLocalPort.Text = "8000";
            //
            // groupRemote
            //
            this.groupRemote.Controls.Add(this.txtRemotePort);
            this.groupRemote.Controls.Add(this.txtRemoteIP);
            this.groupRemote.Controls.Add(this.label3);
            this.groupRemote.Controls.Add(this.label4);
            this.groupRemote.Location = new System.Drawing.Point(220, 75);
            this.groupRemote.Name = "groupRemote";
            this.groupRemote.Size = new System.Drawing.Size(200, 85);
            this.groupRemote.TabIndex = 2;
            this.groupRemote.TabStop = false;
            this.groupRemote.Text = "远程设置";
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "IP:";
            //
            // txtRemoteIP
            //
            this.txtRemoteIP.Location = new System.Drawing.Point(55, 25);
            this.txtRemoteIP.Name = "txtRemoteIP";
            this.txtRemoteIP.Size = new System.Drawing.Size(135, 21);
            this.txtRemoteIP.TabIndex = 1;
            this.txtRemoteIP.Text = "127.0.0.1";
            //
            // label4
            //
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "端口:";
            //
            // txtRemotePort
            //
            this.txtRemotePort.Location = new System.Drawing.Point(55, 52);
            this.txtRemotePort.Name = "txtRemotePort";
            this.txtRemotePort.Size = new System.Drawing.Size(80, 21);
            this.txtRemotePort.TabIndex = 3;
            this.txtRemotePort.Text = "8001";
            //
            // btnConnect
            //
            this.btnConnect.Location = new System.Drawing.Point(12, 170);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(90, 30);
            this.btnConnect.TabIndex = 3;
            this.btnConnect.Text = "连接";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            //
            // btnDisconnect
            //
            this.btnDisconnect.Enabled = false;
            this.btnDisconnect.Location = new System.Drawing.Point(110, 170);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(90, 30);
            this.btnDisconnect.TabIndex = 4;
            this.btnDisconnect.Text = "断开";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            //
            // btnSave
            //
            this.btnSave.Location = new System.Drawing.Point(208, 170);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 30);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "保存配置";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            //
            // lblStatusText
            //
            this.lblStatusText.AutoSize = true;
            this.lblStatusText.Location = new System.Drawing.Point(320, 178);
            this.lblStatusText.Name = "lblStatusText";
            this.lblStatusText.Size = new System.Drawing.Size(35, 12);
            this.lblStatusText.TabIndex = 6;
            this.lblStatusText.Text = "状态:";
            //
            // lblStatus
            //
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold);
            this.lblStatus.ForeColor = System.Drawing.Color.Red;
            this.lblStatus.Location = new System.Drawing.Point(360, 178);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(44, 12);
            this.lblStatus.TabIndex = 7;
            this.lblStatus.Text = "未连接";
            //
            // lblTip
            //
            this.lblTip.AutoSize = true;
            this.lblTip.ForeColor = System.Drawing.Color.Blue;
            this.lblTip.Location = new System.Drawing.Point(220, 35);
            this.lblTip.Name = "lblTip";
            this.lblTip.Size = new System.Drawing.Size(200, 12);
            this.lblTip.TabIndex = 8;
            this.lblTip.Text = "提示：选择模式后点击连接/启动监听";
            //
            // groupSend
            //
            this.groupSend.Controls.Add(this.chkClearAfterSend);
            this.groupSend.Controls.Add(this.btnSend);
            this.groupSend.Controls.Add(this.txtSend);
            this.groupSend.Location = new System.Drawing.Point(12, 210);
            this.groupSend.Name = "groupSend";
            this.groupSend.Size = new System.Drawing.Size(408, 65);
            this.groupSend.TabIndex = 9;
            this.groupSend.TabStop = false;
            this.groupSend.Text = "发送数据";
            //
            // txtSend
            //
            this.txtSend.Location = new System.Drawing.Point(10, 22);
            this.txtSend.Name = "txtSend";
            this.txtSend.Size = new System.Drawing.Size(300, 21);
            this.txtSend.TabIndex = 0;
            this.txtSend.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSend_KeyDown);
            //
            // btnSend
            //
            this.btnSend.Location = new System.Drawing.Point(320, 20);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 25);
            this.btnSend.TabIndex = 1;
            this.btnSend.Text = "发送";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            //
            // chkClearAfterSend
            //
            this.chkClearAfterSend.AutoSize = true;
            this.chkClearAfterSend.Checked = true;
            this.chkClearAfterSend.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkClearAfterSend.Location = new System.Drawing.Point(10, 47);
            this.chkClearAfterSend.Name = "chkClearAfterSend";
            this.chkClearAfterSend.Size = new System.Drawing.Size(96, 16);
            this.chkClearAfterSend.TabIndex = 2;
            this.chkClearAfterSend.Text = "发送后清空";
            this.chkClearAfterSend.UseVisualStyleBackColor = true;
            //
            // groupReceive
            //
            this.groupReceive.Controls.Add(this.btnClear);
            this.groupReceive.Controls.Add(this.txtReceive);
            this.groupReceive.Location = new System.Drawing.Point(12, 285);
            this.groupReceive.Name = "groupReceive";
            this.groupReceive.Size = new System.Drawing.Size(408, 200);
            this.groupReceive.TabIndex = 10;
            this.groupReceive.TabStop = false;
            this.groupReceive.Text = "通讯日志";
            //
            // txtReceive
            //
            this.txtReceive.BackColor = System.Drawing.Color.Black;
            this.txtReceive.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtReceive.ForeColor = System.Drawing.Color.Lime;
            this.txtReceive.Location = new System.Drawing.Point(10, 20);
            this.txtReceive.Multiline = true;
            this.txtReceive.Name = "txtReceive";
            this.txtReceive.ReadOnly = true;
            this.txtReceive.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtReceive.Size = new System.Drawing.Size(388, 140);
            this.txtReceive.TabIndex = 0;
            //
            // btnClear
            //
            this.btnClear.Location = new System.Drawing.Point(323, 166);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 25);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "清空";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            //
            // CommunicationForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 496);
            this.Controls.Add(this.groupReceive);
            this.Controls.Add(this.groupSend);
            this.Controls.Add(this.lblTip);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblStatusText);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.groupRemote);
            this.Controls.Add(this.groupLocal);
            this.Controls.Add(this.groupMode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CommunicationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "通讯设置";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CommunicationForm_FormClosing);
            this.Load += new System.EventHandler(this.CommunicationForm_Load);
            this.groupMode.ResumeLayout(false);
            this.groupMode.PerformLayout();
            this.groupLocal.ResumeLayout(false);
            this.groupLocal.PerformLayout();
            this.groupRemote.ResumeLayout(false);
            this.groupRemote.PerformLayout();
            this.groupSend.ResumeLayout(false);
            this.groupSend.PerformLayout();
            this.groupReceive.ResumeLayout(false);
            this.groupReceive.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.GroupBox groupMode;
        private System.Windows.Forms.RadioButton rdoClient;
        private System.Windows.Forms.RadioButton rdoServer;
        private System.Windows.Forms.GroupBox groupLocal;
        private System.Windows.Forms.ComboBox cboLocalIP;
        private System.Windows.Forms.TextBox txtLocalPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupRemote;
        private System.Windows.Forms.TextBox txtRemotePort;
        private System.Windows.Forms.TextBox txtRemoteIP;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblStatusText;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblTip;
        private System.Windows.Forms.GroupBox groupSend;
        private System.Windows.Forms.CheckBox chkClearAfterSend;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtSend;
        private System.Windows.Forms.GroupBox groupReceive;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TextBox txtReceive;
    }
}
