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