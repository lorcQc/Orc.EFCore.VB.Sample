Module Program
    Sub Main(args As String())
        Using db As New BloggingContext
            ' Note: This sample requires the database to be created before running.

            ' Create
            Console.WriteLine("Inserting a new blog")
            db.Add(New Blog With {
                .Url = "http://blogs.msdn.com/adonet"})
            db.SaveChanges()

            ' Read
            Console.WriteLine("Querying for a blog")
            Dim blog = db.Blogs.
                          OrderBy(Function(b) b.BlogId).
                          First()

            ' Update
            Console.WriteLine("Updating the blog and adding a post")
            blog.Url = "https://devblogs.microsoft.com/dotnet"
            blog.Posts.Add(New Post With {
                .Title = "Hello World",
                .Content = "I wrote an app using EF Core!"})
            db.SaveChanges()

            ' Delete
            Console.WriteLine("Delete the blog")
            db.Remove(blog)
            db.SaveChanges()
        End Using
    End Sub
End Module
