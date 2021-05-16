Imports Microsoft.EntityFrameworkCore

Namespace IndexComposite
    Friend Class MyContext
        Inherits DbContext

        Public Property People As DbSet(Of Person)

#Region "Composite"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.
                Entity(Of Person)().
                HasIndex(Function(p) New With {p.FirstName, p.LastName})
        End Sub
#End Region
    End Class

    Public Class Person
        Public Property PersonId As Integer
        Public Property FirstName As String
        Public Property LastName As String
    End Class
End Namespace
