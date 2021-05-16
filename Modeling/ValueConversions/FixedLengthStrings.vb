Imports Microsoft.EntityFrameworkCore
Imports Microsoft.EntityFrameworkCore.ChangeTracking
Imports Microsoft.EntityFrameworkCore.Diagnostics
Imports Microsoft.EntityFrameworkCore.Storage.ValueConversion

Public Class FixedLengthStrings

    Public Sub Run()
        ConsoleWriteLines("Sample showing value conversions for fixed-length, case-insensitive string keys...")

        Using context As New SampleDbContext
            CleanDatabase(context)

            ConsoleWriteLines("Save new entities...")

            context.AddRange(
                New Blog With {
                    .Id = "dotnet",
                    .Name = ".NET Blog"
                },
                New Post With {
                    .Id = "post1",
                    .BlogId = "dotnet",
                    .Title = "Some good .NET stuff"
                },
                New Post With {
                    .Id = "Post2",
                    .BlogId = "DotNet",
                    .Title = "Some more good .NET stuff"
                })
            context.SaveChanges()
        End Using

        Using context As New SampleDbContext
            ConsoleWriteLines("Read the entities back...")

            Dim blog1 = context.Set(Of Blog)().Include(Function(e) e.Posts).Single()

            ConsoleWriteLines($"The blog has {blog1.Posts.Count} posts with foreign keys '{blog1.Posts.First().BlogId}' and '{blog1.Posts.Skip(1).First().BlogId}'")
        End Using

        ConsoleWriteLines("Sample finished.")
    End Sub

    Public Class SampleDbContext
        Inherits DbContext

#Region "ConfigureFixedLengthStrings"
        Protected Overrides Sub OnModelCreating(modelBuilder1 As ModelBuilder)

            Dim converter As New ValueConverter(Of String, String)(
                Function(v) v,
                Function(v) v.Trim())

            Dim comparer As New ValueComparer(Of String)(
                Function(l, r) String.Equals(l, r, StringComparison.OrdinalIgnoreCase),
                Function(v) v.ToUpper().GetHashCode(),
                Function(v) v)

            modelBuilder1.Entity(Of Blog)().Property(Function(e) e.Id).
                HasColumnType("char(20)").
                HasConversion(converter, comparer)

            modelBuilder1.Entity(Of Post)(
                Sub(b)
                    b.Property(Function(e) e.Id).HasColumnType("char(20)").HasConversion(converter, comparer)
                    b.Property(Function(e) e.BlogId).HasColumnType("char(20)").HasConversion(converter, comparer)
                End Sub)
        End Sub
#End Region

        Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
            Call optionsBuilder.LogTo(AddressOf Console.WriteLine, {RelationalEventId.CommandExecuted}).
                                UseSqlServer("Server=(localdb)\mssqllocaldb;Database=sample_FixedLengthStrings;Integrated Security=True").
                                EnableSensitiveDataLogging()
        End Sub
    End Class

#Region "FixedLengthStringsModel"
    Public Class Blog
        Public Property Id As String
        Public Property Name As String
        Public Property Posts As ICollection(Of Post)
    End Class

    Public Class Post
        Public Property Id As String
        Public Property Title As String
        Public Property Content As String
        Public Property BlogId As String
        Public Property Blog As Blog
    End Class
End Class
#End Region
