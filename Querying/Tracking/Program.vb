Imports System
Imports Microsoft.EntityFrameworkCore

Module Program
    Sub Main()
        Using context As New BloggingContext
            context.Database.EnsureDeleted()
            context.Database.EnsureCreated()
        End Using

        Using context As New BloggingContext
            ' seeding database
            context.Blogs.Add(New Blog With {
                .Url = "http://sample.com/blog"})
            context.Blogs.Add(New Blog With {
                .Url = "http://sample.com/another_blog"})
            context.SaveChanges()
        End Using

#Region "Tracking"
        Using context As New BloggingContext
            Dim blog1 = context.Blogs.SingleOrDefault(Function(b) b.BlogId = 1)
            blog1.Rating = 5
            context.SaveChanges()
        End Using
#End Region

#Region "NoTracking"
        Using context As New BloggingContext
            Dim blogs1 = context.Blogs.AsNoTracking().ToList()
        End Using
#End Region

#Region "NoTrackingWithIdentityResolution"
        Using context As New BloggingContext
            Dim blogs1 = context.Blogs.
                                 AsNoTrackingWithIdentityResolution().
                                 ToList()
        End Using
#End Region

#Region "ContextDefaultTrackingBehavior"
        Using context As New BloggingContext
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking

            Dim blogs1 = context.Blogs.ToList()
        End Using
#End Region

#Region "CustomProjection1"
        Using context As New BloggingContext
            Dim blog1 = context.Blogs.
                                Select(Function(b) New With {.Blog = b, .PostCount = b.Posts.Count()})
        End Using
#End Region

#Region "CustomProjection2"
        Using context As New BloggingContext
            Dim blog1 = context.Blogs.
                                Select(Function(b) New With {.Blog = b,
                                                             .Post = b.Posts.OrderBy(Function(p) p.Rating).LastOrDefault()})
        End Using
#End Region

#Region "CustomProjection3"
        Using context As New BloggingContext
            Dim blog1 = context.Blogs.Select(Function(b) New With {.Id = b.BlogId, b.Url})
        End Using
#End Region

#Region "ClientProjection"
        Using context As New BloggingContext
            Dim blogs1 = context.Blogs.
                                 OrderByDescending(Function(blog1) blog1.Rating).
                                 Select(Function(blog1) New With {.Id = blog1.BlogId, .Url = StandardizeUrl(blog1)}).
                                 ToList()
        End Using
#End Region

    End Sub

#Region "ClientMethod"
    Public Function StandardizeUrl(blog1 As Blog) As String
        Dim url1 = blog1.Url.ToLower()

        If Not url1.StartsWith("http://") Then
            url1 = String.Concat("http://", url1)
        End If

        Return url1
    End Function
#End Region

End Module
