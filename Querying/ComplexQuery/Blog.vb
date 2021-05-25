Public Class Blog
    Public Property BlogId As Integer
    Public Property Url As String
    Public Property Rating As Integer?

    Public Property Posts As List(Of Post)

    Public Property OwnerId As Integer
    Public Property Owner As Person
End Class
