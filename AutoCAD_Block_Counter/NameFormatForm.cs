using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AutoCAD_Block_Counter
{
    public class BlockMapping
    {
        public string BlockName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
    }

    public partial class NameFormatForm : Form
    {
        private string[] _segments;
        private List<CheckBox> _checkBoxes = new();
        public List<int> SelectedIndexes { get; private set; } = new();
        public bool IsConfirmed { get; private set; } = false;
        public string ExcelFileName => txtExcelName.Text.Trim();
        public List<BlockMapping> BlockMappings { get; private set; } = new();

        public NameFormatForm(string firstFileName)
        {
            InitializeComponent();
            ApplyAdditionalStyling();
            ShowSegments(firstFileName);
            InitBlockGrid();
        }

        private void ApplyAdditionalStyling()
        {
            // 設定 DataGridView 樣式
            dgvBlocks.EnableHeadersVisualStyles = false;
            dgvBlocks.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 152, 219);
            dgvBlocks.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvBlocks.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft JhengHei UI", 9.75F, FontStyle.Bold);
            dgvBlocks.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
            dgvBlocks.DefaultCellStyle.SelectionBackColor = Color.FromArgb(173, 216, 230);
            dgvBlocks.DefaultCellStyle.SelectionForeColor = Color.FromArgb(33, 37, 41);
            dgvBlocks.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
            
            // 設定表單圖示（如果需要的話）
            this.BackColor = Color.FromArgb(248, 249, 250);
        }

        private void ShowSegments(string fileName)
        {
            lblFileName.Text = $"範例檔案名稱：{fileName}";
            string nameWithoutExt = System.IO.Path.GetFileNameWithoutExtension(fileName);
            _segments = nameWithoutExt.Split('_');
            flowSegments.Controls.Clear();
            _checkBoxes.Clear();
            
            for (int i = 0; i < _segments.Length; i++)
            {
                var cb = new CheckBox
                {
                    Text = $"第{i + 1}段：{_segments[i]}",
                    Tag = i,
                    AutoSize = true,
                    Font = new Font("Microsoft JhengHei UI", 9.75F, FontStyle.Regular),
                    ForeColor = Color.FromArgb(73, 80, 87),
                    Margin = new Padding(5, 3, 15, 3),
                    BackColor = Color.Transparent
                };
                cb.CheckedChanged += (s, e) => UpdatePreview();
                flowSegments.Controls.Add(cb);
                _checkBoxes.Add(cb);
            }
        }

        private void UpdatePreview()
        {
            var selected = _checkBoxes
                .Where(cb => cb.Checked)
                .Select(cb => _segments[(int)cb.Tag]);
            txtPreview.Text = string.Join("_", selected);
        }

        private void InitBlockGrid()
        {
            dgvBlocks.Columns.Clear();
            dgvBlocks.Rows.Clear();
            dgvBlocks.AllowUserToAddRows = false;
            dgvBlocks.AllowUserToDeleteRows = false;
            dgvBlocks.RowHeadersVisible = false;
            dgvBlocks.ColumnCount = 2;
            dgvBlocks.Columns[0].HeaderText = "原始圖塊名稱";
            dgvBlocks.Columns[1].HeaderText = "Excel顯示名稱";
            dgvBlocks.Columns[0].Width = 200;
            dgvBlocks.Columns[1].Width = 340;
        }

        private void btnAddBlock_Click(object sender, EventArgs e)
        {
            dgvBlocks.Rows.Add();
        }

        private void btnImportBlockMap_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Excel 檔案 (*.xlsx)|*.xlsx";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var imported = new List<BlockMapping>();
                        using (var wb = new XLWorkbook(ofd.FileName))
                        {
                            var ws = wb.Worksheets.First();
                            int row = 1;
                            while (true)
                            {
                                var block = ws.Cell(row, 1).GetString();
                                var display = ws.Cell(row, 2).GetString();
                                if (string.IsNullOrWhiteSpace(block) && string.IsNullOrWhiteSpace(display))
                                    break;
                                if (!string.IsNullOrWhiteSpace(block) && !string.IsNullOrWhiteSpace(display))
                                    imported.Add(new BlockMapping { BlockName = block, DisplayName = display });
                                row++;
                            }
                        }
                        // 先清空現有表格
                        dgvBlocks.Rows.Clear();
                        // 先加載有定義的
                        foreach (var map in imported)
                        {
                            dgvBlocks.Rows.Add(map.BlockName, map.DisplayName);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"匯入失敗: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SelectedIndexes = _checkBoxes
                .Select((cb, idx) => (cb, idx))
                .Where(x => x.cb.Checked)
                .Select(x => x.idx)
                .ToList();
            BlockMappings = new List<BlockMapping>();
            foreach (DataGridViewRow row in dgvBlocks.Rows)
            {
                if (row.IsNewRow) continue;
                string block = row.Cells[0].Value?.ToString()?.Trim() ?? "";
                string display = row.Cells[1].Value?.ToString()?.Trim() ?? "";
                if (!string.IsNullOrWhiteSpace(block) && !string.IsNullOrWhiteSpace(display))
                    BlockMappings.Add(new BlockMapping { BlockName = block, DisplayName = display });
            }
            IsConfirmed = SelectedIndexes.Count > 0 && !string.IsNullOrWhiteSpace(ExcelFileName);
            DialogResult = IsConfirmed ? DialogResult.OK : DialogResult.None;
            if (IsConfirmed && BlockMappings.Count == 0)
            {
                MessageBox.Show("請至少設定一個圖塊對應。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                IsConfirmed = false;
                DialogResult = DialogResult.None;
            }
            if (IsConfirmed) Close();
            else if (!IsConfirmed) MessageBox.Show("請選擇至少一個分段並輸入Excel檔案名稱。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsConfirmed = false;
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
