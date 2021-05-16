Imports Microsoft.EntityFrameworkCore

Namespace AlternateKeyComposite
    Friend Class MyContext
        Inherits DbContext

        Public Property Cars As DbSet(Of Car)

#Region "AlternateKeyComposite"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.
                Entity(Of Car)().
                HasAlternateKey(Function(c) New With {c.State, c.LicensePlate})
        End Sub
    End Class
#End Region

    Friend Class Car
        Public Property CarId As Integer
        Public Property State As String
        Public Property LicensePlate As String
        Public Property Make As String
        Public Property Model As String
    End Class
End Namespace
