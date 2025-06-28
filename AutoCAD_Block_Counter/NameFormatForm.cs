using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AutoCAD_Block_Counter
{
    public partial class NameFormatForm : Form
    {
        private string[] _segments;
        private List<CheckBox> _checkBoxes = new();
        public List<int> SelectedIndexes { get; private set; } = new();
        public bool IsConfirmed { get; private set; } = false;
        public string ExcelFileName => txtExcelName.Text.Trim();

        public NameFormatForm(string firstFileName)
        {
            InitializeComponent();
            ShowSegments(firstFileName);
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

        private void btnOK_Click(object sender, EventArgs e)
        {
            SelectedIndexes = _checkBoxes
                .Select((cb, idx) => (cb, idx))
                .Where(x => x.cb.Checked)
                .Select(x => x.idx)
                .ToList();
            IsConfirmed = SelectedIndexes.Count > 0 && !string.IsNullOrWhiteSpace(ExcelFileName);
            DialogResult = IsConfirmed ? DialogResult.OK : DialogResult.None;
            if (IsConfirmed) Close();
            else MessageBox.Show("請選擇至少一個分段並輸入Excel檔案名稱。", "提示");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsConfirmed = false;
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
