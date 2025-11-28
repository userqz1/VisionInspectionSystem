using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VisionInspectionSystem.BLL;
using VisionInspectionSystem.Models;

namespace VisionInspectionSystem.Forms
{
    public partial class CameraSettingForm : Form
    {
        private List<CameraInfo> _cameraList;

        public CameraSettingForm()
        {
            InitializeComponent();
        }

        private void CameraSettingForm_Load(object sender, EventArgs e)
        {
            RefreshCameraList();
            UpdateCameraInfo();
            UpdateParameterDisplay();

            // 订阅相机事件
            CameraManager.Instance.ImageGrabbed += CameraManager_ImageGrabbed;
            CameraManager.Instance.StateChanged += CameraManager_StateChanged;
        }

        private void CameraSettingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CameraManager.Instance.ImageGrabbed -= CameraManager_ImageGrabbed;
            CameraManager.Instance.StateChanged -= CameraManager_StateChanged;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshCameraList();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (lstCameras.SelectedIndex < 0)
            {
                MessageBox.Show("请选择相机", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var camera = _cameraList[lstCameras.SelectedIndex];
            if (CameraManager.Instance.Connect(camera.SerialNumber))
            {
                UpdateCameraInfo();
                UpdateParameterDisplay();
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            CameraManager.Instance.Disconnect();
            UpdateCameraInfo();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            ApplyParameters();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (CameraManager.Instance.SaveConfig())
            {
                MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnGrabOne_Click(object sender, EventArgs e)
        {
            var image = CameraManager.Instance.GrabOne();
            if (image != null)
            {
                picPreview.Image = image;
            }
        }

        private void btnStartGrab_Click(object sender, EventArgs e)
        {
            CameraManager.Instance.StartGrabbing();
        }

        private void btnStopGrab_Click(object sender, EventArgs e)
        {
            CameraManager.Instance.StopGrabbing();
        }

        private void btnSaveImage_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "JPEG图像|*.jpg|BMP图像|*.bmp|PNG图像|*.png";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    CameraManager.Instance.Camera?.SaveImage(sfd.FileName);
                }
            }
        }

        private void trackExposure_Scroll(object sender, EventArgs e)
        {
            double value = trackExposure.Value;
            txtExposure.Text = value.ToString();
        }

        private void trackGain_Scroll(object sender, EventArgs e)
        {
            double value = trackGain.Value / 10.0;
            txtGain.Text = value.ToString("F1");
        }

        private void RefreshCameraList()
        {
            lstCameras.Items.Clear();
            _cameraList = CameraManager.Instance.EnumerateCameras();
            foreach (var camera in _cameraList)
            {
                lstCameras.Items.Add(camera.ToString());
            }
        }

        private void UpdateCameraInfo()
        {
            var camera = CameraManager.Instance.Camera;
            if (camera != null && camera.IsConnected)
            {
                lblCameraName.Text = camera.CameraName;
                lblSerialNumber.Text = camera.SerialNumber;
                lblModelName.Text = camera.ModelName;
                lblIpAddress.Text = camera.IpAddress;
                lblStatus.Text = "已连接";
                lblStatus.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                lblCameraName.Text = "-";
                lblSerialNumber.Text = "-";
                lblModelName.Text = "-";
                lblIpAddress.Text = "-";
                lblStatus.Text = "未连接";
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void UpdateParameterDisplay()
        {
            var camera = CameraManager.Instance.Camera;
            if (camera != null && camera.IsConnected)
            {
                trackExposure.Minimum = (int)camera.ExposureTimeMin;
                trackExposure.Maximum = (int)camera.ExposureTimeMax;
                trackExposure.Value = (int)camera.ExposureTime;
                txtExposure.Text = camera.ExposureTime.ToString();

                trackGain.Minimum = (int)(camera.GainMin * 10);
                trackGain.Maximum = (int)(camera.GainMax * 10);
                trackGain.Value = (int)(camera.Gain * 10);
                txtGain.Text = camera.Gain.ToString("F1");

                rdoTriggerOff.Checked = camera.TriggerMode == TriggerMode.Off;
                rdoTriggerOn.Checked = camera.TriggerMode == TriggerMode.On;

                rdoSoftware.Checked = camera.TriggerSource == TriggerSource.Software;
                rdoLine1.Checked = camera.TriggerSource == TriggerSource.Line1;
                rdoLine2.Checked = camera.TriggerSource == TriggerSource.Line2;
            }
        }

        private void ApplyParameters()
        {
            if (!CameraManager.Instance.IsConnected) return;

            double exposure = double.Parse(txtExposure.Text);
            double gain = double.Parse(txtGain.Text);

            CameraManager.Instance.SetExposureTime(exposure);
            CameraManager.Instance.SetGain(gain);

            TriggerMode triggerMode = rdoTriggerOn.Checked ? TriggerMode.On : TriggerMode.Off;
            CameraManager.Instance.SetTriggerMode(triggerMode);

            TriggerSource triggerSource = TriggerSource.Software;
            if (rdoLine1.Checked) triggerSource = TriggerSource.Line1;
            else if (rdoLine2.Checked) triggerSource = TriggerSource.Line2;
            CameraManager.Instance.SetTriggerSource(triggerSource);
        }

        private void CameraManager_ImageGrabbed(object sender, System.Drawing.Bitmap e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => CameraManager_ImageGrabbed(sender, e)));
                return;
            }
            picPreview.Image = e;
        }

        private void CameraManager_StateChanged(object sender, Common.CameraState e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => CameraManager_StateChanged(sender, e)));
                return;
            }
            UpdateCameraInfo();
        }
    }
}
