Imports System.Drawing
Module Module1

    Sub Main()
        Dim img As Bitmap = New Bitmap("C:\Users\Z\Desktop\1.bmp")
        Dim column(img.Height) As String
        For x As Integer = 0 To img.Width - 1
            'Dim line(img.Height) As Char
            Dim line As ArrayList = New ArrayList(img.Height)
            For y As Integer = 0 To img.Height - 1
                Dim color As Color = img.GetPixel(x, img.Height - 1 - y)
                If color.R <> &H0 Then
                    'line(y) = "0"
                    line.Insert(y, "0")
                Else
                    'line(y) = "1"
                    line.Insert(y, "1")
                End If
            Next
            For Each e As Char In line
                Console.Write(e)
            Next
            Console.Write(Environment.NewLine)
        Next
        Console.ReadLine()
    End Sub

End Module
