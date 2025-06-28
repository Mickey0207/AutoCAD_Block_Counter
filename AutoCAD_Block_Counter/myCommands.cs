using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using ClosedXML.Excel;
using System.IO;
using System.Windows.Forms;
using AcadApp = Autodesk.AutoCAD.ApplicationServices.Application;

[assembly: CommandClass(typeof(AutoCAD_Block_Counter.MyCommands))]
[assembly: ExtensionApplication(typeof(AutoCAD_Block_Counter.PluginExtension))]

namespace AutoCAD_Block_Counter
{
    public class MyCommands
    {
        // ���o���t���ɦW���ɮצW��
        private static string ExtractShortName(string fileName)
        {
            return Path.GetFileNameWithoutExtension(fileName);
        }

        // �妸�έp��Ƨ����Ҧ�DWG�ɮת��϶��A�ÿ�XExcel
        [CommandMethod("BLOCKCOUNTBATCH", CommandFlags.Modal)]
        public void BlockCountBatch()
        {
            Editor ed = AcadApp.DocumentManager.MdiActiveDocument.Editor;

            string folderPath = string.Empty;
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "�п��DWG�ɮשҦb��Ƨ�";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    folderPath = dialog.SelectedPath;
                }
                else
                {
                    ed.WriteMessage("\n����ܸ�Ƨ��A�ާ@�����C");
                    return;
                }
            }

            var dwgFiles = Directory.GetFiles(folderPath, "*.dwg", SearchOption.TopDirectoryOnly);
            if (dwgFiles.Length == 0)
            {
                ed.WriteMessage("\n�Ӹ�Ƨ����S��DWG�ɮסC");
                return;
            }

            // ���o�Ĥ@���ɮצW�١A�ǵ� NameFormatForm
            string firstFileName = Path.GetFileName(dwgFiles[0]);
            List<int> selectedIndexes = new();
            bool isConfirmed = false;
            using (var dialog = new NameFormatForm(firstFileName))
            {
                if (dialog.ShowDialog() == DialogResult.OK && dialog.IsConfirmed)
                {
                    selectedIndexes = dialog.SelectedIndexes;
                    isConfirmed = true;
                }
            }
            if (!isConfirmed || selectedIndexes.Count == 0)
            {
                ed.WriteMessage("\n������ɮצW�٤��q�A�ާ@�����C");
                return;
            }

            // �έp�Ҧ��ɮת��϶��ƶq
            var allBlockNames = new HashSet<string>();
            var fileBlockCounts = new Dictionary<string, Dictionary<string, int>>();
            foreach (var file in dwgFiles)
            {
                var blockCounts = new Dictionary<string, int>();
                try
                {
                    using (var db = new Database(false, true))
                    {
                        db.ReadDwgFile(file, FileShare.Read, true, null);
                        using (var tr = db.TransactionManager.StartTransaction())
                        {
                            var bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                            var ms = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead);
                            foreach (ObjectId entId in ms)
                            {
                                Entity? ent = tr.GetObject(entId, OpenMode.ForRead) as Entity;
                                if (ent is BlockReference blockRef)
                                {
                                    string name = blockRef.Name;
                                    if (blockCounts.ContainsKey(name))
                                        blockCounts[name]++;
                                    else
                                        blockCounts[name] = 1;
                                    allBlockNames.Add(name);
                                }
                            }
                            tr.Commit();
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage($"\nŪ���ɮ� {Path.GetFileName(file)} �ɵo�Ϳ��~: {ex.Message}");
                }
                fileBlockCounts[Path.GetFileName(file)] = blockCounts;
            }

            // �ץX��Excel
            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                string fileName = $"BlockCountBatch_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                string filePath = Path.Combine(desktopPath, fileName);
                using (var workbook = new XLWorkbook())
                {
                    var ws = workbook.Worksheets.Add("BatchBlockCounts");
                    // ���D�C
                    ws.Cell(1, 1).Value = "�ɮצW��";
                    int col = 2;
                    var blockList = allBlockNames.OrderBy(n => n).ToList();
                    foreach (var blockName in blockList)
                    {
                        ws.Cell(1, col).Value = blockName;
                        col++;
                    }
                    // ��ƦC
                    int row = 2;
                    foreach (var kvp in fileBlockCounts)
                    {
                        // �ھڤĿ諸���q�զX Excel ��ܦW��
                        string shortName = ExtractShortName(kvp.Key);
                        var segments = shortName.Split('_');
                        var selected = selectedIndexes.Where(idx => idx < segments.Length).Select(idx => segments[idx]);
                        string displayName = string.Join("_", selected);
                        ws.Cell(row, 1).Value = displayName;
                        col = 2;
                        foreach (var blockName in blockList)
                        {
                            kvp.Value.TryGetValue(blockName, out int count);
                            ws.Cell(row, col).Value = count;
                            col++;
                        }
                        row++;
                    }
                    workbook.SaveAs(filePath);
                }
                ed.WriteMessage($"\n�w�N�妸�϶��p�Ƶ��G�ץX��: {filePath}");
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage($"\n�ץXExcel�ɵo�Ϳ��~: {ex.Message}");
            }
        }
    }
}
