Imports System.Text.Json
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.EntityFrameworkCore.ChangeTracking
Imports Microsoft.EntityFrameworkCore.Diagnostics
Public Class PrimitiveCollection

    Public Sub Run()
        ConsoleWriteLines("Sample showing value conversions for a collections of primitive values...")
        Using context As SampleDbContext = New SampleDbContext
            CleanDatabase(context)

            ConsoleWriteLines("Save a new entity...")

            context.Add(New Post With
{
                .Tags = New List(Of String) From {
                    "EF Core",
                    "Unicorns",
                    "Donkeys"}})
            context.SaveChanges()
        End Using

        Using context As SampleDbContext = New SampleDbContext
            ConsoleWriteLines("Read the entity back...")

            Dim post1 = context.Set(Of Post)().Single()

            ConsoleWriteLines($"Post with tags {String.Join(", ", post1.Tags)}.")

            ConsoleWriteLines("Changing the value object and saving again")

            post1.Tags.Add("ASP.NET Core")
            context.SaveChanges()
        End Using

        ConsoleWriteLines("Sample finished.")
    End Sub
    Public Class SampleDbContext
        Inherits DbContext

        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
#Region "ConfigurePrimitiveCollection"
            modelBuilder.Entity(Of Post)().Property(Function(e) e.Tags).
                HasConversion(
                    Function(v) JsonSerializer.Serialize(v, Nothing),
                    Function(v) JsonSerializer.Deserialize(Of List(Of String))(v, Nothing),
                    New ValueComparer(Of ICollection(Of String))(
                        Function(c1, c2) c1.SequenceEqual(c2),
                        Function(c) c.Aggregate(0, Function(a, v) HashCode.Combine(a, v.GetHashCode())),
                        Function(c) CType(c.ToList(), ICollection(Of String))))
#End Region
        End Sub
        Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
            Call optionsBuilder.LogTo(AddressOf Console.WriteLine, {RelationalEventId.CommandExecuted}).
                                UseSqlite("Data Source=sample.db").
                                EnableSensitiveDataLogging()
        End Sub
    End Class

#Region "PrimitiveCollectionModel"
    Public Class Post
        Public Property Id As Integer
        Public Property Title As String
        Public Property Contents As String
        Public Property Tags As ICollection(Of String)
    End Class
End Class
#End Region
