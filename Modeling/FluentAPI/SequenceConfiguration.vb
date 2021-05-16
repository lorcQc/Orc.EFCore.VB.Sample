Imports Microsoft.EntityFrameworkCore

Namespace Relational.SequenceConfiguration
    Friend Class MyContext
        Inherits DbContext

        Public Property Orders As DbSet(Of Order)

#Region "SequenceConfiguration"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.
                HasSequence(Of Integer)("OrderNumbers", schema:="shared").
                StartsAt(1000).
                IncrementsBy(5)
        End Sub
#End Region
    End Class

    Public Class Order
        Public Property OrderId As Integer
        Public Property OrderNo As Integer
        Public Property Url As String
    End Class
End Namespace
