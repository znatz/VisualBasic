Imports System.Data.SqlClient
Imports System.Data.OleDb

Public Class Form1

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim St As String = "Data Source=ZNATZ\ZINSTANCE;Initial Catalog=Posco;Persist Security Info=True;User ID=sa; Password=1"
        Dim Qry As String = "Select * From Receipt"


        Dim connection As SqlConnection = New SqlClient.SqlConnection(St)
        connection.Open()
        Dim command As SqlCommand = connection.CreateCommand()
        command.CommandText = Qry
        Dim result As SqlDataReader = command.ExecuteReader
        While result.Read
            TextBox1.Text = TextBox1.Text + result.GetString(1)
        End While

        connection.Close()


        ' In 32bit system, the Provider is "Microsoft.Jet.OLEDB.4.0"
        St = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Z\PGround\VisualBasic\VS2008\TestSQLServer\MASTER\poscoMASTER.MDB"
        Dim mdbConnection As OleDbConnection = New OleDbConnection(St)
        Dim mdbCommand As OleDbCommand = mdbConnection.CreateCommand()
        ' SQL SERVER has not LIMIT clause, use TOP or NEXT, OFFSET instead
        Qry = "SELECT TOP 30 * FROM BTSMAS"
        mdbCommand.CommandText = Qry
        mdbConnection.Open()
        Dim mdbResult As OleDbDataReader = mdbCommand.ExecuteReader
        While mdbResult.Read
            TextBox2.Text = TextBox2.Text + mdbResult.GetString(2)
        End While

        mdbConnection.Close()


    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim f As Form2 = New Form2
        f.Show()
        Me.Close()
    End Sub

    Private Sub SayhiToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SayhiToolStripMenuItem.Click
        Dim f As Form2 = New Form2
        f.Show()
        Me.Close()
    End Sub
End Class
