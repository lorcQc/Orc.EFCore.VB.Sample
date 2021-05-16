Imports Microsoft.EntityFrameworkCore
Imports Microsoft.EntityFrameworkCore.Infrastructure

Public Class DynamicContext
    Inherits DbContext
    Public Property UseIntProperty As Boolean

    Public Property Entities As DbSet(Of ConfigurableEntity)

#Region "OnConfiguring"
    Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
        Call optionsBuilder _
                        .UseInMemoryDatabase("DynamicContext") _
                        .ReplaceService(Of IModelCacheKeyFactory, DynamicModelCacheKeyFactory)()
    End Sub
#End Region

#Region "OnModelCreating"
    Protected Overrides Sub OnModelCreating(modelBuilder1 As ModelBuilder)
        If UseIntProperty Then
            modelBuilder1.Entity(Of ConfigurableEntity)().Ignore(Function(e) e.StringProperty)
        Else
            modelBuilder1.Entity(Of ConfigurableEntity)().Ignore(Function(e) e.IntProperty)
        End If
    End Sub
#End Region
End Class
