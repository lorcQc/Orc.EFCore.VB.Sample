Imports Microsoft.EntityFrameworkCore
Imports Microsoft.EntityFrameworkCore.Infrastructure

Public Class NullSemanticsContext
    Inherits DbContext

    Public Property Entities As DbSet(Of NullSemanticsEntity)

    Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
        Dim relationalNulls As Boolean = False

        If relationalNulls Then
#Region "UseRelationalNulls"
            Call New SqlServerDbContextOptionsBuilder(optionsBuilder).UseRelationalNulls()
#End Region
        End If

        optionsBuilder.UseSqlServer(
            "Server=(localdb)\mssqllocaldb;Database=sample_NullSemanticsSample;Trusted_Connection=True;MultipleActiveResultSets=true")
    End Sub

    Protected Overrides Sub OnModelCreating(modelBuilder1 As ModelBuilder)
        modelBuilder1.Entity(Of NullSemanticsEntity)().HasData(
            New NullSemanticsEntity With {
                    .Id = 1,
                    .Int = 1,
                    .NullableInt = 1,
                    .String1 = "A",
                    .String2 = "A"},
            New NullSemanticsEntity With {
                    .Id = 2,
                    .Int = 2,
                    .NullableInt = 2,
                    .String1 = "A",
                    .String2 = "B"},
            New NullSemanticsEntity With {
                    .Id = 3,
                    .Int = 2,
                    .NullableInt = Nothing,
                    .String1 = Nothing,
                    .String2 = "A"},
            New NullSemanticsEntity With {
                    .Id = 4,
                    .Int = 2,
                    .NullableInt = Nothing,
                    .String1 = "B",
                    .String2 = Nothing},
            New NullSemanticsEntity With {
                    .Id = 5,
                    .Int = 1,
                    .NullableInt = 3,
                    .String1 = Nothing,
                    .String2 = Nothing})
    End Sub
End Class
