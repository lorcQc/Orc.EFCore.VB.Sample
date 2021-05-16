Public Class Blog
    Public Property BlogId As Integer
    Public Property Url As String

    Public Overridable Property Posts As ICollection(Of Post)
End Class
