Imports Microsoft.EntityFrameworkCore

Namespace PrecisionAndScale
    Friend Class MyContext
        Inherits DbContext

        Public Property Blogs As DbSet(Of Blog)

#Region "PrecisionAndScale"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.
                Entity(Of Blog)().
                Property(Function(b) b.Score).
                HasPrecision(14, 2)

            modelBuilder.
                Entity(Of Blog)().
                Property(Function(b) b.LastUpdated).
                HasPrecision(3)
        End Sub
    End Class
#End Region

    Public Class Blog
        Public Property BlogId As Integer
        Public Property Score As Decimal
        Public Property LastUpdated As DateTime
    End Class
End Namespace
