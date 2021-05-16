Imports Microsoft.EntityFrameworkCore

Namespace Relational.IndexNoFilter
    Friend Class MyContext
        Inherits DbContext

        Public Property Blogs As DbSet(Of Blog)

#Region "IndexNoFilter"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.
                Entity(Of Blog)().
                HasIndex(Function(b) b.Url).
                IsUnique().
                HasFilter(Nothing)
        End Sub
    End Class
#End Region

    Public Class Blog
        Public Property BlogId As Integer
        Public Property Url As String
    End Class
End Namespace
