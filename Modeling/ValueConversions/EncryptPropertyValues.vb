Imports Microsoft.EntityFrameworkCore
Imports Microsoft.EntityFrameworkCore.Diagnostics

Public Class EncryptPropertyValues

    Public Sub Run()
        ConsoleWriteLines("Sample showing value conversions for encrypting property values...")

        Using context As New SampleDbContext
            CleanDatabase(context)

            ConsoleWriteLines("Save a new entity...")

            context.Add(New User With {
                .Name = "arthur",
                .Password = "password"
            })

            context.SaveChanges()
        End Using

        Using context As New SampleDbContext
            ConsoleWriteLines("Read the entity back...")

            Dim user = context.Set(Of User)().Single()

            ConsoleWriteLines($"User {user.Name} has password ""{user.Password}""")
        End Using

        ConsoleWriteLines("Sample finished.")
    End Sub

    Public Class SampleDbContext
        Inherits DbContext
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
#Region "ConfigureEncryptPropertyValues"
            modelBuilder.Entity(Of User)().Property(Function(e) e.Password).HasConversion(
                Function(v) New String(v.Reverse().ToArray()),
                Function(v) New String(v.Reverse().ToArray()))
#End Region
        End Sub

        Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
            Call optionsBuilder.LogTo(AddressOf Console.WriteLine, {RelationalEventId.CommandExecuted}).
                                UseSqlite("Data Source=sample.db").
                                EnableSensitiveDataLogging()
        End Sub
    End Class

#Region "EncryptPropertyValuesModel"
    Public Class User
        Public Property Id As Integer
        Public Property Name As String
        Public Property Password As String
    End Class
End Class
#End Region
