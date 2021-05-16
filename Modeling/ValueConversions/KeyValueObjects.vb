Imports Microsoft.EntityFrameworkCore
Imports Microsoft.EntityFrameworkCore.Diagnostics
Imports Microsoft.EntityFrameworkCore.Storage.ValueConversion

Public Class KeyValueObjects

    Public Sub Run()
        ConsoleWriteLines("Sample showing value conversions for a value objects used as keys...")
        Using context As SampleDbContext = New SampleDbContext
            CleanDatabase(context)

            ConsoleWriteLines("Save a new entity...")

            Dim blog1 As Blog = New Blog With {
                .Id = New BlogKey(1),
                .Posts = New List(Of Post) From {
                    New Post With {
                        .Id = New PostKey(1)
                    },
                    New Post With {
                        .Id = New PostKey(2)
                    }
                }}

            context.Add(blog1)
            context.SaveChanges()
        End Using

        Using context As SampleDbContext = New SampleDbContext
            ConsoleWriteLines("Read the entity back...")
            Dim blog1 = context.Set(Of Blog)().Include(Function(e) e.Posts).Single()
        End Using

        ConsoleWriteLines("Sample finished.")
    End Sub
    Public Class SampleDbContext
        Inherits DbContext
#Region "ConfigureKeyValueObjects"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            Dim blogKeyConverter As New ValueConverter(Of BlogKey, Integer)(
                Function(v) v.Id,
                Function(v) New BlogKey(v))

            modelBuilder.Entity(Of Blog)().Property(Function(e) e.Id).HasConversion(blogKeyConverter)

            modelBuilder.Entity(Of Post)(
                    Sub(b)
                        b.Property(Function(e) e.Id).
                          HasConversion(Function(v) v.Id,
                                        Function(v) New PostKey(v))
                        b.Property(Function(e) e.BlogId).HasConversion(blogKeyConverter)
                    End Sub)
        End Sub
#End Region

        Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
            Call optionsBuilder.LogTo(AddressOf Console.WriteLine, {RelationalEventId.CommandExecuted}).
                                UseSqlite("DataSource=KeyValueObjects.db").
                                EnableSensitiveDataLogging()
        End Sub
    End Class

#Region "KeyValueObjectsModel"
    Public Class Blog
        Public Property Id As BlogKey
        Public Property Name As String
        Public Property Posts As ICollection(Of Post)
    End Class

    Public Class Post
        Public Property Id As PostKey
        Public Property Title As String
        Public Property Content As String
        Public Property BlogId As BlogKey?
        Public Property Blog As Blog
    End Class

#End Region

#Region "KeyValueObjects"
    Public Structure BlogKey
        Public Sub New(id1 As Integer)
            Me.Id = id1
        End Sub
        Public ReadOnly Property Id As Integer
    End Structure
    Public Structure PostKey
        Public ReadOnly Property Id As Integer

        Public Sub New(id1 As Integer)
            Me.Id = id1
        End Sub
    End Structure
End Class
#End Region
