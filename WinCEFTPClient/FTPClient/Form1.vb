Imports SmartFTPClient

Public Class Form1


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim client As SmartFTPClient.IFtpClient
        client = New SmartFTPClient.FtpClient
        client.GetLocalFiles()

    End Sub
End Class
