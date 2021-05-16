Imports Microsoft.EntityFrameworkCore
Imports Microsoft.EntityFrameworkCore.Metadata.Builders

Namespace EntityTypeConfiguration
    Friend Class MyContext
        Inherits DbContext

        Public Property Blogs As DbSet(Of Blog)

        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
#Region "ApplyIEntityTypeConfiguration"
            With New BlogEntityTypeConfiguration
                .Configure(modelBuilder.Entity(Of Blog)())
            End With
#End Region

#Region "ApplyConfigurationsFromAssembly"
            modelBuilder.ApplyConfigurationsFromAssembly(GetType(BlogEntityTypeConfiguration).Assembly)
#End Region
        End Sub
    End Class

#Region "IEntityTypeConfiguration"
    Public Class BlogEntityTypeConfiguration
        Implements IEntityTypeConfiguration(Of Blog)

        Public Sub Configure(builder As EntityTypeBuilder(Of Blog)) Implements IEntityTypeConfiguration(Of Blog).Configure
            builder.
                Property(Function(b) b.Url).
                IsRequired()
        End Sub
    End Class
#End Region

    Public Class Blog
        Public Property BlogId As Integer
        Public Property Url As String
    End Class
End Namespace
