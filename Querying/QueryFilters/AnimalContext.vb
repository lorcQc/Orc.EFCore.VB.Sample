Imports Microsoft.EntityFrameworkCore

Public Class AnimalContext
    Inherits DbContext

    Public Property People As DbSet(Of Person)
    Public Property Animals As DbSet(Of Animal)
    Public Property Toys As DbSet(Of Toy)

    Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
        optionsBuilder.UseSqlServer(
                "Server=(localdb)\mssqllocaldb;Database=sample_Querying.QueryFilters.Animals;Trusted_Connection=True;ConnectRetryCount=0;")
    End Sub

    Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
        modelBuilder.Entity(Of Cat)().
            HasOne(Function(c) c.Tolerates).
            WithOne(Function(d) d.FriendsWith).
            HasForeignKey(Of Cat)(Function(c) c.ToleratesId)

        modelBuilder.Entity(Of Dog)().
            HasOne(Function(d) d.FavoriteToy).
            WithOne(Function(t) t.BelongsTo).
            HasForeignKey(Of Toy)(Function(d) d.BelongsToId)

        modelBuilder.Entity(Of Person)().HasQueryFilter(Function(p) p.Pets.Count > 0)
        modelBuilder.Entity(Of Animal)().HasQueryFilter(Function(a) Not a.Name.StartsWith("P"))
        modelBuilder.Entity(Of Toy)().HasQueryFilter(Function(a) a.Name.Length > 5)
        'Invalid query filter configuration as it causes cycles in query filters
        'modelBuilder.Entity(Of Animal).HasQueryFilter(Function(a) a.Owner.Name <> "John")
    End Sub
End Class
