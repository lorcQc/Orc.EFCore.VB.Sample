Imports Microsoft.EntityFrameworkCore

Namespace Relational.TableName
    Friend Class MyContext
        Inherits DbContext

        Public Property Blogs As DbSet(Of Blog)

#Region "TableName"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.
                Entity(Of Blog)().
                ToTable("blogs")
        End Sub
    End Class
#End Region

    Public Class Blog
        Public Property BlogId As Integer
        Public Property Url As String
    End Class
End Namespace
