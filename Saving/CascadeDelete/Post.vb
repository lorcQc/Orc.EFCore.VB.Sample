Namespace CascadeDelete
    Public Class Post
        Public Property PostId As Integer
        Public Property Title As String
        Public Property Content As String

        Public Property BlogId As Integer?
        Public Property Blog As Blog
    End Class
End Namespace
