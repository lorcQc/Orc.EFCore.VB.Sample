Imports Microsoft.EntityFrameworkCore

Public Class BloggingContext
    Inherits DbContext
    Public Property Blogs As DbSet(Of Blog)
    Public Property Posts As DbSet(Of Post)

    Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
        modelBuilder.Entity(Of Blog)().HasMany(Function(b) b.Posts).
            WithOne(Function(p) p.Blog).
            OnDelete(DeleteBehavior.NoAction)

        modelBuilder.Entity(Of PostTag)().HasOne(Function(pt) pt.Post).
            WithMany(Function(p) p.Tags).
            HasForeignKey(Function(pt) pt.PostId)

        modelBuilder.Entity(Of PostTag)().HasOne(Function(pt) pt.Tag).
            WithMany(Function(t) t.Posts).
            HasForeignKey(Function(pt) pt.TagId)

        modelBuilder.Entity(Of Blog)().HasData(
            New Blog With {
                .BlogId = 1,
                .Url = "https://devblogs.microsoft.com/dotnet",
                .Rating = 5,
                .OwnerId = 1},
            New Blog With {
                .BlogId = 2,
                .Url = "https://mytravelblog.com/",
                .Rating = 4,
                .OwnerId = 3})

        modelBuilder.Entity(Of Post)().HasData(
            New Post With {
                .PostId = 1,
                .BlogId = 1,
                .Title = "What's new",
                .Content = "Lorem ipsum dolor sit amet",
                .Rating = 5,
                .AuthorId = 1},
            New Post With {
                .PostId = 2,
                .BlogId = 2,
                .Title = "Around the World in Eighty Days",
                .Content = "consectetur adipiscing elit",
                .Rating = 5,
                .AuthorId = 2},
            New Post With {
                .PostId = 3,
                .BlogId = 2,
                .Title = "Glamping *is* the way",
                .Content = "sed do eiusmod tempor incididunt",
                .Rating = 4,
                .AuthorId = 3},
            New Post With {
                .PostId = 4,
                .BlogId = 2,
                .Title = "Travel in the time of pandemic",
                .Content = "ut labore et dolore magna aliqua",
                .Rating = 3,
                .AuthorId = 3})

        modelBuilder.Entity(Of Person)().HasData(
            New Person With {
                .PersonId = 1,
                .Name = "Dotnet Blog Admin",
                .PhotoId = 1},
            New Person With {
                .PersonId = 2,
                .Name = "Phileas Fogg",
                .PhotoId = 2},
            New Person With {
                .PersonId = 3,
                .Name = "Jane Doe",
                .PhotoId = 3})

        modelBuilder.Entity(Of PersonPhoto)().HasData(
            New PersonPhoto With {
                .PersonPhotoId = 1,
                .Caption = "SN",
                .Photo = New Byte() {&H0, &H1}},
            New PersonPhoto With {
                .PersonPhotoId = 2,
                .Caption = "PF",
                .Photo = New Byte() {&H1, &H2, &H3}},
            New PersonPhoto With {
                .PersonPhotoId = 3,
                .Caption = "JD",
                .Photo = New Byte() {&H1, &H1, &H1}})

        modelBuilder.Entity(Of Tag)().HasData(
            New Tag With {
                .TagId = "general"},
            New Tag With {
                .TagId = "classic"},
            New Tag With {
                .TagId = "opinion"},
            New Tag With {
                .TagId = "informative"})

        modelBuilder.Entity(Of PostTag)().HasData(
            New PostTag With {
                .PostTagId = 1,
                .PostId = 1,
                .TagId = "general"},
            New PostTag With {
                .PostTagId = 2,
                .PostId = 1,
                .TagId = "informative"},
            New PostTag With {
                .PostTagId = 3,
                .PostId = 2,
                .TagId = "classic"},
            New PostTag With {
                .PostTagId = 4,
                .PostId = 3,
                .TagId = "opinion"},
            New PostTag With {
                .PostTagId = 5,
                .PostId = 4,
                .TagId = "opinion"},
            New PostTag With {
                .PostTagId = 6,
                .PostId = 4,
                .TagId = "informative"})
    End Sub
    Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
        optionsBuilder.UseSqlServer(
            "Server=(localdb)\mssqllocaldb;Database=sample_EFQuerying.ComplexQuery;Trusted_Connection=True;ConnectRetryCount=0")
    End Sub
End Class
