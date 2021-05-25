Public Class Post
    Public Property PostId As Integer
    Public Property Title As String
    Public Property Content As String
    Public Property Rating As Integer

    Public Property BlogId As Integer
    Public Property Blog As Blog

    Public Property AuthorId As Integer
    Public Property Author As Person

    Public Property Tags As List(Of PostTag)
End Class
