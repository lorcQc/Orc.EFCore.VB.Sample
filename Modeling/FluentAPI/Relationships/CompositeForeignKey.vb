Imports Microsoft.EntityFrameworkCore

Namespace Relationships.CompositeForeignKey
#Region "CompositeForeignKey"
    Friend Class MyContext
        Inherits DbContext

        Public Property Cars As DbSet(Of Car)
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)

            modelBuilder.Entity(Of Car)().
                HasKey(Function(c) New With {c.State, c.LicensePlate})

            modelBuilder.Entity(Of RecordOfSale)().HasOne(Function(s) s.Car).
                WithMany(Function(c) c.SaleHistory).
                HasForeignKey(Function(s) New With {s.CarState, s.CarLicensePlate})
        End Sub
    End Class

    Public Class Car
        Public Property State As String
        Public Property LicensePlate As String
        Public Property Make As String
        Public Property Model As String
        Public Property SaleHistory As List(Of RecordOfSale)
    End Class

    Public Class RecordOfSale
        Public Property RecordOfSaleId As Integer
        Public Property DateSold As Date
        Public Property Price As Decimal
        Public Property CarState As String
        Public Property CarLicensePlate As String
        Public Property Car As Car
    End Class
#End Region

End Namespace
