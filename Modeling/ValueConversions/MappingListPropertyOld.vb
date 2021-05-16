Imports System.Text.Json
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.EntityFrameworkCore.ChangeTracking
Imports Microsoft.Extensions.Logging

Public Class MappingListPropertyOld

    Public Sub Run()
        ConsoleWriteLines("Sample showing value conversions for a List<int>...")
        Using context As New SampleDbContext
            CleanDatabase(context)

            ConsoleWriteLines("Save a new entity...")

            Dim entity As New EntityType With {
                .MyListProperty = New List(Of Integer) From {1, 2, 3}
            }

            context.Add(entity)
            context.SaveChanges()

            ConsoleWriteLines("Mutate the property value and save again...")

            ' This will be detected and EF will update the database on SaveChanges
            entity.MyListProperty.Add(4)

            context.SaveChanges()
        End Using

        Using context As New SampleDbContext
            ConsoleWriteLines("Read the entity back...")
            Dim entity = context.Set(Of EntityType)().Single()
            Debug.Assert(entity.MyListProperty.SequenceEqual(New List(Of Integer) From {1, 2, 3, 4}))
        End Using

        ConsoleWriteLines("Sample finished.")
    End Sub
    Public Class SampleDbContext
        Inherits DbContext

        Private Shared ReadOnly Logger As ILoggerFactory = LoggerFactory.Create(Function(x) x.AddConsole()) '.SetMinimumLevel(LogLevel.Debug));

        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
#Region "ConfigureListProperty"
            modelBuilder.Entity(Of EntityType)().
                         Property(Function(e) e.MyListProperty).
                         HasConversion(Function(v) JsonSerializer.Serialize(v, Nothing),
                                       Function(v) JsonSerializer.Deserialize(Of List(Of Integer))(v, Nothing))

            Dim valueComparer1 As New ValueComparer(Of List(Of Integer))(
                Function(c1, c2) c1.SequenceEqual(c2),
                Function(c) c.Aggregate(0, Function(a, v) HashCode.Combine(a, v.GetHashCode())),
                Function(c) c.ToList())

            modelBuilder.Entity(Of EntityType)().
                         Property(Function(e) e.MyListProperty).
                         Metadata.
                         SetValueComparer(valueComparer1)
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

#Region "ListProperty"
        Public Property MyListProperty As List(Of Integer)
    End Class
#End Region

End Class
