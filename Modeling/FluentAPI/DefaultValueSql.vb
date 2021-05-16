Imports Microsoft.EntityFrameworkCore

Namespace Relational.DefaultValueSql
    Friend Class MyContext
        Inherits DbContext

        Public Property Blogs As DbSet(Of Blog)

#Region "DefaultValueSql"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.
                Entity(Of Blog)().
                Property(Function(b) b.Created).
                HasDefaultValueSql("getdate()")
        End Sub
#End Region
    End Class

    Public Class Blog
        Public Property BlogId As Integer
        Public Property Url As String
        Public Property Created As DateTime
    End Class
End Namespace
