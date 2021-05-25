Imports System
Imports Microsoft.EntityFrameworkCore

Module Program

    Sub Main()
        Using context As New BloggingContext
            context.Database.EnsureDeleted()
            context.Database.EnsureCreated()
        End Using

#Region "SingleInclude"
        Using context As New BloggingContext
            Dim blogs1 = context.Blogs.
                                 Include(Function(blog) blog.Posts).
                                 ToList()
        End Using
#End Region

#Region "IgnoredInclude"
        Using context As New BloggingContext
            Dim blogs1 = context.Blogs.
                                 Include(Function(blog) blog.Posts).
                                 Select(Function(blog) New With {.Id = blog.BlogId, blog.Url}).
                                 ToList()
        End Using
#End Region

#Region "MultipleIncludes"
        Using context As New BloggingContext
            Dim blogs1 = context.Blogs.
                                 Include(Function(blog) blog.Posts).
                                 Include(Function(blog) blog.Owner).
                                 ToList()
        End Using
#End Region

#Region "SingleThenInclude"
        Using context As New BloggingContext
            Dim blogs1 = context.Blogs.
                                 Include(Function(blog) blog.Posts).
                                 ThenInclude(Function(post) post.Author).
                                 ToList()
        End Using
#End Region

#Region "MultipleThenIncludes"
        Using context As New BloggingContext
            Dim blogs1 = context.Blogs.
                                 Include(Function(blog) blog.Posts).
                                 ThenInclude(Function(post) post.Author).
                                 ThenInclude(Function(author1) author1.Photo).
                                 ToList()
        End Using
#End Region

#Region "IncludeTree"
        Using context As New BloggingContext
            Dim blogs1 = context.Blogs.
                                 Include(Function(blog) blog.Posts).
                                 ThenInclude(Function(post) post.Author).
                                 ThenInclude(Function(author1) author1.Photo).
                                 Include(Function(blog) blog.Owner).
                                 ThenInclude(Function(owner1) owner1.Photo).
                                 ToList()
        End Using
#End Region

#Region "MultipleLeafIncludes"
        Using context As New BloggingContext
            Dim blogs1 = context.Blogs.
                                 Include(Function(blog) blog.Posts).
                                 ThenInclude(Function(post) post.Author).
                                 Include(Function(blog) blog.Posts).
                                 ThenInclude(Function(post) post.Tags).
                                 ToList()
        End Using
#End Region

#Region "IncludeMultipleNavigationsWithSingleInclude"
        Using context As New BloggingContext
            Dim blogs1 = context.Blogs.
                                 Include(Function(blog) blog.Owner.AuthoredPosts).
                                 ThenInclude(Function(post) post.Blog.Owner.Photo).
                                 ToList()
        End Using
#End Region

#Region "AsSplitQuery"
        Using context As New BloggingContext
            Dim blogs1 = context.Blogs.
                                 Include(Function(blog) blog.Posts).
                                 AsSplitQuery().
                                 ToList()
        End Using
#End Region

        Using context As New SplitQueriesBloggingContext
            context.Database.EnsureDeleted()
            context.Database.EnsureCreated()
        End Using

#Region "WithSplitQueryAsDefault"
        Using context As New SplitQueriesBloggingContext
            Dim blogs1 = context.Blogs.
                                 Include(Function(blog) blog.Posts).
                                 ToList()
        End Using
#End Region

#Region "AsSingleQuery"
        Using context As New SplitQueriesBloggingContext
            Dim blogs1 = context.Blogs.
                                 Include(Function(blog) blog.Posts).
                                 AsSingleQuery().ToList()
        End Using
#End Region

#Region "Eager"
        Using context As New BloggingContext
            Dim blog = context.Blogs.Single(Function(b) b.BlogId = 1)

            context.Entry(blog).
                    Collection(Function(b) b.Posts).
                    Load()

            context.Entry(blog).
                    Reference(Function(b) b.Owner).
                    Load()
        End Using
#End Region

#Region "NavQueryAggregate"
        Using context As New BloggingContext
            Dim blog = context.Blogs.Single(Function(b) b.BlogId = 1)

            Dim postCount = context.Entry(blog).
                                    Collection(Function(b) b.Posts).
                                    Query().
                                    Count()
        End Using
#End Region

#Region "NavQueryFiltered"
        Using context As New BloggingContext
            Dim blog = context.Blogs.Single(Function(b) b.BlogId = 1)

            Dim goodPosts = context.Entry(blog).
                                    Collection(Function(b) b.Posts).Query().
                                    Where(Function(p) p.Rating > 3).
                                    ToList()
        End Using
#End Region

#Region "FilteredInclude"
        Using context As New BloggingContext
            Dim filteredBlogs = context.Blogs.Include(
                    Function(blog) blog.Posts.
                                        Where(Function(post) post.BlogId = 1).
                                        OrderByDescending(Function(post) post.Title).
                                        Take(5)
                    ).ToList()
        End Using
#End Region

#Region "MultipleLeafIncludesFiltered1"
        Using context As New BloggingContext
            Dim filteredBlogs = context.Blogs.
                Include(Function(blog) blog.Posts.Where(Function(post) post.BlogId = 1)).
                ThenInclude(Function(post) post.Author).
                Include(Function(blog) blog.Posts).
                ThenInclude(Function(post) post.Tags.OrderBy(Function(postTag) postTag.TagId).
                Skip(3)).
                ToList()
        End Using
#End Region

#Region "MultipleLeafIncludesFiltered2"
        Using context As New BloggingContext
            Dim filteredBlogs = context.Blogs.
                                        Include(Function(blog) blog.Posts.Where(Function(post) post.BlogId = 1)).
                                        ThenInclude(Function(post) post.Author).
                                        Include(Function(blog) blog.Posts.Where(Function(post) post.BlogId = 1)).
                                        ThenInclude(Function(post) post.Tags.OrderBy(Function(postTag) postTag.TagId).
                                        Skip(3)).
                                        ToList()
        End Using
#End Region

    End Sub

End Module
