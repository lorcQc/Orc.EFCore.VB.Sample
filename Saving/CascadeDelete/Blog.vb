Namespace CascadeDelete
    Public Class Blog
        Public Property BlogId As Integer
        Public Property Url As String

        Public Property Posts As List(Of Post) = New List(Of Post)
    End Class
End Namespace
