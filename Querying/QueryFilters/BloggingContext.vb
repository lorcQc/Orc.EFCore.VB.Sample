Imports Microsoft.EntityFrameworkCore

Public Class BloggingContext
    Inherits DbContext

    Private ReadOnly _tenantId As String

    Public Sub New(tenant As String)
        _tenantId = tenant
    End Sub

    Public Property Blogs As DbSet(Of Blog)
    Public Property Posts As DbSet(Of Post)

    Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
        optionsBuilder.UseSqlServer(
                "Server=(localdb)\mssqllocaldb;Database=sample_Querying.QueryFilters.Blogging;Trusted_Connection=True;ConnectRetryCount=0;")
    End Sub

    Protected Overrides Sub OnModelCreating(modelBuilder1 As ModelBuilder)
        modelBuilder1.Entity(Of Blog)().[Property](Of String)("_tenantId").HasColumnName("TenantId")

        ' Configure entity filters
#Region "FilterConfiguration"
        modelBuilder1.Entity(Of Blog)().HasQueryFilter(Function(b) EF.Property(Of String)(b, "_tenantId") = _tenantId)
        modelBuilder1.Entity(Of Post)().HasQueryFilter(Function(p) Not p.IsDeleted)
#End Region
    End Sub

    Public Overrides Function SaveChanges() As Integer
        ChangeTracker.DetectChanges()

        For Each item In ChangeTracker.Entries().Where(
            Function(e) e.State = EntityState.Added AndAlso e.Metadata.GetProperties().Any(Function(p) p.Name = "_tenantId"))
            item.CurrentValues("_tenantId") = _tenantId
        Next

        For Each item In ChangeTracker.Entries(Of Post)().Where(Function(e) e.State = EntityState.Deleted)
            item.State = EntityState.Modified
            item.CurrentValues("IsDeleted") = True
        Next

        Return MyBase.SaveChanges()
    End Function
End Class
