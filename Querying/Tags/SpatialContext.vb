Imports Microsoft.EntityFrameworkCore
Imports NetTopologySuite.Geometries

Public Class SpatialContext
    Inherits DbContext

    Public Property People As DbSet(Of Person)

    Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
        modelBuilder.Entity(Of Person)(
            Sub(b)
                b.Property(Function(e) e.Location).HasColumnType("geometry")
                b.HasData(
                    New Person With {
                        .Id = 1,
                        .Location = New Point(0, 1)},
                    New Person With {
                        .Id = 2,
                        .Location = New Point(2, 1)},
                    New Person With {
                        .Id = 3,
                        .Location = New Point(4, 5)})
            End Sub)
    End Sub

    Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
        optionsBuilder.UseSqlServer(
            "Server=(localdb)\mssqllocaldb;Database=sample_EFQuerying.Tags;Trusted_Connection=True;ConnectRetryCount=0",
            Function(b) b.UseNetTopologySuite())
    End Sub
End Class
