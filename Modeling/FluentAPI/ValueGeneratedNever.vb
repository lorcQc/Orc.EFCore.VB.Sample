Imports Microsoft.EntityFrameworkCore

Namespace ValueGeneratedNever
    Friend Class MyContext
        Inherits DbContext

        Public Property Blogs As DbSet(Of Blog)

#Region "ValueGeneratedNever"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.
                Entity(Of Blog)().
                Property(Function(b) b.BlogId).ValueGeneratedNever()
        End Sub
#End Region
    End Class

    Public Class Blog
        Public Property BlogId As Integer
        Public Property Url As String
    End Class
End Namespace
