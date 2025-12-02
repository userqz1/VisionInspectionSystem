namespace VisionInspectionSystem.Forms
{
    partial class MainForm
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnRun = new System.Windows.Forms.ToolStripButton();
            this.btnSingleGrab = new System.Windows.Forms.ToolStripButton();
            this.btnOfflineTest = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnCameraSettings = new System.Windows.Forms.ToolStripButton();
            this.btnCommSettings = new System.Windows.Forms.ToolStripButton();
            this.btnModelManage = new System.Windows.Forms.ToolStripButton();
            this.btnStatistics = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnUserManage = new System.Windows.Forms.ToolStripButton();
            this.btnLogout = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCameraLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCameraStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCommLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCommStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.cogRecordDisplay1 = new Cognex.VisionPro.CogRecordDisplay();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupResult = new System.Windows.Forms.GroupBox();
            this.lblRunTimeValue = new System.Windows.Forms.Label();
            this.lblRunTimeLabel = new System.Windows.Forms.Label();
            this.lblScore = new System.Windows.Forms.Label();
            this.lblScoreLabel = new System.Windows.Forms.Label();
            this.lblAngle = new System.Windows.Forms.Label();
            this.lblAngleLabel = new System.Windows.Forms.Label();
            this.lblY = new System.Windows.Forms.Label();
            this.lblYLabel = new System.Windows.Forms.Label();
            this.lblX = new System.Windows.Forms.Label();
            this.lblXLabel = new System.Windows.Forms.Label();
            this.lblResult = new System.Windows.Forms.Label();
            this.groupStats = new System.Windows.Forms.GroupBox();
            this.lblRunTime = new System.Windows.Forms.Label();
            this.lblRunTimeText = new System.Windows.Forms.Label();
            this.lblYield = new System.Windows.Forms.Label();
            this.lblYieldLabel = new System.Windows.Forms.Label();
            this.lblNG = new System.Windows.Forms.Label();
            this.lblNGLabel = new System.Windows.Forms.Label();
            this.lblOK = new System.Windows.Forms.Label();
            this.lblOKLabel = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblTotalLabel = new System.Windows.Forms.Label();
            this.groupLog = new System.Windows.Forms.GroupBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblCurrentModel = new System.Windows.Forms.Label();
            this.lblModelLabel = new System.Windows.Forms.Label();
            this.cboModel = new System.Windows.Forms.ComboBox();
            this.lblCurrentUser = new System.Windows.Forms.Label();
            this.lblUserLabel = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cogRecordDisplay1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupResult.SuspendLayout();
            this.groupStats.SuspendLayout();
            this.groupLog.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRun,
            this.btnSingleGrab,
            this.btnOfflineTest,
            this.toolStripSeparator1,
            this.btnCameraSettings,
            this.btnCommSettings,
            this.btnModelManage,
            this.btnStatistics,
            this.toolStripSeparator2,
            this.btnUserManage,
            this.btnLogout});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.toolStrip1.Size = new System.Drawing.Size(1800, 33);
            this.toolStrip1.TabIndex = 0;
            // 
            // btnRun
            // 
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(50, 28);
            this.btnRun.Text = "运行";
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnSingleGrab
            // 
            this.btnSingleGrab.Name = "btnSingleGrab";
            this.btnSingleGrab.Size = new System.Drawing.Size(86, 28);
            this.btnSingleGrab.Text = "单帧采集";
            this.btnSingleGrab.Click += new System.EventHandler(this.btnSingleGrab_Click);
            // 
            // btnOfflineTest
            // 
            this.btnOfflineTest.Name = "btnOfflineTest";
            this.btnOfflineTest.Size = new System.Drawing.Size(86, 28);
            this.btnOfflineTest.Text = "离线测试";
            this.btnOfflineTest.Click += new System.EventHandler(this.btnOfflineTest_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 33);
            // 
            // btnCameraSettings
            // 
            this.btnCameraSettings.Name = "btnCameraSettings";
            this.btnCameraSettings.Size = new System.Drawing.Size(86, 28);
            this.btnCameraSettings.Text = "相机设置";
            this.btnCameraSettings.Click += new System.EventHandler(this.btnCameraSettings_Click);
            // 
            // btnCommSettings
            // 
            this.btnCommSettings.Name = "btnCommSettings";
            this.btnCommSettings.Size = new System.Drawing.Size(86, 28);
            this.btnCommSettings.Text = "通讯设置";
            this.btnCommSettings.Click += new System.EventHandler(this.btnCommSettings_Click);
            // 
            // btnModelManage
            // 
            this.btnModelManage.Name = "btnModelManage";
            this.btnModelManage.Size = new System.Drawing.Size(86, 28);
            this.btnModelManage.Text = "版型管理";
            this.btnModelManage.Click += new System.EventHandler(this.btnModelManage_Click);
            // 
            // btnStatistics
            // 
            this.btnStatistics.Name = "btnStatistics";
            this.btnStatistics.Size = new System.Drawing.Size(86, 28);
            this.btnStatistics.Text = "数据统计";
            this.btnStatistics.Click += new System.EventHandler(this.btnStatistics_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 33);
            // 
            // btnUserManage
            // 
            this.btnUserManage.Name = "btnUserManage";
            this.btnUserManage.Size = new System.Drawing.Size(86, 28);
            this.btnUserManage.Text = "用户管理";
            this.btnUserManage.Click += new System.EventHandler(this.btnUserManage_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(50, 28);
            this.btnLogout.Text = "注销";
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatusLabel,
            this.lblStatus,
            this.toolStripStatusLabel1,
            this.lblCameraLabel,
            this.lblCameraStatus,
            this.toolStripStatusLabel2,
            this.lblCommLabel,
            this.lblCommStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 1019);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 21, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1800, 31);
            this.statusStrip1.TabIndex = 1;
            // 
            // lblStatusLabel
            // 
            this.lblStatusLabel.Name = "lblStatusLabel";
            this.lblStatusLabel.Size = new System.Drawing.Size(64, 24);
            this.lblStatusLabel.Text = "状态：";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(42, 24);
            this.lblStatus.Text = "Idle";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(25, 24);
            this.toolStripStatusLabel1.Text = "  |";
            // 
            // lblCameraLabel
            // 
            this.lblCameraLabel.Name = "lblCameraLabel";
            this.lblCameraLabel.Size = new System.Drawing.Size(64, 24);
            this.lblCameraLabel.Text = "相机：";
            // 
            // lblCameraStatus
            // 
            this.lblCameraStatus.ForeColor = System.Drawing.Color.Red;
            this.lblCameraStatus.Name = "lblCameraStatus";
            this.lblCameraStatus.Size = new System.Drawing.Size(127, 24);
            this.lblCameraStatus.Text = "Disconnected";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(25, 24);
            this.toolStripStatusLabel2.Text = "  |";
            // 
            // lblCommLabel
            // 
            this.lblCommLabel.Name = "lblCommLabel";
            this.lblCommLabel.Size = new System.Drawing.Size(64, 24);
            this.lblCommLabel.Text = "通讯：";
            // 
            // lblCommStatus
            // 
            this.lblCommStatus.ForeColor = System.Drawing.Color.Red;
            this.lblCommStatus.Name = "lblCommStatus";
            this.lblCommStatus.Size = new System.Drawing.Size(127, 24);
            this.lblCommStatus.Text = "Disconnected";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 93);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.cogRecordDisplay1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1800, 926);
            this.splitContainer1.SplitterDistance = 1125;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 2;
            // 
            // cogRecordDisplay1
            //
            this.cogRecordDisplay1.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.cogRecordDisplay1.ColorMapLowerRoiLimit = 0D;
            this.cogRecordDisplay1.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.cogRecordDisplay1.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.cogRecordDisplay1.ColorMapUpperRoiLimit = 1D;
            this.cogRecordDisplay1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cogRecordDisplay1.DoubleTapZoomCycleLength = 2;
            this.cogRecordDisplay1.DoubleTapZoomSensitivity = 2.5D;
            this.cogRecordDisplay1.Location = new System.Drawing.Point(0, 0);
            this.cogRecordDisplay1.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogRecordDisplay1.MouseWheelSensitivity = 1D;
            this.cogRecordDisplay1.Name = "cogRecordDisplay1";
            this.cogRecordDisplay1.OcxState = null;
            this.cogRecordDisplay1.Size = new System.Drawing.Size(1125, 926);
            this.cogRecordDisplay1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupResult);
            this.splitContainer2.Panel1.Controls.Add(this.groupStats);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupLog);
            this.splitContainer2.Size = new System.Drawing.Size(669, 926);
            this.splitContainer2.SplitterDistance = 460;
            this.splitContainer2.SplitterWidth = 6;
            this.splitContainer2.TabIndex = 0;
            // 
            // groupResult
            // 
            this.groupResult.Controls.Add(this.lblRunTimeValue);
            this.groupResult.Controls.Add(this.lblRunTimeLabel);
            this.groupResult.Controls.Add(this.lblScore);
            this.groupResult.Controls.Add(this.lblScoreLabel);
            this.groupResult.Controls.Add(this.lblAngle);
            this.groupResult.Controls.Add(this.lblAngleLabel);
            this.groupResult.Controls.Add(this.lblY);
            this.groupResult.Controls.Add(this.lblYLabel);
            this.groupResult.Controls.Add(this.lblX);
            this.groupResult.Controls.Add(this.lblXLabel);
            this.groupResult.Controls.Add(this.lblResult);
            this.groupResult.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupResult.Location = new System.Drawing.Point(0, 0);
            this.groupResult.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupResult.Name = "groupResult";
            this.groupResult.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupResult.Size = new System.Drawing.Size(669, 235);
            this.groupResult.TabIndex = 0;
            this.groupResult.TabStop = false;
            this.groupResult.Text = "检测结果";
            // 
            // lblRunTimeValue
            // 
            this.lblRunTimeValue.AutoSize = true;
            this.lblRunTimeValue.Font = new System.Drawing.Font("Arial", 12F);
            this.lblRunTimeValue.Location = new System.Drawing.Point(540, 78);
            this.lblRunTimeValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRunTimeValue.Name = "lblRunTimeValue";
            this.lblRunTimeValue.Size = new System.Drawing.Size(64, 27);
            this.lblRunTimeValue.TabIndex = 10;
            this.lblRunTimeValue.Text = "0 ms";
            // 
            // lblRunTimeLabel
            // 
            this.lblRunTimeLabel.AutoSize = true;
            this.lblRunTimeLabel.Location = new System.Drawing.Point(450, 82);
            this.lblRunTimeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRunTimeLabel.Name = "lblRunTimeLabel";
            this.lblRunTimeLabel.Size = new System.Drawing.Size(53, 18);
            this.lblRunTimeLabel.TabIndex = 9;
            this.lblRunTimeLabel.Text = "耗时:";
            // 
            // lblScore
            // 
            this.lblScore.AutoSize = true;
            this.lblScore.Font = new System.Drawing.Font("Arial", 12F);
            this.lblScore.Location = new System.Drawing.Point(540, 40);
            this.lblScore.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(79, 27);
            this.lblScore.TabIndex = 8;
            this.lblScore.Text = "0.00%";
            // 
            // lblScoreLabel
            // 
            this.lblScoreLabel.AutoSize = true;
            this.lblScoreLabel.Location = new System.Drawing.Point(450, 45);
            this.lblScoreLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblScoreLabel.Name = "lblScoreLabel";
            this.lblScoreLabel.Size = new System.Drawing.Size(53, 18);
            this.lblScoreLabel.TabIndex = 7;
            this.lblScoreLabel.Text = "得分:";
            // 
            // lblAngle
            // 
            this.lblAngle.AutoSize = true;
            this.lblAngle.Font = new System.Drawing.Font("Arial", 12F);
            this.lblAngle.Location = new System.Drawing.Point(330, 116);
            this.lblAngle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAngle.Name = "lblAngle";
            this.lblAngle.Size = new System.Drawing.Size(71, 27);
            this.lblAngle.TabIndex = 6;
            this.lblAngle.Text = "0.000";
            // 
            // lblAngleLabel
            // 
            this.lblAngleLabel.AutoSize = true;
            this.lblAngleLabel.Location = new System.Drawing.Point(240, 120);
            this.lblAngleLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAngleLabel.Name = "lblAngleLabel";
            this.lblAngleLabel.Size = new System.Drawing.Size(53, 18);
            this.lblAngleLabel.TabIndex = 5;
            this.lblAngleLabel.Text = "角度:";
            // 
            // lblY
            // 
            this.lblY.AutoSize = true;
            this.lblY.Font = new System.Drawing.Font("Arial", 12F);
            this.lblY.Location = new System.Drawing.Point(330, 78);
            this.lblY.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblY.Name = "lblY";
            this.lblY.Size = new System.Drawing.Size(71, 27);
            this.lblY.TabIndex = 4;
            this.lblY.Text = "0.000";
            // 
            // lblYLabel
            // 
            this.lblYLabel.AutoSize = true;
            this.lblYLabel.Location = new System.Drawing.Point(240, 82);
            this.lblYLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblYLabel.Name = "lblYLabel";
            this.lblYLabel.Size = new System.Drawing.Size(26, 18);
            this.lblYLabel.TabIndex = 3;
            this.lblYLabel.Text = "Y:";
            // 
            // lblX
            // 
            this.lblX.AutoSize = true;
            this.lblX.Font = new System.Drawing.Font("Arial", 12F);
            this.lblX.Location = new System.Drawing.Point(330, 40);
            this.lblX.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblX.Name = "lblX";
            this.lblX.Size = new System.Drawing.Size(71, 27);
            this.lblX.TabIndex = 2;
            this.lblX.Text = "0.000";
            // 
            // lblXLabel
            // 
            this.lblXLabel.AutoSize = true;
            this.lblXLabel.Location = new System.Drawing.Point(240, 45);
            this.lblXLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblXLabel.Name = "lblXLabel";
            this.lblXLabel.Size = new System.Drawing.Size(26, 18);
            this.lblXLabel.TabIndex = 1;
            this.lblXLabel.Text = "X:";
            // 
            // lblResult
            // 
            this.lblResult.BackColor = System.Drawing.Color.Gray;
            this.lblResult.Font = new System.Drawing.Font("Arial", 36F, System.Drawing.FontStyle.Bold);
            this.lblResult.ForeColor = System.Drawing.Color.White;
            this.lblResult.Location = new System.Drawing.Point(30, 38);
            this.lblResult.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(180, 105);
            this.lblResult.TabIndex = 0;
            this.lblResult.Text = "--";
            this.lblResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupStats
            // 
            this.groupStats.Controls.Add(this.lblRunTime);
            this.groupStats.Controls.Add(this.lblRunTimeText);
            this.groupStats.Controls.Add(this.lblYield);
            this.groupStats.Controls.Add(this.lblYieldLabel);
            this.groupStats.Controls.Add(this.lblNG);
            this.groupStats.Controls.Add(this.lblNGLabel);
            this.groupStats.Controls.Add(this.lblOK);
            this.groupStats.Controls.Add(this.lblOKLabel);
            this.groupStats.Controls.Add(this.lblTotal);
            this.groupStats.Controls.Add(this.lblTotalLabel);
            this.groupStats.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupStats.Location = new System.Drawing.Point(0, 235);
            this.groupStats.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupStats.Name = "groupStats";
            this.groupStats.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupStats.Size = new System.Drawing.Size(669, 225);
            this.groupStats.TabIndex = 1;
            this.groupStats.TabStop = false;
            this.groupStats.Text = "实时统计";
            // 
            // lblRunTime
            // 
            this.lblRunTime.AutoSize = true;
            this.lblRunTime.Font = new System.Drawing.Font("Arial", 14F);
            this.lblRunTime.Location = new System.Drawing.Point(300, 84);
            this.lblRunTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRunTime.Name = "lblRunTime";
            this.lblRunTime.Size = new System.Drawing.Size(126, 32);
            this.lblRunTime.TabIndex = 9;
            this.lblRunTime.Text = "00:00:00";
            // 
            // lblRunTimeText
            // 
            this.lblRunTimeText.AutoSize = true;
            this.lblRunTimeText.Location = new System.Drawing.Point(225, 90);
            this.lblRunTimeText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRunTimeText.Name = "lblRunTimeText";
            this.lblRunTimeText.Size = new System.Drawing.Size(89, 18);
            this.lblRunTimeText.TabIndex = 8;
            this.lblRunTimeText.Text = "运行时间:";
            // 
            // lblYield
            // 
            this.lblYield.AutoSize = true;
            this.lblYield.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.lblYield.ForeColor = System.Drawing.Color.Blue;
            this.lblYield.Location = new System.Drawing.Point(300, 34);
            this.lblYield.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblYield.Name = "lblYield";
            this.lblYield.Size = new System.Drawing.Size(122, 43);
            this.lblYield.TabIndex = 7;
            this.lblYield.Text = "0.00%";
            // 
            // lblYieldLabel
            // 
            this.lblYieldLabel.AutoSize = true;
            this.lblYieldLabel.Location = new System.Drawing.Point(225, 45);
            this.lblYieldLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblYieldLabel.Name = "lblYieldLabel";
            this.lblYieldLabel.Size = new System.Drawing.Size(53, 18);
            this.lblYieldLabel.TabIndex = 6;
            this.lblYieldLabel.Text = "良率:";
            // 
            // lblNG
            // 
            this.lblNG.AutoSize = true;
            this.lblNG.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.lblNG.ForeColor = System.Drawing.Color.Red;
            this.lblNG.Location = new System.Drawing.Point(105, 129);
            this.lblNG.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNG.Name = "lblNG";
            this.lblNG.Size = new System.Drawing.Size(31, 33);
            this.lblNG.TabIndex = 5;
            this.lblNG.Text = "0";
            // 
            // lblNGLabel
            // 
            this.lblNGLabel.AutoSize = true;
            this.lblNGLabel.Location = new System.Drawing.Point(30, 135);
            this.lblNGLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNGLabel.Name = "lblNGLabel";
            this.lblNGLabel.Size = new System.Drawing.Size(35, 18);
            this.lblNGLabel.TabIndex = 4;
            this.lblNGLabel.Text = "NG:";
            // 
            // lblOK
            // 
            this.lblOK.AutoSize = true;
            this.lblOK.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.lblOK.ForeColor = System.Drawing.Color.Green;
            this.lblOK.Location = new System.Drawing.Point(105, 84);
            this.lblOK.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOK.Name = "lblOK";
            this.lblOK.Size = new System.Drawing.Size(31, 33);
            this.lblOK.TabIndex = 3;
            this.lblOK.Text = "0";
            // 
            // lblOKLabel
            // 
            this.lblOKLabel.AutoSize = true;
            this.lblOKLabel.Location = new System.Drawing.Point(30, 90);
            this.lblOKLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOKLabel.Name = "lblOKLabel";
            this.lblOKLabel.Size = new System.Drawing.Size(35, 18);
            this.lblOKLabel.TabIndex = 2;
            this.lblOKLabel.Text = "OK:";
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.lblTotal.Location = new System.Drawing.Point(105, 39);
            this.lblTotal.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(31, 33);
            this.lblTotal.TabIndex = 1;
            this.lblTotal.Text = "0";
            // 
            // lblTotalLabel
            // 
            this.lblTotalLabel.AutoSize = true;
            this.lblTotalLabel.Location = new System.Drawing.Point(30, 45);
            this.lblTotalLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTotalLabel.Name = "lblTotalLabel";
            this.lblTotalLabel.Size = new System.Drawing.Size(53, 18);
            this.lblTotalLabel.TabIndex = 0;
            this.lblTotalLabel.Text = "总数:";
            // 
            // groupLog
            // 
            this.groupLog.Controls.Add(this.txtLog);
            this.groupLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupLog.Location = new System.Drawing.Point(0, 0);
            this.groupLog.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupLog.Name = "groupLog";
            this.groupLog.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupLog.Size = new System.Drawing.Size(669, 316);
            this.groupLog.TabIndex = 0;
            this.groupLog.TabStop = false;
            this.groupLog.Text = "运行日志";
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.Color.Black;
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtLog.ForeColor = System.Drawing.Color.Lime;
            this.txtLog.Location = new System.Drawing.Point(4, 25);
            this.txtLog.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(661, 287);
            this.txtLog.TabIndex = 0;
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.lblCurrentModel);
            this.panelTop.Controls.Add(this.lblModelLabel);
            this.panelTop.Controls.Add(this.cboModel);
            this.panelTop.Controls.Add(this.lblCurrentUser);
            this.panelTop.Controls.Add(this.lblUserLabel);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 33);
            this.panelTop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1800, 60);
            this.panelTop.TabIndex = 3;
            // 
            // lblCurrentModel
            // 
            this.lblCurrentModel.AutoSize = true;
            this.lblCurrentModel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.lblCurrentModel.ForeColor = System.Drawing.Color.Green;
            this.lblCurrentModel.Location = new System.Drawing.Point(398, 21);
            this.lblCurrentModel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurrentModel.Name = "lblCurrentModel";
            this.lblCurrentModel.Size = new System.Drawing.Size(65, 18);
            this.lblCurrentModel.TabIndex = 4;
            this.lblCurrentModel.Text = "未选择";
            // 
            // lblModelLabel
            // 
            this.lblModelLabel.AutoSize = true;
            this.lblModelLabel.Location = new System.Drawing.Point(300, 21);
            this.lblModelLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblModelLabel.Name = "lblModelLabel";
            this.lblModelLabel.Size = new System.Drawing.Size(89, 18);
            this.lblModelLabel.TabIndex = 3;
            this.lblModelLabel.Text = "当前版型:";
            // 
            // cboModel
            // 
            this.cboModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboModel.FormattingEnabled = true;
            this.cboModel.Location = new System.Drawing.Point(600, 15);
            this.cboModel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboModel.Name = "cboModel";
            this.cboModel.Size = new System.Drawing.Size(223, 26);
            this.cboModel.TabIndex = 2;
            this.cboModel.SelectedIndexChanged += new System.EventHandler(this.cboModel_SelectedIndexChanged);
            // 
            // lblCurrentUser
            // 
            this.lblCurrentUser.AutoSize = true;
            this.lblCurrentUser.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.lblCurrentUser.ForeColor = System.Drawing.Color.Blue;
            this.lblCurrentUser.Location = new System.Drawing.Point(112, 21);
            this.lblCurrentUser.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurrentUser.Name = "lblCurrentUser";
            this.lblCurrentUser.Size = new System.Drawing.Size(58, 18);
            this.lblCurrentUser.TabIndex = 1;
            this.lblCurrentUser.Text = "admin";
            // 
            // lblUserLabel
            // 
            this.lblUserLabel.AutoSize = true;
            this.lblUserLabel.Location = new System.Drawing.Point(15, 21);
            this.lblUserLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUserLabel.Name = "lblUserLabel";
            this.lblUserLabel.Size = new System.Drawing.Size(89, 18);
            this.lblUserLabel.TabIndex = 0;
            this.lblUserLabel.Text = "当前用户:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1800, 1050);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "视觉检测系统";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cogRecordDisplay1)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupResult.ResumeLayout(false);
            this.groupResult.PerformLayout();
            this.groupStats.ResumeLayout(false);
            this.groupStats.PerformLayout();
            this.groupLog.ResumeLayout(false);
            this.groupLog.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnRun;
        private System.Windows.Forms.ToolStripButton btnSingleGrab;
        private System.Windows.Forms.ToolStripButton btnOfflineTest;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnCameraSettings;
        private System.Windows.Forms.ToolStripButton btnCommSettings;
        private System.Windows.Forms.ToolStripButton btnModelManage;
        private System.Windows.Forms.ToolStripButton btnStatistics;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnUserManage;
        private System.Windows.Forms.ToolStripButton btnLogout;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel lblCameraLabel;
        private System.Windows.Forms.ToolStripStatusLabel lblCameraStatus;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel lblCommLabel;
        private System.Windows.Forms.ToolStripStatusLabel lblCommStatus;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Cognex.VisionPro.CogRecordDisplay cogRecordDisplay1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupResult;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.Label lblRunTimeValue;
        private System.Windows.Forms.Label lblRunTimeLabel;
        private System.Windows.Forms.Label lblScore;
        private System.Windows.Forms.Label lblScoreLabel;
        private System.Windows.Forms.Label lblAngle;
        private System.Windows.Forms.Label lblAngleLabel;
        private System.Windows.Forms.Label lblY;
        private System.Windows.Forms.Label lblYLabel;
        private System.Windows.Forms.Label lblX;
        private System.Windows.Forms.Label lblXLabel;
        private System.Windows.Forms.GroupBox groupStats;
        private System.Windows.Forms.Label lblRunTime;
        private System.Windows.Forms.Label lblRunTimeText;
        private System.Windows.Forms.Label lblYield;
        private System.Windows.Forms.Label lblYieldLabel;
        private System.Windows.Forms.Label lblNG;
        private System.Windows.Forms.Label lblNGLabel;
        private System.Windows.Forms.Label lblOK;
        private System.Windows.Forms.Label lblOKLabel;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblTotalLabel;
        private System.Windows.Forms.GroupBox groupLog;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblCurrentModel;
        private System.Windows.Forms.Label lblModelLabel;
        private System.Windows.Forms.ComboBox cboModel;
        private System.Windows.Forms.Label lblCurrentUser;
        private System.Windows.Forms.Label lblUserLabel;
    }
}
