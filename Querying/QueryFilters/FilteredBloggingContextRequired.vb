Imports Microsoft.EntityFrameworkCore

Public Class FilteredBloggingContextRequired
    Inherits DbContext
    Public Property Blogs As DbSet(Of Blog)
    Public Property Posts As DbSet(Of Post)

    Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
        optionsBuilder _
            .UseSqlServer(
                "Server=(localdb)\mssqllocaldb;Database=sample_Querying.QueryFilters.BloggingRequired;Trusted_Connection=True;ConnectRetryCount=0;")
    End Sub

    Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
        Dim setup As String = "OptionalNav"

        If setup = "Faulty" Then
            ' Incorrect setup - Required navigation used to reference entity that has query filter defined,
            ' but no query filter for the entity on the other side of the navigation.
#Region "IncorrectFilter"
            modelBuilder.Entity(Of Blog)().HasMany(Function(b) b.Posts).WithOne(Function(p) p.Blog).IsRequired()
            modelBuilder.Entity(Of Blog)().HasQueryFilter(Function(b) b.Url.Contains("fish"))
#End Region
        ElseIf setup = "OptionalNav" Then
            ' The relationship is marked as optional so dependent can exist even if principal is filtered out.
#Region "OptionalNavigation"
            modelBuilder.Entity(Of Blog)().HasMany(Function(b) b.Posts).WithOne(Function(p) p.Blog).IsRequired(False)
            modelBuilder.Entity(Of Blog)().HasQueryFilter(Function(b) b.Url.Contains("fish"))
#End Region
        ElseIf setup = "NavigationInFilter" Then
#Region "NavigationInFilter"
            modelBuilder.Entity(Of Blog)().HasMany(Function(b) b.Posts).WithOne(Function(p) p.Blog)
            modelBuilder.Entity(Of Blog)().HasQueryFilter(Function(b) b.Posts.Count > 0)
            modelBuilder.Entity(Of Post)().HasQueryFilter(Function(p) p.Title.Contains("fish"))
#End Region
        Else
            ' The relationship is still required but there is a matching filter configured on dependent side too,
            ' which matches principal side. So if principal is filtered out, the dependent would also be.
#Region "MatchingFilters"
            modelBuilder.Entity(Of Blog)().HasMany(Function(b) b.Posts).WithOne(Function(p) p.Blog).IsRequired()
            modelBuilder.Entity(Of Blog)().HasQueryFilter(Function(b) b.Url.Contains("fish"))
            modelBuilder.Entity(Of Post)().HasQueryFilter(Function(p) p.Blog.Url.Contains("fish"))
#End Region
        End If
    End Sub
End Class
