Imports System.Text.Json
Imports System.Text.Json.Serialization
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.EntityFrameworkCore.ChangeTracking
Imports Microsoft.EntityFrameworkCore.Diagnostics

Public Class ValueObjectCollection

    Public Sub Run()
        ConsoleWriteLines("Sample showing value conversions for a collection of value objects...")
        Using context As SampleDbContext = New SampleDbContext
            CleanDatabase(context)

            ConsoleWriteLines("Save a new entity...")

            context.Add(
                New Blog With {
                    .Finances = New List(Of AnnualFinance) From {
                                New AnnualFinance(2018, New Money(326.65D, Currency.UsDollars), New Money(125D, Currency.UsDollars)),
                                New AnnualFinance(2019, New Money(112.2D, Currency.UsDollars), New Money(125D, Currency.UsDollars)),
                             New AnnualFinance(2020, New Money(25.77D, Currency.UsDollars), New Money(125D, Currency.UsDollars))
                    }})

            context.SaveChanges()
        End Using

        Using context As SampleDbContext = New SampleDbContext
            ConsoleWriteLines("Read the entity back...")

            Dim blog1 = context.Set(Of Blog)().Single()

            ConsoleWriteLines($"Blog with finances {String.Join(", ", blog1.Finances.Select(Function(f) $"{f.Year}: I={f.Income} E={f.Expenses} R={f.Revenue}"))}.")

            ConsoleWriteLines("Changing the value object and saving again")

            blog1.Finances.Add(New AnnualFinance(2021, New Money(12D, Currency.UsDollars), New Money(125D, Currency.UsDollars)))
            context.SaveChanges()
        End Using

        ConsoleWriteLines("Sample finished.")
    End Sub
    Public Class SampleDbContext
        Inherits DbContext
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
#Region "ConfigureValueObjectCollection"
            modelBuilder.Entity(Of Blog)().Property(Function(e) e.Finances) _
                .HasConversion(
                    Function(v) JsonSerializer.Serialize(v, Nothing),
                    Function(v) JsonSerializer.Deserialize(Of List(Of AnnualFinance))(v, Nothing),
                    New ValueComparer(Of IList(Of AnnualFinance))(
                        Function(c1, c2) c1.SequenceEqual(c2),
                        Function(c) c.Aggregate(0, Function(a, v) HashCode.Combine(a, v.GetHashCode())),
                        Function(c) CType(c.ToList(), IList(Of AnnualFinance))))
#End Region
        End Sub
        Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
            Call optionsBuilder.LogTo(AddressOf Console.WriteLine, {RelationalEventId.CommandExecuted}).
                                UseSqlite("Data Source=sample.db").
                                EnableSensitiveDataLogging()
        End Sub
    End Class

#Region "ValueObjectCollection"
    Public Structure AnnualFinance
        <JsonConstructor>
        Public Sub New(year As Integer, income As Money, expenses As Money)
            Me.Year = year
            Me.Income = income
            Me.Expenses = expenses
        End Sub
        Public ReadOnly Property Year As Integer
        Public ReadOnly Property Income As Money
        Public ReadOnly Property Expenses As Money
        Public ReadOnly Property Revenue As Money
            Get
                Return New Money(Income.Amount - Expenses.Amount, Income.Currency)
            End Get
        End Property
    End Structure
#End Region

#Region "ValueObjectCollectionMoney"
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
#End Region

#Region "ValueObjectCollectionModel"
    Public Class Blog
        Public Property Id As Integer
        Public Property Name As String

        Public Property Finances As IList(Of AnnualFinance)
    End Class
End Class
#End Region
