using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using VisionInspectionSystem.BLL;
using VisionInspectionSystem.HAL;
using VisionInspectionSystem.Models;
using VisionInspectionSystem.DAL;

namespace VisionInspectionSystem.Forms
{
    /// <summary>
    /// VisionPro离线测试窗体
    /// </summary>
    public partial class OfflineTestForm : Form
    {
        private VisionProProcessor _processor;
        private string _currentVppPath;
        private string _currentImagePath;

        public OfflineTestForm()
        {
            InitializeComponent();
            _processor = new VisionProProcessor();
        }

        private void OfflineTestForm_Load(object sender, EventArgs e)
        {
            // 初始化界面
            UpdateUI();
        }

        private void OfflineTestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _processor?.Dispose();
        }

        #region 按钮事件

        /// <summary>
        /// 加载VPP文件
        /// </summary>
        private void btnLoadVpp_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "选择VisionPro作业文件";
                ofd.Filter = "VisionPro文件|*.vpp|所有文件|*.*";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    LoadVppFile(ofd.FileName);
                }
            }
        }

        /// <summary>
        /// 加载测试图像
        /// </summary>
        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "选择测试图像";
                ofd.Filter = "图像文件|*.bmp;*.jpg;*.jpeg;*.png;*.tiff;*.tif|所有文件|*.*";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    LoadTestImage(ofd.FileName);
                }
            }
        }

        /// <summary>
        /// 运行检测
        /// </summary>
        private void btnRun_Click(object sender, EventArgs e)
        {
            if (!_processor.IsJobLoaded)
            {
                MessageBox.Show("请先加载VPP文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(_currentImagePath))
            {
                MessageBox.Show("请先加载测试图像", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            RunInspection();
        }

        /// <summary>
        /// 查看VPP信息
        /// </summary>
        private void btnVppInfo_Click(object sender, EventArgs e)
        {
            if (!_processor.IsJobLoaded)
            {
                MessageBox.Show("请先加载VPP文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string info = _processor.GetInputOutputSummary();
            txtOutputs.Text = info;
        }

        /// <summary>
        /// 清除结果
        /// </summary>
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtOutputs.Clear();
            dgvOutputs.Rows.Clear();
            picResult.Image = null;
            lblResult.Text = "-";
            lblResult.BackColor = SystemColors.Control;
            lblRunTime.Text = "-";
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 加载VPP文件
        /// </summary>
        private void LoadVppFile(string path)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                txtOutputs.Text = "正在加载VPP文件...\r\n";

                if (_processor.LoadJob(path))
                {
                    _currentVppPath = path;
                    lblVppPath.Text = System.IO.Path.GetFileName(path);
                    lblVppPath.ToolTipText = path;

                    // 显示输入输出信息
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine($"VPP文件加载成功: {path}");
                    sb.AppendLine();
                    sb.AppendLine(_processor.GetInputOutputSummary());

                    txtOutputs.Text = sb.ToString();

                    // 填充输入参数到DataGridView
                    FillInputsGrid();

                    UpdateUI();
                }
                else
                {
                    txtOutputs.Text = "VPP文件加载失败！";
                    MessageBox.Show("VPP文件加载失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                txtOutputs.Text = $"加载VPP失败: {ex.Message}\r\n\r\n{ex.StackTrace}";
                MessageBox.Show($"加载VPP失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 加载测试图像
        /// </summary>
        private void LoadTestImage(string path)
        {
            try
            {
                using (var img = new Bitmap(path))
                {
                    // 复制图像以避免文件锁定
                    picSource.Image?.Dispose();
                    picSource.Image = new Bitmap(img);
                }

                _currentImagePath = path;
                lblImagePath.Text = System.IO.Path.GetFileName(path);
                lblImagePath.ToolTipText = path;

                txtOutputs.AppendText($"\r\n已加载图像: {path}\r\n");

                UpdateUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载图像失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 运行检测
        /// </summary>
        private void RunInspection()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                txtOutputs.AppendText("\r\n--- 开始检测 ---\r\n");

                // 执行离线检测
                InspectionResult result = _processor.RunOffline(_currentImagePath);

                if (result != null)
                {
                    // 显示结果
                    DisplayResult(result);
                }
                else
                {
                    txtOutputs.AppendText("检测返回空结果\r\n");
                }
            }
            catch (Exception ex)
            {
                txtOutputs.AppendText($"检测异常: {ex.Message}\r\n{ex.StackTrace}\r\n");
                MessageBox.Show($"检测失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 显示检测结果
        /// </summary>
        private void DisplayResult(InspectionResult result)
        {
            // 更新结果标签
            lblResult.Text = result.IsPass ? "OK" : "NG";
            lblResult.BackColor = result.IsPass ? Color.LimeGreen : Color.Red;
            lblResult.ForeColor = Color.White;
            lblRunTime.Text = $"{result.RunTime:F1} ms";

            // 显示结果图像
            if (result.ResultImage != null)
            {
                picResult.Image?.Dispose();
                picResult.Image = result.ResultImage;
            }

            // 填充输出参数到DataGridView
            FillOutputsGrid();

            // 输出详细信息到文本框
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"检测完成 - {(result.IsPass ? "OK" : "NG")}");
            sb.AppendLine($"运行时间: {result.RunTime:F2} ms");
            sb.AppendLine($"消息: {result.Message}");
            sb.AppendLine();
            sb.AppendLine("=== 基本结果 ===");
            sb.AppendLine($"  X: {result.X:F4}");
            sb.AppendLine($"  Y: {result.Y:F4}");
            sb.AppendLine($"  Angle: {result.Angle:F4}");
            sb.AppendLine($"  Score: {result.Score:F4}");
            sb.AppendLine();
            sb.AppendLine("=== 所有输出参数 ===");

            foreach (var output in _processor.LastOutputs)
            {
                string valueStr = FormatOutputValue(output.Value.Value);
                sb.AppendLine($"  [{output.Value.ValueType}] {output.Key}: {valueStr}");
            }

            txtOutputs.AppendText(sb.ToString());
            txtOutputs.AppendText("\r\n--- 检测结束 ---\r\n");

            // 滚动到底部
            txtOutputs.SelectionStart = txtOutputs.TextLength;
            txtOutputs.ScrollToCaret();
        }

        /// <summary>
        /// 填充输入参数到表格
        /// </summary>
        private void FillInputsGrid()
        {
            dgvInputs.Rows.Clear();
            foreach (var input in _processor.GetAllInputs())
            {
                dgvInputs.Rows.Add(input.Name, input.ValueType, FormatOutputValue(input.Value));
            }
        }

        /// <summary>
        /// 填充输出参数到表格
        /// </summary>
        private void FillOutputsGrid()
        {
            dgvOutputs.Rows.Clear();
            foreach (var output in _processor.LastOutputs)
            {
                string valueStr = FormatOutputValue(output.Value.Value);
                dgvOutputs.Rows.Add(output.Key, output.Value.ValueType, valueStr);
            }
        }

        /// <summary>
        /// 格式化输出值
        /// </summary>
        private string FormatOutputValue(object value)
        {
            if (value == null) return "null";
            if (value is double d) return d.ToString("F6");
            if (value is float f) return f.ToString("F6");
            if (value is Cognex.VisionPro.ICogImage) return "[CogImage]";
            if (value is Cognex.VisionPro.ICogGraphic) return "[CogGraphic]";
            return value.ToString();
        }

        /// <summary>
        /// 更新界面状态
        /// </summary>
        private void UpdateUI()
        {
            btnRun.Enabled = _processor.IsJobLoaded && !string.IsNullOrEmpty(_currentImagePath);
            btnVppInfo.Enabled = _processor.IsJobLoaded;
        }

        #endregion
    }
}
