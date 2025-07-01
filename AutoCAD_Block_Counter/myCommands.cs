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
            string excelFileName = string.Empty;
            bool isConfirmed = false;
            List<BlockMapping> blockMappings = new();
            using (var dialog = new NameFormatForm(firstFileName))
            {
                if (dialog.ShowDialog() == DialogResult.OK && dialog.IsConfirmed)
                {
                    selectedIndexes = dialog.SelectedIndexes;
                    excelFileName = dialog.ExcelFileName;
                    blockMappings = dialog.BlockMappings;
                    isConfirmed = true;
                }
            }
            if (!isConfirmed || selectedIndexes.Count == 0 || string.IsNullOrWhiteSpace(excelFileName) || blockMappings.Count == 0)
            {
                ed.WriteMessage("\n������ɮצW�٤��q�Υ���JExcel�ɮצW�٩Υ��]�w�϶��A�ާ@�����C");
                return;
            }

            // �έp�Ҧ��ɮת��϶��ƶq�P�ϼh�]���϶��W��+�ϼh�զX�έp�^
            var allBlockLayerCombos = new HashSet<string>(); // "BlockName@LayerName" ���զX
            var fileBlockLayerCounts = new Dictionary<string, Dictionary<string, int>>(); // file => ("BlockName@LayerName" => count)
            
            foreach (var file in dwgFiles)
            {
                var blockLayerCounts = new Dictionary<string, int>();
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
                                    string layer = blockRef.Layer;
                                    string blockLayerKey = $"{name}@{layer}"; // �� @ �Ÿ����j�϶��W�٩M�ϼh�W��
                                    
                                    if (blockLayerCounts.ContainsKey(blockLayerKey))
                                        blockLayerCounts[blockLayerKey]++;
                                    else
                                        blockLayerCounts[blockLayerKey] = 1;
                                    
                                    allBlockLayerCombos.Add(blockLayerKey);
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
                fileBlockLayerCounts[Path.GetFileName(file)] = blockLayerCounts;
            }

            // �ץX��Excel
            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                string fileName = excelFileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase) ? excelFileName : excelFileName + ".xlsx";
                string filePath = Path.Combine(desktopPath, fileName);
                using (var workbook = new XLWorkbook())
                {
                    var ws = workbook.Worksheets.Add("BatchBlockCounts");
                    var blockList = blockMappings.Select(b => b.BlockName).ToList();
                    var blockDisplayList = blockMappings.Select(b => b.DisplayName).ToList();
                    var fileList = fileBlockLayerCounts.Keys.ToList();
                    
                    // �����w�w�q�M���w�q���϶�+�ϼh�զX
                    var definedBlockLayerCombos = new List<(string BlockName, string DisplayName, string Layer, string Key)>();
                    var undefinedBlockLayerCombos = new List<(string BlockName, string Layer, string Key)>();
                    
                    foreach (var combo in allBlockLayerCombos.OrderBy(x => x))
                    {
                        var parts = combo.Split('@');
                        if (parts.Length == 2)
                        {
                            string blockName = parts[0];
                            string layerName = parts[1];
                            
                            var mapping = blockMappings.FirstOrDefault(m => m.BlockName == blockName);
                            if (mapping != null)
                            {
                                definedBlockLayerCombos.Add((blockName, mapping.DisplayName, layerName, combo));
                            }
                            else
                            {
                                undefinedBlockLayerCombos.Add((blockName, layerName, combo));
                            }
                        }
                    }
                    
                    // ���ϼh���ձƧ�
                    definedBlockLayerCombos = definedBlockLayerCombos
                        .OrderBy(x => x.Layer)
                        .ThenBy(x => x.BlockName)
                        .ToList();
                    undefinedBlockLayerCombos = undefinedBlockLayerCombos
                        .OrderBy(x => x.Layer)
                        .ThenBy(x => x.BlockName)
                        .ToList();
                    
                    // �ϼh�C����t
                    var allLayers = definedBlockLayerCombos.Select(x => x.Layer)
                        .Concat(undefinedBlockLayerCombos.Select(x => x.Layer))
                        .Distinct().ToList();
                    var layerColorMap = new Dictionary<string, XLColor>();
                    var palette = new[] { XLColor.LightBlue, XLColor.LightGreen, XLColor.LightYellow, XLColor.LightPink, XLColor.LightGray, XLColor.LightCyan, XLColor.LightSalmon, XLColor.LightCoral };
                    for (int i = 0; i < allLayers.Count; i++)
                        layerColorMap[allLayers[i]] = palette[i % palette.Length];
                    
                    // ���D�C
                    ws.Cell(1, 1).Value = "��l�϶�";
                    ws.Cell(1, 2).Value = "�s�϶��W��";
                    ws.Cell(1, 3).Value = "�ϼh�W��";
                    int colStart = 4;
                    for (int i = 0; i < fileList.Count; i++)
                    {
                        string shortName = ExtractShortName(fileList[i]);
                        var segments = shortName.Split('_');
                        var selected = selectedIndexes.Where(idx => idx < segments.Length).Select(idx => segments[idx]);
                        string displayName = string.Join("_", selected);
                        ws.Cell(1, i + colStart).Value = displayName;
                    }
                    // �[�`���
                    int sumCol = fileList.Count + colStart;
                    ws.Cell(1, sumCol).Value = "�U�Ӽh�[�`�έp";
                    
                    // �w�w�q�϶�
                    int row = 2;
                    foreach (var info in definedBlockLayerCombos)
                    {
                        ws.Cell(row, 1).Value = info.BlockName;
                        ws.Cell(row, 2).Value = info.DisplayName;
                        ws.Cell(row, 3).Value = info.Layer;
                        
                        // �]�w�ϼh�I����
                        if (!string.IsNullOrEmpty(info.Layer) && layerColorMap.ContainsKey(info.Layer))
                            ws.Range(row, 1, row, sumCol).Style.Fill.BackgroundColor = layerColorMap[info.Layer];
                        
                        int sum = 0;
                        for (int c = 0; c < fileList.Count; c++)
                        {
                            var blockLayerCounts = fileBlockLayerCounts[fileList[c]];
                            blockLayerCounts.TryGetValue(info.Key, out int count);
                            ws.Cell(row, c + colStart).Value = count;
                            sum += count;
                        }
                        ws.Cell(row, sumCol).Value = sum;
                        row++;
                    }
                    
                    // ���w�q�϶�
                    foreach (var info in undefinedBlockLayerCombos)
                    {
                        ws.Cell(row, 1).Value = info.BlockName;
                        ws.Cell(row, 2).Value = ""; // ���w�q��ܦW��
                        ws.Cell(row, 3).Value = info.Layer;
                        
                        // �]�w�ϼh�I����
                        if (!string.IsNullOrEmpty(info.Layer) && layerColorMap.ContainsKey(info.Layer))
                            ws.Range(row, 1, row, sumCol).Style.Fill.BackgroundColor = layerColorMap[info.Layer];
                        
                        int sum = 0;
                        for (int c = 0; c < fileList.Count; c++)
                        {
                            var blockLayerCounts = fileBlockLayerCounts[fileList[c]];
                            blockLayerCounts.TryGetValue(info.Key, out int count);
                            ws.Cell(row, c + colStart).Value = count;
                            sum += count;
                        }
                        ws.Cell(row, sumCol).Value = sum;
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
