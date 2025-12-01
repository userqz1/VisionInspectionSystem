using System;
using System.Windows.Forms;
using VisionInspectionSystem.BLL;
using VisionInspectionSystem.Common;
using VisionInspectionSystem.Models;

namespace VisionInspectionSystem.Forms
{
    public partial class CommunicationForm : Form
    {
        private bool _isStarted = false;

        public CommunicationForm()
        {
            InitializeComponent();
        }

        private void CommunicationForm_Load(object sender, EventArgs e)
        {
            // 加载本机IP列表
            LoadLocalIPs();

            // 加载配置
            LoadConfig();

            // 订阅事件
            CommunicationManager.Instance.DataReceived += OnDataReceived;
            CommunicationManager.Instance.ConnectionStateChanged += OnConnectionStateChanged;
            CommunicationManager.Instance.Tcp.ClientConnected += OnClientConnected;
            CommunicationManager.Instance.Tcp.ClientDisconnected += OnClientDisconnected;
            CommunicationManager.Instance.Tcp.ErrorOccurred += OnErrorOccurred;

            // 更新界面状态
            UpdateUIState();
            UpdateConnectionStatus();
        }

        private void CommunicationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CommunicationManager.Instance.DataReceived -= OnDataReceived;
            CommunicationManager.Instance.ConnectionStateChanged -= OnConnectionStateChanged;
            CommunicationManager.Instance.Tcp.ClientConnected -= OnClientConnected;
            CommunicationManager.Instance.Tcp.ClientDisconnected -= OnClientDisconnected;
            CommunicationManager.Instance.Tcp.ErrorOccurred -= OnErrorOccurred;
        }

        private void LoadLocalIPs()
        {
            cboLocalIP.Items.Clear();
            cboLocalIP.Items.Add("127.0.0.1");
            cboLocalIP.Items.Add("0.0.0.0"); // 监听所有网卡

            var ips = Utils.GetLocalIPAddresses();
            foreach (var ip in ips)
            {
                cboLocalIP.Items.Add(ip);
            }
        }

        private void LoadConfig()
        {
            var config = CommunicationManager.Instance.Config;
            rdoServer.Checked = config.Mode == CommunicationMode.Server;
            rdoClient.Checked = config.Mode == CommunicationMode.Client;

            // 本地设置
            if (cboLocalIP.Items.Contains(config.LocalIP))
                cboLocalIP.SelectedItem = config.LocalIP;
            else
                cboLocalIP.Text = config.LocalIP;
            txtLocalPort.Text = config.LocalPort.ToString();

            // 远程设置
            txtRemoteIP.Text = config.RemoteIP;
            txtRemotePort.Text = config.RemotePort.ToString();

            _isStarted = CommunicationManager.Instance.IsConnected;
        }

        private void rdoServer_CheckedChanged(object sender, EventArgs e)
        {
            UpdateUIState();
        }

        private void rdoClient_CheckedChanged(object sender, EventArgs e)
        {
            UpdateUIState();
        }

        private void UpdateUIState()
        {
            bool isServer = rdoServer.Checked;

            // 服务端模式：本地设置重要，远程设置不需要
            // 客户端模式：远程设置重要，本地设置不需要
            groupLocal.Text = isServer ? "本地监听设置 *" : "本地设置（不需要）";
            groupLocal.Enabled = isServer;
            groupRemote.Text = isServer ? "远程设置（不需要）" : "远程连接设置 *";
            groupRemote.Enabled = !isServer;

            // 更新按钮文字
            btnConnect.Text = isServer ? "启动监听" : "连接";
            btnDisconnect.Text = isServer ? "停止监听" : "断开";

            // 更新提示
            if (isServer)
            {
                lblTip.Text = "提示：服务端 - 设置本地IP和端口，等待客户端连接";
            }
            else
            {
                lblTip.Text = "提示：客户端 - 只需设置目标服务端IP和端口";
            }
        }

        private void UpdateConnectionStatus()
        {
            bool isConnected = CommunicationManager.Instance.IsConnected;
            lblStatus.Text = isConnected ? "已连接" : (_isStarted && rdoServer.Checked ? "监听中..." : "未连接");
            lblStatus.ForeColor = isConnected ? System.Drawing.Color.Green :
                                  (_isStarted && rdoServer.Checked ? System.Drawing.Color.Orange : System.Drawing.Color.Red);

            btnConnect.Enabled = !_isStarted;
            btnDisconnect.Enabled = _isStarted;
            rdoServer.Enabled = !_isStarted;
            rdoClient.Enabled = !_isStarted;

            // 根据模式控制设置区域的启用状态
            if (_isStarted)
            {
                groupLocal.Enabled = false;
                groupRemote.Enabled = false;
            }
            else
            {
                // 服务端模式只需要本地设置，客户端模式只需要远程设置
                groupLocal.Enabled = rdoServer.Checked;
                groupRemote.Enabled = rdoClient.Checked;
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            // 验证输入
            if (rdoServer.Checked)
            {
                if (string.IsNullOrEmpty(cboLocalIP.Text))
                {
                    MessageBox.Show("请选择或输入本地IP地址", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int port;
                if (!int.TryParse(txtLocalPort.Text, out port) || port <= 0 || port > 65535)
                {
                    MessageBox.Show("请输入有效的端口号(1-65535)", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(txtRemoteIP.Text))
                {
                    MessageBox.Show("请输入远程IP地址", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int port;
                if (!int.TryParse(txtRemotePort.Text, out port) || port <= 0 || port > 65535)
                {
                    MessageBox.Show("请输入有效的端口号(1-65535)", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            SaveConfig();

            if (CommunicationManager.Instance.Start())
            {
                _isStarted = true;
                if (rdoServer.Checked)
                {
                    AddLog($"服务端启动成功，监听 {cboLocalIP.Text}:{txtLocalPort.Text}，等待客户端连接...");
                }
                else
                {
                    AddLog($"正在连接 {txtRemoteIP.Text}:{txtRemotePort.Text}...");
                }
            }
            else
            {
                AddLog("启动失败！请检查设置。");
            }

            UpdateConnectionStatus();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            CommunicationManager.Instance.Stop();
            _isStarted = false;
            AddLog(rdoServer.Checked ? "服务端已停止" : "已断开连接");
            UpdateConnectionStatus();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSend.Text))
            {
                MessageBox.Show("请输入要发送的内容", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!CommunicationManager.Instance.IsConnected)
            {
                MessageBox.Show("未连接，无法发送", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (CommunicationManager.Instance.Send(txtSend.Text))
            {
                AddLog($"[发送] {txtSend.Text}");
                if (chkClearAfterSend.Checked)
                {
                    txtSend.Clear();
                }
            }
            else
            {
                AddLog("[发送失败]");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtReceive.Clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveConfig();
            CommunicationManager.Instance.SaveConfig();
            MessageBox.Show("配置已保存", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SaveConfig()
        {
            int localPort = 8000;
            int remotePort = 8001;

            if (!int.TryParse(txtLocalPort.Text, out localPort) || localPort <= 0 || localPort > 65535)
            {
                localPort = 8000;
            }
            if (!int.TryParse(txtRemotePort.Text, out remotePort) || remotePort <= 0 || remotePort > 65535)
            {
                remotePort = 8001;
            }

            var config = new CommunicationConfig
            {
                Mode = rdoServer.Checked ? CommunicationMode.Server : CommunicationMode.Client,
                LocalIP = string.IsNullOrEmpty(cboLocalIP.Text) ? "127.0.0.1" : cboLocalIP.Text,
                LocalPort = localPort,
                RemoteIP = string.IsNullOrEmpty(txtRemoteIP.Text) ? "127.0.0.1" : txtRemoteIP.Text,
                RemotePort = remotePort
            };
            CommunicationManager.Instance.UpdateConfig(config);
        }

        private void txtSend_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSend_Click(sender, e);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        #region 事件处理

        private void OnDataReceived(object sender, string e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => OnDataReceived(sender, e)));
                return;
            }
            AddLog($"[收到] {e}");

            // 服务端模式下自动回复"0"
            if (rdoServer.Checked && CommunicationManager.Instance.IsConnected)
            {
                if (CommunicationManager.Instance.Send("0"))
                {
                    AddLog("[自动回复] 0");
                }
            }
        }

        private void OnConnectionStateChanged(object sender, ConnectionState e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => OnConnectionStateChanged(sender, e)));
                return;
            }

            switch (e)
            {
                case ConnectionState.Connected:
                    AddLog("连接已建立");
                    break;
                case ConnectionState.Disconnected:
                    AddLog("连接已断开");
                    break;
                case ConnectionState.Connecting:
                    AddLog("正在连接...");
                    break;
            }

            UpdateConnectionStatus();
        }

        private void OnClientConnected(object sender, string e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => OnClientConnected(sender, e)));
                return;
            }
            AddLog($"[客户端连接] {e}");
            UpdateConnectionStatus();
        }

        private void OnClientDisconnected(object sender, string e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => OnClientDisconnected(sender, e)));
                return;
            }
            AddLog($"[客户端断开] {e}");
            UpdateConnectionStatus();
        }

        private void OnErrorOccurred(object sender, string e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => OnErrorOccurred(sender, e)));
                return;
            }
            AddLog($"[错误] {e}");
        }

        #endregion

        private void AddLog(string message)
        {
            string log = $"[{DateTime.Now:HH:mm:ss}] {message}\r\n";
            txtReceive.AppendText(log);
            txtReceive.ScrollToCaret();
        }
    }
}
