Imports Microsoft.EntityFrameworkCore

Namespace CascadeDelete
    Public NotInheritable Class Sample

        Public Shared Sub Run()
            DeleteBehaviorSample(DeleteBehavior.Cascade, True)
            DeleteBehaviorSample(DeleteBehavior.ClientSetNull, True)
            DeleteBehaviorSample(DeleteBehavior.SetNull, True)
            DeleteBehaviorSample(DeleteBehavior.Restrict, True)

            DeleteBehaviorSample(DeleteBehavior.Cascade, False)
            DeleteBehaviorSample(DeleteBehavior.ClientSetNull, False)
            DeleteBehaviorSample(DeleteBehavior.SetNull, False)
            DeleteBehaviorSample(DeleteBehavior.Restrict, False)

            DeleteOrphansSample(DeleteBehavior.Cascade, True)
            DeleteOrphansSample(DeleteBehavior.ClientSetNull, True)
            DeleteOrphansSample(DeleteBehavior.SetNull, True)
            DeleteOrphansSample(DeleteBehavior.Restrict, True)

            DeleteOrphansSample(DeleteBehavior.Cascade, False)
            DeleteOrphansSample(DeleteBehavior.ClientSetNull, False)
            DeleteOrphansSample(DeleteBehavior.SetNull, False)
            DeleteOrphansSample(DeleteBehavior.Restrict, False)
        End Sub

        Private Shared Sub DeleteBehaviorSample(deleteBehavior1 As DeleteBehavior, requiredRelationship As Boolean)
            Console.WriteLine(
                $"Test using DeleteBehavior.{deleteBehavior1} with {(If(requiredRelationship, "required", "optional"))} relationship:")

            InitializeDatabase(requiredRelationship)

#Region "DeleteBehaviorVariations"
            Using context As New BloggingContext(deleteBehavior1, requiredRelationship)

                Dim blog = context.Blogs.Include(Function(b) b.Posts).First()
                Dim posts1 = blog.Posts.ToList()

                DumpEntities("  After loading entities:", context, blog, posts1)

                context.Remove(blog)

                DumpEntities($"  After deleting blog '{blog.BlogId}':", context, blog, posts1)

                Try
                    Console.WriteLine()
                    Console.WriteLine("  Saving changes:")

                    context.SaveChanges()

                    DumpSql()

                    DumpEntities("  After SaveChanges:", context, blog, posts1)
                Catch e As Exception
                    DumpSql()

                    Console.WriteLine()
                    Console.WriteLine(
                        $"  SaveChanges threw {e.[GetType]().Name}: {(If(TypeOf e Is DbUpdateException, e.InnerException.Message, e.Message))}")
                End Try

                Console.WriteLine()
            End Using
#End Region

        End Sub

        Private Shared Sub DeleteOrphansSample(deleteBehavior1 As DeleteBehavior, requiredRelationship As Boolean)
            Console.WriteLine(
                $"Test deleting orphans with DeleteBehavior.{deleteBehavior1} and {(If(requiredRelationship, "a required", "an optional"))} relationship:")

            InitializeDatabase(requiredRelationship)

#Region "DeleteOrphansVariations"
            Using context As New BloggingContext(deleteBehavior1, requiredRelationship)

                Dim blog = context.Blogs.Include(Function(b) b.Posts).First()
                Dim posts1 = blog.Posts.ToList()

                DumpEntities("  After loading entities:", context, blog, posts1)

                blog.Posts.Clear()

                DumpEntities("  After making posts orphans:", context, blog, posts1)

                Try
                    Console.WriteLine()
                    Console.WriteLine("  Saving changes:")

                    context.SaveChanges()

                    DumpSql()

                    DumpEntities("  After SaveChanges:", context, blog, posts1)
                Catch e As Exception
                    DumpSql()

                    Console.WriteLine()
                    Console.WriteLine(
                        $"  SaveChanges threw {e.[GetType]().Name}: {(If(TypeOf e Is DbUpdateException, e.InnerException.Message, e.Message))}")
                End Try

                Console.WriteLine()
            End Using
#End Region

        End Sub

        Private Shared Sub InitializeDatabase(requiredRelationship As Boolean)
            Using context As New BloggingContext(DeleteBehavior.ClientSetNull, requiredRelationship)
                context.Database.EnsureDeleted()
                context.Database.EnsureCreated()

                context.Blogs.Add(
                    New Blog With {
                .Url = "http://sample.com",
                .Posts = New List(Of Post) From {
                    New Post With {
                        .Title = "Saving Data with EF"},
                    New Post With {
                        .Title = "Cascade Delete with EF"}}
                    })

                context.SaveChanges()
            End Using
        End Sub

        Private Shared Sub DumpEntities(message1 As String, context As BloggingContext, blog As Blog, posts1 As IList(Of Post))
            Console.WriteLine()
            Console.WriteLine(message1)

            Dim blogEntry = context.Entry(blog)

            Console.WriteLine($"    Blog '{blog.BlogId}' is in state {blogEntry.State} with {posts1.Count} posts referenced.")

            For Each post1 As Post In posts1
                Dim postEntry = context.Entry(post1)

                Console.WriteLine(
                    $"      Post '{post1.PostId}' is in state {postEntry.State} " &
                    $"with FK '{If(post1.BlogId?.ToString(), "null")}' and {(If(post1.Blog Is Nothing, "no reference to a blog.", $"reference to blog '{post1.BlogId}'."))}")
            Next
        End Sub

        Private Shared Sub DumpSql()
            For Each logMessage In BloggingContext.LogMessages
                Console.WriteLine("    " & logMessage)
            Next
        End Sub

    End Class
End Namespace
