Imports Microsoft.EntityFrameworkCore

Namespace Relational.ColumnDataType
    Friend Class MyContext
        Inherits DbContext

        Public Property Blogs As DbSet(Of Blog)

#Region "ColumnDataType"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.Entity(Of Blog)(
                Sub(eb)
                    eb.Property(Function(b) b.Url).HasColumnType("varchar(200)")
                    eb.Property(Function(b) b.Rating).HasColumnType("decimal(5, 2)")
                End Sub)
        End Sub
    End Class
#End Region

    Public Class Blog
        Public Property BlogId As Integer
        Public Property Url As String
        Public Property Rating As Decimal
    End Class
End Namespace
