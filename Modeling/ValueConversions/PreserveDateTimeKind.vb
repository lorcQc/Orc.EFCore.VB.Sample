Imports Microsoft.EntityFrameworkCore
Imports Microsoft.EntityFrameworkCore.Diagnostics

Public Class PreserveDateTimeKind

    Public Sub Run()
        ConsoleWriteLines("Sample showing value conversions for preserving/setting DateTime.Kind...")
        Using context As SampleDbContext = New SampleDbContext
            CleanDatabase(context)

            ConsoleWriteLines("Save new entities...")

            context.AddRange(
                New Post With {
                    .Title = "Post 1",
                    .PostedOn = New DateTime(1973, 9, 3, 0, 0, 0, 0, DateTimeKind.Utc),
                    .LastUpdated = New DateTime(1974, 9, 3, 0, 0, 0, 0, DateTimeKind.Utc),
                    .DeletedOn = New DateTime(2007, 9, 3, 0, 0, 0, 0, DateTimeKind.Utc)
                },
                New Post With {
                    .Title = "Post 2",
                    .PostedOn = New DateTime(1975, 9, 3, 0, 0, 0, 0, DateTimeKind.Local),
                    .LastUpdated = New DateTime(1976, 9, 3, 0, 0, 0, 0, DateTimeKind.Utc),
                    .DeletedOn = New DateTime(2017, 9, 3, 0, 0, 0, 0, DateTimeKind.Utc)
                })
            context.SaveChanges()
        End Using

        Using context As SampleDbContext = New SampleDbContext
            ConsoleWriteLines("Read the entities back...")

            Dim blog1 = context.Set(Of Post)().Single(Function(e) e.Title = "Post 1")

            ConsoleWriteLines($"Blog 1: PostedOn.Kind = {blog1.PostedOn.Kind} LastUpdated.Kind = {blog1.LastUpdated.Kind} DeletedOn.Kind = {blog1.DeletedOn.Kind}")

            Dim blog2 = context.Set(Of Post)().Single(Function(e) e.Title = "Post 2")

            ConsoleWriteLines($"Blog 2: PostedOn.Kind = {blog2.PostedOn.Kind} LastUpdated.Kind = {blog2.LastUpdated.Kind} DeletedOn.Kind = {blog2.DeletedOn.Kind}")
        End Using

        ConsoleWriteLines("Sample finished.")
    End Sub
    Public Class SampleDbContext
        Inherits DbContext
        Protected Overrides Sub OnModelCreating(modelBuilder1 As ModelBuilder)
#Region "ConfigurePreserveDateTimeKind1"
            modelBuilder1.Entity(Of Post)().Property(Function(e) e.PostedOn) _
                .HasConversion(Of Long)()
#End Region

#Region "ConfigurePreserveDateTimeKind2"
            modelBuilder1.Entity(Of Post)().Property(Function(e) e.LastUpdated) _
                .HasConversion(
                    Function(v) v,
                    Function(v) New Date(v.Ticks, DateTimeKind.Utc))
#End Region

#Region "ConfigurePreserveDateTimeKind3"
            modelBuilder1.Entity(Of Post)().Property(Function(e) e.LastUpdated).
                HasConversion(
                    Function(v) v.ToUniversalTime(),
                    Function(v) New DateTime(v.Ticks, DateTimeKind.Utc))
#End Region
        End Sub

        Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
            Call optionsBuilder.LogTo(AddressOf Console.WriteLine, {RelationalEventId.CommandExecuted}).
                                UseSqlite("Data Source=sample.db").
                                EnableSensitiveDataLogging()
        End Sub
    End Class

#Region "PreserveDateTimeKindModel"
    Public Class Post
        Public Property Id As Integer

        Public Property Title As String
        Public Property Content As String

        Public Property PostedOn As DateTime
        Public Property LastUpdated As DateTime
        Public Property DeletedOn As DateTime
    End Class
End Class
#End Region
