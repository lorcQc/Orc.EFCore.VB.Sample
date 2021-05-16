Imports Microsoft.EntityFrameworkCore

Namespace DiscriminatorPropertyConfiguration
    Public Class MyContext
        Inherits DbContext

        Public Property Blogs As DbSet(Of Blog)

#Region "DiscriminatorPropertyConfiguration"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.
                Entity(Of Blog)().
                Property("Discriminator").
                HasMaxLength(200)
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
