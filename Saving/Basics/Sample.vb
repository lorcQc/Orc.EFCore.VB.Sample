Namespace Basics
    Public NotInheritable Class Sample
        Public Shared Sub Run()
            Using context As New BloggingContext
                context.Database.EnsureDeleted()
                context.Database.EnsureCreated()
            End Using

#Region "Add"
            Using context As New BloggingContext
                Dim blog1 As New Blog With {
                    .Url = "http://example.com"}
                context.Blogs.Add(blog1)
                context.SaveChanges()
            End Using
#End Region

#Region "Update"
            Using context As New BloggingContext
                Dim blog1 = context.Blogs.First()
                blog1.Url = "http://example.com/blog"
                context.SaveChanges()
            End Using
#End Region

#Region "Remove"
            Using context As New BloggingContext
                Dim blog1 = context.Blogs.First()
                context.Blogs.Remove(blog1)
                context.SaveChanges()
            End Using
#End Region

#Region "MultipleOperations"
            Using context As New BloggingContext
                ' seeding database
                context.Blogs.Add(New Blog With {
                    .Url = "http://example.com/blog"})
                context.Blogs.Add(New Blog With {
                    .Url = "http://example.com/another_blog"})
                context.SaveChanges()
            End Using

            Using context As New BloggingContext
                ' add
                context.Blogs.Add(New Blog With {
                    .Url = "http://example.com/blog_one"})
                context.Blogs.Add(New Blog With {
                    .Url = "http://example.com/blog_two"})

                ' update
                Dim firstBlog = context.Blogs.First()
                firstBlog.Url = ""

                ' remove
                Dim lastBlog = context.Blogs.OrderBy(Function(e) e.BlogId).Last()
                context.Blogs.Remove(lastBlog)

                context.SaveChanges()
            End Using
#End Region

        End Sub
    End Class
End Namespace
