Imports Microsoft.EntityFrameworkCore

Namespace AlternateKeySingle
    Friend Class MyContext
        Inherits DbContext

        Public Property Cars As DbSet(Of Car)

#Region "AlternateKeySingle"
        Protected Overrides Sub OnModelCreating(modelBuilder1 As ModelBuilder)
            modelBuilder1.Entity(Of Car)().HasAlternateKey(Function(c) c.LicensePlate)
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
