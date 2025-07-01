# AutoCAD_Block_Counter

## Version 1.0.0
已完成初期功能,能夠於 AutoCad 中完成圖塊計數功能，並且輸出至命令列表。
命令名稱 : `BlockCounter`

## Version 2.0.0
將圖塊計數結果輸出至 Excel 檔案中。
命令名稱 : `BlockCounterBatch`

## Version 2.1
可將某一資料夾內的所有 DWG 進行圖塊計數，將結果輸出至 Excel 檔案中。
命令名稱 : `BlockCounterBatch`

## Version 2.2
將輸出自Excel中的檔案名稱格式改成 : "樓層_編輯者名稱_版本"。
命令名稱 : `BlockCounterBatch`

## Version 2.3
可利用勾選方式預覽輸出自Excel中的檔案名稱格式。

## Version 2.3.1
Excel 中的排序方式更改
A1=檔案名稱
A2~An=所有圖塊名稱 
B1~N1=檔案名稱（由我選擇的分段組合）

## Version 2.4
支援匯入Excel檔案以定義新輸出的圖塊名稱。

## Version 3.0
新增圖塊所在的圖層顯示功能
新增圖層勾選功能
新增不同圖層不同顏色的功能
新增所有圖塊統計功能

## Version 3.0.1
修改使用者介面大小

## Version 3.1.0
?? **UI 美化更新**
- 新增現代化配色方案和視覺設計
- 改善控制項佈局和間距
- 使用 Microsoft JhengHei UI 字體優化中文顯示

## Version 3.2.0
?? **圖層分別統計功能**
- **重要改進**: 同一圖塊在不同圖層現在會分別顯示和統計
- 自動按「圖塊名稱@圖層名稱」組合進行統計
- Excel 輸出中圖層資訊預設顯示，無需勾選
- 解決了相同圖塊在不同圖層被合併統計的問題

**使用說明**: 例如圖塊 "A" 在圖層 "Layer1" 有 3 個，在圖層 "Layer2" 有 2 個，
現在會分別顯示為兩行：
- A (Layer1): 3 個
- A (Layer2): 2 個

## Version 3.2.1
?? **視窗大小優化**
- 調整視窗大小從 620x620 提升至 820x650
- 增加檔案名稱分段顯示區域大小 (760x80)
- 擴大 DataGridView 和相關輸入欄位寬度
- 解決檔案名稱分段文字被截斷的問題
- 改善多分段檔案名稱的顯示體驗

**改進效果**: 現在能完整顯示更多和更長的檔案名稱分段，不會出現內容被截斷的情況。

---
## 版本相容性
- AutoCAD 2025
- AutoCAD 2024
- Framework : .Net 8.0
- OS : Windows 11
---
## 編輯所需工具
- Visual Studio 2022
- AutoCAD
- AutoCAD .NET Wizard(https://aps.autodesk.com/developer/overview/autocad)
- AutoCAD ObjectARX Wizard(https://aps.autodesk.com/developer/overview/autocad)
- .Net Framework 4.8(https://dotnet.microsoft.com/zh-tw/download/dotnet-framework)
- .Net 8.0 (https://dotnet.microsoft.com/zh-tw/download/dotnet/8.0)