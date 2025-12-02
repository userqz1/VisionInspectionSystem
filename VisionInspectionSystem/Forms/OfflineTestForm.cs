using System;
using System.Collections.Generic;
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
        private readonly string _recipeConfigDir;

        // 是否已经运行过一次检测（用于第二次点击自动切换下一张）
        private bool _hasRunOnce;

        // 版型列表
        private List<RecipeInfo> _recipeList;

        // 是否正在加载版型（防止触发SelectedIndexChanged）
        private bool _isLoadingRecipe;

        #endregion

        #region 内部类 - 版型信息

        private class RecipeInfo
        {
            public string Name { get; set; }
            public string FilePath { get; set; }
            public string VppPath { get; set; }
            public string ImagePath { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }

        #endregion

        #region 构造函数

        public OfflineTestForm()
        {
            InitializeComponent();
            _processor = new VisionProProcessor();

            // 配方文件路径
            _recipeConfigDir = Path.Combine(Application.StartupPath, "Config", "Recipes");

            _hasRunOnce = false;
            _recipeList = new List<RecipeInfo>();
            _isLoadingRecipe = false;
        }

        #endregion

        #region 窗体事件

        private void OfflineTestForm_Load(object sender, EventArgs e)
        {
            // 确保配方目录存在
            if (!Directory.Exists(_recipeConfigDir))
            {
                Directory.CreateDirectory(_recipeConfigDir);
            }

            // 加载版型列表
            LoadRecipeList();

            txtOutputs.Text = "欢迎使用VisionPro离线测试工具\r\n\r\n请先加载VPP文件和测试图像，或选择已保存的版型。\r\n";
            UpdateUI();
        }

        private void OfflineTestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 关闭图像文件
            CloseImageFile();
            _processor?.Dispose();
        }

        #endregion

        #region 按钮事件 - 文件加载

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

        #endregion

        #region 按钮事件 - 检测运行

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

            // 如果已经运行过一次，第二次点击时自动切换到下一张图像
            if (_hasRunOnce)
            {
                _currentImageIndex++;
                if (_currentImageIndex >= _maxImageCount)
                {
                    _currentImageIndex = 0;
                }
                DisplayCurrentImage();
            }

            RunInspection();
            _hasRunOnce = true;
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
            _hasRunOnce = false; // 手动切换后重置
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
            _hasRunOnce = false; // 手动切换后重置
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
            _hasRunOnce = false;
        }

        #endregion

        #region 按钮事件 - 版型管理

        /// <summary>
        /// 保存版型
        /// </summary>
        private void btnSaveRecipe_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentVppPath))
            {
                MessageBox.Show("请先加载VPP文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 弹出输入框让用户输入版型名称
            string recipeName = ShowInputDialog("保存版型", "请输入版型名称:", "新版型");
            if (string.IsNullOrEmpty(recipeName))
            {
                return;
            }

            // 检查名称是否已存在
            string filePath = Path.Combine(_recipeConfigDir, $"{recipeName}.ini");
            if (File.Exists(filePath))
            {
                if (MessageBox.Show($"版型 \"{recipeName}\" 已存在，是否覆盖？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }
            }

            // 保存版型
            SaveRecipeToFile(filePath, recipeName);

            // 刷新版型列表
            LoadRecipeList();

            // 选中刚保存的版型
            for (int i = 0; i < cboCurrentRecipe.Items.Count; i++)
            {
                if (cboCurrentRecipe.Items[i] is RecipeInfo recipe && recipe.Name == recipeName)
                {
                    _isLoadingRecipe = true;
                    cboCurrentRecipe.SelectedIndex = i;
                    _isLoadingRecipe = false;
                    break;
                }
            }
        }

        /// <summary>
        /// 版型管理
        /// </summary>
        private void btnManageRecipe_Click(object sender, EventArgs e)
        {
            ShowRecipeManageDialog();
        }

        /// <summary>
        /// 显示版型管理对话框
        /// </summary>
        private void ShowRecipeManageDialog()
        {
            Form manageForm = new Form()
            {
                Width = 450,
                Height = 350,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "版型管理",
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false
            };

            ListBox lstRecipes = new ListBox()
            {
                Left = 20,
                Top = 20,
                Width = 300,
                Height = 250
            };

            Button btnDelete = new Button()
            {
                Text = "删除",
                Left = 340,
                Top = 20,
                Width = 80,
                Height = 30
            };

            Button btnOpenDir = new Button()
            {
                Text = "打开目录",
                Left = 340,
                Top = 60,
                Width = 80,
                Height = 30
            };

            Button btnRefresh = new Button()
            {
                Text = "刷新",
                Left = 340,
                Top = 100,
                Width = 80,
                Height = 30
            };

            Button btnClose = new Button()
            {
                Text = "关闭",
                Left = 340,
                Top = 240,
                Width = 80,
                Height = 30,
                DialogResult = DialogResult.Cancel
            };

            // 加载版型列表
            Action refreshList = () =>
            {
                lstRecipes.Items.Clear();
                if (Directory.Exists(_recipeConfigDir))
                {
                    var files = Directory.GetFiles(_recipeConfigDir, "*.ini");
                    foreach (var file in files)
                    {
                        lstRecipes.Items.Add(Path.GetFileNameWithoutExtension(file));
                    }
                }
            };
            refreshList();

            // 删除按钮事件
            btnDelete.Click += (s, args) =>
            {
                if (lstRecipes.SelectedItem == null)
                {
                    MessageBox.Show("请先选择要删除的版型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string recipeName = lstRecipes.SelectedItem.ToString();
                if (MessageBox.Show($"确定要删除版型 \"{recipeName}\" 吗？\n\n此操作不可恢复！", "确认删除",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    try
                    {
                        string filePath = Path.Combine(_recipeConfigDir, $"{recipeName}.ini");
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                            MessageBox.Show($"版型 \"{recipeName}\" 已删除", "删除成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            refreshList();

                            // 同时刷新主窗体的下拉框
                            LoadRecipeList();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"删除失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            };

            // 打开目录按钮事件
            btnOpenDir.Click += (s, args) =>
            {
                if (Directory.Exists(_recipeConfigDir))
                {
                    System.Diagnostics.Process.Start("explorer.exe", _recipeConfigDir);
                }
            };

            // 刷新按钮事件
            btnRefresh.Click += (s, args) =>
            {
                refreshList();
                LoadRecipeList(); // 同时刷新下拉框
            };

            manageForm.Controls.Add(lstRecipes);
            manageForm.Controls.Add(btnDelete);
            manageForm.Controls.Add(btnOpenDir);
            manageForm.Controls.Add(btnRefresh);
            manageForm.Controls.Add(btnClose);
            manageForm.CancelButton = btnClose;

            manageForm.ShowDialog(this);
        }

        /// <summary>
        /// 当前版型选择改变
        /// </summary>
        private void cboCurrentRecipe_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoadingRecipe) return;

            if (cboCurrentRecipe.SelectedItem is RecipeInfo recipe)
            {
                LoadRecipeFromFile(recipe.FilePath, recipe.Name);
            }
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
                lblStatus.Text = "正在加载VPP...";

                if (_processor.LoadJob(path))
                {
                    _currentVppPath = path;

                    // 更新路径显示
                    lblVppPathDisplay.Text = path;

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

                    UpdateUI();
                    lblStatus.Text = "VPP加载成功";

                    // 提示加载成功
                    MessageBox.Show($"VPP文件加载成功!\n\n文件: {Path.GetFileName(path)}\n输入参数: {_processor.InputNames.Count}个\n输出参数: {_processor.OutputNames.Count}个",
                        "加载成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    txtOutputs.Text = "VPP文件加载失败！请确保文件是有效的ToolBlock格式。";
                    lblStatus.Text = "VPP加载失败";
                    MessageBox.Show("VPP文件加载失败！\n请确保文件是有效的ToolBlock格式。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                txtOutputs.Text = $"加载VPP失败: {ex.Message}\r\n\r\n{ex.StackTrace}";
                lblStatus.Text = "VPP加载失败";
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
                lblStatus.Text = "正在加载图像...";

                // 先关闭之前的文件
                CloseImageFile();

                // 使用CogImageFile统一处理各种图像格式
                _imageFile = new CogImageFile();
                _imageFile.Open(path, CogImageFileModeConstants.Read);

                _maxImageCount = _imageFile.Count;
                _currentImageIndex = 0;
                _currentImagePath = path;
                _hasRunOnce = false;

                // 更新路径显示
                lblImagePathDisplay.Text = path;

                // 更新图像索引显示
                UpdateImageIndexLabel();

                txtOutputs.AppendText($"\r\n========================================\r\n");
                txtOutputs.AppendText($"图像文件加载成功!\r\n");
                txtOutputs.AppendText($"文件: {path}\r\n");
                txtOutputs.AppendText($"图像数量: {_maxImageCount}\r\n");
                txtOutputs.AppendText($"========================================\r\n");

                // 显示第一张图像
                DisplayCurrentImage();

                UpdateUI();
                lblStatus.Text = $"图像加载成功，共 {_maxImageCount} 张";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载图像失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtOutputs.AppendText($"加载图像失败: {ex.Message}\r\n");
                lblStatus.Text = "图像加载失败";
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
                lblStatus.Text = "正在检测...";
                txtOutputs.AppendText($"\r\n--- 开始检测 (图像 {_currentImageIndex + 1}/{_maxImageCount}) ---\r\n");

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
            lblStatus.Text = $"检测完成 - {(result.IsPass ? "OK" : "NG")} ({result.RunTime:F1}ms)";

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

        #region 私有方法 - 版型保存/加载

        /// <summary>
        /// 加载版型列表到下拉框
        /// </summary>
        private void LoadRecipeList()
        {
            _isLoadingRecipe = true;
            cboCurrentRecipe.Items.Clear();
            _recipeList.Clear();

            if (Directory.Exists(_recipeConfigDir))
            {
                var files = Directory.GetFiles(_recipeConfigDir, "*.ini");
                foreach (var file in files)
                {
                    var recipe = LoadRecipeInfo(file);
                    if (recipe != null)
                    {
                        _recipeList.Add(recipe);
                        cboCurrentRecipe.Items.Add(recipe);
                    }
                }
            }

            _isLoadingRecipe = false;
        }

        /// <summary>
        /// 从文件读取版型信息
        /// </summary>
        private RecipeInfo LoadRecipeInfo(string filePath)
        {
            try
            {
                var recipe = new RecipeInfo
                {
                    FilePath = filePath,
                    Name = Path.GetFileNameWithoutExtension(filePath)
                };

                foreach (string line in File.ReadAllLines(filePath))
                {
                    if (line.StartsWith("VppPath="))
                    {
                        recipe.VppPath = line.Substring("VppPath=".Length).Trim();
                    }
                    else if (line.StartsWith("ImagePath="))
                    {
                        recipe.ImagePath = line.Substring("ImagePath=".Length).Trim();
                    }
                }

                return recipe;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 保存配方到文件
        /// </summary>
        private void SaveRecipeToFile(string filePath, string recipeName)
        {
            try
            {
                // 确保目录存在
                string configDir = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(configDir))
                {
                    Directory.CreateDirectory(configDir);
                }

                // 写入配置
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine($"[Recipe]");
                    writer.WriteLine($"Name={recipeName}");
                    writer.WriteLine($"VppPath={_currentVppPath ?? ""}");
                    writer.WriteLine($"ImagePath={_currentImagePath ?? ""}");
                    writer.WriteLine($"SaveTime={DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                }

                lblStatus.Text = $"版型 \"{recipeName}\" 保存成功";
                txtOutputs.AppendText($"\r\n版型 \"{recipeName}\" 保存成功: {filePath}\r\n");
                MessageBox.Show($"版型 \"{recipeName}\" 保存成功!", "保存成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LogHelper.Info("OfflineTest", $"版型保存成功: {recipeName} -> {filePath}");
            }
            catch (Exception ex)
            {
                lblStatus.Text = "版型保存失败";
                MessageBox.Show($"保存版型失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogHelper.Error("OfflineTest", $"保存版型失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 从文件加载配方
        /// </summary>
        private void LoadRecipeFromFile(string filePath, string recipeName)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    MessageBox.Show($"版型文件不存在: {filePath}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string vppPath = "";
                string imagePath = "";

                // 读取配置
                foreach (string line in File.ReadAllLines(filePath))
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

                txtOutputs.Text = $"正在加载版型 \"{recipeName}\"...\r\n";
                lblStatus.Text = $"正在加载版型 \"{recipeName}\"...";

                // 加载VPP文件
                if (!string.IsNullOrEmpty(vppPath))
                {
                    if (File.Exists(vppPath))
                    {
                        txtOutputs.AppendText($"VPP: {vppPath}\r\n");

                        if (_processor.LoadJob(vppPath))
                        {
                            _currentVppPath = vppPath;
                            lblVppPathDisplay.Text = vppPath;

                            txtOutputs.AppendText("VPP加载成功!\r\n");
                            txtOutputs.AppendText(_processor.GetInputOutputSummary());
                            FillInputsGrid();
                        }
                        else
                        {
                            txtOutputs.AppendText("VPP加载失败!\r\n");
                        }
                    }
                    else
                    {
                        txtOutputs.AppendText($"VPP文件不存在: {vppPath}\r\n");
                    }
                }

                // 加载图像文件
                if (!string.IsNullOrEmpty(imagePath))
                {
                    if (File.Exists(imagePath))
                    {
                        txtOutputs.AppendText($"\r\n正在加载图像: {imagePath}\r\n");

                        try
                        {
                            CloseImageFile();
                            _imageFile = new CogImageFile();
                            _imageFile.Open(imagePath, CogImageFileModeConstants.Read);

                            _maxImageCount = _imageFile.Count;
                            _currentImageIndex = 0;
                            _currentImagePath = imagePath;
                            _hasRunOnce = false;

                            lblImagePathDisplay.Text = imagePath;

                            UpdateImageIndexLabel();
                            DisplayCurrentImage();

                            txtOutputs.AppendText($"图像加载成功! 共 {_maxImageCount} 张\r\n");
                        }
                        catch (Exception ex)
                        {
                            txtOutputs.AppendText($"图像加载失败: {ex.Message}\r\n");
                        }
                    }
                    else
                    {
                        txtOutputs.AppendText($"图像文件不存在: {imagePath}\r\n");
                    }
                }

                txtOutputs.AppendText("\r\n========================================\r\n");
                txtOutputs.AppendText($"版型 \"{recipeName}\" 加载完成。\r\n");

                UpdateUI();
                lblStatus.Text = $"版型 \"{recipeName}\" 加载完成";

                LogHelper.Info("OfflineTest", $"版型加载成功: {recipeName}");
            }
            catch (Exception ex)
            {
                LogHelper.Error("OfflineTest", $"加载版型失败: {ex.Message}");
                txtOutputs.Text = $"加载版型失败: {ex.Message}\r\n";
                lblStatus.Text = "版型加载失败";
                MessageBox.Show($"加载版型失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region 私有方法 - 辅助

        /// <summary>
        /// 显示输入对话框
        /// </summary>
        private string ShowInputDialog(string title, string prompt, string defaultValue)
        {
            Form inputForm = new Form()
            {
                Width = 400,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = title,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lblPrompt = new Label() { Left = 20, Top = 20, Width = 340, Text = prompt };
            TextBox txtInput = new TextBox() { Left = 20, Top = 45, Width = 340, Text = defaultValue };
            Button btnOk = new Button() { Text = "确定", Left = 200, Width = 75, Top = 80, DialogResult = DialogResult.OK };
            Button btnCancel = new Button() { Text = "取消", Left = 285, Width = 75, Top = 80, DialogResult = DialogResult.Cancel };

            inputForm.Controls.Add(lblPrompt);
            inputForm.Controls.Add(txtInput);
            inputForm.Controls.Add(btnOk);
            inputForm.Controls.Add(btnCancel);
            inputForm.AcceptButton = btnOk;
            inputForm.CancelButton = btnCancel;

            return inputForm.ShowDialog() == DialogResult.OK ? txtInput.Text.Trim() : null;
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
            btnPrevImage.Enabled = hasImage && _maxImageCount > 1;
            btnNextImage.Enabled = hasImage && _maxImageCount > 1;
        }

        #endregion
    }
}
