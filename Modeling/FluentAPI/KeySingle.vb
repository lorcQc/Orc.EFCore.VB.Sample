Imports Microsoft.EntityFrameworkCore

Namespace KeySingle
    Friend Class MyContext
        Inherits DbContext

        Public Property Cars As DbSet(Of Car)

#Region "KeySingle"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.
                Entity(Of Car)().
                HasKey(Function(c) c.LicensePlate)
        End Sub
#End Region
    End Class

    Friend Class Car
        Public Property LicensePlate As String

        Public Property Make As String
        Public Property Model As String
    End Class
End Namespace
