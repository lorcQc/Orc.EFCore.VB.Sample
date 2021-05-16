Imports Microsoft.EntityFrameworkCore

Namespace Relational.ComputedColumn
    Friend Class MyContext
        Inherits DbContext

        Public Property People As DbSet(Of Person)

        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
#Region "DefaultComputedColumn"
            modelBuilder.
                Entity(Of Person)().
                Property(Function(p) p.DisplayName).
                HasComputedColumnSql("[LastName] + ', ' + [FirstName]")
#End Region

#Region "StoredComputedColumn"
            modelBuilder.
                Entity(Of Person)().
                Property(Function(p) p.NameLength).
                HasComputedColumnSql("LEN([LastName]) + LEN([FirstName])", stored:=True)
#End Region
        End Sub
    End Class

    Public Class Person
        Public Property PersonId As Integer
        Public Property FirstName As String
        Public Property LastName As String
        Public Property DisplayName As String
        Public Property NameLength As Integer
    End Class
End Namespace
