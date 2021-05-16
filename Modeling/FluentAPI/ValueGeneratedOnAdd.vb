Imports Microsoft.EntityFrameworkCore

Namespace ValueGeneratedOnAdd
    Friend Class MyContext
        Inherits DbContext

        Public Property Blogs As DbSet(Of Blog)

#Region "ValueGeneratedOnAdd"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.
                Entity(Of Blog)().
                Property(Function(b) b.Inserted).
                ValueGeneratedOnAdd()
        End Sub
#End Region
    End Class

    Public Class Blog
        Public Property BlogId As Integer
        Public Property Url As String
        Public Property Inserted As DateTime
    End Class
End Namespace
