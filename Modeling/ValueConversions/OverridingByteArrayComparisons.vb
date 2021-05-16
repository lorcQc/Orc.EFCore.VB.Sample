Imports Microsoft.EntityFrameworkCore
Imports Microsoft.EntityFrameworkCore.ChangeTracking
Imports Microsoft.Extensions.Logging

Public Class OverridingByteArrayComparisons

    Public Sub Run()
        ConsoleWriteLines("Sample showing overriding byte array comparisons...")

        Using context As New SampleDbContext
            CleanDatabase(context)

            ConsoleWriteLines("Save a new entity...")

            Dim entity As New EntityType With {
                    .MyBytes = New Byte() {1, 2, 3}
            }

            context.Add(entity)
            context.SaveChanges()

            ConsoleWriteLines("Mutate the property value and save again...")

            ' Normally mutating the byte array would not be detected by EF Core.
            ' In this case it will be detected because the comparer in the model is overridden.
            entity.MyBytes(1) = 4

            context.SaveChanges()
        End Using

        Using context As New SampleDbContext
            ConsoleWriteLines("Read the entity back...")

            Dim entity = context.Set(Of EntityType)().Single()
            Debug.Assert(entity.MyBytes.SequenceEqual(New Byte() {1, 4, 3}))
        End Using

        ConsoleWriteLines("Sample finished.")
    End Sub
    Public Class SampleDbContext
        Inherits DbContext

        Private Shared ReadOnly Logger As ILoggerFactory = LoggerFactory.Create(Sub(x) x.AddConsole()) '.SetMinimumLevel(LogLevel.Debug));

        Protected Overrides Sub OnModelCreating(modelBuilder1 As ModelBuilder)
#Region "OverrideComparer"
            modelBuilder1 _
                    .Entity(Of EntityType)().[Property](Function(e) e.MyBytes) _
                    .Metadata _
                    .SetValueComparer(
                        New ValueComparer(Of Byte())(
                            Function(c1, c2) c1.SequenceEqual(c2),
                            Function(c) c.Aggregate(0, Function(a, v) HashCode.Combine(a, v.GetHashCode())),
                            Function(c) c.ToArray()))
#End Region
        End Sub

        Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
            Call optionsBuilder.UseLoggerFactory(Logger).
                                UseSqlite("Data Source=sample.db").
                                EnableSensitiveDataLogging()
        End Sub
    End Class

    Public Class EntityType
        Public Property Id As Integer
        Public Property MyBytes As Byte()
    End Class
End Class
