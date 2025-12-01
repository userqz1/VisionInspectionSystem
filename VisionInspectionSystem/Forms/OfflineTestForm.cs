using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using VisionInspectionSystem.BLL;
using VisionInspectionSystem.HAL;
using VisionInspectionSystem.Models;
using VisionInspectionSystem.DAL;

// VisionPro 引用
using Cognex.VisionPro;
using Cognex.VisionPro.ImageFile;

namespace VisionInspectionSystem.Forms
{
    /// <summary>
    /// VisionPro离线测试窗体
    /// </summary>
    public partial class OfflineTestForm : Form
    {
        #region 私有字段

        private VisionProProcessor _processor;
        private string _currentVppPath;
        private string _currentImagePath;

        // 图像文件操作
        private CogImageFile _imageFile;
        private int _currentImageIndex;
        private int _maxImageCount;

        // 配方配置文件路径
        private readonly string _recipeConfigPath;

        #endregion

        #region 构造函数

        public OfflineTestForm()
        {
            InitializeComponent();
            _processor = new VisionProProcessor();
            _recipeConfigPath = Path.Combine(Application.StartupPath, "Config", "OfflineTestRecipe.ini");
        }

        #endregion

        #region 窗体事件

        private void OfflineTestForm_Load(object sender, EventArgs e)
        {
            // 加载上次的配方设置
            LoadRecipeConfig();
            UpdateUI();
        }

        private void OfflineTestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 关闭图像文件
            CloseImageFile();
            _processor?.Dispose();
        }

        #endregion

        #region 按钮事件

        /// <summary>
        /// 加载VPP文件
        /// </summary>
        private void btnLoadVpp_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "选择VisionPro ToolBlock文件";
                ofd.Filter = "VisionPro文件|*.vpp|所有文件|*.*";

                // 如果有上次路径，设置初始目录
                if (!string.IsNullOrEmpty(_currentVppPath) && File.Exists(_currentVppPath))
                {
                    ofd.InitialDirectory = Path.GetDirectoryName(_currentVppPath);
                }

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    LoadVppFile(ofd.FileName);
                }
            }
        }

        /// <summary>
        /// 加载测试图像（支持IDB/CDB/BMP/TIFF等）
        /// </summary>
        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "选择测试图像或图像数据库";
                ofd.Filter = "所有支持格式|*.idb;*.cdb;*.bmp;*.tiff;*.tif;*.jpg;*.jpeg;*.png|" +
                             "VisionPro图像数据库|*.idb;*.cdb|" +
                             "图像文件|*.bmp;*.tiff;*.tif;*.jpg;*.jpeg;*.png|" +
                             "所有文件|*.*";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    LoadImageFile(ofd.FileName);
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

            if (_imageFile == null || _maxImageCount == 0)
            {
                MessageBox.Show("请先加载测试图像", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            RunInspection();
        }

        /// <summary>
        /// 上一张图像
        /// </summary>
        private void btnPrevImage_Click(object sender, EventArgs e)
        {
            if (_imageFile == null || _maxImageCount == 0) return;

            _currentImageIndex--;
            if (_currentImageIndex < 0)
            {
                _currentImageIndex = _maxImageCount - 1;
            }
            DisplayCurrentImage();
        }

        /// <summary>
        /// 下一张图像
        /// </summary>
        private void btnNextImage_Click(object sender, EventArgs e)
        {
            if (_imageFile == null || _maxImageCount == 0) return;

            _currentImageIndex++;
            if (_currentImageIndex >= _maxImageCount)
            {
                _currentImageIndex = 0;
            }
            DisplayCurrentImage();
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
            cogRecordDisplay2.Record = null;
            cogRecordDisplay2.Image = null;
            lblResult.Text = "-";
            lblResult.BackColor = SystemColors.Control;
            lblResult.ForeColor = SystemColors.ControlText;
            lblRunTime.Text = "-";
        }

        #endregion

        #region 私有方法 - VPP加载

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
                    lblVppPath.Text = Path.GetFileName(path);
                    lblVppPath.ToolTipText = path;

                    // 显示输入输出信息
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("========================================");
                    sb.AppendLine("VPP文件加载成功!");
                    sb.AppendLine("========================================");
                    sb.AppendLine($"文件: {path}");
                    sb.AppendLine();
                    sb.AppendLine(_processor.GetInputOutputSummary());

                    txtOutputs.Text = sb.ToString();

                    // 填充输入参数到DataGridView
                    FillInputsGrid();

                    // 保存配方配置
                    SaveRecipeConfig();

                    UpdateUI();

                    // 提示加载成功
                    MessageBox.Show($"VPP文件加载成功!\n\n文件: {Path.GetFileName(path)}\n输入参数: {_processor.InputNames.Count}个\n输出参数: {_processor.OutputNames.Count}个\n\n配方已保存，下次启动将自动加载。",
                        "加载成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    txtOutputs.Text = "VPP文件加载失败！请确保文件是有效的ToolBlock格式。";
                    MessageBox.Show("VPP文件加载失败！\n请确保文件是有效的ToolBlock格式。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        #endregion

        #region 私有方法 - 图像文件操作

        /// <summary>
        /// 加载图像文件（支持IDB/CDB/BMP/TIFF/JPG/PNG）
        /// </summary>
        private void LoadImageFile(string path)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                // 先关闭之前的文件
                CloseImageFile();

                // 使用CogImageFile统一处理各种图像格式
                _imageFile = new CogImageFile();
                _imageFile.Open(path, CogImageFileModeConstants.Read);

                _maxImageCount = _imageFile.Count;
                _currentImageIndex = 0;
                _currentImagePath = path;

                lblImagePath.Text = Path.GetFileName(path);
                lblImagePath.ToolTipText = path;

                // 更新图像索引显示
                UpdateImageIndexLabel();

                txtOutputs.AppendText($"\r\n========================================\r\n");
                txtOutputs.AppendText($"图像文件加载成功!\r\n");
                txtOutputs.AppendText($"文件: {path}\r\n");
                txtOutputs.AppendText($"图像数量: {_maxImageCount}\r\n");
                txtOutputs.AppendText($"========================================\r\n");

                // 显示第一张图像
                DisplayCurrentImage();

                // 保存配方配置
                SaveRecipeConfig();

                UpdateUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载图像失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtOutputs.AppendText($"加载图像失败: {ex.Message}\r\n");
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 关闭图像文件
        /// </summary>
        private void CloseImageFile()
        {
            if (_imageFile != null)
            {
                try
                {
                    _imageFile.Close();
                }
                catch { }
                _imageFile = null;
            }
            _maxImageCount = 0;
            _currentImageIndex = 0;
        }

        /// <summary>
        /// 显示当前图像
        /// </summary>
        private void DisplayCurrentImage()
        {
            if (_imageFile == null || _maxImageCount == 0) return;

            try
            {
                // 获取当前图像
                ICogImage cogImage = _imageFile[_currentImageIndex];

                // 使用CogRecordDisplay显示原始图像
                cogRecordDisplay1.Image = cogImage;
                cogRecordDisplay1.Fit(true);

                // 更新索引标签
                UpdateImageIndexLabel();
            }
            catch (Exception ex)
            {
                txtOutputs.AppendText($"显示图像失败: {ex.Message}\r\n");
            }
        }

        /// <summary>
        /// 更新图像索引标签
        /// </summary>
        private void UpdateImageIndexLabel()
        {
            lblImageIndex.Text = $"第 {_currentImageIndex + 1} / {_maxImageCount} 张";
        }

        /// <summary>
        /// 获取当前CogImage
        /// </summary>
        private ICogImage GetCurrentCogImage()
        {
            if (_imageFile == null || _maxImageCount == 0)
                return null;

            return _imageFile[_currentImageIndex];
        }

        #endregion

        #region 私有方法 - 检测运行

        /// <summary>
        /// 运行检测
        /// </summary>
        private void RunInspection()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                txtOutputs.AppendText("\r\n--- 开始检测 ---\r\n");

                // 获取当前CogImage
                ICogImage cogImage = GetCurrentCogImage();
                if (cogImage == null)
                {
                    txtOutputs.AppendText("无法获取当前图像\r\n");
                    return;
                }

                // 执行检测（直接传入CogImage）
                InspectionResult result = _processor.Run(cogImage);

                if (result != null)
                {
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

            // 使用CogRecordDisplay显示结果图像和图形叠加
            try
            {
                if (_processor.ToolBlock != null)
                {
                    if (result.IsPass)
                    {
                        // OK时显示带图形叠加的结果图像
                        // 使用SubRecords[recordName]方式获取指定的输出图像记录
                        ICogRecord lastRunRecord = _processor.ToolBlock.CreateLastRunRecord();
                        if (lastRunRecord != null && lastRunRecord.SubRecords != null)
                        {
                            ICogRecord displayRecord = null;

                            // 尝试常见的输出图像记录名称
                            string[] possibleRecordNames = new string[]
                            {
                                "LastRun.CogFixtureTool1.OutputImage",
                                "LastRun.OutputImage",
                                "CogFixtureTool1.OutputImage"
                            };

                            foreach (string recordName in possibleRecordNames)
                            {
                                try
                                {
                                    displayRecord = lastRunRecord.SubRecords[recordName];
                                    if (displayRecord != null)
                                    {
                                        txtOutputs.AppendText($"使用Record: {recordName}\r\n");
                                        break;
                                    }
                                }
                                catch { }
                            }

                            // 如果没找到指定名称的记录，使用第一个SubRecord
                            if (displayRecord == null && lastRunRecord.SubRecords.Count > 0)
                            {
                                displayRecord = lastRunRecord.SubRecords[0];
                                txtOutputs.AppendText($"使用默认Record: {displayRecord?.RecordKey}\r\n");
                            }

                            if (displayRecord != null)
                            {
                                cogRecordDisplay2.Record = displayRecord;
                                cogRecordDisplay2.Fit(true);
                            }
                        }
                    }
                    else
                    {
                        // NG时显示原图，避免残留上次OK的结果
                        ICogImage currentImage = GetCurrentCogImage();
                        if (currentImage != null)
                        {
                            cogRecordDisplay2.Record = null;
                            cogRecordDisplay2.Image = currentImage;
                            cogRecordDisplay2.Fit(true);
                            txtOutputs.AppendText("NG - 显示原图\r\n");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                txtOutputs.AppendText($"显示结果图像失败: {ex.Message}\r\n");
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

        #endregion

        #region 私有方法 - 配方保存/加载

        /// <summary>
        /// 保存配方配置
        /// </summary>
        private void SaveRecipeConfig()
        {
            try
            {
                // 确保目录存在
                string configDir = Path.GetDirectoryName(_recipeConfigPath);
                if (!Directory.Exists(configDir))
                {
                    Directory.CreateDirectory(configDir);
                }

                // 写入配置
                using (StreamWriter writer = new StreamWriter(_recipeConfigPath))
                {
                    writer.WriteLine("[OfflineTestRecipe]");
                    writer.WriteLine($"VppPath={_currentVppPath ?? ""}");
                    writer.WriteLine($"ImagePath={_currentImagePath ?? ""}");
                    writer.WriteLine($"SaveTime={DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                }

                LogHelper.Info("OfflineTest", $"配方保存成功: {_recipeConfigPath}");
            }
            catch (Exception ex)
            {
                LogHelper.Error("OfflineTest", $"保存配方失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 加载配方配置
        /// </summary>
        private void LoadRecipeConfig()
        {
            try
            {
                if (!File.Exists(_recipeConfigPath))
                {
                    txtOutputs.Text = "欢迎使用VisionPro离线测试工具\r\n\r\n请先加载VPP文件和测试图像。\r\n";
                    return;
                }

                string vppPath = "";
                string imagePath = "";

                // 读取配置
                foreach (string line in File.ReadAllLines(_recipeConfigPath))
                {
                    if (line.StartsWith("VppPath="))
                    {
                        vppPath = line.Substring("VppPath=".Length).Trim();
                    }
                    else if (line.StartsWith("ImagePath="))
                    {
                        imagePath = line.Substring("ImagePath=".Length).Trim();
                    }
                }

                // 自动加载VPP文件
                if (!string.IsNullOrEmpty(vppPath) && File.Exists(vppPath))
                {
                    txtOutputs.Text = $"正在加载上次的配方...\r\nVPP: {vppPath}\r\n";

                    if (_processor.LoadJob(vppPath))
                    {
                        _currentVppPath = vppPath;
                        lblVppPath.Text = Path.GetFileName(vppPath);
                        lblVppPath.ToolTipText = vppPath;

                        txtOutputs.AppendText("VPP加载成功!\r\n");
                        txtOutputs.AppendText(_processor.GetInputOutputSummary());
                        FillInputsGrid();
                    }
                }

                // 自动加载图像文件
                if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                {
                    txtOutputs.AppendText($"\r\n正在加载图像: {imagePath}\r\n");

                    try
                    {
                        _imageFile = new CogImageFile();
                        _imageFile.Open(imagePath, CogImageFileModeConstants.Read);

                        _maxImageCount = _imageFile.Count;
                        _currentImageIndex = 0;
                        _currentImagePath = imagePath;

                        lblImagePath.Text = Path.GetFileName(imagePath);
                        lblImagePath.ToolTipText = imagePath;

                        UpdateImageIndexLabel();
                        DisplayCurrentImage();

                        txtOutputs.AppendText($"图像加载成功! 共 {_maxImageCount} 张\r\n");
                    }
                    catch (Exception ex)
                    {
                        txtOutputs.AppendText($"图像加载失败: {ex.Message}\r\n");
                    }
                }

                txtOutputs.AppendText("\r\n========================================\r\n");
                txtOutputs.AppendText("配方加载完成，可以开始检测。\r\n");

                LogHelper.Info("OfflineTest", "配方加载成功");
            }
            catch (Exception ex)
            {
                LogHelper.Error("OfflineTest", $"加载配方失败: {ex.Message}");
                txtOutputs.Text = $"加载配方失败: {ex.Message}\r\n\r\n请手动加载VPP文件和测试图像。\r\n";
            }
        }

        #endregion

        #region 私有方法 - 辅助

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
            if (value is ICogImage) return "[CogImage]";
            if (value is ICogGraphic) return "[CogGraphic]";
            return value.ToString();
        }

        /// <summary>
        /// 更新界面状态
        /// </summary>
        private void UpdateUI()
        {
            bool hasImage = _imageFile != null && _maxImageCount > 0;

            btnRun.Enabled = _processor.IsJobLoaded && hasImage;
            btnVppInfo.Enabled = _processor.IsJobLoaded;
            btnPrevImage.Enabled = hasImage && _maxImageCount > 1;
            btnNextImage.Enabled = hasImage && _maxImageCount > 1;
        }

        #endregion
    }
}
