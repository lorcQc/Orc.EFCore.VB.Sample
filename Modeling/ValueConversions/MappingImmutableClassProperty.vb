Imports Microsoft.EntityFrameworkCore
Imports Microsoft.Extensions.Logging

Public Class MappingImmutableClassProperty

    Public Sub Run()
        ConsoleWriteLines("Sample showing value conversions for a simple immutable class...")

        Using context As SampleDbContext = New SampleDbContext
            CleanDatabase(context)

            ConsoleWriteLines("Save a new entity...")

            Dim entity As MyEntityType = New MyEntityType With {
                    .MyProperty = New ImmutableClass(7)
            }

            context.Add(entity)
            context.SaveChanges()

            ConsoleWriteLines("Change the property value and save again...")

            ' This will be detected and EF will update the database on SaveChanges
            entity.MyProperty = New ImmutableClass(77)

            context.SaveChanges()
        End Using

        Using context As SampleDbContext = New SampleDbContext
            ConsoleWriteLines("Read the entity back...")
            Dim entity = context.Set(Of MyEntityType)().Single()
            Debug.Assert(entity.MyProperty.Value = 77)
        End Using

        ConsoleWriteLines("Sample finished.")
    End Sub

    Public Class SampleDbContext
        Inherits DbContext

        Private Shared ReadOnly Logger As ILoggerFactory = LoggerFactory.Create(Sub(x) x.AddConsole()) '.SetMinimumLevel(LogLevel.Debug))

        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)

#Region "ConfigureImmutableClassProperty"
            modelBuilder.Entity(Of MyEntityType)().
                         Property(Function(e) e.MyProperty).
                         HasConversion(Function(v) v.Value,
                                       Function(v) New ImmutableClass(v))
#End Region
        End Sub
        Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
            Call optionsBuilder.UseLoggerFactory(Logger).
                                UseSqlite("Data Source=sample.db").
                                EnableSensitiveDataLogging()
        End Sub
    End Class

    Public Class MyEntityType
        Public Property Id As Integer
        Public Property MyProperty As ImmutableClass
    End Class

#Region "SimpleImmutableClass"
    Public NotInheritable Class ImmutableClass
        Implements IEquatable(Of ImmutableClass)

        Public Sub New(value As Integer)
            Me.Value = value
        End Sub

        Public ReadOnly Property Value As Integer

        Private Overloads Function Equals(other As ImmutableClass) As Boolean _
            Implements IEquatable(Of ImmutableClass).Equals

            Return Value = other.Value
        End Function

        Public Overrides Function Equals(obj As Object) As Boolean
            Dim TempVar As Boolean = TypeOf obj Is ImmutableClass
            Dim other As ImmutableClass = obj
            Return ReferenceEquals(Me, obj) OrElse TempVar AndAlso Equals(other)
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return Value.GetHashCode()
        End Function

    End Class
End Class
#End Region
