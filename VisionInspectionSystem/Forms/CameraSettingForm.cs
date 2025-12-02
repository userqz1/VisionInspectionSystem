using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using VisionInspectionSystem.BLL;
using VisionInspectionSystem.Models;

namespace VisionInspectionSystem.Forms
{
    public partial class CameraSettingForm : Form
    {
        private List<CameraInfo> _cameraList;
        private bool _isGrabbing = false;
        private Thread _grabThread;

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
            // 停止采集
            StopGrabbing();

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
            StopGrabbing();
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

        /// <summary>
        /// 单次采集 - 统一使用GrabOne，内部自动处理各种触发模式
        /// </summary>
        private void btnGrabOne_Click(object sender, EventArgs e)
        {
            if (!CameraManager.Instance.IsConnected) return;

            var image = CameraManager.Instance.GrabOne();
            if (image != null)
            {
                picPreview.Image = image;
            }
        }

        /// <summary>
        /// 连续采集 - 根据当前触发模式自动处理
        /// </summary>
        private void btnStartGrab_Click(object sender, EventArgs e)
        {
            if (!CameraManager.Instance.IsConnected) return;

            _isGrabbing = true;
            CameraManager.Instance.StartGrabbing();

            var camera = CameraManager.Instance.Camera;

            // 软触发模式需要启动线程持续发送触发信号
            if (camera.TriggerMode == TriggerMode.On &&
                camera.TriggerSource == TriggerSource.Software)
            {
                _grabThread = new Thread(SoftTriggerProc);
                _grabThread.IsBackground = true;
                _grabThread.Start();
            }

            UpdateButtonState(true);
        }

        /// <summary>
        /// 停止采集
        /// </summary>
        private void btnStopGrab_Click(object sender, EventArgs e)
        {
            StopGrabbing();
            UpdateButtonState(CameraManager.Instance.IsConnected);
        }

        private void StopGrabbing()
        {
            _isGrabbing = false;
            CameraManager.Instance.StopGrabbing();
        }

        /// <summary>
        /// 软触发连续发送线程
        /// </summary>
        private void SoftTriggerProc()
        {
            while (_isGrabbing && CameraManager.Instance.IsConnected)
            {
                CameraManager.Instance.SoftwareTrigger();
                Thread.Sleep(50);  // 触发间隔50ms，约20fps
            }
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

            // 实时应用曝光时间
            if (CameraManager.Instance.IsConnected)
            {
                CameraManager.Instance.SetExposureTime(value);
            }
        }

        private void trackGain_Scroll(object sender, EventArgs e)
        {
            double value = trackGain.Value / 10.0;
            txtGain.Text = value.ToString("F1");

            // 实时应用增益
            if (CameraManager.Instance.IsConnected)
            {
                CameraManager.Instance.SetGain(value);
            }
        }

        private void RefreshCameraList()
        {
            lstCameras.Items.Clear();
            try
            {
                _cameraList = CameraManager.Instance.EnumerateCameras();

                if (_cameraList == null || _cameraList.Count == 0)
                {
                    lstCameras.Items.Add("未找到相机设备");
                    MessageBox.Show("未检测到相机设备。\n\n请检查：\n1. 相机是否已连接并上电\n2. 网络/USB连接是否正常\n3. 是否已关闭pylon Viewer等占用相机的程序",
                        "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    foreach (var camera in _cameraList)
                    {
                        lstCameras.Items.Add(camera.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"枚举相机失败：\n{ex.Message}\n\n详细信息：\n{ex.ToString()}",
                    "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateCameraInfo()
        {
            var camera = CameraManager.Instance.Camera;
            bool isConnected = camera != null && camera.IsConnected;

            if (isConnected)
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

            // 更新按钮状态
            UpdateButtonState(isConnected);
        }

        private void UpdateButtonState(bool isConnected)
        {
            // 连接/断开按钮互斥
            btnConnect.Enabled = !isConnected;
            btnDisconnect.Enabled = isConnected;

            // 采集按钮
            btnGrabOne.Enabled = isConnected && !_isGrabbing;
            btnStartGrab.Enabled = isConnected && !_isGrabbing;
            btnStopGrab.Enabled = isConnected && _isGrabbing;
            btnSaveImage.Enabled = isConnected;

            // 参数设置（采集时禁用）
            btnApply.Enabled = isConnected && !_isGrabbing;
            btnSave.Enabled = isConnected && !_isGrabbing;
            groupTriggerMode.Enabled = isConnected && !_isGrabbing;
            groupTriggerSource.Enabled = isConnected && !_isGrabbing && rdoHardware.Checked;
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

                // 根据当前触发模式和触发源设置单选按钮
                if (camera.TriggerMode == TriggerMode.Off)
                {
                    rdoTriggerOff.Checked = true;
                }
                else if (camera.TriggerSource == TriggerSource.Software)
                {
                    rdoSoftware.Checked = true;
                }
                else
                {
                    rdoHardware.Checked = true;
                }

                rdoLine1.Checked = camera.TriggerSource == TriggerSource.Line1;
                rdoLine2.Checked = camera.TriggerSource == TriggerSource.Line2;
                if (!rdoLine1.Checked && !rdoLine2.Checked)
                {
                    rdoLine1.Checked = true;  // 默认Line1
                }

                // 更新触发源选项的启用状态
                UpdateTriggerSourceState();
            }
        }

        private void UpdateTriggerSourceState()
        {
            // 硬件触发模式下才显示触发源选项
            groupTriggerSource.Enabled = rdoHardware.Checked;
        }

        private void rdoTriggerMode_CheckedChanged(object sender, EventArgs e)
        {
            UpdateTriggerSourceState();

            // 实时应用触发模式
            if (CameraManager.Instance.IsConnected)
            {
                ApplyTriggerMode();
            }
        }

        private void rdoTriggerSource_CheckedChanged(object sender, EventArgs e)
        {
            // 实时应用触发源
            if (CameraManager.Instance.IsConnected && rdoHardware.Checked)
            {
                ApplyTriggerMode();
            }
        }

        private void ApplyTriggerMode()
        {
            if (rdoTriggerOff.Checked)
            {
                CameraManager.Instance.SetTriggerMode(TriggerMode.Off);
            }
            else
            {
                CameraManager.Instance.SetTriggerMode(TriggerMode.On);

                if (rdoSoftware.Checked)
                {
                    CameraManager.Instance.SetTriggerSource(TriggerSource.Software);
                }
                else if (rdoHardware.Checked)
                {
                    if (rdoLine1.Checked)
                        CameraManager.Instance.SetTriggerSource(TriggerSource.Line1);
                    else if (rdoLine2.Checked)
                        CameraManager.Instance.SetTriggerSource(TriggerSource.Line2);
                }
            }
        }

        private void ApplyParameters()
        {
            if (!CameraManager.Instance.IsConnected) return;

            try
            {
                // 从文本框应用参数（用于手动输入的情况）
                double exposure = double.Parse(txtExposure.Text);
                double gain = double.Parse(txtGain.Text);

                CameraManager.Instance.SetExposureTime(exposure);
                CameraManager.Instance.SetGain(gain);
                ApplyTriggerMode();

                // 更新滑块位置
                if (exposure >= trackExposure.Minimum && exposure <= trackExposure.Maximum)
                    trackExposure.Value = (int)exposure;
                if (gain * 10 >= trackGain.Minimum && gain * 10 <= trackGain.Maximum)
                    trackGain.Value = (int)(gain * 10);

                MessageBox.Show("参数应用成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"参数应用失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
