using System;
using System.IO;
using System.Windows.Forms;
using VisionInspectionSystem.BLL;
using VisionInspectionSystem.Common;
using VisionInspectionSystem.DAL;
using VisionInspectionSystem.Models;

namespace VisionInspectionSystem.Forms
{
    /// <summary>
    /// 版型管理窗体
    /// </summary>
    public partial class ModelManageForm : Form
    {
        #region 私有字段

        private ModelConfig _currentConfig;

        #endregion

        #region 构造函数

        public ModelManageForm()
        {
            InitializeComponent();
        }

        #endregion

        #region 窗体事件

        private void ModelManageForm_Load(object sender, EventArgs e)
        {
            // 初始化图像格式下拉框
            cboImageFormat.SelectedIndex = 0;

            // 加载版型列表
            LoadModelList();

            // 更新按钮状态
            UpdateButtonState();
        }

        #endregion

        #region 版型列表操作

        /// <summary>
        /// 加载版型列表
        /// </summary>
        private void LoadModelList()
        {
            lstModels.Items.Clear();
            var modelNames = ModelManager.Instance.GetAllModelNames();
            foreach (var name in modelNames)
            {
                lstModels.Items.Add(name);
            }
        }

        /// <summary>
        /// 选择版型改变
        /// </summary>
        private void lstModels_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstModels.SelectedItem == null)
            {
                ClearConfigDisplay();
                return;
            }

            string modelName = lstModels.SelectedItem.ToString();
            LoadModelConfig(modelName);
            UpdateButtonState();
        }

        /// <summary>
        /// 加载版型配置到界面
        /// </summary>
        private void LoadModelConfig(string modelName)
        {
            _currentConfig = ModelManager.Instance.LoadModelConfig(modelName);

            if (_currentConfig != null)
            {
                // 基本信息
                txtModelName.Text = _currentConfig.ModelName;
                txtDescription.Text = _currentConfig.Description;
                lblCreatorValue.Text = _currentConfig.Creator ?? "-";
                lblCreateTimeValue.Text = _currentConfig.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                lblModifyTimeValue.Text = _currentConfig.ModifyTime.ToString("yyyy-MM-dd HH:mm:ss");

                // VPP配置
                txtVppPath.Text = _currentConfig.VppFilePath ?? "";

                // 输出设置
                txtResultFormat.Text = _currentConfig.ResultFormat ?? "OK,{X},{Y},{R}";
                numDecimalPlaces.Value = _currentConfig.DecimalPlaces;
                chkSaveOkImage.Checked = _currentConfig.SaveOkImage;
                chkSaveNgImage.Checked = _currentConfig.SaveNgImage;

                // 图像格式
                int formatIndex = cboImageFormat.FindStringExact(_currentConfig.ImageFormat ?? "jpg");
                cboImageFormat.SelectedIndex = formatIndex >= 0 ? formatIndex : 0;
            }
            else
            {
                ClearConfigDisplay();
            }
        }

        /// <summary>
        /// 清空配置显示
        /// </summary>
        private void ClearConfigDisplay()
        {
            _currentConfig = null;
            txtModelName.Text = "";
            txtDescription.Text = "";
            lblCreatorValue.Text = "-";
            lblCreateTimeValue.Text = "-";
            lblModifyTimeValue.Text = "-";
            txtVppPath.Text = "";
            txtResultFormat.Text = "OK,{X},{Y},{R}";
            numDecimalPlaces.Value = 3;
            chkSaveOkImage.Checked = true;
            chkSaveNgImage.Checked = true;
            cboImageFormat.SelectedIndex = 0;
        }

        /// <summary>
        /// 更新按钮状态
        /// </summary>
        private void UpdateButtonState()
        {
            bool hasSelection = lstModels.SelectedItem != null;
            bool hasPermission = GlobalVariables.HasPermission(PermissionType.ModelEdit);

            btnRename.Enabled = hasSelection && hasPermission;
            btnCopy.Enabled = hasSelection && hasPermission;
            btnDelete.Enabled = hasSelection && hasPermission;
            btnSave.Enabled = hasSelection && hasPermission;
            btnCreate.Enabled = hasPermission;
            btnBrowseVpp.Enabled = hasSelection && hasPermission;

            // 配置区域
            txtDescription.ReadOnly = !hasPermission;
            txtResultFormat.ReadOnly = !hasPermission;
            numDecimalPlaces.Enabled = hasPermission;
            chkSaveOkImage.Enabled = hasPermission;
            chkSaveNgImage.Enabled = hasPermission;
            cboImageFormat.Enabled = hasPermission;
        }

        #endregion

        #region 按钮事件 - 版型操作

        /// <summary>
        /// 新建版型
        /// </summary>
        private void btnCreate_Click(object sender, EventArgs e)
        {
            string modelName = ShowInputDialog("新建版型", "请输入版型名称:", "");
            if (string.IsNullOrEmpty(modelName))
            {
                return;
            }

            // 检查名称是否已存在
            if (ModelManager.Instance.ModelExists(modelName))
            {
                MessageBox.Show($"版型 \"{modelName}\" 已存在", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 创建版型
            if (ModelManager.Instance.CreateModel(modelName, ""))
            {
                MessageBox.Show($"版型 \"{modelName}\" 创建成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadModelList();

                // 选中新创建的版型
                int index = lstModels.FindStringExact(modelName);
                if (index >= 0)
                {
                    lstModels.SelectedIndex = index;
                }
            }
            else
            {
                MessageBox.Show("创建版型失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 重命名版型
        /// </summary>
        private void btnRename_Click(object sender, EventArgs e)
        {
            if (lstModels.SelectedItem == null) return;

            string oldName = lstModels.SelectedItem.ToString();
            string newName = ShowInputDialog("重命名版型", "请输入新的版型名称:", oldName);

            if (string.IsNullOrEmpty(newName) || newName == oldName)
            {
                return;
            }

            // 检查新名称是否已存在
            if (ModelManager.Instance.ModelExists(newName))
            {
                MessageBox.Show($"版型 \"{newName}\" 已存在", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 重命名版型
            if (ModelManager.Instance.RenameModel(oldName, newName))
            {
                MessageBox.Show($"版型重命名成功: {oldName} -> {newName}", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadModelList();

                // 选中重命名后的版型
                int index = lstModels.FindStringExact(newName);
                if (index >= 0)
                {
                    lstModels.SelectedIndex = index;
                }
            }
            else
            {
                MessageBox.Show("重命名版型失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 复制版型
        /// </summary>
        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (lstModels.SelectedItem == null) return;

            string sourceName = lstModels.SelectedItem.ToString();
            string newName = ShowInputDialog("复制版型", "请输入新版型名称:", $"{sourceName}_Copy");

            if (string.IsNullOrEmpty(newName))
            {
                return;
            }

            // 检查新名称是否已存在
            if (ModelManager.Instance.ModelExists(newName))
            {
                MessageBox.Show($"版型 \"{newName}\" 已存在", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 复制版型
            if (ModelManager.Instance.CopyModel(sourceName, newName))
            {
                MessageBox.Show($"版型复制成功: {sourceName} -> {newName}", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadModelList();

                // 选中复制后的版型
                int index = lstModels.FindStringExact(newName);
                if (index >= 0)
                {
                    lstModels.SelectedIndex = index;
                }
            }
            else
            {
                MessageBox.Show("复制版型失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 删除版型
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstModels.SelectedItem == null) return;

            string modelName = lstModels.SelectedItem.ToString();

            // 检查是否是当前使用的版型
            if (GlobalVariables.CurrentModel?.ModelName == modelName)
            {
                MessageBox.Show("不能删除当前正在使用的版型", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 确认删除
            if (MessageBox.Show($"确定要删除版型 \"{modelName}\" 吗？\n\n此操作将删除版型目录下的所有文件，不可恢复！",
                "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                return;
            }

            // 删除版型
            if (ModelManager.Instance.DeleteModel(modelName))
            {
                MessageBox.Show($"版型 \"{modelName}\" 已删除", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadModelList();
                ClearConfigDisplay();
                UpdateButtonState();
            }
            else
            {
                MessageBox.Show("删除版型失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region 按钮事件 - 配置操作

        /// <summary>
        /// 浏览VPP文件
        /// </summary>
        private void btnBrowseVpp_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "选择VisionPro ToolBlock文件";
                ofd.Filter = "VisionPro文件|*.vpp|所有文件|*.*";

                if (!string.IsNullOrEmpty(txtVppPath.Text) && File.Exists(txtVppPath.Text))
                {
                    ofd.InitialDirectory = Path.GetDirectoryName(txtVppPath.Text);
                }

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtVppPath.Text = ofd.FileName;
                }
            }
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_currentConfig == null)
            {
                MessageBox.Show("请先选择一个版型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 更新配置
            _currentConfig.Description = txtDescription.Text;
            _currentConfig.VppFilePath = txtVppPath.Text;
            _currentConfig.ResultFormat = txtResultFormat.Text;
            _currentConfig.DecimalPlaces = (int)numDecimalPlaces.Value;
            _currentConfig.SaveOkImage = chkSaveOkImage.Checked;
            _currentConfig.SaveNgImage = chkSaveNgImage.Checked;
            _currentConfig.ImageFormat = cboImageFormat.SelectedItem?.ToString() ?? "jpg";

            // 保存配置
            if (ModelManager.Instance.SaveModelConfig(_currentConfig))
            {
                MessageBox.Show("配置保存成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 刷新显示
                LoadModelConfig(_currentConfig.ModelName);
            }
            else
            {
                MessageBox.Show("配置保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region 辅助方法

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

            return inputForm.ShowDialog(this) == DialogResult.OK ? txtInput.Text.Trim() : null;
        }

        #endregion
    }
}
