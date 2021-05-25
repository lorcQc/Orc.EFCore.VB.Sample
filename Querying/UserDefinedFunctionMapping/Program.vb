Imports System
Imports Microsoft.EntityFrameworkCore

Module Program
    Sub Main(args As String())

        Using context As New BloggingContext
            context.Database.EnsureDeleted()
            context.Database.EnsureCreated()

            context.Database.ExecuteSqlRaw(
                "CREATE FUNCTION dbo.CommentedPostCountForBlog(@id int)
                    RETURNS int
                    AS
                    BEGIN
                        RETURN (SELECT COUNT(*)
                            FROM [Posts] AS [p]
                            WHERE ([p].[BlogId] = @id) AND ((
                                SELECT COUNT(*)
                                FROM [Comments] AS [c]
                                WHERE [p].[PostId] = [c].[PostId]) > 0));
                    END")

            context.Database.ExecuteSqlRaw(
                "CREATE FUNCTION [dbo].[ConcatStrings] (@prm1 nvarchar(max), @prm2 nvarchar(max))
                    RETURNS nvarchar(max)
                    AS
                    BEGIN
                        RETURN @prm1 + @prm2;
                    END")

            context.Database.ExecuteSqlRaw(
                "CREATE FUNCTION dbo.PostsWithPopularComments(@likeThreshold int)
                    RETURNS TABLE
                    AS
                    RETURN
                    (
                        SELECT [p].[PostId], [p].[BlogId], [p].[Content], [p].[Rating], [p].[Title]
                        FROM [Posts] AS [p]
                        WHERE (
                            SELECT COUNT(*)
                            FROM [Comments] AS [c]
                            WHERE ([p].[PostId] = [c].[PostId]) AND ([c].[Likes] >= @likeThreshold)) > 0
                    )")

#Region "BasicQuery"
            Dim query1 = From b In context.Blogs
                         Where context.ActivePostCountForBlog(b.BlogId) > 1
                         Select b
#End Region
            Dim result1 = query1.ToList()

#Region "HasTranslationQuery"
            Dim query2 = From p In context.Posts
                         Select context.PercentageDifference(p.BlogId, 3)
#End Region
            Dim result2 = query2.ToList()

#Region "NullabilityPropagationExamples"
            Dim query3 = context.Blogs.Where(Function(e) context.ConcatStrings(e.Url, e.Rating.ToString()) <> "https://mytravelblog.com/4")
            Dim query4 = context.Blogs.Where(Function(e) context.ConcatStringsOptimized(e.Url, e.Rating.ToString()) <> "https://mytravelblog.com/4")
#End Region

            Dim result3 = query3.ToList()
            Dim result4 = query4.ToList()

#Region "TableValuedFunctionQuery"
            Dim likeThreshold As Integer = 3
            Dim query5 = From p In context.PostsWithPopularComments(likeThreshold)
                         Order By p.Rating
                         Select p
#End Region
            Dim result5 = query5.ToList()

        End Using
    End Sub
End Module
