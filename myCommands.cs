using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
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
            }
        }
    }
}
