Imports Microsoft.EntityFrameworkCore

Namespace RelatedData
    Public NotInheritable Class Sample
        Public Shared Sub Run()

            Using context As New BloggingContext
                context.Database.EnsureDeleted()
                context.Database.EnsureCreated()
            End Using

#Region "AddingGraphOfEntities"
            Using context As BloggingContext = New BloggingContext
                Dim blog1 As New Blog With {
                    .Url = "http://blogs.msdn.com/dotnet",
                    .Posts = New List(Of Post) From {
                    New Post With {
                        .Title = "Intro to C#"},
                    New Post With {
                        .Title = "Intro to VB.NET"},
                    New Post With {
                        .Title = "Intro to F#"}
                }
            }

                context.Blogs.Add(blog1)
                context.SaveChanges()
            End Using
#End Region

#Region "AddingRelatedEntity"
            Using context As New BloggingContext
                Dim blog1 = context.Blogs.Include(Function(b) b.Posts).First()
                Dim post1 As New Post With {
                    .Title = "Intro to EF Core"}

                blog1.Posts.Add(post1)
                context.SaveChanges()
            End Using
#End Region

#Region "ChangingRelationships"
            Using context As New BloggingContext
                Dim blog1 As New Blog With {
                    .Url = "http://blogs.msdn.com/visualstudio"}
                Dim post1 = context.Posts.First()

                post1.Blog = blog1
                context.SaveChanges()
            End Using
#End Region

#Region "RemovingRelationships"
            Using context As New BloggingContext
                Dim blog1 = context.Blogs.Include(Function(b) b.Posts).First()
                Dim post1 = blog1.Posts.First()

                blog1.Posts.Remove(post1)
                context.SaveChanges()
            End Using
#End Region
        End Sub

        Public Class BloggingContext
            Inherits DbContext
            Public Property Blogs As DbSet(Of Blog)
            Public Property Posts As DbSet(Of Post)
            Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
                optionsBuilder.UseSqlServer(
                    "Server=(localdb)\mssqllocaldb;Database=sample_EFSaving.RelatedData;Trusted_Connection=True;ConnectRetryCount=0")
            End Sub
        End Class

        Public Class Blog
            Public Property BlogId As Integer
            Public Property Url As String

            Public Property Posts As List(Of Post)
        End Class

        Public Class Post
            Public Property PostId As Integer
            Public Property Title As String
            Public Property Content As String

            Public Property BlogId As Integer
            Public Property Blog As Blog
        End Class
    End Class
End Namespace
