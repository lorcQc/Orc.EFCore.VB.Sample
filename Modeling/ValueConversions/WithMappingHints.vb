Imports Microsoft.EntityFrameworkCore
Imports Microsoft.EntityFrameworkCore.Diagnostics
Imports Microsoft.EntityFrameworkCore.Storage.ValueConversion
Public Class WithMappingHints

    Public Sub Run()
        ConsoleWriteLines("Sample showing value conversions with mapping hints for facets...")
        Using context As SampleDbContext = New SampleDbContext
            CleanDatabase(context)

            ConsoleWriteLines("Save a entities...")

            context.Add(New Order1 With
{
                .Price = New Dollars(3.99D)})
            context.Add(New Order2 With
{
                .Price = New Dollars(3.99D)})
            context.SaveChanges()
        End Using
        Using context As SampleDbContext = New SampleDbContext
            ConsoleWriteLines("Read the entities back...")

            Dim entity1 = context.[Set](Of Order1)().[Single]()
            Dim entity2 = context.[Set](Of Order2)().[Single]()
        End Using

        ConsoleWriteLines("Sample finished.")
    End Sub
    Public Class SampleDbContext
        Inherits DbContext

        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
#Region "ConverterWithMappingHints"
            Dim converter As New ValueConverter(Of Dollars, Decimal)(
                Function(v) v.Amount,
                Function(v) New Dollars(v),
                New ConverterMappingHints(precision:=16, scale:=2))
#End Region

            modelBuilder.Entity(Of Order1)().Property(Function(e) e.Price).
                HasConversion(converter)

#Region "ConfigureWithFacets"
            modelBuilder.Entity(Of Order2)().Property(Function(e) e.Price).
                HasConversion(converter).
                HasPrecision(20, 2)
#End Region
        End Sub
        Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
            Call optionsBuilder.LogTo(AddressOf Console.WriteLine, {RelationalEventId.CommandExecuted}).
                                UseSqlite("Data Source=sample.db").
                                EnableSensitiveDataLogging()
        End Sub
    End Class

    Public Class Order1
        Public Property Id As Integer
        Public Property Price As Dollars
    End Class

    Public Class Order2
        Public Property Id As Integer
        Public Property Price As Dollars
    End Class

    Public Structure Dollars
        Public ReadOnly Property Amount As Decimal
        Public Sub New(amount As Decimal)
            Me.Amount = amount
        End Sub
    End Structure
End Class
