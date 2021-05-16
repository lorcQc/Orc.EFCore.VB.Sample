Imports Microsoft.EntityFrameworkCore

Namespace Relational.AlternateKeyName

    Friend Class MyContext
        Inherits DbContext

        Public Property Cars As DbSet(Of Car)

#Region "AlternateKeyName"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.
                Entity(Of Car)().
                HasAlternateKey(Function(c) c.LicensePlate).
                HasName("AlternateKey_LicensePlate")
        End Sub
    End Class
#End Region

    Friend Class Car
        Public Property CarId As Integer
        Public Property LicensePlate As String
        Public Property Make As String
        Public Property Model As String
    End Class
End Namespace
