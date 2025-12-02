using System;
using System.Drawing;
using System.Windows.Forms;
using Cognex.VisionPro;
using VisionInspectionSystem.BLL;
using VisionInspectionSystem.Common;
using VisionInspectionSystem.DAL;
using VisionInspectionSystem.Models;

namespace VisionInspectionSystem.Forms
{
    public partial class MainForm : Form
    {
        #region 字段

        private Timer _updateTimer;

        #endregion

        #region 构造函数

        public MainForm()
        {
            InitializeComponent();
        }

        #endregion

        #region 窗体事件

        private void MainForm_Load(object sender, EventArgs e)
        {
            // 初始化界面
            InitializeUI();

            // 订阅事件
            SubscribeEvents();

            // 加载版型列表
            LoadModelList();

            // 启动更新定时器
            StartUpdateTimer();

            LogHelper.Info("MainForm", "主窗体加载完成");
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (InspectionManager.Instance.IsRunning)
            {
                var result = MessageBox.Show("系统正在运行，确定要退出吗？", "确认",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }

                InspectionManager.Instance.Stop();
            }

            // 清理资源
            _updateTimer?.Stop();
            CameraManager.Instance.Dispose();
            CommunicationManager.Instance.Dispose();
            InspectionManager.Instance.Dispose();
        }

        #endregion

        #region 初始化

        private void InitializeUI()
        {
            // 设置标题
            this.Text = $"{Constants.APP_NAME} V{Constants.APP_VERSION}";

            // 更新用户信息
            UpdateUserInfo();

            // 更新权限相关控件
            UpdatePermissionUI();

            // 初始化状态
            UpdateStatus(SystemState.Idle);
        }

        private void SubscribeEvents()
        {
            // 相机事件
            CameraManager.Instance.ImageGrabbed += CameraManager_ImageGrabbed;
            CameraManager.Instance.StateChanged += CameraManager_StateChanged;

            // 通讯事件
            CommunicationManager.Instance.DataReceived += CommunicationManager_DataReceived;
            CommunicationManager.Instance.ConnectionStateChanged += CommunicationManager_ConnectionStateChanged;

            // 检测事件
            InspectionManager.Instance.InspectionCompleted += InspectionManager_InspectionCompleted;
            InspectionManager.Instance.StateChanged += InspectionManager_StateChanged;

            // 版型事件
            ModelManager.Instance.ModelChanged += ModelManager_ModelChanged;
        }

        private void StartUpdateTimer()
        {
            _updateTimer = new Timer();
            _updateTimer.Interval = 1000;
            _updateTimer.Tick += UpdateTimer_Tick;
            _updateTimer.Start();
        }

        #endregion

        #region 工具栏按钮事件

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (!InspectionManager.Instance.IsRunning)
            {
                if (GlobalVariables.CurrentModel == null)
                {
                    MessageBox.Show("请先选择版型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 检查相机和通讯
                if (!CameraManager.Instance.IsConnected)
                {
                    var result = MessageBox.Show("相机未连接，是否继续运行？", "提示",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.No) return;
                }

                InspectionManager.Instance.Start();
                CameraManager.Instance.StartGrabbing();
                btnRun.Text = "停止";
            }
            else
            {
                InspectionManager.Instance.Stop();
                CameraManager.Instance.StopGrabbing();
                btnRun.Text = "运行";
            }
        }

        private void btnSingleGrab_Click(object sender, EventArgs e)
        {
            var image = CameraManager.Instance.GrabOne();
            if (image != null)
            {
                cogRecordDisplay1.Image = new Cognex.VisionPro.CogImage8Grey(image);
            }
        }

        private void btnOfflineTest_Click(object sender, EventArgs e)
        {
            if (!GlobalVariables.HasPermission(PermissionType.OfflineTest))
            {
                MessageBox.Show("没有离线测试权限", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 打开离线测试窗体
            using (var form = new OfflineTestForm())
            {
                form.ShowDialog(this);
            }
        }

        private void btnCameraSettings_Click(object sender, EventArgs e)
        {
            if (!GlobalVariables.HasPermission(PermissionType.CameraSettings))
            {
                MessageBox.Show("没有相机设置权限", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var form = new CameraSettingForm())
            {
                form.ShowDialog(this);
            }
        }

        private void btnCommSettings_Click(object sender, EventArgs e)
        {
            if (!GlobalVariables.HasPermission(PermissionType.CommunicationSettings))
            {
                MessageBox.Show("没有通讯设置权限", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var form = new CommunicationForm())
            {
                form.ShowDialog(this);
            }
        }

        private void btnModelManage_Click(object sender, EventArgs e)
        {
            using (var form = new ModelManageForm())
            {
                form.ShowDialog(this);
                LoadModelList(); // 刷新版型列表
            }
        }

        private void btnStatistics_Click(object sender, EventArgs e)
        {
            using (var form = new StatisticsForm())
            {
                form.ShowDialog(this);
            }
        }

        private void btnUserManage_Click(object sender, EventArgs e)
        {
            if (!GlobalVariables.HasPermission(PermissionType.UserManagement))
            {
                MessageBox.Show("没有用户管理权限", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var form = new UserManageForm())
            {
                form.ShowDialog(this);
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (InspectionManager.Instance.IsRunning)
            {
                MessageBox.Show("请先停止运行", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("确定要注销登录吗？", "确认",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                AuthenticationManager.Instance.Logout();

                // 返回登录窗口
                this.Hide();
                using (var loginForm = new LoginForm())
                {
                    if (loginForm.ShowDialog() == DialogResult.OK)
                    {
                        UpdateUserInfo();
                        UpdatePermissionUI();
                        this.Show();
                    }
                    else
                    {
                        Application.Exit();
                    }
                }
            }
        }

        #endregion

        #region 版型选择

        private void LoadModelList()
        {
            cboModel.Items.Clear();
            var models = ModelManager.Instance.GetAllModelNames();
            foreach (var model in models)
            {
                cboModel.Items.Add(model);
            }

            if (GlobalVariables.CurrentModel != null)
            {
                cboModel.Text = GlobalVariables.CurrentModel.ModelName;
            }
        }

        private void cboModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboModel.SelectedIndex >= 0)
            {
                string modelName = cboModel.SelectedItem.ToString();
                ModelManager.Instance.SwitchModel(modelName);
            }
        }

        #endregion

        #region 事件处理

        private void CameraManager_ImageGrabbed(object sender, Bitmap e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => CameraManager_ImageGrabbed(sender, e)));
                return;
            }

            cogRecordDisplay1.Image = new Cognex.VisionPro.CogImage8Grey(e);
        }

        private void CameraManager_StateChanged(object sender, CameraState e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => CameraManager_StateChanged(sender, e)));
                return;
            }

            lblCameraStatus.Text = e.ToString();
            lblCameraStatus.ForeColor = e == CameraState.Connected || e == CameraState.Grabbing
                ? Color.Green : Color.Red;
        }

        private void CommunicationManager_DataReceived(object sender, string e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => CommunicationManager_DataReceived(sender, e)));
                return;
            }

            AddLog($"[收到] {e}");

            // 处理运行命令
            if (e.Trim() == GlobalVariables.RunConfig.StartCommand)
            {
                ProcessInspection();
            }
        }

        /// <summary>
        /// 处理检测流程
        /// </summary>
        private void ProcessInspection()
        {
            try
            {
                // 检查相机是否连接
                if (!CameraManager.Instance.IsConnected)
                {
                    AddLog("[错误] 相机未连接");
                    return;
                }

                // 检查版型是否加载
                if (GlobalVariables.CurrentModel == null)
                {
                    AddLog("[错误] 未选择版型");
                    return;
                }

                AddLog("[检测] 开始检测流程...");

                // 1. 拍照
                Bitmap image = CameraManager.Instance.GrabOne();
                if (image == null)
                {
                    AddLog("[错误] 采集图像失败");
                    SendResult(false);
                    return;
                }

                AddLog("[检测] 图像采集完成");

                // 显示图像
                cogRecordDisplay1.Image = new Cognex.VisionPro.CogImage8Grey(image);

                // 2. 执行检测
                InspectionResult result = InspectionManager.Instance.RunInspection(image);

                if (result == null)
                {
                    AddLog("[错误] 检测失败");
                    SendResult(false);
                    return;
                }

                // 3. 更新显示
                UpdateInspectionResult(result);

                // 4. 发送结果
                if (GlobalVariables.RunConfig.AutoSendResult)
                {
                    SendResult(result.IsPass);
                }

                AddLog($"[检测] 检测完成 - {(result.IsPass ? "OK" : "NG")}");
            }
            catch (Exception ex)
            {
                AddLog($"[错误] 检测异常: {ex.Message}");
                SendResult(false);
            }
        }

        /// <summary>
        /// 发送检测结果
        /// </summary>
        private void SendResult(bool isPass)
        {
            string command = isPass ? GlobalVariables.RunConfig.OkCommand : GlobalVariables.RunConfig.NgCommand;

            if (CommunicationManager.Instance.IsConnected)
            {
                bool sent = CommunicationManager.Instance.Send(command);
                AddLog($"[发送] {(isPass ? "OK" : "NG")} -> {command} {(sent ? "成功" : "失败")}");
            }
        }

        /// <summary>
        /// 更新检测结果显示
        /// </summary>
        private void UpdateInspectionResult(InspectionResult result)
        {
            lblResult.Text = result.IsPass ? "OK" : "NG";
            lblResult.ForeColor = result.IsPass ? System.Drawing.Color.Green : System.Drawing.Color.Red;

            lblX.Text = result.X.ToString("F3");
            lblY.Text = result.Y.ToString("F3");
            lblAngle.Text = result.Angle.ToString("F3");

            lblRunTimeValue.Text = $"{result.RunTime:F1} ms";

            // 更新统计（从 StatisticsManager 获取）
            var stats = StatisticsManager.Instance.GetTodayStatistics();
            if (stats != null)
            {
                lblTotal.Text = stats.TotalCount.ToString();
                lblOK.Text = stats.OkCount.ToString();
                lblNG.Text = stats.NgCount.ToString();
                lblYield.Text = stats.YieldRate.ToString("F2") + "%";
            }
        }

        private void CommunicationManager_ConnectionStateChanged(object sender, ConnectionState e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => CommunicationManager_ConnectionStateChanged(sender, e)));
                return;
            }

            lblCommStatus.Text = e.ToString();
            lblCommStatus.ForeColor = e == ConnectionState.Connected ? Color.Green : Color.Red;
        }

        private void InspectionManager_InspectionCompleted(object sender, InspectionResult e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => InspectionManager_InspectionCompleted(sender, e)));
                return;
            }

            UpdateResultDisplay(e);
            UpdateStatisticsDisplay();
            AddLog($"[检测] {e}");
        }

        private void InspectionManager_StateChanged(object sender, SystemState e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => InspectionManager_StateChanged(sender, e)));
                return;
            }

            UpdateStatus(e);
        }

        private void ModelManager_ModelChanged(object sender, ModelConfig e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => ModelManager_ModelChanged(sender, e)));
                return;
            }

            lblCurrentModel.Text = e?.ModelName ?? "未选择";
            AddLog($"[版型] 切换到: {e?.ModelName}");
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            // 更新运行时间
            if (InspectionManager.Instance.IsRunning)
            {
                var stats = InspectionManager.Instance.RealtimeStats;
                lblRunTime.Text = Utils.FormatTimeSpan(stats.RunDuration);
            }
        }

        #endregion

        #region UI更新

        private void UpdateUserInfo()
        {
            var user = GlobalVariables.CurrentUser;
            if (user != null)
            {
                lblCurrentUser.Text = $"{user.UserName} ({user.Level})";
            }
        }

        private void UpdatePermissionUI()
        {
            btnCameraSettings.Enabled = GlobalVariables.HasPermission(PermissionType.CameraSettings);
            btnCommSettings.Enabled = GlobalVariables.HasPermission(PermissionType.CommunicationSettings);
            btnOfflineTest.Enabled = GlobalVariables.HasPermission(PermissionType.OfflineTest);
            btnUserManage.Enabled = GlobalVariables.HasPermission(PermissionType.UserManagement);
        }

        private void UpdateStatus(SystemState state)
        {
            lblStatus.Text = state.ToString();
            switch (state)
            {
                case SystemState.Running:
                    lblStatus.ForeColor = Color.Green;
                    break;
                case SystemState.Error:
                    lblStatus.ForeColor = Color.Red;
                    break;
                default:
                    lblStatus.ForeColor = Color.Black;
                    break;
            }
        }

        private void UpdateResultDisplay(InspectionResult result)
        {
            if (result == null) return;

            // 更新结果标签
            lblResult.Text = result.IsPass ? "OK" : "NG";
            lblResult.BackColor = result.IsPass ? Color.Green : Color.Red;

            // 更新坐标
            lblX.Text = result.X.ToString("F3");
            lblY.Text = result.Y.ToString("F3");
            lblAngle.Text = result.Angle.ToString("F3");
            lblScore.Text = result.Score.ToString("F2") + "%";
            lblRunTimeValue.Text = result.RunTime.ToString("F1") + "ms";

            // 更新结果图像
            if (result.ResultImage != null)
            {
                cogRecordDisplay1.Image = new Cognex.VisionPro.CogImage8Grey(result.ResultImage);
            }
        }

        private void UpdateStatisticsDisplay()
        {
            var stats = InspectionManager.Instance.RealtimeStats;
            lblTotal.Text = stats.TotalCount.ToString();
            lblOK.Text = stats.OkCount.ToString();
            lblNG.Text = stats.NgCount.ToString();
            lblYield.Text = stats.YieldRate.ToString("F2") + "%";
        }

        private void AddLog(string message)
        {
            string log = $"[{DateTime.Now:HH:mm:ss}] {message}";
            txtLog.AppendText(log + Environment.NewLine);

            // 限制日志行数
            if (txtLog.Lines.Length > Constants.MAX_LOG_LINES)
            {
                string[] lines = txtLog.Lines;
                string[] newLines = new string[lines.Length - 100];
                Array.Copy(lines, 100, newLines, 0, newLines.Length);
                txtLog.Lines = newLines;
            }
        }

        #endregion
    }
}
