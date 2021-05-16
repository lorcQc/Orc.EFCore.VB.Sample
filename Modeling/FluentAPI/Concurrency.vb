Imports Microsoft.EntityFrameworkCore

Namespace Concurrency

    Friend Class MyContext
        Inherits DbContext

        Public Property People As DbSet(Of Person)

#Region "Concurrency"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.
                Entity(Of Person)().
                Property(Function(p) p.LastName).
                IsConcurrencyToken()
        End Sub
    End Class
#End Region

    Public Class Person
        Public Property PersonId As Integer
        Public Property LastName As String
        Public Property FirstName As String
    End Class
End Namespace
