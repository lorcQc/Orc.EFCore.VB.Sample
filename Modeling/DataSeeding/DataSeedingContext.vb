Imports Microsoft.EntityFrameworkCore

Public Class DataSeedingContext
    Inherits DbContext

    Public Property Blogs As DbSet(Of Blog)
    Public Property Posts As DbSet(Of Post)

    Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
        Call optionsBuilder.
                UseSqlServer("Server=(localdb)\mssqllocaldb;Database=sample_EFDataSeeding;Trusted_Connection=True;ConnectRetryCount=0")
    End Sub

    Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)

        modelBuilder.Entity(Of Blog)(Sub(e)
                                         e.Property(Function(x) x.Url).IsRequired()
                                     End Sub)

#Region "BlogSeed"
        modelBuilder.Entity(Of Blog)().HasData(
            New Blog With {
            .BlogId = 1,
            .Url = "http://sample.com"})
#End Region

        modelBuilder.Entity(Of Post)(
            Sub(e)
                e.HasOne(Function(d) d.Blog).
                WithMany(Function(p) p.Posts).
                HasForeignKey("BlogId")
            End Sub)

#Region "PostSeed"
        modelBuilder.Entity(Of Post)().HasData(
            New Post With {
                .BlogId = 1,
                .PostId = 1,
                .Title = "First post",
                .Content = "Test 1"})
#End Region

#Region "AnonymousPostSeed"
        modelBuilder.Entity(Of Post)().HasData(
            New With {
            .BlogId = 1,
            .PostId = 2,
            .Title = "Second post",
            .Content = "Test 2"})
#End Region

#Region "OwnedTypeSeed"
        modelBuilder.Entity(Of Post)().
            OwnsOne(Function(p) p.AuthorName).
            HasData(New With {.PostId = 1, .First = "Andriy", .Last = "Svyryd"},
                    New With {.PostId = 2, .First = "Diego", .Last = "Vega"})
#End Region
    End Sub
End Class
