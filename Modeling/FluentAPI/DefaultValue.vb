Imports Microsoft.EntityFrameworkCore

Namespace Relational.DefaultValue

    Friend Class MyContext
        Inherits DbContext

        Public Property Blogs As DbSet(Of Blog)

#Region "DefaultValue"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.
                Entity(Of Blog)().
                Property(Function(b) b.Rating).
                HasDefaultValue(3)
        End Sub
#End Region
    End Class

    Public Class Blog
        Public Property BlogId As Integer
        Public Property Url As String
        Public Property Rating As Integer
    End Class
End Namespace
