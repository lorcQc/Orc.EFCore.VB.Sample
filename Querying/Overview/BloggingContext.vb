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
    End Sub

    Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
        optionsBuilder.UseSqlServer(
            "Server=(localdb)\mssqllocaldb;Database=sample_EFQuerying.Overview;Trusted_Connection=True;ConnectRetryCount=0")
    End Sub
End Class
