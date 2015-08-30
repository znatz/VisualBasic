Imports System.Net.Sockets
Imports System.Net
Imports System.Net.IPHostEntry

Imports System.IO
Module Module1
    Dim remoteEndpoint As IPEndPoint
    Dim socket As Socket
    Sub Main()

        'Dim hostname As String = "posco-cloud.sakura.ne.jp"
        Dim hostname As String = "192.168.24.52"
        Dim ipHostEntry As IPHostEntry = Dns.GetHostEntry(hostname)
        Dim addr As IPAddress = ipHostEntry.AddressList(0)
        'Dim endpoint As IPEndPoint = New IPEndPoint(addr, 21)
        Dim endpoint As IPEndPoint = New IPEndPoint(IPAddress.Parse(hostname), 21)
        Console.WriteLine(endpoint)
        socket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        socket.Connect(endpoint)

        sendCommand(socket, "USER test")
        sendCommand(socket, "PASS test")
        'sendCommand(socket, "USER posco-cloud")
        'sendCommand(socket, "PASS p8vx98hzru")
        sendCommand(socket, "TYPE I")
        Threading.Thread.Sleep(200)

        remoteEndpoint = getPASV(socket, "PASV")

        sendCommand(socket, "STOR preloader.gif")
        Dim th As Threading.Thread = New Threading.Thread(AddressOf sendFile)
        th.Start()
        th.Join()
        'sendFile()
        sendCommand(socket, "QUIT")
        socket.Close()
        Console.ReadLine()

    End Sub
    Sub sendFile()

        Dim fs As FileStream = New FileStream("preloader.gif", FileMode.Open)
        Dim fBuffer(fs.Length) As Byte
        fs.Read(fBuffer, 0, fs.Length)
        'Dim tmpStr As String = System.Text.Encoding.UTF8.GetString(fBuffer)
        'fBuffer = System.Text.Encoding.UTF8.GetBytes(tmpStr)

        Dim sendSocket As Socket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)

        Try
            sendSocket.Connect(remoteEndpoint)
            Console.WriteLine("Remote endpoint : " & sendSocket.RemoteEndPoint.ToString)
            sendSocket.Send(fBuffer, fBuffer.Length, 0)
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try

        'Dim tcp As New System.Net.Sockets.TcpClient
        'tcp.Connect(remoteEndpoint.Address, remoteEndpoint.Port)
        'tcp.GetStream().Write(fBuffer, 0, fBuffer.Length)
        'tcp.Close()

    End Sub
    Sub sendCommand(ByRef socket As Socket, ByVal comm As String)
        Dim buffer(255) As Byte
        comm = comm & ControlChars.CrLf
        Dim commBytes() As Byte = System.Text.Encoding.ASCII.GetBytes(comm)
        socket.Send(commBytes, commBytes.Length, 0)
        If comm.Substring(0, 3) = "STOR" Then
            Return
        End If
        Dim i As Integer = socket.Receive(buffer)
        Console.WriteLine(System.Text.Encoding.ASCII.GetString(buffer))
    End Sub

    Function getPASV(ByRef socket As Socket, ByVal comm As String) As IPEndPoint
        Dim buffer(255) As Byte
        comm = comm & ControlChars.CrLf
        Dim commBytes() As Byte = System.Text.Encoding.ASCII.GetBytes(comm)
        socket.Send(commBytes, commBytes.Length, 0)

        Threading.Thread.Sleep(200)
        Dim i As Integer = socket.Receive(buffer)
        Console.WriteLine(System.Text.Encoding.ASCII.GetString(buffer))
        Dim response = System.Text.Encoding.ASCII.GetString(buffer)

        Dim startpos As Integer = response.IndexOf("(") + 1
        Dim endpos As Integer = response.IndexOf(")")
        Dim parts() As String = response.Substring(startpos, endpos - startpos).Split(",")
        Dim remoteEndpoint As IPEndPoint = New IPEndPoint( _
                                                IPAddress.Parse( _
                                                        parts(0) & "." & _
                                                        parts(1) & "." & _
                                                        parts(2) & "." & _
                                                        parts(3)), _
                                                        Convert.ToInt32(parts(4)) * 256 + Convert.ToInt32(parts(5)))
        Return remoteEndpoint
    End Function


End Module
