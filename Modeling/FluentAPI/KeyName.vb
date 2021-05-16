Imports Microsoft.EntityFrameworkCore

Namespace Relational.KeyName
    Friend Class MyContext
        Inherits DbContext

        Public Property Blogs As DbSet(Of Blog)

#Region "KeyName"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.
                Entity(Of Blog)().
                HasKey(Function(b) b.BlogId).
                HasName("PrimaryKey_BlogId")
        End Sub
#End Region
    End Class

    Public Class Blog
        Public Property BlogId As Integer
        Public Property Url As String
    End Class
End Namespace
