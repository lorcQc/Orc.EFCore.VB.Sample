Imports Microsoft.EntityFrameworkCore

Namespace Relationships.CascadeDelete
    Friend Class MyContext
        Inherits DbContext

        Public Property Blogs As DbSet(Of Blog)
        Public Property Posts As DbSet(Of Post)

#Region "CascadeDelete"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.Entity(Of Post)().HasOne(Function(p) p.Blog).
                WithMany(Function(b) b.Posts).
                OnDelete(DeleteBehavior.Cascade)
        End Sub
#End Region
    End Class

    Public Class Blog
        Public Property BlogId As Integer
        Public Property Url As String
        Public Property Posts As List(Of Post)
    End Class

    Public Class Post
        Public Property PostId As Integer
        Public Property Title As String
        Public Property Content As String
        Public Property BlogId As Integer?
        Public Property Blog As Blog
    End Class
End Namespace
