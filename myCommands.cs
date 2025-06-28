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
        // The CommandMethod attribute can be applied to any public  member 
        // function of any public class.
        // The function should take no arguments and return nothing.
        // If the method is an intance member then the enclosing class is 
        // intantiated for each document. If the member is a static member then
        // the enclosing class is NOT intantiated.
        //
        // NOTE: CommandMethod has overloads where you can provide helpid and
        // context menu.

        // Modal Command with localized name
        [CommandMethod("MyGroup", "MyCommand", "MyCommandLocal", CommandFlags.Modal)]
        public void MyCommand() // This method can have any name
        {
            // Put your command code here
            Document doc = AcadApp.DocumentManager.MdiActiveDocument;
            Autodesk.AutoCAD.EditorInput.Editor ed;
            if (doc != null)
            {
                ed = doc.Editor;
                ed.WriteMessage("Hello, this is your first command.");

            }
        }

        // Modal Command with pickfirst selection
        [CommandMethod("MyGroup", "MyPickFirst", "MyPickFirstLocal", CommandFlags.Modal | CommandFlags.UsePickSet)]
        public void MyPickFirst() // This method can have any name
        {
            PromptSelectionResult result = AcadApp.DocumentManager.MdiActiveDocument.Editor.GetSelection();
            if (result.Status == PromptStatus.OK)
            {
                // There are selected entities
                // Put your command using pickfirst set code here
            }
            else
            {
                // There are no selected entities
                // Put your command code here
            }
        }

        // Application Session Command with localized name
        [CommandMethod("MyGroup", "MySessionCmd", "MySessionCmdLocal", CommandFlags.Modal | CommandFlags.Session)]
        public void MySessionCmd() // This method can have any name
        {
            // Put your command code here
        }

        // LispFunction is similar to CommandMethod but it creates a lisp 
        // callable function. Many return types are supported not just string
        // or integer.
        [LispFunction("MyLispFunction", "MyLispFunctionLocal")]
        public int MyLispFunction(ResultBuffer args) // This method can have any name
        {
            // Put your command code here

            // Return a value to the AutoCAD Lisp Interpreter
            return 1;
        }

        // Modal Command for counting blocks in the drawing
        [CommandMethod("BLOCKCOUNT", CommandFlags.Modal)]
        public void BlockCount()
        {
            Document doc = AcadApp.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;
            Dictionary<string, int> blockCounts = new();

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                BlockTableRecord ms = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead);

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
                    }
                }
                tr.Commit();
            }

            if (blockCounts.Count == 0)
            {
                ed.WriteMessage("\n圖面中沒有圖塊參考。");
            }
            else
            {
                ed.WriteMessage("\n圖塊計數結果：");
                foreach (var kvp in blockCounts)
                {
                    ed.WriteMessage($"\n圖塊名稱: {kvp.Key}, 數量: {kvp.Value}");
                }

                // 匯出到Excel
                try
                {
                    string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                    string fileName = $"BlockCount_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                    string filePath = Path.Combine(desktopPath, fileName);

                    using (var workbook = new XLWorkbook())
                    {
                        var ws = workbook.Worksheets.Add("BlockCounts");
                        ws.Cell(1, 1).Value = "圖塊名稱";
                        ws.Cell(1, 2).Value = "數量";
                        int row = 2;
                        foreach (var kvp in blockCounts)
                        {
                            ws.Cell(row, 1).Value = kvp.Key;
                            ws.Cell(row, 2).Value = kvp.Value;
                            row++;
                        }
                        workbook.SaveAs(filePath);
                    }
                    ed.WriteMessage($"\n已將圖塊計數結果匯出至: {filePath}");
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage($"\n匯出Excel時發生錯誤: {ex.Message}");
                }
            }
        }

        // 擷取檔案名稱中的樓層_名稱_版本部分
        private string ExtractShortName(string filename)
        {
            // 只取檔名不含副檔名
            var name = Path.GetFileNameWithoutExtension(filename);
            var parts = name.Split('_');
            // 預期格式: 前綴_樓層_日期_名稱_版本
            // 取第2、4、5段（index 1, 3, 4）
            if (parts.Length >= 5)
                return $"{parts[1]}_{parts[3]}_{parts[4]}";
            else if (parts.Length >= 3)
                return string.Join("_", parts.Skip(1));
            else
                return name;
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

            // 統計所有檔案的圖塊數量
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
                    ed.WriteMessage($"\n讀取檔案 {Path.GetFileName(file)} 時發生錯誤: {ex.Message}");
                }
                fileBlockCounts[Path.GetFileName(file)] = blockCounts;
            }

            // 匯出到Excel
            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                string fileName = $"BlockCountBatch_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                string filePath = Path.Combine(desktopPath, fileName);
                using (var workbook = new XLWorkbook())
                {
                    var ws = workbook.Worksheets.Add("BatchBlockCounts");
                    // 標題列
                    ws.Cell(1, 1).Value = "檔案名稱";
                    int col = 2;
                    var blockList = allBlockNames.OrderBy(n => n).ToList();
                    foreach (var blockName in blockList)
                    {
                        ws.Cell(1, col).Value = blockName;
                        col++;
                    }
                    // 資料列
                    int row = 2;
                    foreach (var kvp in fileBlockCounts)
                    {
                        ws.Cell(row, 1).Value = ExtractShortName(kvp.Key);
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
                ed.WriteMessage($"\n已將批次圖塊計數結果匯出至: {filePath}");
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage($"\n匯出Excel時發生錯誤: {ex.Message}");
            }
        }
    }
}