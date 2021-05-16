Imports System.Text.Json
Imports System.Text.Json.Serialization
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.EntityFrameworkCore.Diagnostics

Public Class CompositeValueObject

    Public Sub Run()

        ConsoleWriteLines("Sample showing value conversions for a composite value object...")

        Using context As New SampleDbContext
            CleanDatabase(context)

            ConsoleWriteLines("Save a new entity...")

            context.Add(New Order With {
                .Price = New Money(3.99D, Currency.UsDollars)}
            )

            context.SaveChanges()
        End Using

        Using context As New SampleDbContext
            ConsoleWriteLines("Read the entity back...")

            Dim order1 = context.Set(Of Order)().Single()

            ConsoleWriteLines($"Order with price {order1.Price.Amount} in {order1.Price.Currency}.")
        End Using

        ConsoleWriteLines("Sample finished.")
    End Sub

    Public Class SampleDbContext
        Inherits DbContext
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)

#Region "ConfigureCompositeValueObject"
            modelBuilder.Entity(Of Order)().
                         Property(Function(e) e.Price).
                         HasConversion(Function(v) JsonSerializer.Serialize(v, Nothing),
                                       Function(v) JsonSerializer.Deserialize(Of Money)(v, Nothing))
#End Region

        End Sub
        Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
            optionsBuilder.LogTo(AddressOf Console.WriteLine, {RelationalEventId.CommandExecuted}).
                           UseSqlite("DataSource=sample.db").
                           EnableSensitiveDataLogging()
        End Sub
    End Class

#Region "CompositeValueObjectModel"
    Public Class Order
        Public Property Id As Integer

        Public Property Price As Money
    End Class
#End Region

#Region "CompositeValueObject"
    Public Structure Money

        <JsonConstructor>
        Public Sub New(amount As Decimal, currency As Currency)
            Me.Amount = amount
            Me.Currency = currency
        End Sub

        Public Overrides Function ToString() As String
            Return (If(Currency = Currency.UsDollars, "$", "£")) & Amount
        End Function

        Public ReadOnly Property Amount As Decimal
        Public ReadOnly Property Currency As Currency
    End Structure

    Public Enum Currency
        UsDollars
        PoundsStirling
    End Enum
End Class
#End Region

