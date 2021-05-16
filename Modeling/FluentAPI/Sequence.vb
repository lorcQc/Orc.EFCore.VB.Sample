Imports Microsoft.EntityFrameworkCore

Namespace Relational.Sequence
    Friend Class MyContext
        Inherits DbContext

        Public Property Orders As DbSet(Of Order)

#Region "Sequence"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.
                HasSequence(Of Integer)("OrderNumbers")

            modelBuilder.
                Entity(Of Order)().
                Property(Function(o) o.OrderNo).
                HasDefaultValueSql("NEXT VALUE FOR shared.OrderNumbers")
        End Sub
    End Class
#End Region

    Public Class Order
        Public Property OrderId As Integer
        Public Property OrderNo As Integer
        Public Property Url As String
    End Class
End Namespace
