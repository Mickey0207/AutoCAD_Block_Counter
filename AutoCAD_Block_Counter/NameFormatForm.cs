using ClosedXML.Excel;
using System;
using System.Collections.Generic;
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
        public bool ShowLayerInExcel => chkShowLayer.Checked;

        public NameFormatForm(string firstFileName)
        {
            InitializeComponent();
            ShowSegments(firstFileName);
            InitBlockGrid();
        }

        private void ShowSegments(string fileName)
        {
            lblFileName.Text = $"�d���ɮצW�١G{fileName}";
            string nameWithoutExt = System.IO.Path.GetFileNameWithoutExtension(fileName);
            _segments = nameWithoutExt.Split('_');
            flowSegments.Controls.Clear();
            _checkBoxes.Clear();
            for (int i = 0; i < _segments.Length; i++)
            {
                var cb = new CheckBox
                {
                    Text = $"��{i + 1}�q�G{_segments[i]}",
                    Tag = i,
                    AutoSize = true
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
            dgvBlocks.Columns[0].HeaderText = "��l�϶��W��";
            dgvBlocks.Columns[1].HeaderText = "Excel��ܦW��";
            dgvBlocks.Columns[0].Width = 140;
            dgvBlocks.Columns[1].Width = 160;
        }

        private void btnAddBlock_Click(object sender, EventArgs e)
        {
            dgvBlocks.Rows.Add();
        }

        private void btnImportBlockMap_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Excel �ɮ� (*.xlsx)|*.xlsx";
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
                        // ���M�Ų{�����
                        dgvBlocks.Rows.Clear();
                        // ���[�����w�q��
                        foreach (var map in imported)
                        {
                            dgvBlocks.Rows.Add(map.BlockName, map.DisplayName);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"�פJ����: {ex.Message}");
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
                MessageBox.Show("�Цܤֳ]�w�@�ӹ϶������C", "����");
                IsConfirmed = false;
                DialogResult = DialogResult.None;
            }
            if (IsConfirmed) Close();
            else if (!IsConfirmed) MessageBox.Show("�п�ܦܤ֤@�Ӥ��q�ÿ�JExcel�ɮצW�١C", "����");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsConfirmed = false;
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
