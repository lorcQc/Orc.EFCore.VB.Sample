Imports Microsoft.EntityFrameworkCore
Imports Microsoft.EntityFrameworkCore.ChangeTracking
Imports Microsoft.EntityFrameworkCore.Diagnostics

Public Class CaseInsensitiveStrings

    Public Sub Run()
        ConsoleWriteLines("Sample showing value conversions for case-insensitive string keys...")

        Using context As New SampleDbContext()

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

        Using context As New SampleDbContext()

            ConsoleWriteLines("Read the entities back...")

            Dim blog = context.Set(Of Blog)().
                               Include(Function(e) e.Posts).Single()

            ConsoleWriteLines($"The blog has {blog.Posts.Count} posts with foreign keys ""{blog.Posts.First().BlogId}"" and ""{blog.Posts.Skip(1).First().BlogId}""")
        End Using

        ConsoleWriteLines("Sample finished.")
    End Sub

    Public Class SampleDbContext
        Inherits DbContext

#Region "ConfigureCaseInsensitiveStrings"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)

            Dim comparer = New ValueComparer(Of String)(
                Function(l, r) String.Equals(l, r, StringComparison.OrdinalIgnoreCase),
                Function(v) v.ToUpper().GetHashCode(),
                Function(v) v)

            modelBuilder.Entity(Of Blog)().
                Property(Function(e) e.Id).
                Metadata.SetValueComparer(comparer)

            modelBuilder.Entity(Of Post)(
                Sub(b)
                    b.Property(Function(e) e.Id).Metadata.SetValueComparer(comparer)
                    b.Property(Function(e) e.BlogId).Metadata.SetValueComparer(comparer)
                End Sub)
        End Sub
#End Region

        Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
            optionsBuilder.
                LogTo(AddressOf Console.WriteLine, {RelationalEventId.CommandExecuted}).
                UseSqlServer("Server=(localdb)\mssqllocaldb;Database=sample_CaseInsensitiveStrings;Integrated Security=True").
                EnableSensitiveDataLogging()
        End Sub
    End Class

#Region "CaseInsensitiveStringsModel"
    Public Class Blog
        Public Property Id As String
        Public Property Name As String

        Public Property Posts As ICollection(Of Post)

        Sub New()
            Posts = New HashSet(Of Post)
        End Sub
    End Class

    Public Class Post
        Public Property Id As String
        Public Property Title As String
        Public Property Content As String
        Public Property BlogId As String
        Public Property Blog As Blog
    End Class
#End Region
End Class
