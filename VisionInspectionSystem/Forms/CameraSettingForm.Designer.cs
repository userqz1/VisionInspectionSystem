namespace VisionInspectionSystem.Forms
{
    partial class CameraSettingForm
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

        private void InitializeComponent()
        {
            this.groupCameraList = new System.Windows.Forms.GroupBox();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lstCameras = new System.Windows.Forms.ListBox();
            this.groupCameraInfo = new System.Windows.Forms.GroupBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblIpAddress = new System.Windows.Forms.Label();
            this.lblModelName = new System.Windows.Forms.Label();
            this.lblSerialNumber = new System.Windows.Forms.Label();
            this.lblCameraName = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupParameters = new System.Windows.Forms.GroupBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.groupTriggerSource = new System.Windows.Forms.GroupBox();
            this.rdoLine2 = new System.Windows.Forms.RadioButton();
            this.rdoLine1 = new System.Windows.Forms.RadioButton();
            this.groupTriggerMode = new System.Windows.Forms.GroupBox();
            this.rdoHardware = new System.Windows.Forms.RadioButton();
            this.rdoSoftware = new System.Windows.Forms.RadioButton();
            this.rdoTriggerOff = new System.Windows.Forms.RadioButton();
            this.txtGain = new System.Windows.Forms.TextBox();
            this.trackGain = new System.Windows.Forms.TrackBar();
            this.lblGain = new System.Windows.Forms.Label();
            this.txtExposure = new System.Windows.Forms.TextBox();
            this.trackExposure = new System.Windows.Forms.TrackBar();
            this.lblExposure = new System.Windows.Forms.Label();
            this.groupPreview = new System.Windows.Forms.GroupBox();
            this.btnSaveImage = new System.Windows.Forms.Button();
            this.btnStopGrab = new System.Windows.Forms.Button();
            this.btnStartGrab = new System.Windows.Forms.Button();
            this.btnGrabOne = new System.Windows.Forms.Button();
            this.picPreview = new System.Windows.Forms.PictureBox();
            this.groupCameraList.SuspendLayout();
            this.groupCameraInfo.SuspendLayout();
            this.groupParameters.SuspendLayout();
            this.groupTriggerSource.SuspendLayout();
            this.groupTriggerMode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackGain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackExposure)).BeginInit();
            this.groupPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).BeginInit();
            this.SuspendLayout();
            //
            // groupCameraList
            //
            this.groupCameraList.Controls.Add(this.btnDisconnect);
            this.groupCameraList.Controls.Add(this.btnConnect);
            this.groupCameraList.Controls.Add(this.btnRefresh);
            this.groupCameraList.Controls.Add(this.lstCameras);
            this.groupCameraList.Location = new System.Drawing.Point(12, 12);
            this.groupCameraList.Name = "groupCameraList";
            this.groupCameraList.Size = new System.Drawing.Size(250, 180);
            this.groupCameraList.TabIndex = 0;
            this.groupCameraList.Text = "相机列表";
            //
            // lstCameras
            //
            this.lstCameras.Location = new System.Drawing.Point(10, 20);
            this.lstCameras.Size = new System.Drawing.Size(230, 108);
            //
            // btnRefresh
            //
            this.btnRefresh.Location = new System.Drawing.Point(10, 140);
            this.btnRefresh.Size = new System.Drawing.Size(70, 30);
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            //
            // btnConnect
            //
            this.btnConnect.Location = new System.Drawing.Point(90, 140);
            this.btnConnect.Size = new System.Drawing.Size(70, 30);
            this.btnConnect.Text = "连接";
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            //
            // btnDisconnect
            //
            this.btnDisconnect.Location = new System.Drawing.Point(170, 140);
            this.btnDisconnect.Size = new System.Drawing.Size(70, 30);
            this.btnDisconnect.Text = "断开";
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            //
            // groupCameraInfo
            //
            this.groupCameraInfo.Controls.Add(this.lblStatus);
            this.groupCameraInfo.Controls.Add(this.lblIpAddress);
            this.groupCameraInfo.Controls.Add(this.lblModelName);
            this.groupCameraInfo.Controls.Add(this.lblSerialNumber);
            this.groupCameraInfo.Controls.Add(this.lblCameraName);
            this.groupCameraInfo.Controls.Add(this.label5);
            this.groupCameraInfo.Controls.Add(this.label4);
            this.groupCameraInfo.Controls.Add(this.label3);
            this.groupCameraInfo.Controls.Add(this.label2);
            this.groupCameraInfo.Controls.Add(this.label1);
            this.groupCameraInfo.Location = new System.Drawing.Point(270, 12);
            this.groupCameraInfo.Size = new System.Drawing.Size(250, 180);
            this.groupCameraInfo.Text = "相机信息";
            //
            // label1-5 and lblXXX
            //
            this.label1.Location = new System.Drawing.Point(15, 25); this.label1.Text = "名称:"; this.label1.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 50); this.label2.Text = "序列号:"; this.label2.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 75); this.label3.Text = "型号:"; this.label3.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 100); this.label4.Text = "IP:"; this.label4.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 125); this.label5.Text = "状态:"; this.label5.AutoSize = true;
            this.lblCameraName.Location = new System.Drawing.Point(80, 25); this.lblCameraName.Text = "-"; this.lblCameraName.AutoSize = true;
            this.lblSerialNumber.Location = new System.Drawing.Point(80, 50); this.lblSerialNumber.Text = "-"; this.lblSerialNumber.AutoSize = true;
            this.lblModelName.Location = new System.Drawing.Point(80, 75); this.lblModelName.Text = "-"; this.lblModelName.AutoSize = true;
            this.lblIpAddress.Location = new System.Drawing.Point(80, 100); this.lblIpAddress.Text = "-"; this.lblIpAddress.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(80, 125); this.lblStatus.Text = "未连接"; this.lblStatus.AutoSize = true;
            //
            // groupParameters
            //
            this.groupParameters.Controls.Add(this.btnSave);
            this.groupParameters.Controls.Add(this.btnApply);
            this.groupParameters.Controls.Add(this.groupTriggerSource);
            this.groupParameters.Controls.Add(this.groupTriggerMode);
            this.groupParameters.Controls.Add(this.txtGain);
            this.groupParameters.Controls.Add(this.trackGain);
            this.groupParameters.Controls.Add(this.lblGain);
            this.groupParameters.Controls.Add(this.txtExposure);
            this.groupParameters.Controls.Add(this.trackExposure);
            this.groupParameters.Controls.Add(this.lblExposure);
            this.groupParameters.Location = new System.Drawing.Point(12, 200);
            this.groupParameters.Size = new System.Drawing.Size(508, 200);
            this.groupParameters.Text = "参数设置";
            //
            // lblExposure, trackExposure, txtExposure
            //
            this.lblExposure.Location = new System.Drawing.Point(15, 25); this.lblExposure.Text = "曝光时间(μs):"; this.lblExposure.AutoSize = true;
            this.trackExposure.Location = new System.Drawing.Point(100, 20); this.trackExposure.Size = new System.Drawing.Size(300, 45);
            this.trackExposure.Scroll += new System.EventHandler(this.trackExposure_Scroll);
            this.txtExposure.Location = new System.Drawing.Point(410, 25); this.txtExposure.Size = new System.Drawing.Size(80, 21);
            //
            // lblGain, trackGain, txtGain
            //
            this.lblGain.Location = new System.Drawing.Point(15, 70); this.lblGain.Text = "增益(dB):"; this.lblGain.AutoSize = true;
            this.trackGain.Location = new System.Drawing.Point(100, 65); this.trackGain.Size = new System.Drawing.Size(300, 45);
            this.trackGain.Scroll += new System.EventHandler(this.trackGain_Scroll);
            this.txtGain.Location = new System.Drawing.Point(410, 70); this.txtGain.Size = new System.Drawing.Size(80, 21);
            //
            // groupTriggerMode
            //
            this.groupTriggerMode.Controls.Add(this.rdoTriggerOff);
            this.groupTriggerMode.Controls.Add(this.rdoSoftware);
            this.groupTriggerMode.Controls.Add(this.rdoHardware);
            this.groupTriggerMode.Location = new System.Drawing.Point(15, 110);
            this.groupTriggerMode.Size = new System.Drawing.Size(280, 50);
            this.groupTriggerMode.Text = "触发模式";
            this.rdoTriggerOff.Location = new System.Drawing.Point(10, 20); this.rdoTriggerOff.Text = "自由运行"; this.rdoTriggerOff.AutoSize = true;
            this.rdoTriggerOff.CheckedChanged += new System.EventHandler(this.rdoTriggerMode_CheckedChanged);
            this.rdoSoftware.Location = new System.Drawing.Point(90, 20); this.rdoSoftware.Text = "软触发"; this.rdoSoftware.AutoSize = true;
            this.rdoSoftware.CheckedChanged += new System.EventHandler(this.rdoTriggerMode_CheckedChanged);
            this.rdoHardware.Location = new System.Drawing.Point(165, 20); this.rdoHardware.Text = "硬件触发"; this.rdoHardware.AutoSize = true;
            this.rdoHardware.CheckedChanged += new System.EventHandler(this.rdoTriggerMode_CheckedChanged);
            //
            // groupTriggerSource
            //
            this.groupTriggerSource.Controls.Add(this.rdoLine1);
            this.groupTriggerSource.Controls.Add(this.rdoLine2);
            this.groupTriggerSource.Location = new System.Drawing.Point(305, 110);
            this.groupTriggerSource.Size = new System.Drawing.Size(140, 50);
            this.groupTriggerSource.Text = "硬件触发源";
            this.rdoLine1.Location = new System.Drawing.Point(10, 20); this.rdoLine1.Text = "Line1"; this.rdoLine1.AutoSize = true;
            this.rdoLine1.CheckedChanged += new System.EventHandler(this.rdoTriggerSource_CheckedChanged);
            this.rdoLine2.Location = new System.Drawing.Point(75, 20); this.rdoLine2.Text = "Line2"; this.rdoLine2.AutoSize = true;
            this.rdoLine2.CheckedChanged += new System.EventHandler(this.rdoTriggerSource_CheckedChanged);
            //
            // btnApply, btnSave
            //
            this.btnApply.Location = new System.Drawing.Point(410, 115); this.btnApply.Size = new System.Drawing.Size(80, 30); this.btnApply.Text = "应用";
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            this.btnSave.Location = new System.Drawing.Point(410, 155); this.btnSave.Size = new System.Drawing.Size(80, 30); this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            //
            // groupPreview
            //
            this.groupPreview.Controls.Add(this.picPreview);
            this.groupPreview.Controls.Add(this.btnGrabOne);
            this.groupPreview.Controls.Add(this.btnStartGrab);
            this.groupPreview.Controls.Add(this.btnStopGrab);
            this.groupPreview.Controls.Add(this.btnSaveImage);
            this.groupPreview.Location = new System.Drawing.Point(530, 12);
            this.groupPreview.Size = new System.Drawing.Size(450, 388);
            this.groupPreview.Text = "预览";
            //
            // picPreview
            //
            this.picPreview.BackColor = System.Drawing.Color.Black;
            this.picPreview.Location = new System.Drawing.Point(10, 20);
            this.picPreview.Size = new System.Drawing.Size(430, 320);
            this.picPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            //
            // 采集按钮
            //
            this.btnGrabOne.Location = new System.Drawing.Point(10, 350); this.btnGrabOne.Size = new System.Drawing.Size(100, 30); this.btnGrabOne.Text = "单次采集";
            this.btnGrabOne.Click += new System.EventHandler(this.btnGrabOne_Click);
            this.btnStartGrab.Location = new System.Drawing.Point(120, 350); this.btnStartGrab.Size = new System.Drawing.Size(100, 30); this.btnStartGrab.Text = "连续采集";
            this.btnStartGrab.Click += new System.EventHandler(this.btnStartGrab_Click);
            this.btnStopGrab.Location = new System.Drawing.Point(230, 350); this.btnStopGrab.Size = new System.Drawing.Size(100, 30); this.btnStopGrab.Text = "停止";
            this.btnStopGrab.Click += new System.EventHandler(this.btnStopGrab_Click);
            this.btnSaveImage.Location = new System.Drawing.Point(340, 350); this.btnSaveImage.Size = new System.Drawing.Size(100, 30); this.btnSaveImage.Text = "保存图像";
            this.btnSaveImage.Click += new System.EventHandler(this.btnSaveImage_Click);
            //
            // CameraSettingForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(994, 411);
            this.Controls.Add(this.groupPreview);
            this.Controls.Add(this.groupParameters);
            this.Controls.Add(this.groupCameraInfo);
            this.Controls.Add(this.groupCameraList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CameraSettingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "相机设置";
            this.Load += new System.EventHandler(this.CameraSettingForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CameraSettingForm_FormClosing);
            this.groupCameraList.ResumeLayout(false);
            this.groupCameraInfo.ResumeLayout(false);
            this.groupCameraInfo.PerformLayout();
            this.groupParameters.ResumeLayout(false);
            this.groupParameters.PerformLayout();
            this.groupTriggerSource.ResumeLayout(false);
            this.groupTriggerSource.PerformLayout();
            this.groupTriggerMode.ResumeLayout(false);
            this.groupTriggerMode.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackGain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackExposure)).EndInit();
            this.groupPreview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.GroupBox groupCameraList;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ListBox lstCameras;
        private System.Windows.Forms.GroupBox groupCameraInfo;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblIpAddress;
        private System.Windows.Forms.Label lblModelName;
        private System.Windows.Forms.Label lblSerialNumber;
        private System.Windows.Forms.Label lblCameraName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupParameters;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.GroupBox groupTriggerSource;
        private System.Windows.Forms.RadioButton rdoLine2;
        private System.Windows.Forms.RadioButton rdoLine1;
        private System.Windows.Forms.GroupBox groupTriggerMode;
        private System.Windows.Forms.RadioButton rdoHardware;
        private System.Windows.Forms.RadioButton rdoSoftware;
        private System.Windows.Forms.RadioButton rdoTriggerOff;
        private System.Windows.Forms.TextBox txtGain;
        private System.Windows.Forms.TrackBar trackGain;
        private System.Windows.Forms.Label lblGain;
        private System.Windows.Forms.TextBox txtExposure;
        private System.Windows.Forms.TrackBar trackExposure;
        private System.Windows.Forms.Label lblExposure;
        private System.Windows.Forms.GroupBox groupPreview;
        private System.Windows.Forms.Button btnSaveImage;
        private System.Windows.Forms.Button btnStopGrab;
        private System.Windows.Forms.Button btnStartGrab;
        private System.Windows.Forms.Button btnGrabOne;
        private System.Windows.Forms.PictureBox picPreview;
    }
}
