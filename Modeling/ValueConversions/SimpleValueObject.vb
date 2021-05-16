Imports Microsoft.EntityFrameworkCore
Imports Microsoft.EntityFrameworkCore.Diagnostics

Public Class SimpleValueObject

    Public Sub Run()
        ConsoleWriteLines("Sample showing value conversions for a simple value object...")

        Using context As SampleDbContext = New SampleDbContext
            CleanDatabase(context)

            ConsoleWriteLines("Save a new entity...")

            context.Add(
                New Order With {
                    .Price = New Dollars(3.99D)
                })
            context.SaveChanges()
        End Using

        Using context As SampleDbContext = New SampleDbContext
            ConsoleWriteLines("Read the entity back...")
            Dim entity = context.Set(Of Order)().Single()
        End Using

        ConsoleWriteLines("Sample finished.")
    End Sub

    Public Class SampleDbContext
        Inherits DbContext
        Protected Overrides Sub OnModelCreating(modelBuilder1 As ModelBuilder)
#Region "ConfigureImmutableStructProperty"
            modelBuilder1.Entity(Of Order)().Property(Function(e) e.Price).
                HasConversion(
                    Function(v) v.Amount,
                    Function(v) New Dollars(v))
#End Region
        End Sub

        Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
            Call optionsBuilder.LogTo(AddressOf Console.WriteLine, {RelationalEventId.CommandExecuted}).
                                UseSqlite("Data Source=sample.db").
                                EnableSensitiveDataLogging()
        End Sub
    End Class

#Region "SimpleValueObjectModel"
    Public Class Order
        Public Property Id As Integer
        Public Property Price As Dollars
    End Class

#End Region

#Region "SimpleValueObject"
    Public Structure Dollars
        Public ReadOnly Property Amount As Decimal

        Public Sub New(amount As Decimal)
            Me.Amount = amount
        End Sub

        Public Overrides Function ToString() As String
            Return $"${Amount}"
        End Function
    End Structure
End Class
#End Region
