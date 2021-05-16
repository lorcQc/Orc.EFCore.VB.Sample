Imports Microsoft.EntityFrameworkCore

Namespace Relational.IndexFilter
    Friend Class MyContext
        Inherits DbContext

        Public Property Blogs As DbSet(Of Blog)

#Region "IndexFilter"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.
                Entity(Of Blog)().
                HasIndex(Function(b) b.Url).
                HasFilter("[Url] IS NOT NULL")
        End Sub
    End Class
#End Region

    Public Class Blog
        Public Property BlogId As Integer
        Public Property Url As String
    End Class
End Namespace
