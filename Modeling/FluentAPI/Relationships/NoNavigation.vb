Imports Microsoft.EntityFrameworkCore

Namespace Relationships.NoNavigation
#Region "NoNavigation"
    Friend Class MyContext
        Inherits DbContext

        Public Property Blogs As DbSet(Of Blog)
        Public Property Posts As DbSet(Of Post)

        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.Entity(Of Post)().
                         HasOne(Of Blog)().
                         WithMany().
                         HasForeignKey(Function(p) p.BlogId)
        End Sub
    End Class

    Public Class Blog
        Public Property BlogId As Integer
        Public Property Url As String
    End Class

    Public Class Post
        Public Property PostId As Integer
        Public Property Title As String
        Public Property Content As String

        Public Property BlogId As Integer
    End Class
#End Region

End Namespace
