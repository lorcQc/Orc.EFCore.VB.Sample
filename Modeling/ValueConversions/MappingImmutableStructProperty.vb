Imports Microsoft.EntityFrameworkCore
Imports Microsoft.Extensions.Logging

Public Class MappingImmutableStructProperty

    Public Sub Run()
        ConsoleWriteLines("Sample showing value conversions for a simple immutable struct...")
        Using context As SampleDbContext = New SampleDbContext
            CleanDatabase(context)

            ConsoleWriteLines("Save a new entity...")

            Dim entity As EntityType = New EntityType With {
                .MyProperty = New ImmutableStruct(6)
            }

            context.Add(entity)
            context.SaveChanges()

            ConsoleWriteLines("Change the property value and save again...")

            ' This will be detected and EF will update the database on SaveChanges
            entity.MyProperty = New ImmutableStruct(66)

            context.SaveChanges()
        End Using

        Using context As SampleDbContext = New SampleDbContext
            ConsoleWriteLines("Read the entity back...")

            Dim entity = context.Set(Of EntityType)().Single()

            Debug.Assert(entity.MyProperty.Value = 66)
        End Using

        ConsoleWriteLines("Sample finished.")
    End Sub
    Public Class SampleDbContext
        Inherits DbContext
        Private Shared ReadOnly Logger As ILoggerFactory = LoggerFactory.Create(Function(x) x.AddConsole()) '.SetMinimumLevel(LogLevel.Debug));
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
#Region "ConfigureImmutableStructProperty"
            modelBuilder.
                Entity(Of EntityType)().
                    Property(Function(e) e.MyProperty).
                        HasConversion(Function(v) v.Value,
                                      Function(v) New ImmutableStruct(v))
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
        Public Property MyProperty As ImmutableStruct
    End Class

#Region "SimpleImmutableStruct"
    Public Structure ImmutableStruct

        Public ReadOnly Property Value As Integer

        Public Sub New(value As Integer)
            Me.Value = value
        End Sub

    End Structure
End Class
#End Region
