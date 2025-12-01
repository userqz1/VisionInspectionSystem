namespace VisionInspectionSystem.Forms
{
    partial class ModelManageForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBoxModels = new System.Windows.Forms.GroupBox();
            this.lstModels = new System.Windows.Forms.ListBox();
            this.panelModelButtons = new System.Windows.Forms.Panel();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnRename = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.groupBoxConfig = new System.Windows.Forms.GroupBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBoxSettings = new System.Windows.Forms.GroupBox();
            this.cboImageFormat = new System.Windows.Forms.ComboBox();
            this.lblImageFormat = new System.Windows.Forms.Label();
            this.chkSaveNgImage = new System.Windows.Forms.CheckBox();
            this.chkSaveOkImage = new System.Windows.Forms.CheckBox();
            this.numDecimalPlaces = new System.Windows.Forms.NumericUpDown();
            this.lblDecimalPlaces = new System.Windows.Forms.Label();
            this.txtResultFormat = new System.Windows.Forms.TextBox();
            this.lblResultFormat = new System.Windows.Forms.Label();
            this.groupBoxVpp = new System.Windows.Forms.GroupBox();
            this.btnBrowseVpp = new System.Windows.Forms.Button();
            this.txtVppPath = new System.Windows.Forms.TextBox();
            this.lblVppPath = new System.Windows.Forms.Label();
            this.groupBoxInfo = new System.Windows.Forms.GroupBox();
            this.lblModifyTimeValue = new System.Windows.Forms.Label();
            this.lblModifyTime = new System.Windows.Forms.Label();
            this.lblCreateTimeValue = new System.Windows.Forms.Label();
            this.lblCreateTime = new System.Windows.Forms.Label();
            this.lblCreatorValue = new System.Windows.Forms.Label();
            this.lblCreator = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtModelName = new System.Windows.Forms.TextBox();
            this.lblModelName = new System.Windows.Forms.Label();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBoxModels.SuspendLayout();
            this.panelModelButtons.SuspendLayout();
            this.groupBoxConfig.SuspendLayout();
            this.groupBoxSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDecimalPlaces)).BeginInit();
            this.groupBoxVpp.SuspendLayout();
            this.groupBoxInfo.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            //
            // splitContainer1
            //
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            //
            // splitContainer1.Panel1
            //
            this.splitContainer1.Panel1.Controls.Add(this.groupBoxModels);
            //
            // splitContainer1.Panel2
            //
            this.splitContainer1.Panel2.Controls.Add(this.groupBoxConfig);
            this.splitContainer1.Size = new System.Drawing.Size(900, 550);
            this.splitContainer1.SplitterDistance = 280;
            this.splitContainer1.TabIndex = 0;
            //
            // groupBoxModels
            //
            this.groupBoxModels.Controls.Add(this.lstModels);
            this.groupBoxModels.Controls.Add(this.panelModelButtons);
            this.groupBoxModels.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxModels.Location = new System.Drawing.Point(0, 0);
            this.groupBoxModels.Name = "groupBoxModels";
            this.groupBoxModels.Size = new System.Drawing.Size(280, 550);
            this.groupBoxModels.TabIndex = 0;
            this.groupBoxModels.TabStop = false;
            this.groupBoxModels.Text = "版型列表";
            //
            // lstModels
            //
            this.lstModels.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstModels.FormattingEnabled = true;
            this.lstModels.ItemHeight = 12;
            this.lstModels.Location = new System.Drawing.Point(3, 17);
            this.lstModels.Name = "lstModels";
            this.lstModels.Size = new System.Drawing.Size(274, 480);
            this.lstModels.TabIndex = 1;
            this.lstModels.SelectedIndexChanged += new System.EventHandler(this.lstModels_SelectedIndexChanged);
            //
            // panelModelButtons
            //
            this.panelModelButtons.Controls.Add(this.btnDelete);
            this.panelModelButtons.Controls.Add(this.btnCopy);
            this.panelModelButtons.Controls.Add(this.btnRename);
            this.panelModelButtons.Controls.Add(this.btnCreate);
            this.panelModelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelModelButtons.Location = new System.Drawing.Point(3, 497);
            this.panelModelButtons.Name = "panelModelButtons";
            this.panelModelButtons.Size = new System.Drawing.Size(274, 50);
            this.panelModelButtons.TabIndex = 0;
            //
            // btnDelete
            //
            this.btnDelete.Location = new System.Drawing.Point(207, 12);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(60, 28);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            //
            // btnCopy
            //
            this.btnCopy.Location = new System.Drawing.Point(141, 12);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(60, 28);
            this.btnCopy.TabIndex = 2;
            this.btnCopy.Text = "复制";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            //
            // btnRename
            //
            this.btnRename.Location = new System.Drawing.Point(75, 12);
            this.btnRename.Name = "btnRename";
            this.btnRename.Size = new System.Drawing.Size(60, 28);
            this.btnRename.TabIndex = 1;
            this.btnRename.Text = "重命名";
            this.btnRename.UseVisualStyleBackColor = true;
            this.btnRename.Click += new System.EventHandler(this.btnRename_Click);
            //
            // btnCreate
            //
            this.btnCreate.Location = new System.Drawing.Point(9, 12);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(60, 28);
            this.btnCreate.TabIndex = 0;
            this.btnCreate.Text = "新建";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            //
            // groupBoxConfig
            //
            this.groupBoxConfig.Controls.Add(this.btnSave);
            this.groupBoxConfig.Controls.Add(this.groupBoxSettings);
            this.groupBoxConfig.Controls.Add(this.groupBoxVpp);
            this.groupBoxConfig.Controls.Add(this.groupBoxInfo);
            this.groupBoxConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxConfig.Location = new System.Drawing.Point(0, 0);
            this.groupBoxConfig.Name = "groupBoxConfig";
            this.groupBoxConfig.Size = new System.Drawing.Size(616, 550);
            this.groupBoxConfig.TabIndex = 0;
            this.groupBoxConfig.TabStop = false;
            this.groupBoxConfig.Text = "版型配置";
            //
            // btnSave
            //
            this.btnSave.Location = new System.Drawing.Point(510, 510);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 30);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "保存配置";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            //
            // groupBoxSettings
            //
            this.groupBoxSettings.Controls.Add(this.cboImageFormat);
            this.groupBoxSettings.Controls.Add(this.lblImageFormat);
            this.groupBoxSettings.Controls.Add(this.chkSaveNgImage);
            this.groupBoxSettings.Controls.Add(this.chkSaveOkImage);
            this.groupBoxSettings.Controls.Add(this.numDecimalPlaces);
            this.groupBoxSettings.Controls.Add(this.lblDecimalPlaces);
            this.groupBoxSettings.Controls.Add(this.txtResultFormat);
            this.groupBoxSettings.Controls.Add(this.lblResultFormat);
            this.groupBoxSettings.Location = new System.Drawing.Point(15, 330);
            this.groupBoxSettings.Name = "groupBoxSettings";
            this.groupBoxSettings.Size = new System.Drawing.Size(585, 170);
            this.groupBoxSettings.TabIndex = 2;
            this.groupBoxSettings.TabStop = false;
            this.groupBoxSettings.Text = "输出设置";
            //
            // cboImageFormat
            //
            this.cboImageFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboImageFormat.FormattingEnabled = true;
            this.cboImageFormat.Items.AddRange(new object[] {
            "jpg",
            "bmp",
            "png",
            "tiff"});
            this.cboImageFormat.Location = new System.Drawing.Point(120, 130);
            this.cboImageFormat.Name = "cboImageFormat";
            this.cboImageFormat.Size = new System.Drawing.Size(100, 20);
            this.cboImageFormat.TabIndex = 7;
            //
            // lblImageFormat
            //
            this.lblImageFormat.AutoSize = true;
            this.lblImageFormat.Location = new System.Drawing.Point(20, 134);
            this.lblImageFormat.Name = "lblImageFormat";
            this.lblImageFormat.Size = new System.Drawing.Size(77, 12);
            this.lblImageFormat.TabIndex = 6;
            this.lblImageFormat.Text = "图像保存格式:";
            //
            // chkSaveNgImage
            //
            this.chkSaveNgImage.AutoSize = true;
            this.chkSaveNgImage.Checked = true;
            this.chkSaveNgImage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSaveNgImage.Location = new System.Drawing.Point(150, 100);
            this.chkSaveNgImage.Name = "chkSaveNgImage";
            this.chkSaveNgImage.Size = new System.Drawing.Size(90, 16);
            this.chkSaveNgImage.TabIndex = 5;
            this.chkSaveNgImage.Text = "保存NG图像";
            this.chkSaveNgImage.UseVisualStyleBackColor = true;
            //
            // chkSaveOkImage
            //
            this.chkSaveOkImage.AutoSize = true;
            this.chkSaveOkImage.Checked = true;
            this.chkSaveOkImage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSaveOkImage.Location = new System.Drawing.Point(22, 100);
            this.chkSaveOkImage.Name = "chkSaveOkImage";
            this.chkSaveOkImage.Size = new System.Drawing.Size(90, 16);
            this.chkSaveOkImage.TabIndex = 4;
            this.chkSaveOkImage.Text = "保存OK图像";
            this.chkSaveOkImage.UseVisualStyleBackColor = true;
            //
            // numDecimalPlaces
            //
            this.numDecimalPlaces.Location = new System.Drawing.Point(120, 62);
            this.numDecimalPlaces.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.numDecimalPlaces.Name = "numDecimalPlaces";
            this.numDecimalPlaces.Size = new System.Drawing.Size(80, 21);
            this.numDecimalPlaces.TabIndex = 3;
            this.numDecimalPlaces.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            //
            // lblDecimalPlaces
            //
            this.lblDecimalPlaces.AutoSize = true;
            this.lblDecimalPlaces.Location = new System.Drawing.Point(20, 66);
            this.lblDecimalPlaces.Name = "lblDecimalPlaces";
            this.lblDecimalPlaces.Size = new System.Drawing.Size(53, 12);
            this.lblDecimalPlaces.TabIndex = 2;
            this.lblDecimalPlaces.Text = "小数位数:";
            //
            // txtResultFormat
            //
            this.txtResultFormat.Location = new System.Drawing.Point(120, 28);
            this.txtResultFormat.Name = "txtResultFormat";
            this.txtResultFormat.Size = new System.Drawing.Size(440, 21);
            this.txtResultFormat.TabIndex = 1;
            this.txtResultFormat.Text = "OK,{X},{Y},{R}";
            //
            // lblResultFormat
            //
            this.lblResultFormat.AutoSize = true;
            this.lblResultFormat.Location = new System.Drawing.Point(20, 32);
            this.lblResultFormat.Name = "lblResultFormat";
            this.lblResultFormat.Size = new System.Drawing.Size(77, 12);
            this.lblResultFormat.TabIndex = 0;
            this.lblResultFormat.Text = "输出结果格式:";
            //
            // groupBoxVpp
            //
            this.groupBoxVpp.Controls.Add(this.btnBrowseVpp);
            this.groupBoxVpp.Controls.Add(this.txtVppPath);
            this.groupBoxVpp.Controls.Add(this.lblVppPath);
            this.groupBoxVpp.Location = new System.Drawing.Point(15, 240);
            this.groupBoxVpp.Name = "groupBoxVpp";
            this.groupBoxVpp.Size = new System.Drawing.Size(585, 80);
            this.groupBoxVpp.TabIndex = 1;
            this.groupBoxVpp.TabStop = false;
            this.groupBoxVpp.Text = "VisionPro配置";
            //
            // btnBrowseVpp
            //
            this.btnBrowseVpp.Location = new System.Drawing.Point(520, 35);
            this.btnBrowseVpp.Name = "btnBrowseVpp";
            this.btnBrowseVpp.Size = new System.Drawing.Size(40, 25);
            this.btnBrowseVpp.TabIndex = 2;
            this.btnBrowseVpp.Text = "...";
            this.btnBrowseVpp.UseVisualStyleBackColor = true;
            this.btnBrowseVpp.Click += new System.EventHandler(this.btnBrowseVpp_Click);
            //
            // txtVppPath
            //
            this.txtVppPath.Location = new System.Drawing.Point(120, 37);
            this.txtVppPath.Name = "txtVppPath";
            this.txtVppPath.Size = new System.Drawing.Size(390, 21);
            this.txtVppPath.TabIndex = 1;
            //
            // lblVppPath
            //
            this.lblVppPath.AutoSize = true;
            this.lblVppPath.Location = new System.Drawing.Point(20, 41);
            this.lblVppPath.Name = "lblVppPath";
            this.lblVppPath.Size = new System.Drawing.Size(77, 12);
            this.lblVppPath.TabIndex = 0;
            this.lblVppPath.Text = "VPP文件路径:";
            //
            // groupBoxInfo
            //
            this.groupBoxInfo.Controls.Add(this.lblModifyTimeValue);
            this.groupBoxInfo.Controls.Add(this.lblModifyTime);
            this.groupBoxInfo.Controls.Add(this.lblCreateTimeValue);
            this.groupBoxInfo.Controls.Add(this.lblCreateTime);
            this.groupBoxInfo.Controls.Add(this.lblCreatorValue);
            this.groupBoxInfo.Controls.Add(this.lblCreator);
            this.groupBoxInfo.Controls.Add(this.txtDescription);
            this.groupBoxInfo.Controls.Add(this.lblDescription);
            this.groupBoxInfo.Controls.Add(this.txtModelName);
            this.groupBoxInfo.Controls.Add(this.lblModelName);
            this.groupBoxInfo.Location = new System.Drawing.Point(15, 25);
            this.groupBoxInfo.Name = "groupBoxInfo";
            this.groupBoxInfo.Size = new System.Drawing.Size(585, 205);
            this.groupBoxInfo.TabIndex = 0;
            this.groupBoxInfo.TabStop = false;
            this.groupBoxInfo.Text = "基本信息";
            //
            // lblModifyTimeValue
            //
            this.lblModifyTimeValue.AutoSize = true;
            this.lblModifyTimeValue.Location = new System.Drawing.Point(380, 175);
            this.lblModifyTimeValue.Name = "lblModifyTimeValue";
            this.lblModifyTimeValue.Size = new System.Drawing.Size(11, 12);
            this.lblModifyTimeValue.TabIndex = 9;
            this.lblModifyTimeValue.Text = "-";
            //
            // lblModifyTime
            //
            this.lblModifyTime.AutoSize = true;
            this.lblModifyTime.Location = new System.Drawing.Point(300, 175);
            this.lblModifyTime.Name = "lblModifyTime";
            this.lblModifyTime.Size = new System.Drawing.Size(59, 12);
            this.lblModifyTime.TabIndex = 8;
            this.lblModifyTime.Text = "修改时间:";
            //
            // lblCreateTimeValue
            //
            this.lblCreateTimeValue.AutoSize = true;
            this.lblCreateTimeValue.Location = new System.Drawing.Point(90, 175);
            this.lblCreateTimeValue.Name = "lblCreateTimeValue";
            this.lblCreateTimeValue.Size = new System.Drawing.Size(11, 12);
            this.lblCreateTimeValue.TabIndex = 7;
            this.lblCreateTimeValue.Text = "-";
            //
            // lblCreateTime
            //
            this.lblCreateTime.AutoSize = true;
            this.lblCreateTime.Location = new System.Drawing.Point(20, 175);
            this.lblCreateTime.Name = "lblCreateTime";
            this.lblCreateTime.Size = new System.Drawing.Size(59, 12);
            this.lblCreateTime.TabIndex = 6;
            this.lblCreateTime.Text = "创建时间:";
            //
            // lblCreatorValue
            //
            this.lblCreatorValue.AutoSize = true;
            this.lblCreatorValue.Location = new System.Drawing.Point(380, 30);
            this.lblCreatorValue.Name = "lblCreatorValue";
            this.lblCreatorValue.Size = new System.Drawing.Size(11, 12);
            this.lblCreatorValue.TabIndex = 5;
            this.lblCreatorValue.Text = "-";
            //
            // lblCreator
            //
            this.lblCreator.AutoSize = true;
            this.lblCreator.Location = new System.Drawing.Point(300, 30);
            this.lblCreator.Name = "lblCreator";
            this.lblCreator.Size = new System.Drawing.Size(47, 12);
            this.lblCreator.TabIndex = 4;
            this.lblCreator.Text = "创建人:";
            //
            // txtDescription
            //
            this.txtDescription.Location = new System.Drawing.Point(90, 60);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(470, 100);
            this.txtDescription.TabIndex = 3;
            //
            // lblDescription
            //
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(20, 63);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(35, 12);
            this.lblDescription.TabIndex = 2;
            this.lblDescription.Text = "描述:";
            //
            // txtModelName
            //
            this.txtModelName.Location = new System.Drawing.Point(90, 26);
            this.txtModelName.Name = "txtModelName";
            this.txtModelName.ReadOnly = true;
            this.txtModelName.Size = new System.Drawing.Size(180, 21);
            this.txtModelName.TabIndex = 1;
            //
            // lblModelName
            //
            this.lblModelName.AutoSize = true;
            this.lblModelName.Location = new System.Drawing.Point(20, 30);
            this.lblModelName.Name = "lblModelName";
            this.lblModelName.Size = new System.Drawing.Size(59, 12);
            this.lblModelName.TabIndex = 0;
            this.lblModelName.Text = "版型名称:";
            //
            // panelBottom
            //
            this.panelBottom.Controls.Add(this.btnClose);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 550);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(900, 50);
            this.panelBottom.TabIndex = 1;
            //
            // btnClose
            //
            this.btnClose.Location = new System.Drawing.Point(795, 12);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 28);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            //
            // ModelManageForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panelBottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ModelManageForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "版型管理";
            this.Load += new System.EventHandler(this.ModelManageForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBoxModels.ResumeLayout(false);
            this.panelModelButtons.ResumeLayout(false);
            this.groupBoxConfig.ResumeLayout(false);
            this.groupBoxSettings.ResumeLayout(false);
            this.groupBoxSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDecimalPlaces)).EndInit();
            this.groupBoxVpp.ResumeLayout(false);
            this.groupBoxVpp.PerformLayout();
            this.groupBoxInfo.ResumeLayout(false);
            this.groupBoxInfo.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBoxModels;
        private System.Windows.Forms.ListBox lstModels;
        private System.Windows.Forms.Panel panelModelButtons;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnRename;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.GroupBox groupBoxConfig;
        private System.Windows.Forms.GroupBox groupBoxInfo;
        private System.Windows.Forms.TextBox txtModelName;
        private System.Windows.Forms.Label lblModelName;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblCreatorValue;
        private System.Windows.Forms.Label lblCreator;
        private System.Windows.Forms.Label lblModifyTimeValue;
        private System.Windows.Forms.Label lblModifyTime;
        private System.Windows.Forms.Label lblCreateTimeValue;
        private System.Windows.Forms.Label lblCreateTime;
        private System.Windows.Forms.GroupBox groupBoxVpp;
        private System.Windows.Forms.Button btnBrowseVpp;
        private System.Windows.Forms.TextBox txtVppPath;
        private System.Windows.Forms.Label lblVppPath;
        private System.Windows.Forms.GroupBox groupBoxSettings;
        private System.Windows.Forms.ComboBox cboImageFormat;
        private System.Windows.Forms.Label lblImageFormat;
        private System.Windows.Forms.CheckBox chkSaveNgImage;
        private System.Windows.Forms.CheckBox chkSaveOkImage;
        private System.Windows.Forms.NumericUpDown numDecimalPlaces;
        private System.Windows.Forms.Label lblDecimalPlaces;
        private System.Windows.Forms.TextBox txtResultFormat;
        private System.Windows.Forms.Label lblResultFormat;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Button btnClose;
    }
}
