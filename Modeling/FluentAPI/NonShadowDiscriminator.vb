Imports Microsoft.EntityFrameworkCore

Namespace NonShadowDiscriminator
    Public Class MyContext
        Inherits DbContext

        Public Property Blogs As DbSet(Of Blog)

#Region "NonShadowDiscriminator"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.
                Entity(Of Blog)().
                HasDiscriminator(Function(b) b.BlogType)

            modelBuilder.
                Entity(Of Blog)().
                Property(Function(e) e.BlogType).
                HasMaxLength(200).
                HasColumnName("blog_type")
        End Sub
#End Region
    End Class

    Public Class Blog
        Public Property BlogId As Integer
        Public Property Url As String
        Public Property BlogType As String
    End Class

    Public Class RssBlog
        Inherits Blog
        Public Property RssUrl As String
    End Class
End Namespace
