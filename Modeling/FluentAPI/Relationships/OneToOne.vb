Imports Microsoft.EntityFrameworkCore

Namespace Relationships.OneToOne
#Region "OneToOne"
    Friend Class MyContext
        Inherits DbContext

        Public Property Blogs As DbSet(Of Blog)
        Public Property BlogImages As DbSet(Of BlogImage)

        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.Entity(Of Blog)().
                HasOne(Function(b) b.BlogImage).
                WithOne(Function(i) i.Blog).
                HasForeignKey(Of BlogImage)(Function(b) b.BlogForeignKey)
        End Sub
    End Class

    Public Class Blog
        Public Property BlogId As Integer
        Public Property Url As String

        Public Property BlogImage As BlogImage
    End Class

    Public Class BlogImage
        Public Property BlogImageId As Integer
        Public Property Image As Byte()
        Public Property Caption As String

        Public Property BlogForeignKey As Integer
        Public Property Blog As Blog
    End Class
#End Region

End Namespace
