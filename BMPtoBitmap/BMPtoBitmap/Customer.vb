Public Class Customer
    Implements IDisposable
    Private _name As String
    Private _age As Integer
    Public ReadOnly Property name() As String
        Get
            Return _name
        End Get
    End Property
    Public WriteOnly Property name(ByVal val)
        Set(value)
            _name = val
        End Set
    End Property
    Public ReadOnly Property age() As Integer
        Get
            Return _age
        End Get
    End Property
    Public WriteOnly Property age(ByVal i)
        Set(value)
            _age = i
        End Set
    End Property
    Sub New(ByVal n As String, ByVal i As Integer)
        _name = n
        _age = i
    End Sub
    Public Sub Dispose() Implements IDisposable.Dispose
        Console.WriteLine("End")
    End Sub
End Class
