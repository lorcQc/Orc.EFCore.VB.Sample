Imports Microsoft.EntityFrameworkCore

Namespace Disconnected
    Public Module Sample

        Public Sub Run()
            Using context As New BloggingContext
                context.Database.EnsureDeleted()
                context.Database.EnsureCreated()
            End Using

            IsItNew()
            InsertAndUpdateSingleEntity()
            InsertOrUpdateSingleEntityStoreGenerated()
            InsertOrUpdateSingleEntityFind()
            InsertAndUpdateGraph()
            InsertOrUpdateGraphStoreGenerated()
            InsertOrUpdateGraphFind()
            InsertUpdateOrDeleteGraphFind()
            InsertUpdateOrDeleteTrackGraph()
        End Sub

        Private Sub IsItNew()
            Console.WriteLine()
            Console.WriteLine("Show entity-specific check for key set:")

            Using context As New BloggingContext
                Dim blog1 As New Blog With {
                    .Url = "http://sample.com"}

                ' Key is not set for a new entity
                Console.WriteLine($"  Blog entity is {(If(IsItNew(blog1), "new", "existing"))}.")

                context.Add(blog1)
                context.SaveChanges()

                ' Key is now set
                Console.WriteLine($"  Blog entity is {(If(IsItNew(blog1), "new", "existing"))}.")
            End Using

            Console.WriteLine()
            Console.WriteLine("Show general IsKeySet:")

            Using context As New BloggingContext
                Dim blog1 As New Blog With {
                    .Url = "http://sample.com"}

                ' Key is not set for a new entity
                Console.WriteLine($"  Blog entity is {(If(IsItNew(context, CObj(blog1)), "new", "existing"))}.")

                context.Add(blog1)
                context.SaveChanges()

                ' Key is now set
                Console.WriteLine($"  Blog entity is {(If(IsItNew(context, CObj(blog1)), "new", "existing"))}.")
            End Using

            Console.WriteLine()
            Console.WriteLine("Show key set on Add:")

            Using context As New BloggingContext
                Dim blog1 As New Blog With {
                    .Url = "http://sample.com"}

                ' Key is not set for a new entity
                Console.WriteLine($"  Blog entity is {(If(IsItNew(context, CObj(blog1)), "new", "existing"))}.")

                context.Add(blog1)

                ' Key is set as soon as Add assigns a key, even if it is temporary
                Console.WriteLine($"  Blog entity is {(If(IsItNew(context, CObj(blog1)), "new", "existing"))}.")
            End Using

            Console.WriteLine()
            Console.WriteLine("Show using query to check for new entity:")

            Using context As New BloggingContext
                Dim blog1 As New Blog With {
                    .Url = "http://sample.com"}

                Console.WriteLine($"  Blog entity is {(If(IsItNew(context, blog1), "new", "existing"))}.")

                context.Add(blog1)
                context.SaveChanges()

                Console.WriteLine($"  Blog entity is {(If(IsItNew(context, blog1), "new", "existing"))}.")
            End Using
        End Sub
        Private Sub InsertAndUpdateSingleEntity()
            Console.WriteLine()
            Console.WriteLine("Save single entity with explicit insert or update:")

            Dim blog1 As New Blog With {
                .Url = "http://sample.com"}

            Using context As New BloggingContext
                Console.WriteLine($"  Inserting with URL {blog1.Url}")
                Insert(context, blog1)
            End Using

            Using context As New BloggingContext
                Console.WriteLine($"  Found with URL {context.Blogs.[Single](Function(b) b.BlogId = blog1.BlogId).Url}")
            End Using

            Using context As New BloggingContext
                blog1.Url = "https://sample.com"
                Console.WriteLine($"  Updating with URL {blog1.Url}")
                Update(context, blog1)
            End Using

            Using context As New BloggingContext
                Console.WriteLine($"  Found with URL {context.Blogs.[Single](Function(b) b.BlogId = blog1.BlogId).Url}")
            End Using
        End Sub

        Private Sub InsertOrUpdateSingleEntityStoreGenerated()
            Console.WriteLine()
            Console.WriteLine("Save single entity with auto-generated key:")

            Dim blog1 As New Blog With {
                .Url = "http://sample.com"}

            Using context As New BloggingContext
                Console.WriteLine($"  Inserting with URL {blog1.Url}")
                InsertOrUpdate(context, CObj(blog1))
            End Using

            Using context As New BloggingContext
                Console.WriteLine($"  Found with URL {context.Blogs.[Single](Function(b) b.BlogId = blog1.BlogId).Url}")
            End Using

            Using context As New BloggingContext
                blog1.Url = "https://sample.com"
                Console.WriteLine($"  Updating with URL {blog1.Url}")
                InsertOrUpdate(context, CObj(blog1))
            End Using

            Using context As New BloggingContext
                Console.WriteLine($"  Found with URL {context.Blogs.[Single](Function(b) b.BlogId = blog1.BlogId).Url}")
            End Using
        End Sub

        Private Sub InsertOrUpdateSingleEntityFind()
            Console.WriteLine()
            Console.WriteLine("Save single entity with any kind of key:")

            Dim blog1 As New Blog With {
                .Url = "http://sample.com"}

            Using context As New BloggingContext
                Console.WriteLine($"  Inserting with URL {blog1.Url}")
                InsertOrUpdate(context, blog1)
            End Using

            Using context As New BloggingContext
                Console.WriteLine($"  Found with URL {context.Blogs.[Single](Function(b) b.BlogId = blog1.BlogId).Url}")
            End Using

            Using context As New BloggingContext
                blog1.Url = "https://sample.com"
                Console.WriteLine($"  Updating with URL {blog1.Url}")
                InsertOrUpdate(context, blog1)
            End Using

            Using context As New BloggingContext
                Console.WriteLine($"  Found with URL {context.Blogs.[Single](Function(b) b.BlogId = blog1.BlogId).Url}")
            End Using
        End Sub

        Private Sub InsertAndUpdateGraph()
            Console.WriteLine()
            Console.WriteLine("Save graph with explicit insert or update:")

            Dim blog1 = CreateBlogAndPosts()

            Using context As New BloggingContext
                Console.WriteLine($"  Inserting with URL {blog1.Url} and {blog1.Posts(0).Title}, {blog1.Posts(1).Title}")
                InsertGraph(context, blog1)
            End Using

            Using context As New BloggingContext
                Dim read = context.Blogs.Include(Function(b) b.Posts).[Single](Function(b) b.BlogId = blog1.BlogId)
                Console.WriteLine($"  Found with URL {read.Url} and {read.Posts(0).Title}, {read.Posts(1).Title}")
            End Using

            Using context As New BloggingContext
                blog1.Url = "https://sample.com"
                blog1.Posts(0).Title = "Post A"
                blog1.Posts(1).Title = "Post B"

                Console.WriteLine($"  Updating with URL {blog1.Url}")
                UpdateGraph(context, blog1)
            End Using

            Using context As New BloggingContext
                Dim read = context.Blogs.Include(Function(b) b.Posts).[Single](Function(b) b.BlogId = blog1.BlogId)
                Console.WriteLine($"  Found with URL {read.Url} and {read.Posts(0).Title}, {read.Posts(1).Title}")
            End Using
        End Sub

        Private Sub InsertOrUpdateGraphStoreGenerated()
            Console.WriteLine()
            Console.WriteLine("Save graph with auto-generated key:")

            Dim blog1 = CreateBlogAndPosts()

            Using context As New BloggingContext
                Console.WriteLine($"  Inserting with URL {blog1.Url} and {blog1.Posts(0).Title}, {blog1.Posts(1).Title}")
                InsertOrUpdateGraph(context, CObj(blog1))
            End Using

            Using context As New BloggingContext
                Dim read = context.Blogs.Include(Function(b) b.Posts).[Single](Function(b) b.BlogId = blog1.BlogId)
                Console.WriteLine($"  Found with URL {read.Url} and {read.Posts(0).Title}, {read.Posts(1).Title}")
            End Using

            Using context As New BloggingContext
                blog1.Url = "https://sample.com"
                blog1.Posts(0).Title = "Post A"
                blog1.Posts(1).Title = "Post B"
                blog1.Posts.Add(New Post With {
                    .Title = "New Post"})

                Console.WriteLine($"  Updating with URL {blog1.Url}")
                InsertOrUpdateGraph(context, CObj(blog1))
            End Using

            Using context As New BloggingContext
                Dim read = context.Blogs.Include(Function(b) b.Posts).[Single](Function(b) b.BlogId = blog1.BlogId)
                Console.WriteLine($"  Found with URL {read.Url} and {read.Posts(0).Title}, {read.Posts(1).Title}, {read.Posts(2).Title}")
            End Using
        End Sub

        Private Sub InsertOrUpdateGraphFind()
            Console.WriteLine()
            Console.WriteLine("Save graph with any kind of key:")

            Dim blog1 = CreateBlogAndPosts()

            Using context As New BloggingContext
                Console.WriteLine($"  Inserting with URL {blog1.Url} and {blog1.Posts(0).Title}, {blog1.Posts(1).Title}")
                InsertOrUpdateGraph(context, blog1)
            End Using

            Using context As New BloggingContext
                Dim read = context.Blogs.Include(Function(b) b.Posts).[Single](Function(b) b.BlogId = blog1.BlogId)
                Console.WriteLine($"  Found with URL {read.Url} and {read.Posts(0).Title}, {read.Posts(1).Title}")
            End Using

            Using context As New BloggingContext
                blog1.Url = "https://sample.com"
                blog1.Posts(0).Title = "Post A"
                blog1.Posts(1).Title = "Post B"
                blog1.Posts.Add(New Post With {
                    .Title = "New Post"})

                Console.WriteLine($"  Updating with URL {blog1.Url}")
                InsertOrUpdateGraph(context, blog1)
            End Using

            Using context As New BloggingContext
                Dim read = context.Blogs.Include(Function(b) b.Posts).[Single](Function(b) b.BlogId = blog1.BlogId)
                Console.WriteLine($"  Found with URL {read.Url} and {read.Posts(0).Title}, {read.Posts(1).Title}, {read.Posts(2).Title}")
            End Using
        End Sub

        Private Sub InsertUpdateOrDeleteGraphFind()
            Console.WriteLine()
            Console.WriteLine("Save graph with deletes and any kind of key:")

            Dim blog1 = CreateBlogAndPosts()

            Using context As New BloggingContext
                Console.WriteLine($"  Inserting with URL {blog1.Url} and {blog1.Posts(0).Title}, {blog1.Posts(1).Title}")
                InsertUpdateOrDeleteGraph(context, blog1)
            End Using

            Using context As New BloggingContext
                Dim read = context.Blogs.Include(Function(b) b.Posts).[Single](Function(b) b.BlogId = blog1.BlogId)
                Console.WriteLine($"  Found with URL {read.Url} and {read.Posts(0).Title}, {read.Posts(1).Title}")
            End Using

            Using context As New BloggingContext
                blog1.Url = "https://sample.com"
                blog1.Posts(0).Title = "Post A"
                blog1.Posts.Remove(blog1.Posts(1))
                blog1.Posts.Add(New Post With {
                    .Title = "New Post"})

                Console.WriteLine($"  Updating with URL {blog1.Url}")
                InsertUpdateOrDeleteGraph(context, blog1)
            End Using

            Using context As New BloggingContext
                Dim read = context.Blogs.Include(Function(b) b.Posts).[Single](Function(b) b.BlogId = blog1.BlogId)
                Console.WriteLine($"  Found with URL {read.Url} and {read.Posts(0).Title}, {read.Posts(1).Title}")
            End Using
        End Sub

        Private Sub InsertUpdateOrDeleteTrackGraph()
            Console.WriteLine()
            Console.WriteLine("Save graph using TrackGraph:")

            Dim blog1 = CreateBlogAndPosts()
            blog1.IsNew = True
            blog1.Posts(0).IsNew = True
            blog1.Posts(1).IsNew = True

            Using context As New BloggingContext
                Console.WriteLine($"  Inserting with URL {blog1.Url} and {blog1.Posts(0).Title}, {blog1.Posts(1).Title}")
                SaveAnnotatedGraph(context, blog1)
            End Using

            Using context As New BloggingContext
                Dim read = context.Blogs.Include(Function(b) b.Posts).[Single](Function(b) b.BlogId = blog1.BlogId)
                Console.WriteLine($"  Found with URL {read.Url} and {read.Posts(0).Title}, {read.Posts(1).Title}")
            End Using

            blog1.IsNew = False
            blog1.Posts(0).IsNew = False
            blog1.Posts(1).IsNew = False

            Using context As New BloggingContext
                blog1.Url = "https://sample.com"
                blog1.IsChanged = True
                blog1.Posts(0).Title = "Post A"
                blog1.Posts(0).IsDeleted = True
                blog1.Posts(1).Title = "Post B"
                blog1.Posts.Add(New Post With {
                    .Title = "New Post",
                    .IsNew = True})

                Console.WriteLine($"  Updating with URL {blog1.Url}")
                SaveAnnotatedGraph(context, blog1)
            End Using

            Using context As New BloggingContext
                Dim read = context.Blogs.Include(Function(b) b.Posts).[Single](Function(b) b.BlogId = blog1.BlogId)
                Console.WriteLine($"  Found with URL {read.Url} and {read.Posts(0).Title}, {read.Posts(1).Title}")
            End Using
        End Sub

#Region "IsItNewSimple"
        Public Function IsItNew(blog1 As Blog) As Boolean
            Return blog1.BlogId = 0
        End Function
#End Region

#Region "IsItNewGeneral"
        Public Function IsItNew(context As DbContext, entity As Object) As Boolean
            Return Not context.Entry(entity).IsKeySet
        End Function
#End Region

#Region "IsItNewQuery"
        Public Function IsItNew(context As BloggingContext, blog1 As Blog) As Boolean
            Return context.Blogs.Find(blog1.BlogId) Is Nothing
        End Function
#End Region

#Region "InsertAndUpdateSingleEntity"
        Public Sub Insert(context As DbContext, entity As Object)
            context.Add(entity)
            context.SaveChanges()
        End Sub

        Public Sub Update(context As DbContext, entity As Object)
            context.Update(entity)
            context.SaveChanges()
        End Sub
#End Region

#Region "InsertOrUpdateSingleEntity"
        Public Sub InsertOrUpdate(context As DbContext, entity As Object)
            context.Update(entity)
            context.SaveChanges()
        End Sub
#End Region

#Region "InsertOrUpdateSingleEntityWithFind"
        Public Sub InsertOrUpdate(context As BloggingContext, blog1 As Blog)
            Dim existingBlog = context.Blogs.Find(blog1.BlogId)
            If existingBlog Is Nothing Then
                context.Add(blog1)
            Else
                context.Entry(existingBlog).CurrentValues.SetValues(blog1)
            End If

            context.SaveChanges()
        End Sub
#End Region

        Private Function CreateBlogAndPosts() As Blog
#Region "CreateBlogAndPosts"
            Dim blog1 As New Blog With
            {
            .Url = "http://sample.com",
            .Posts = New List(Of Post) From {
                New Post With {
                    .Title = "Post 1"},
                New Post With {
                    .Title = "Post 2"}}
            }
#End Region

            Return blog1
        End Function

#Region "InsertGraph"
        Public Sub InsertGraph(context As DbContext, rootEntity As Object)
            context.Add(rootEntity)
            context.SaveChanges()
        End Sub
#End Region

#Region "UpdateGraph"
        Public Sub UpdateGraph(context As DbContext, rootEntity As Object)
            context.Update(rootEntity)
            context.SaveChanges()
        End Sub
#End Region

#Region "InsertOrUpdateGraph"
        Public Sub InsertOrUpdateGraph(context As DbContext, rootEntity As Object)
            context.Update(rootEntity)
            context.SaveChanges()
        End Sub
#End Region

#Region "InsertOrUpdateGraphWithFind"
        Public Sub InsertOrUpdateGraph(context As BloggingContext, blog1 As Blog)
            Dim existingBlog = context.Blogs.
                                       Include(Function(b) b.Posts).
                                       FirstOrDefault(Function(b) b.BlogId = blog1.BlogId)

            If existingBlog Is Nothing Then
                context.Add(blog1)
            Else
                context.Entry(existingBlog).CurrentValues.SetValues(blog1)
                For Each post1 In blog1.Posts
                    Dim existingPost = existingBlog.Posts _
        .FirstOrDefault(Function(p) p.PostId = post1.PostId)

                    If existingPost Is Nothing Then
                        existingBlog.Posts.Add(post1)
                    Else
                        context.Entry(existingPost).CurrentValues.SetValues(post1)
                    End If
                Next
            End If

            context.SaveChanges()
        End Sub
#End Region

#Region "InsertUpdateOrDeleteGraphWithFind"
        Public Sub InsertUpdateOrDeleteGraph(context As BloggingContext, blog1 As Blog)
            Dim existingBlog = context.Blogs.
                                       Include(Function(b) b.Posts).
                                       FirstOrDefault(Function(b) b.BlogId = blog1.BlogId)

            If existingBlog Is Nothing Then
                context.Add(blog1)
            Else
                context.Entry(existingBlog).CurrentValues.SetValues(blog1)
                For Each post1 In blog1.Posts
                    Dim existingPost = existingBlog.Posts.FirstOrDefault(Function(p) p.PostId = post1.PostId)

                    If existingPost Is Nothing Then
                        existingBlog.Posts.Add(post1)
                    Else
                        context.Entry(existingPost).CurrentValues.SetValues(post1)
                    End If
                Next

                For Each post1 In existingBlog.Posts
                    If Not blog1.Posts.Any(Function(p) p.PostId = post1.PostId) Then
                        context.Remove(post1)
                    End If
                Next
            End If

            context.SaveChanges()
        End Sub
#End Region

#Region "TrackGraph"
        Public Sub SaveAnnotatedGraph(context As DbContext, rootEntity As Object)
            context.ChangeTracker.TrackGraph(
                rootEntity,
                Sub(n)
                    Dim entity = CType(n.Entry.Entity, EntityBase)
                    n.Entry.State = If(entity.IsNew, EntityState.Added,
                                                     If(entity.IsChanged, EntityState.Modified,
                                                                          If(entity.IsDeleted, EntityState.Deleted,
                                                                                               EntityState.Unchanged)))
                End Sub)

            context.SaveChanges()
        End Sub
#End Region
    End Module

End Namespace
