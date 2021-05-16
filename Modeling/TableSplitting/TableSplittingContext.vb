Imports Microsoft.EntityFrameworkCore
Public Class TableSplittingContext
    Inherits DbContext

    Public Property Orders As DbSet(Of Order)
    Public Property DetailedOrders As DbSet(Of DetailedOrder)

    Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
        Call optionsBuilder.
            UseSqlServer("Server=(localdb)\mssqllocaldb;Database=sample_EFTableSplitting;Trusted_Connection=True;ConnectRetryCount=0")
    End Sub

    Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
#Region "TableSplitting"
        modelBuilder.Entity(Of DetailedOrder)(
            Sub(dob)
                dob.ToTable("Orders")
                dob.Property(Function(o) o.Status).HasColumnName("Status")
            End Sub)

        modelBuilder.Entity(Of Order)(
            Sub(ob)
                ob.ToTable("Orders")
                ob.Property(Function(o) o.Status).HasColumnName("Status")
                ob.HasOne(Function(o) o.DetailedOrder).WithOne().HasForeignKey(Of DetailedOrder)(Function(o) o.Id)
            End Sub)
#End Region

#Region "ConcurrencyToken"
        modelBuilder.
            Entity(Of Order)().
            Property(Of Byte())("Version").
            IsRowVersion().
            HasColumnName("Version")

        modelBuilder.
            Entity(Of DetailedOrder)().
            Property(Function(o) o.Version).
            IsRowVersion().
            HasColumnName("Version")
#End Region
    End Sub
End Class
