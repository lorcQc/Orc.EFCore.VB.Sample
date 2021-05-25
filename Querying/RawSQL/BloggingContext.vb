Imports Microsoft.EntityFrameworkCore

Public Class BloggingContext
    Inherits DbContext

    Public Property Blogs As DbSet(Of Blog)

    Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
        modelBuilder.Entity(Of Blog)().HasData(
            New Blog With {
                .BlogId = 1,
                .Url = "https://devblogs.microsoft.com/dotnet",
                .Rating = 5},
            New Blog With {
                .BlogId = 2,
                .Url = "https://mytravelblog.com/",
                .Rating = 4})

        modelBuilder.Entity(Of Post)().HasData(
            New Post With {
                .PostId = 1,
                .BlogId = 1,
                .Title = "What's new",
                .Content = "Lorem ipsum dolor sit amet",
                .Rating = 5},
            New Post With {
                .PostId = 2,
                .BlogId = 2,
                .Title = "Around the World in Eighty Days",
                .Content = "consectetur adipiscing elit",
                .Rating = 5},
            New Post With {
                .PostId = 3,
                .BlogId = 2,
                .Title = "Glamping *is* the way",
                .Content = "sed do eiusmod tempor incididunt",
                .Rating = 4},
            New Post With {
                .PostId = 4,
                .BlogId = 2,
                .Title = "Travel in the time of pandemic",
                .Content = "ut labore et dolore magna aliqua",
                .Rating = 3})
    End Sub

    Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
        optionsBuilder.UseSqlServer("Server=(localdb)\mssqllocaldb;Database=sample_EFQuerying.RawSQL;Trusted_Connection=True;ConnectRetryCount=0")
    End Sub
End Class
