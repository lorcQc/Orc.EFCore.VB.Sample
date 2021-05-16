Imports System
Imports Microsoft.EntityFrameworkCore

Module Program
    Sub Main()

        Using context As New DataSeedingContext
            context.Database.EnsureCreated()

            Dim testBlog = context.Blogs.FirstOrDefault(Function(b) b.Url = "http://test.com")
            If testBlog Is Nothing Then
                context.Blogs.Add(New Blog With {
                    .Url = "http://test.com"})
            End If

            context.SaveChanges()
        End Using

        Using context As New DataSeedingContext
            For Each blog In context.Blogs.Include(Function(b) b.Posts)
                Console.WriteLine($"Blog {blog.Url}")

                For Each post In blog.Posts
                    Console.WriteLine($"	{post.Title}: {post.Content} by {post.AuthorName.First} {post.AuthorName.Last}")
                Next
            Next
        End Using

    End Sub
End Module
