Imports Microsoft.EntityFrameworkCore

Namespace ValueGeneratedOnAddOrUpdate
    Friend Class MyContext
        Inherits DbContext

        Public Property Blogs As DbSet(Of Blog)

#Region "ValueGeneratedOnAddOrUpdate"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.
                Entity(Of Blog)().
                Property(Function(b) b.LastUpdated).
                ValueGeneratedOnAddOrUpdate()
        End Sub
#End Region
    End Class

    Public Class Blog
        Public Property BlogId As Integer
        Public Property Url As String
        Public Property LastUpdated As DateTime
    End Class
End Namespace
