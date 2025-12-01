namespace VisionInspectionSystem.Forms
{
    partial class OfflineTestForm
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
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cogRecordDisplay1 = new Cognex.VisionPro.CogRecordDisplay();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cogRecordDisplay2 = new Cognex.VisionPro.CogRecordDisplay();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dgvOutputs = new System.Windows.Forms.DataGridView();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgvInputs = new System.Windows.Forms.DataGridView();
            this.colInputName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInputType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInputValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.txtOutputs = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClear = new System.Windows.Forms.Button();
            this.lblRunTime = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblResult = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnLoadImage = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnLoadVpp = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnPrevImage = new System.Windows.Forms.Button();
            this.btnNextImage = new System.Windows.Forms.Button();
            this.lblImageIndex = new System.Windows.Forms.Label();
            this.lblVppPathDisplay = new System.Windows.Forms.Label();
            this.lblImagePathDisplay = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cboCurrentRecipe = new System.Windows.Forms.ComboBox();
            this.btnSaveRecipe = new System.Windows.Forms.Button();
            this.btnManageRecipe = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cogRecordDisplay1)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cogRecordDisplay2)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOutputs)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInputs)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            //
            // splitContainer1
            //
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 130);
            this.splitContainer1.Name = "splitContainer1";
            //
            // splitContainer1.Panel1
            //
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            //
            // splitContainer1.Panel2
            //
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(1200, 520);
            this.splitContainer1.SplitterDistance = 800;
            this.splitContainer1.TabIndex = 0;
            //
            // splitContainer2
            //
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            //
            // splitContainer2.Panel1
            //
            this.splitContainer2.Panel1.Controls.Add(this.groupBox1);
            //
            // splitContainer2.Panel2
            //
            this.splitContainer2.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer2.Size = new System.Drawing.Size(800, 520);
            this.splitContainer2.SplitterDistance = 395;
            this.splitContainer2.TabIndex = 0;
            //
            // groupBox1
            //
            this.groupBox1.Controls.Add(this.cogRecordDisplay1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(395, 520);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "原始图像";
            //
            // cogRecordDisplay1
            //
            this.cogRecordDisplay1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cogRecordDisplay1.Location = new System.Drawing.Point(3, 17);
            this.cogRecordDisplay1.Name = "cogRecordDisplay1";
            this.cogRecordDisplay1.Size = new System.Drawing.Size(389, 500);
            this.cogRecordDisplay1.TabIndex = 0;
            //
            // groupBox2
            //
            this.groupBox2.Controls.Add(this.cogRecordDisplay2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(401, 520);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "结果图像";
            //
            // cogRecordDisplay2
            //
            this.cogRecordDisplay2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cogRecordDisplay2.Location = new System.Drawing.Point(3, 17);
            this.cogRecordDisplay2.Name = "cogRecordDisplay2";
            this.cogRecordDisplay2.Size = new System.Drawing.Size(395, 500);
            this.cogRecordDisplay2.TabIndex = 0;
            //
            // tabControl1
            //
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(396, 520);
            this.tabControl1.TabIndex = 0;
            //
            // tabPage1
            //
            this.tabPage1.Controls.Add(this.dgvOutputs);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(388, 494);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "输出参数";
            this.tabPage1.UseVisualStyleBackColor = true;
            //
            // dgvOutputs
            //
            this.dgvOutputs.AllowUserToAddRows = false;
            this.dgvOutputs.AllowUserToDeleteRows = false;
            this.dgvOutputs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvOutputs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOutputs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colType,
            this.colValue});
            this.dgvOutputs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOutputs.Location = new System.Drawing.Point(3, 3);
            this.dgvOutputs.Name = "dgvOutputs";
            this.dgvOutputs.ReadOnly = true;
            this.dgvOutputs.RowHeadersVisible = false;
            this.dgvOutputs.RowTemplate.Height = 23;
            this.dgvOutputs.Size = new System.Drawing.Size(382, 488);
            this.dgvOutputs.TabIndex = 0;
            //
            // colName
            //
            this.colName.FillWeight = 120F;
            this.colName.HeaderText = "参数名称";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            //
            // colType
            //
            this.colType.FillWeight = 80F;
            this.colType.HeaderText = "类型";
            this.colType.Name = "colType";
            this.colType.ReadOnly = true;
            //
            // colValue
            //
            this.colValue.FillWeight = 120F;
            this.colValue.HeaderText = "值";
            this.colValue.Name = "colValue";
            this.colValue.ReadOnly = true;
            //
            // tabPage2
            //
            this.tabPage2.Controls.Add(this.dgvInputs);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(388, 494);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "输入参数";
            this.tabPage2.UseVisualStyleBackColor = true;
            //
            // dgvInputs
            //
            this.dgvInputs.AllowUserToAddRows = false;
            this.dgvInputs.AllowUserToDeleteRows = false;
            this.dgvInputs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvInputs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInputs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colInputName,
            this.colInputType,
            this.colInputValue});
            this.dgvInputs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvInputs.Location = new System.Drawing.Point(3, 3);
            this.dgvInputs.Name = "dgvInputs";
            this.dgvInputs.ReadOnly = true;
            this.dgvInputs.RowHeadersVisible = false;
            this.dgvInputs.RowTemplate.Height = 23;
            this.dgvInputs.Size = new System.Drawing.Size(382, 488);
            this.dgvInputs.TabIndex = 0;
            //
            // colInputName
            //
            this.colInputName.FillWeight = 120F;
            this.colInputName.HeaderText = "参数名称";
            this.colInputName.Name = "colInputName";
            this.colInputName.ReadOnly = true;
            //
            // colInputType
            //
            this.colInputType.FillWeight = 80F;
            this.colInputType.HeaderText = "类型";
            this.colInputType.Name = "colInputType";
            this.colInputType.ReadOnly = true;
            //
            // colInputValue
            //
            this.colInputValue.FillWeight = 120F;
            this.colInputValue.HeaderText = "值";
            this.colInputValue.Name = "colInputValue";
            this.colInputValue.ReadOnly = true;
            //
            // tabPage3
            //
            this.tabPage3.Controls.Add(this.txtOutputs);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(388, 494);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "日志";
            this.tabPage3.UseVisualStyleBackColor = true;
            //
            // txtOutputs
            //
            this.txtOutputs.BackColor = System.Drawing.Color.White;
            this.txtOutputs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOutputs.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOutputs.Location = new System.Drawing.Point(0, 0);
            this.txtOutputs.Multiline = true;
            this.txtOutputs.Name = "txtOutputs";
            this.txtOutputs.ReadOnly = true;
            this.txtOutputs.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtOutputs.Size = new System.Drawing.Size(388, 494);
            this.txtOutputs.TabIndex = 0;
            this.txtOutputs.WordWrap = false;
            //
            // panel1
            //
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.btnManageRecipe);
            this.panel1.Controls.Add(this.btnSaveRecipe);
            this.panel1.Controls.Add(this.cboCurrentRecipe);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.lblImagePathDisplay);
            this.panel1.Controls.Add(this.lblVppPathDisplay);
            this.panel1.Controls.Add(this.btnClear);
            this.panel1.Controls.Add(this.lblRunTime);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.lblResult);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.btnRun);
            this.panel1.Controls.Add(this.lblImageIndex);
            this.panel1.Controls.Add(this.btnNextImage);
            this.panel1.Controls.Add(this.btnPrevImage);
            this.panel1.Controls.Add(this.btnLoadImage);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btnLoadVpp);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1200, 130);
            this.panel1.TabIndex = 1;
            //
            // btnClear
            //
            this.btnClear.Location = new System.Drawing.Point(830, 90);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(80, 30);
            this.btnClear.TabIndex = 10;
            this.btnClear.Text = "清除";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            //
            // lblRunTime
            //
            this.lblRunTime.AutoSize = true;
            this.lblRunTime.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRunTime.Location = new System.Drawing.Point(1050, 60);
            this.lblRunTime.Name = "lblRunTime";
            this.lblRunTime.Size = new System.Drawing.Size(18, 22);
            this.lblRunTime.TabIndex = 8;
            this.lblRunTime.Text = "-";
            //
            // label5
            //
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(990, 65);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 7;
            this.label5.Text = "运行时间:";
            //
            // lblResult
            //
            this.lblResult.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResult.Location = new System.Drawing.Point(1050, 10);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(100, 45);
            this.lblResult.TabIndex = 6;
            this.lblResult.Text = "-";
            this.lblResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // label4
            //
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(990, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "检测结果:";
            //
            // btnRun
            //
            this.btnRun.BackColor = System.Drawing.Color.LimeGreen;
            this.btnRun.Enabled = false;
            this.btnRun.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRun.ForeColor = System.Drawing.Color.White;
            this.btnRun.Location = new System.Drawing.Point(650, 10);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(170, 70);
            this.btnRun.TabIndex = 4;
            this.btnRun.Text = "运行检测";
            this.btnRun.UseVisualStyleBackColor = false;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            //
            // btnLoadImage
            //
            this.btnLoadImage.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLoadImage.Location = new System.Drawing.Point(80, 52);
            this.btnLoadImage.Name = "btnLoadImage";
            this.btnLoadImage.Size = new System.Drawing.Size(32, 28);
            this.btnLoadImage.TabIndex = 2;
            this.btnLoadImage.Text = "...";
            this.btnLoadImage.UseVisualStyleBackColor = true;
            this.btnLoadImage.Click += new System.EventHandler(this.btnLoadImage_Click);
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "测试图像:";
            //
            // btnLoadVpp
            //
            this.btnLoadVpp.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLoadVpp.Location = new System.Drawing.Point(80, 12);
            this.btnLoadVpp.Name = "btnLoadVpp";
            this.btnLoadVpp.Size = new System.Drawing.Size(32, 28);
            this.btnLoadVpp.TabIndex = 0;
            this.btnLoadVpp.Text = "...";
            this.btnLoadVpp.UseVisualStyleBackColor = true;
            this.btnLoadVpp.Click += new System.EventHandler(this.btnLoadVpp_Click);
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "VPP文件:";
            //
            // lblVppPathDisplay
            //
            this.lblVppPathDisplay.AutoEllipsis = true;
            this.lblVppPathDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblVppPathDisplay.Location = new System.Drawing.Point(120, 12);
            this.lblVppPathDisplay.Name = "lblVppPathDisplay";
            this.lblVppPathDisplay.Size = new System.Drawing.Size(510, 28);
            this.lblVppPathDisplay.TabIndex = 14;
            this.lblVppPathDisplay.Text = "未加载";
            this.lblVppPathDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblImagePathDisplay
            //
            this.lblImagePathDisplay.AutoEllipsis = true;
            this.lblImagePathDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblImagePathDisplay.Location = new System.Drawing.Point(120, 52);
            this.lblImagePathDisplay.Name = "lblImagePathDisplay";
            this.lblImagePathDisplay.Size = new System.Drawing.Size(510, 28);
            this.lblImagePathDisplay.TabIndex = 15;
            this.lblImagePathDisplay.Text = "未加载";
            this.lblImagePathDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // btnPrevImage
            //
            this.btnPrevImage.Enabled = false;
            this.btnPrevImage.Location = new System.Drawing.Point(830, 52);
            this.btnPrevImage.Name = "btnPrevImage";
            this.btnPrevImage.Size = new System.Drawing.Size(40, 28);
            this.btnPrevImage.TabIndex = 11;
            this.btnPrevImage.Text = "<";
            this.btnPrevImage.UseVisualStyleBackColor = true;
            this.btnPrevImage.Click += new System.EventHandler(this.btnPrevImage_Click);
            //
            // btnNextImage
            //
            this.btnNextImage.Enabled = false;
            this.btnNextImage.Location = new System.Drawing.Point(875, 52);
            this.btnNextImage.Name = "btnNextImage";
            this.btnNextImage.Size = new System.Drawing.Size(40, 28);
            this.btnNextImage.TabIndex = 12;
            this.btnNextImage.Text = ">";
            this.btnNextImage.UseVisualStyleBackColor = true;
            this.btnNextImage.Click += new System.EventHandler(this.btnNextImage_Click);
            //
            // lblImageIndex
            //
            this.lblImageIndex.AutoSize = true;
            this.lblImageIndex.Location = new System.Drawing.Point(830, 20);
            this.lblImageIndex.Name = "lblImageIndex";
            this.lblImageIndex.Size = new System.Drawing.Size(77, 12);
            this.lblImageIndex.TabIndex = 13;
            this.lblImageIndex.Text = "第 0 / 0 张";
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 16;
            this.label3.Text = "当前版型:";
            //
            // cboCurrentRecipe
            //
            this.cboCurrentRecipe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCurrentRecipe.FormattingEnabled = true;
            this.cboCurrentRecipe.Location = new System.Drawing.Point(80, 96);
            this.cboCurrentRecipe.Name = "cboCurrentRecipe";
            this.cboCurrentRecipe.Size = new System.Drawing.Size(200, 20);
            this.cboCurrentRecipe.TabIndex = 17;
            this.cboCurrentRecipe.SelectedIndexChanged += new System.EventHandler(this.cboCurrentRecipe_SelectedIndexChanged);
            //
            // btnSaveRecipe
            //
            this.btnSaveRecipe.Location = new System.Drawing.Point(290, 94);
            this.btnSaveRecipe.Name = "btnSaveRecipe";
            this.btnSaveRecipe.Size = new System.Drawing.Size(80, 25);
            this.btnSaveRecipe.TabIndex = 18;
            this.btnSaveRecipe.Text = "保存版型";
            this.btnSaveRecipe.UseVisualStyleBackColor = true;
            this.btnSaveRecipe.Click += new System.EventHandler(this.btnSaveRecipe_Click);
            //
            // btnManageRecipe
            //
            this.btnManageRecipe.Location = new System.Drawing.Point(380, 94);
            this.btnManageRecipe.Name = "btnManageRecipe";
            this.btnManageRecipe.Size = new System.Drawing.Size(80, 25);
            this.btnManageRecipe.TabIndex = 19;
            this.btnManageRecipe.Text = "版型管理";
            this.btnManageRecipe.UseVisualStyleBackColor = true;
            this.btnManageRecipe.Click += new System.EventHandler(this.btnManageRecipe_Click);
            //
            // statusStrip1
            //
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 650);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1200, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            //
            // lblStatus
            //
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(32, 17);
            this.lblStatus.Text = "就绪";
            //
            // OfflineTestForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 672);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "OfflineTestForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VisionPro 离线测试";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OfflineTestForm_FormClosing);
            this.Load += new System.EventHandler(this.OfflineTestForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cogRecordDisplay1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cogRecordDisplay2)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOutputs)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvInputs)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox1;
        private Cognex.VisionPro.CogRecordDisplay cogRecordDisplay1;
        private System.Windows.Forms.GroupBox groupBox2;
        private Cognex.VisionPro.CogRecordDisplay cogRecordDisplay2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dgvOutputs;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dgvInputs;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox txtOutputs;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnLoadImage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnLoadVpp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblRunTime;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInputName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInputType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInputValue;
        private System.Windows.Forms.Button btnPrevImage;
        private System.Windows.Forms.Button btnNextImage;
        private System.Windows.Forms.Label lblImageIndex;
        private System.Windows.Forms.Label lblVppPathDisplay;
        private System.Windows.Forms.Label lblImagePathDisplay;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboCurrentRecipe;
        private System.Windows.Forms.Button btnSaveRecipe;
        private System.Windows.Forms.Button btnManageRecipe;
    }
}
