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
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Label lblLayerInfo;

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
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.lblLayerInfo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBlocks)).BeginInit();
            this.pnlHeader.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.pnlHeader.Controls.Add(this.lblHint);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(620, 60);
            this.pnlHeader.TabIndex = 13;
            // 
            // lblHint
            // 
            this.lblHint.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblHint.AutoSize = true;
            this.lblHint.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblHint.ForeColor = System.Drawing.Color.White;
            this.lblHint.Location = new System.Drawing.Point(20, 20);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(361, 20);
            this.lblHint.TabIndex = 0;
            this.lblHint.Text = "請選擇要組合的檔案名稱分段（以底線分隔）";
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.pnlMain.Controls.Add(this.lblFileName);
            this.pnlMain.Controls.Add(this.flowSegments);
            this.pnlMain.Controls.Add(this.lblPreview);
            this.pnlMain.Controls.Add(this.txtPreview);
            this.pnlMain.Controls.Add(this.lblExcelName);
            this.pnlMain.Controls.Add(this.txtExcelName);
            this.pnlMain.Controls.Add(this.lblLayerInfo);
            this.pnlMain.Controls.Add(this.dgvBlocks);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 60);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(20);
            this.pnlMain.Size = new System.Drawing.Size(620, 490);
            this.pnlMain.TabIndex = 14;
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblFileName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(80)))), ((int)(((byte)(87)))));
            this.lblFileName.Location = new System.Drawing.Point(20, 20);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(108, 17);
            this.lblFileName.TabIndex = 1;
            this.lblFileName.Text = "範例檔案名稱：";
            // 
            // flowSegments
            // 
            this.flowSegments.BackColor = System.Drawing.Color.White;
            this.flowSegments.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowSegments.Location = new System.Drawing.Point(20, 50);
            this.flowSegments.Name = "flowSegments";
            this.flowSegments.Padding = new System.Windows.Forms.Padding(10);
            this.flowSegments.Size = new System.Drawing.Size(560, 50);
            this.flowSegments.TabIndex = 2;
            // 
            // lblPreview
            // 
            this.lblPreview.AutoSize = true;
            this.lblPreview.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblPreview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(80)))), ((int)(((byte)(87)))));
            this.lblPreview.Location = new System.Drawing.Point(20, 120);
            this.lblPreview.Name = "lblPreview";
            this.lblPreview.Size = new System.Drawing.Size(77, 17);
            this.lblPreview.TabIndex = 3;
            this.lblPreview.Text = "組合預覽：";
            // 
            // txtPreview
            // 
            this.txtPreview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.txtPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPreview.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtPreview.Location = new System.Drawing.Point(110, 118);
            this.txtPreview.Name = "txtPreview";
            this.txtPreview.ReadOnly = true;
            this.txtPreview.Size = new System.Drawing.Size(470, 25);
            this.txtPreview.TabIndex = 4;
            // 
            // lblExcelName
            // 
            this.lblExcelName.AutoSize = true;
            this.lblExcelName.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblExcelName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(80)))), ((int)(((byte)(87)))));
            this.lblExcelName.Location = new System.Drawing.Point(20, 160);
            this.lblExcelName.Name = "lblExcelName";
            this.lblExcelName.Size = new System.Drawing.Size(113, 17);
            this.lblExcelName.TabIndex = 7;
            this.lblExcelName.Text = "Excel檔案名稱：";
            // 
            // txtExcelName
            // 
            this.txtExcelName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtExcelName.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtExcelName.Location = new System.Drawing.Point(140, 158);
            this.txtExcelName.Name = "txtExcelName";
            this.txtExcelName.Size = new System.Drawing.Size(440, 25);
            this.txtExcelName.TabIndex = 8;
            // 
            // lblLayerInfo
            // 
            this.lblLayerInfo.AutoSize = true;
            this.lblLayerInfo.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.lblLayerInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.lblLayerInfo.Location = new System.Drawing.Point(20, 200);
            this.lblLayerInfo.Name = "lblLayerInfo";
            this.lblLayerInfo.Size = new System.Drawing.Size(410, 16);
            this.lblLayerInfo.TabIndex = 12;
            this.lblLayerInfo.Text = "提示：同一圖塊在不同圖層會分別顯示和統計，圖層資訊會自動包含在輸出中";
            // 
            // dgvBlocks
            // 
            this.dgvBlocks.BackgroundColor = System.Drawing.Color.White;
            this.dgvBlocks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dgvBlocks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBlocks.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(226)))), ((int)(((byte)(230)))));
            this.dgvBlocks.Location = new System.Drawing.Point(20, 230);
            this.dgvBlocks.Name = "dgvBlocks";
            this.dgvBlocks.RowTemplate.Height = 25;
            this.dgvBlocks.Size = new System.Drawing.Size(560, 240);
            this.dgvBlocks.TabIndex = 9;
            // 
            // pnlButtons
            // 
            this.pnlButtons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.pnlButtons.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlButtons.Controls.Add(this.btnAddBlock);
            this.pnlButtons.Controls.Add(this.btnImportBlockMap);
            this.pnlButtons.Controls.Add(this.btnOK);
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(0, 550);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(620, 70);
            this.pnlButtons.TabIndex = 15;
            // 
            // btnAddBlock
            // 
            this.btnAddBlock.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnAddBlock.FlatAppearance.BorderSize = 0;
            this.btnAddBlock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddBlock.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnAddBlock.ForeColor = System.Drawing.Color.White;
            this.btnAddBlock.Location = new System.Drawing.Point(20, 20);
            this.btnAddBlock.Name = "btnAddBlock";
            this.btnAddBlock.Size = new System.Drawing.Size(120, 35);
            this.btnAddBlock.TabIndex = 10;
            this.btnAddBlock.Text = "新增圖塊列";
            this.btnAddBlock.UseVisualStyleBackColor = false;
            this.btnAddBlock.Click += new System.EventHandler(this.btnAddBlock_Click);
            // 
            // btnImportBlockMap
            // 
            this.btnImportBlockMap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.btnImportBlockMap.FlatAppearance.BorderSize = 0;
            this.btnImportBlockMap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportBlockMap.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnImportBlockMap.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.btnImportBlockMap.Location = new System.Drawing.Point(160, 20);
            this.btnImportBlockMap.Name = "btnImportBlockMap";
            this.btnImportBlockMap.Size = new System.Drawing.Size(120, 35);
            this.btnImportBlockMap.TabIndex = 11;
            this.btnImportBlockMap.Text = "匯入圖塊對應";
            this.btnImportBlockMap.UseVisualStyleBackColor = false;
            this.btnImportBlockMap.Click += new System.EventHandler(this.btnImportBlockMap_Click);
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnOK.FlatAppearance.BorderSize = 0;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Location = new System.Drawing.Point(380, 20);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 35);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "確定";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(500, 20);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // NameFormatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(620, 620);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NameFormatForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "檔案名稱格式設定";
            ((System.ComponentModel.ISupportInitialize)(this.dgvBlocks)).EndInit();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion
    }
}
