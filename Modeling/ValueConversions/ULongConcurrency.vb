Imports Microsoft.EntityFrameworkCore
Imports Microsoft.EntityFrameworkCore.Diagnostics

Public Class ULongConcurrency

    Public Sub Run()
        ConsoleWriteLines("Sample showing how to map rowversion to ulong...")

        Using context As New SampleDbContext
            CleanDatabase(context)

            ConsoleWriteLines("Save a new entity...")

            context.Add(
                New Blog With {
                    .Name = "OneUnicorn"
                })
            context.SaveChanges()
        End Using

        Using context As New SampleDbContext
            ConsoleWriteLines("Read the entity back in one context...")

            Dim blog1 = context.Set(Of Blog)().Single()
            blog1.Name = "TwoUnicorns"
            Using context2 As SampleDbContext = New SampleDbContext
                ConsoleWriteLines("Change the blog name and save in a different context...")

                context2.Set(Of Blog)().Single().Name = "1unicorn2"
                context2.SaveChanges()
            End Using

            Try
                ConsoleWriteLines("Change the blog name and save in the first context...")

                context.SaveChanges()
            Catch e As DbUpdateConcurrencyException
                ConsoleWriteLines($"{e.GetType().FullName}: {e.Message}")

                Dim databaseValues = context.Entry(blog1).GetDatabaseValues()
                context.Entry(blog1).OriginalValues.SetValues(databaseValues)

                ConsoleWriteLines("Refresh original values and save again...")

                context.SaveChanges()
            End Try
        End Using

        ConsoleWriteLines("Sample finished.")
    End Sub

    Public Class SampleDbContext
        Inherits DbContext

        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
#Region "ConfigureULongConcurrency"
            modelBuilder.Entity(Of Blog)().
                Property(Function(e) e.Version).
                IsRowVersion().
                HasConversion(Of Byte())()
#End Region
        End Sub

        Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
            Call optionsBuilder.LogTo(AddressOf Console.WriteLine, {RelationalEventId.CommandExecuted}).
                                UseSqlServer("Server=(localdb)\mssqllocaldb;Database=sample_ULongConcurrency;Integrated Security=True").
                                EnableSensitiveDataLogging()
        End Sub
    End Class

#Region "ULongConcurrencyModel"
    Public Class Blog
        Public Property Id As Integer
        Public Property Name As String
        Public Property Version As ULong
    End Class
End Class
#End Region
