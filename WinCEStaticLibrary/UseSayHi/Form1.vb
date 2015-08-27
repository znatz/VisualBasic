Imports LibraryJustSayHi.Class1

Public Class Form1

    Private Sub Label1_ParentChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.ParentChanged
        Dim c As LibraryJustSayHi.Class1 = New LibraryJustSayHi.Class1
        c.Greeting(Label1)
    End Sub
End Class
