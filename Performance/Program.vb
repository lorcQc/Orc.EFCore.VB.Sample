Imports Microsoft.EntityFrameworkCore
Imports Performance.LazyLoading

Module Program
    Sub Main()
        Using context As New BloggingContext
            context.Database.EnsureDeleted()
            context.Database.EnsureCreated()

            context.Add(New Blog With {
                .Url = "http://someblog.microsoft.com",
                .Rating = 0,
                .Posts = New List(Of Post) From {
                    New Post With {
                        .Title = "Post 1",
                        .Content = "Sometimes..."},
                    New Post With {
                        .Title = "Post 2",
                        .Content = "Other times..."}
                }
            })

            context.SaveChanges()
        End Using

        Using context As New BloggingContext
#Region "Indexes"
            ' Matches on start, so uses an index (on SQL Server)
            Dim posts1 = context.Posts.Where(Function(p) p.Title.StartsWith("A")).ToList()
            ' Matches on end, so does not use the index
            Dim posts2 = context.Posts.Where(Function(p) p.Title.EndsWith("A")).ToList()
#End Region

        End Using

        Using context As New BloggingContext
#Region "ProjectEntities"
            For Each blog1 In context.Blogs
                Console.WriteLine("Blog: " & blog1.Url)
            Next
#End Region

#Region "ProjectSingleProperty"
            For Each blogName In context.Blogs.Select(Function(b) b.Url)
                Console.WriteLine("Blog: " & blogName)
            Next
#End Region
        End Using

        Using context As New BloggingContext
#Region "NoLimit"
            Dim blogsAll = context.Posts.
                                   Where(Function(p) p.Title.StartsWith("A")).
                                   ToList()
#End Region

#Region "Limit25"
            Dim blogs25 = context.Posts.
                                  Where(Function(p) p.Title.StartsWith("A")).
                                  Take(25).
                                  ToList()
#End Region
        End Using

        Using context As New BloggingContext
#Region "EagerlyLoadRelatedAndProject"
            For Each blog In context.Blogs.
                                     Select(Function(b) New With {b.Url, b.Posts}).ToList()

                For Each post In blog.Posts
                    Console.WriteLine($"Blog {blog.Url}, Post: {post.Title}")
                Next
            Next
#End Region
        End Using

        Using context As New BloggingContext
#Region "BufferingAndStreaming"
            ' ToList and ToArray cause the entire resultset to be buffered:
            Dim blogsList = context.Posts.Where(Function(p) p.Title.StartsWith("A")).ToList()
            Dim blogsArray = context.Posts.Where(Function(p) p.Title.StartsWith("A")).ToArray()

            ' Foreach streams, processing one row at a time:
            For Each blog In context.Posts.Where(Function(p) p.Title.StartsWith("A"))
                ' ...
            Next

            ' This method represents a filter that cannot be translated to SQL for execution in the
            ' database, And must be run on the client as a .NET method
            Dim SomeDotNetMethod = Function(post As Post) As Boolean
                                       Return True
                                   End Function

            ' AsEnumerable also streams, allowing you to execute LINQ operators on the client-side:
            Dim doubleFilteredBlogs = context.Posts.
                                              Where(Function(p) p.Title.StartsWith("A")). ' Translated to SQL and executed in the database
                                              AsEnumerable().Where(Function(p) SomeDotNetMethod(p)) ' Executed at the client on all database results
#End Region

        End Using

        Using context As New BloggingContext
#Region "SaveChangesBatching"
            Dim blog1 = context.Blogs.[Single](Function(b) b.Url = "http://someblog.microsoft.com")
            blog1.Url = "http://someotherblog.microsoft.com"
            context.Add(New Blog With {
                .Url = "http://newblog1.microsoft.com"})
            context.Add(New Blog With {
                .Url = "http://newblog2.microsoft.com"})
            context.SaveChanges()
#End Region

        End Using

        Using context As New BloggingContext
#Region "QueriesWithConstants"
            Dim post1 = context.Posts.FirstOrDefault(Function(p) p.Title = "post1")
            Dim post2 = context.Posts.FirstOrDefault(Function(p) p.Title = "post2")
#End Region
        End Using

        Using context As New BloggingContext
#Region "QueriesWithParameterization"
            Dim postTitle As String = "post1"
            Dim post1 = context.Posts.FirstOrDefault(Function(p) p.Title = postTitle)
            postTitle = "post2"
            Dim post2 = context.Posts.FirstOrDefault(Function(p) p.Title = postTitle)
#End Region
        End Using

        Using context As New LazyBloggingContext
            context.Database.EnsureDeleted()
            context.Database.EnsureCreated()

            For i = 0 To 9
                context.Blogs.Add(
                    New LazyLoading.Blog With {
                            .Url = $"http://blog{i}.microsoft.com",
                            .Posts = New List(Of LazyLoading.Post) From {
                                New LazyLoading.Post With {
                                    .Title = $"1st post of blog{i}"},
                                New LazyLoading.Post With {
                                    .Title = $"2nd post of blog{i}"}
                            }
                    })
            Next

            context.SaveChanges()
        End Using

        Using context As New LazyBloggingContext
#Region "NPlusOne"
            For Each blog In context.Blogs.ToList()
                For Each post In blog.Posts
                    Console.WriteLine($"Blog {blog.Url}, Post: {post.Title}")
                Next
            Next
#End Region
        End Using

        Using context As New EmployeeContext
            context.Database.EnsureDeleted()
            context.Database.EnsureCreated()

            For i = 0 To 9
                context.Employees.Add(New Employee)
            Next
        End Using

        Using context As New EmployeeContext
#Region "UpdateWithoutBulk"
            For Each employee1 In context.Employees
                employee1.Salary += 1000
            Next

            context.SaveChanges()
#End Region
        End Using

        Using context As New EmployeeContext
#Region "UpdateWithBulk"
            context.Database.ExecuteSqlRaw("UPDATE [Employees] SET [Salary] = [Salary] + 1000;")
#End Region
        End Using

    End Sub
End Module
