Imports Microsoft.EntityFrameworkCore

Namespace InheritanceDbSets
#Region "InheritanceDbSets"
    Friend Class MyContext
        Inherits DbContext
        Public Property Blogs As DbSet(Of Blog)
        Public Property RssBlogs As DbSet(Of RssBlog)
    End Class

    Public Class Blog
        Public Property BlogId As Integer
        Public Property Url As String
    End Class

    Public Class RssBlog
        Inherits Blog
        Public Property RssUrl As String
    End Class
#End Region

End Namespace
