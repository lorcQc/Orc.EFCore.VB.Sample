Imports System
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.Extensions.Logging

Module Program
    Sub Main()
        SetupDatabase()
        Using db As New BloggingContext
#Region "Query"
            Dim postCounts = db.BlogPostCounts.ToList()

            For Each postCount In postCounts
                Console.WriteLine($"{postCount.BlogName} has {postCount.PostCount} posts.")
                Console.WriteLine()
            Next
#End Region
        End Using
    End Sub

    Private Sub SetupDatabase()
        Using db As New BloggingContext
            If db.Database.EnsureCreated() Then
                db.Blogs.Add(
                    New Blog With {
                        .Name = "Fish Blog",
                        .Url = "http://sample.com/blogs/fish",
                        .Posts = New List(Of Post) From {
                            New Post With {
                                .Title = "Fish care 101"},
                            New Post With {
                                .Title = "Caring for tropical fish"},
                            New Post With {
                                .Title = "Types of ornamental fish"}
                        }
                    })

                db.Blogs.Add(
                    New Blog With {
                        .Name = "Cats Blog",
                        .Url = "http://sample.com/blogs/cats",
                        .Posts = New List(Of Post) From {
                            New Post With {
                                .Title = "Cat care 101"},
                            New Post With {
                                .Title = "Caring for tropical cats"},
                            New Post With {
                                .Title = "Types of ornamental cats"}
                        }
                    })

                db.Blogs.Add(
                    New Blog With {
                        .Name = "Catfish Blog",
                        .Url = "http://sample.com/blogs/catfish",
                        .Posts = New List(Of Post) From {
                            New Post With {
                                .Title = "Catfish care 101"},
                            New Post With {
                                .Title = "History of the catfish name"}
                        }
                    })

                db.SaveChanges()

#Region "View"
                db.Database.ExecuteSqlRaw(
                    "CREATE VIEW View_BlogPostCounts AS
                            SELECT b.Name, Count(p.PostId) as PostCount
                            FROM Blogs b
                            JOIN Posts p on p.BlogId = b.BlogId
                            GROUP BY b.Name;")
#End Region
            End If
        End Using
    End Sub

End Module

Public Class BloggingContext
    Inherits DbContext

    Private Shared ReadOnly _loggerFactory As ILoggerFactory = LoggerFactory.Create(
            Function(builder) builder.AddConsole().AddFilter(Function(c, l) l = LogLevel.Information AndAlso Not c.EndsWith("Connection")))

    Public Property Blogs As DbSet(Of Blog)
    Public Property Posts As DbSet(Of Post)

#Region "DbSet"
    Public Property BlogPostCounts As DbSet(Of BlogPostsCount)
#End Region

    Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
        optionsBuilder.
            UseSqlServer("Server=(localdb)\mssqllocaldb;Database=sample_KeylessEntityTypes;Trusted_Connection=True;ConnectRetryCount=0;").
            UseLoggerFactory(_loggerFactory)
    End Sub

#Region "Configuration"
    Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
        modelBuilder.Entity(Of BlogPostsCount)(
                Sub(eb)
                    eb.HasNoKey()
                    eb.ToView("View_BlogPostCounts")
                    eb.Property(Function(v) v.BlogName).HasColumnName("Name")
                End Sub)
    End Sub
End Class
#End Region

#Region "Entities"
Public Class Blog
    Public Property BlogId As Integer
    Public Property Name As String
    Public Property Url As String
    Public Property Posts As ICollection(Of Post)
End Class

Public Class Post
    Public Property PostId As Integer
    Public Property Title As String
    Public Property Content As String
    Public Property BlogId As Integer
End Class
#End Region

#Region "KeylessEntityType"
Public Class BlogPostsCount
    Public Property BlogName As String
    Public Property PostCount As Integer
End Class

#End Region