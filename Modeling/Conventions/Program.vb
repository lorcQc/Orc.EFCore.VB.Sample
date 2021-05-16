Imports System
Imports Conventions.EntityTypes
Imports Microsoft.EntityFrameworkCore

Module Program
    Sub Main(args As String())
        Using context As New MyContextWithFunctionMapping
            context.Database.EnsureDeleted()
            context.Database.EnsureCreated()

            context.Database.ExecuteSqlRaw(
                "CREATE FUNCTION dbo.BlogsWithMultiplePosts()
                        RETURNS TABLE
                        AS
                        RETURN
                        (
                            SELECT b.Url, COUNT(p.BlogId) AS PostCount
                            FROM Blogs AS b
                            JOIN Posts AS p ON b.BlogId = p.BlogId
                            GROUP BY b.BlogId, b.Url
                            HAVING COUNT(p.BlogId) > 1
                        )")

#Region "ToFunctionQuery"
            Dim query = From b In context.Set(Of BlogWithMultiplePosts)()
                        Where b.PostCount > 3
                        Select New With {b.Url, b.PostCount}
#End Region
            Dim result = query.ToList()
        End Using
    End Sub
End Module
