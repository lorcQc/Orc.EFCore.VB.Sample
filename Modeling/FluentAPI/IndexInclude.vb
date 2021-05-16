Imports Microsoft.EntityFrameworkCore

Namespace Relational.IndexInclude
    Friend Class MyContext
        Inherits DbContext

        Public Property Posts As DbSet(Of Post)

#Region "IndexInclude"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.
                Entity(Of Post)().
                HasIndex(Function(p) p.Url).
                IncludeProperties(Function(p) New With {p.Title, p.PublishedOn})
        End Sub
#End Region
    End Class

    Public Class Post
        Public Property PostId As Integer
        Public Property Url As String
        Public Property Title As String
        Public Property PublishedOn As Date
    End Class
End Namespace
