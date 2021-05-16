Imports Microsoft.EntityFrameworkCore

Namespace DiscriminatorConfiguration
    Public Class MyContext
        Inherits DbContext

        Public Property Blogs As DbSet(Of Blog)
#Region "DiscriminatorConfiguration"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.
                Entity(Of Blog)().
                HasDiscriminator(Of String)("blog_type").
                HasValue(Of Blog)("blog_base").
                HasValue(Of RssBlog)("blog_rss")
        End Sub
#End Region
    End Class

    Public Class Blog
        Public Property BlogId As Integer
        Public Property Url As String
    End Class

    Public Class RssBlog
        Inherits Blog
        Public Property RssUrl As String
    End Class
End Namespace
