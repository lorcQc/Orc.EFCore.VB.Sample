Namespace Disconnected
    Public Class Blog
        Inherits EntityBase

        Public Property BlogId As Integer
        Public Property Url As String

        Public Property Posts As List(Of Post)
    End Class
End Namespace
