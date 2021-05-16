Imports Microsoft.EntityFrameworkCore

Public Class OwnedEntityContext
    Inherits DbContext

    Public Property Orders As DbSet(Of Order)
    Public Property DetailedOrders As DbSet(Of DetailedOrder)

    Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
        Call optionsBuilder.
            UseSqlServer("Server=(localdb)\mssqllocaldb;Database=sample_EFOwnedEntity;Trusted_Connection=True;ConnectRetryCount=0")
    End Sub

    Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
#Region "OwnsOne"
        modelBuilder.
            Entity(Of Order)().
            OwnsOne(Function(p) p.ShippingAddress)
#End Region

#Region "OwnsOneString"
        modelBuilder.
            Entity(Of Order)().
            OwnsOne(GetType(StreetAddress), "ShippingAddress")
#End Region

#Region "ColumnNames"
        modelBuilder.
            Entity(Of Order)().
            OwnsOne(
                Function(o) o.ShippingAddress,
                Sub(sa)
                    sa.Property(Function(p) p.Street).
                        HasColumnName("ShipsToStreet")
                    sa.Property(Function(p) p.City).
                        HasColumnName("ShipsToCity")
                End Sub)
#End Region

#Region "Required"
        modelBuilder.Entity(Of Order)(
            Sub(ob)
                ob.OwnsOne(
                    Function(o) o.ShippingAddress,
                    Sub(sa)
                        sa.Property(Function(p) p.Street).
                            IsRequired()
                        sa.Property(Function(p) p.City).
                            IsRequired()
                    End Sub)

                ob.Navigation(Function(o) o.ShippingAddress).
                   IsRequired()
            End Sub)
#End Region

#Region "OwnsOneNested"
        modelBuilder.
            Entity(Of DetailedOrder)().
            OwnsOne(
                Function(p) p.OrderDetails,
                Sub(od)
                    od.WithOwner(Function(d) d.Order)
                    od.Navigation(Function(d) d.Order).UsePropertyAccessMode(PropertyAccessMode.Property)
                    od.OwnsOne(Function(c) c.BillingAddress)
                    od.OwnsOne(Function(c) c.ShippingAddress)
                End Sub)
#End Region

#Region "OwnsOneTable"
        modelBuilder.
            Entity(Of DetailedOrder)().
            OwnsOne(
                Function(p) p.OrderDetails,
                Sub(od)
                    od.ToTable("OrderDetails")
                End Sub)
#End Region

#Region "OwnsMany"
        modelBuilder.
            Entity(Of Distributor)().
            OwnsMany(
                Function(p) p.ShippingCenters,
                Sub(a)
                    a.WithOwner().HasForeignKey("OwnerId")
                    a.Property(Of Integer)("Id")
                    a.HasKey("Id")
                End Sub)
#End Region
    End Sub
End Class
