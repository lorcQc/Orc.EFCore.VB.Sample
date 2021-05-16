Imports Microsoft.EntityFrameworkCore

Namespace Index
    Friend Class MyContext
        Inherits DbContext
        Public Property Blogs As DbSet(Of Blog)
    End Class

#Region "Index"
    <Index(NameOf(Blog.Url))>
    Public Class Blog
        Public Property BlogId As Integer
        Public Property Url As String
    End Class
#End Region

End Namespace
