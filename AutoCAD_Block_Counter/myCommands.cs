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
        // 取得不含副檔名的檔案名稱
        private static string ExtractShortName(string fileName)
        {
            return Path.GetFileNameWithoutExtension(fileName);
        }

        // 批次統計資料夾內所有DWG檔案的圖塊，並輸出Excel
        [CommandMethod("BLOCKCOUNTBATCH", CommandFlags.Modal)]
        public void BlockCountBatch()
        {
            Editor ed = AcadApp.DocumentManager.MdiActiveDocument.Editor;

            string folderPath = string.Empty;
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "請選擇DWG檔案所在資料夾";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    folderPath = dialog.SelectedPath;
                }
                else
                {
                    ed.WriteMessage("\n未選擇資料夾，操作取消。");
                    return;
                }
            }

            var dwgFiles = Directory.GetFiles(folderPath, "*.dwg", SearchOption.TopDirectoryOnly);
            if (dwgFiles.Length == 0)
            {
                ed.WriteMessage("\n該資料夾內沒有DWG檔案。");
                return;
            }

            // 取得第一個檔案名稱，傳給 NameFormatForm
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
                ed.WriteMessage("\n未選擇檔案名稱分段或未輸入Excel檔案名稱或未設定圖塊，操作取消。");
                return;
            }

            // 統計所有檔案的圖塊數量與圖層（按圖塊名稱+圖層組合統計）
            var allBlockLayerCombos = new HashSet<string>(); // "BlockName@LayerName" 的組合
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
                                    string blockLayerKey = $"{name}@{layer}"; // 用 @ 符號分隔圖塊名稱和圖層名稱
                                    
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
                    ed.WriteMessage($"\n讀取檔案 {Path.GetFileName(file)} 時發生錯誤: {ex.Message}");
                }
                fileBlockLayerCounts[Path.GetFileName(file)] = blockLayerCounts;
            }

            // 匯出到Excel
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
                    
                    // 分離已定義和未定義的圖塊+圖層組合
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
                    
                    // 按圖層分組排序
                    definedBlockLayerCombos = definedBlockLayerCombos
                        .OrderBy(x => x.Layer)
                        .ThenBy(x => x.BlockName)
                        .ToList();
                    undefinedBlockLayerCombos = undefinedBlockLayerCombos
                        .OrderBy(x => x.Layer)
                        .ThenBy(x => x.BlockName)
                        .ToList();
                    
                    // 圖層顏色分配
                    var allLayers = definedBlockLayerCombos.Select(x => x.Layer)
                        .Concat(undefinedBlockLayerCombos.Select(x => x.Layer))
                        .Distinct().ToList();
                    var layerColorMap = new Dictionary<string, XLColor>();
                    var palette = new[] { XLColor.LightBlue, XLColor.LightGreen, XLColor.LightYellow, XLColor.LightPink, XLColor.LightGray, XLColor.LightCyan, XLColor.LightSalmon, XLColor.LightCoral };
                    for (int i = 0; i < allLayers.Count; i++)
                        layerColorMap[allLayers[i]] = palette[i % palette.Length];
                    
                    // 標題列
                    ws.Cell(1, 1).Value = "原始圖塊";
                    ws.Cell(1, 2).Value = "新圖塊名稱";
                    ws.Cell(1, 3).Value = "圖層名稱";
                    int colStart = 4;
                    for (int i = 0; i < fileList.Count; i++)
                    {
                        string shortName = ExtractShortName(fileList[i]);
                        var segments = shortName.Split('_');
                        var selected = selectedIndexes.Where(idx => idx < segments.Length).Select(idx => segments[idx]);
                        string displayName = string.Join("_", selected);
                        ws.Cell(1, i + colStart).Value = displayName;
                    }
                    // 加總欄位
                    int sumCol = fileList.Count + colStart;
                    ws.Cell(1, sumCol).Value = "各樓層加總統計";
                    
                    // 已定義圖塊
                    int row = 2;
                    foreach (var info in definedBlockLayerCombos)
                    {
                        ws.Cell(row, 1).Value = info.BlockName;
                        ws.Cell(row, 2).Value = info.DisplayName;
                        ws.Cell(row, 3).Value = info.Layer;
                        
                        // 設定圖層背景色
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
                    
                    // 未定義圖塊
                    foreach (var info in undefinedBlockLayerCombos)
                    {
                        ws.Cell(row, 1).Value = info.BlockName;
                        ws.Cell(row, 2).Value = ""; // 未定義顯示名稱
                        ws.Cell(row, 3).Value = info.Layer;
                        
                        // 設定圖層背景色
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
                ed.WriteMessage($"\n已將批次圖塊計數結果匯出至: {filePath}");
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage($"\n匯出Excel時發生錯誤: {ex.Message}");
            }
        }
    }
}
