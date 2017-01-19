# Wpf-embed-Win32
這是一個 WPF 寄存 Win32 應用程式的實作

理論上可以將 .exe 格式的圖形化應用程式嵌入 WPF 實作的應用程式中（ 類似 iframe 的運作模式）

對於 WPF, MFC, Windows Form 類型的應用程式理論上有較佳的支援能力

尚無法保證可以運行其他類型的框架所開發的應用程式

分為兩種實作
Type 1 僅需時做 WPF 中的 Windows 並於 xaml 建立用於裝載應用程式的 cavans 元件即可

Type 2 則是採用實作 HwndHost 的方式
更詳細的內容請參考以下所附的文檔與連結

參考資料
https://msdn.microsoft.com/zh-tw/library/ms752055.aspx
http://stackoverflow.com/questions/6477851/how-to-run-an-application-inside-wpf-application/13698077#13698077
http://stackoverflow.com/questions/5028598/hosting-external-app-in-wpf-window
