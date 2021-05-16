Imports Microsoft.EntityFrameworkCore

Namespace MaxLength
    Friend Class MyContext
        Inherits DbContext

        Public Property Blogs As DbSet(Of Blog)

#Region "MaxLength"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.
                Entity(Of Blog)().
                Property(Function(b) b.Url).
                HasMaxLength(500)
        End Sub
#End Region
    End Class

    Public Class Blog
        Public Property BlogId As Integer
        Public Property Url As String
    End Class
End Namespace
