namespace AutoCAD_Block_Counter
{
    partial class NameFormatForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblHint;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.FlowLayoutPanel flowSegments;
        private System.Windows.Forms.TextBox txtPreview;
        private System.Windows.Forms.Label lblPreview;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblExcelName;
        private System.Windows.Forms.TextBox txtExcelName;
        private System.Windows.Forms.DataGridView dgvBlocks;
        private System.Windows.Forms.Button btnAddBlock;
        private System.Windows.Forms.Button btnImportBlockMap;
        private System.Windows.Forms.CheckBox chkShowLayer;

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
            this.lblHint = new System.Windows.Forms.Label();
            this.lblFileName = new System.Windows.Forms.Label();
            this.flowSegments = new System.Windows.Forms.FlowLayoutPanel();
            this.lblPreview = new System.Windows.Forms.Label();
            this.txtPreview = new System.Windows.Forms.TextBox();
            this.lblExcelName = new System.Windows.Forms.Label();
            this.txtExcelName = new System.Windows.Forms.TextBox();
            this.dgvBlocks = new System.Windows.Forms.DataGridView();
            this.btnAddBlock = new System.Windows.Forms.Button();
            this.btnImportBlockMap = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkShowLayer = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBlocks)).BeginInit();
            this.SuspendLayout();
            // 
            // lblHint
            // 
            this.lblHint.AutoSize = true;
            this.lblHint.Location = new System.Drawing.Point(20, 20);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(320, 15);
            this.lblHint.TabIndex = 0;
            this.lblHint.Text = "請選擇要組合的檔案名稱分段（以底線分隔）";
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.Location = new System.Drawing.Point(20, 50);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(92, 15);
            this.lblFileName.TabIndex = 1;
            this.lblFileName.Text = "範例檔案名稱：";
            // 
            // flowSegments
            // 
            this.flowSegments.Location = new System.Drawing.Point(20, 80);
            this.flowSegments.Name = "flowSegments";
            this.flowSegments.Size = new System.Drawing.Size(540, 40);
            this.flowSegments.TabIndex = 2;
            // 
            // lblPreview
            // 
            this.lblPreview.AutoSize = true;
            this.lblPreview.Location = new System.Drawing.Point(20, 130);
            this.lblPreview.Name = "lblPreview";
            this.lblPreview.Size = new System.Drawing.Size(68, 15);
            this.lblPreview.TabIndex = 3;
            this.lblPreview.Text = "組合預覽：";
            // 
            // txtPreview
            // 
            this.txtPreview.Location = new System.Drawing.Point(100, 127);
            this.txtPreview.Name = "txtPreview";
            this.txtPreview.ReadOnly = true;
            this.txtPreview.Size = new System.Drawing.Size(460, 23);
            this.txtPreview.TabIndex = 4;
            // 
            // lblExcelName
            // 
            this.lblExcelName.AutoSize = true;
            this.lblExcelName.Location = new System.Drawing.Point(20, 170);
            this.lblExcelName.Name = "lblExcelName";
            this.lblExcelName.Size = new System.Drawing.Size(97, 15);
            this.lblExcelName.TabIndex = 7;
            this.lblExcelName.Text = "Excel檔案名稱：";
            // 
            // txtExcelName
            // 
            this.txtExcelName.Location = new System.Drawing.Point(120, 167);
            this.txtExcelName.Name = "txtExcelName";
            this.txtExcelName.Size = new System.Drawing.Size(440, 23);
            this.txtExcelName.TabIndex = 8;
            // 
            // chkShowLayer
            // 
            this.chkShowLayer.AutoSize = true;
            this.chkShowLayer.Location = new System.Drawing.Point(20, 205);
            this.chkShowLayer.Name = "chkShowLayer";
            this.chkShowLayer.Size = new System.Drawing.Size(180, 19);
            this.chkShowLayer.TabIndex = 12;
            this.chkShowLayer.Text = "在Excel中顯示圖塊圖層名稱";
            this.chkShowLayer.UseVisualStyleBackColor = true;
            // 
            // dgvBlocks
            // 
            this.dgvBlocks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBlocks.Location = new System.Drawing.Point(20, 240);
            this.dgvBlocks.Name = "dgvBlocks";
            this.dgvBlocks.RowTemplate.Height = 25;
            this.dgvBlocks.Size = new System.Drawing.Size(540, 260);
            this.dgvBlocks.TabIndex = 9;
            // 
            // btnAddBlock
            // 
            this.btnAddBlock.Location = new System.Drawing.Point(20, 520);
            this.btnAddBlock.Name = "btnAddBlock";
            this.btnAddBlock.Size = new System.Drawing.Size(120, 35);
            this.btnAddBlock.TabIndex = 10;
            this.btnAddBlock.Text = "新增圖塊列";
            this.btnAddBlock.UseVisualStyleBackColor = true;
            this.btnAddBlock.Click += new System.EventHandler(this.btnAddBlock_Click);
            // 
            // btnImportBlockMap
            // 
            this.btnImportBlockMap.Location = new System.Drawing.Point(160, 520);
            this.btnImportBlockMap.Name = "btnImportBlockMap";
            this.btnImportBlockMap.Size = new System.Drawing.Size(120, 35);
            this.btnImportBlockMap.TabIndex = 11;
            this.btnImportBlockMap.Text = "匯入圖塊對應";
            this.btnImportBlockMap.UseVisualStyleBackColor = true;
            this.btnImportBlockMap.Click += new System.EventHandler(this.btnImportBlockMap_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(320, 520);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 35);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "確定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(460, 520);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // NameFormatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 600);
            this.Controls.Add(this.btnImportBlockMap);
            this.Controls.Add(this.btnAddBlock);
            this.Controls.Add(this.dgvBlocks);
            this.Controls.Add(this.txtExcelName);
            this.Controls.Add(this.lblExcelName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtPreview);
            this.Controls.Add(this.lblPreview);
            this.Controls.Add(this.flowSegments);
            this.Controls.Add(this.lblFileName);
            this.Controls.Add(this.lblHint);
            this.Controls.Add(this.chkShowLayer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NameFormatForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "檔案名稱格式設定";
            ((System.ComponentModel.ISupportInitialize)(this.dgvBlocks)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
