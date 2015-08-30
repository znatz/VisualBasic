Imports System.Net
Imports System.Net.Sockets
Imports System.Net.IPHostEntry
Imports System.Text

Public Class Form1

    Public hostname As String = "posco-cloud.sakura.ne.jp"
    'Public hostname As String = "192.168.24.52"
    Private sock As Socket
    Private sendSocket As Socket

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim iphe As IPHostEntry = Dns.GetHostEntry(hostname)
        Dim adList As System.Net.IPAddress() = iphe.AddressList
        Dim ftps As IPEndPoint

        sock = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        ftps = New IPEndPoint(IPAddress.Parse(adList(0).ToString()), 21)

        sock.Connect(ftps)

        SendCommand("USER posco-cloud")
        SendCommand("PASS p8vx98hzru")
        'SendCommand("USER test")
        'SendCommand("PASS test")
        'SendCommand("CWD \test")
        'SendCommand("PWD")
        SendCommand("TYPE I")
        'SendCommand("PASV")
        Dim localendpoint As IPEndPoint = SendCommandAndGetEndpoint("PASV")

        SendCommand("STOR test.txt")


        Dim fs As New System.IO.FileStream("\test.txt", System.IO.FileMode.Open, System.IO.FileAccess.Read)
        Dim buffer(200) As Byte
        Dim x As Integer = CInt(fs.Length)
        fs.Read(buffer, 0, 200)
        Dim contents As String = Encoding.ASCII.GetString(buffer, 0, buffer.Length)

        MessageBox.Show(localendpoint.ToString)


        sendSocket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        sendSocket.Connect(localendpoint)

        sendSocket.Send(Encoding.ASCII.GetBytes(contents), SocketFlags.DontRoute)

        'sendSocket.Close()


        '//終わりましたよとコマンド送信
        SendCommand("PASV")
        SendCommand("QUIT")

    End Sub


    Private Sub SendCommand(ByVal sCommand As String)

        Dim bytes(255) As Byte
        Dim i As Integer

        sCommand = sCommand & ControlChars.CrLf
        Dim cmdbytes As Byte() = Encoding.ASCII.GetBytes(sCommand)
        sock.Send(cmdbytes, cmdbytes.Length, 0)

        i = sock.Receive(bytes)
        MessageBox.Show(Encoding.UTF8.GetString(bytes, 0, bytes.Length - 1))

    End Sub

    Private Sub sendSocketSendCommand(ByVal sCommand As String)

        Dim bytes(255) As Byte
        Dim i As Integer

        sCommand = sCommand & ControlChars.CrLf
        Dim cmdbytes As Byte() = Encoding.ASCII.GetBytes(sCommand)
        sendSocket.Send(cmdbytes, cmdbytes.Length, 0)

        i = sock.Receive(bytes)
        MessageBox.Show(Encoding.UTF8.GetString(bytes, 0, bytes.Length - 1))

    End Sub


    Private Function SendCommandAndGetEndpoint(ByVal sCommand As String) As IPEndPoint

        Dim bytes(255) As Byte
        Dim i As Integer

        sCommand = sCommand & ControlChars.CrLf
        Dim cmdbytes As Byte() = Encoding.ASCII.GetBytes(sCommand)
        sock.Send(cmdbytes, cmdbytes.Length, 0)

        '　Get　reply　from　the　server.
        i = sock.Receive(bytes)

        Dim response As String = Encoding.UTF8.GetString(bytes, 0, bytes.Length)

        MessageBox.Show("PASV get : " + response)

        Dim startpos = response.IndexOf("(") + 1
        Dim endpos = response.IndexOf(")")
        MessageBox.Show(startpos.ToString + " : " + endpos.ToString)

        Dim ips = response.Substring(startpos, endpos - startpos)



        Dim parts() As String = ips.Split(",")
        Dim ipAddr As String = parts(0) + "." + parts(1) + "." + parts(2) + "." + parts(3)
        Dim portNo As String = Integer.Parse(parts(4)) * 256 + Integer.Parse(parts(5))
        Dim localendpoint As IPEndPoint = New IPEndPoint(IPAddress.Parse(ipAddr), portNo)
        Return localendpoint

    End Function
End Class
