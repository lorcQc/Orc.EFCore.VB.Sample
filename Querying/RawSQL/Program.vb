Imports System
Imports Microsoft.Data.SqlClient
Imports Microsoft.EntityFrameworkCore

Module Program
    Sub Main()

        Using context As New BloggingContext
            context.Database.EnsureDeleted()
            context.Database.EnsureCreated()

            context.Database.ExecuteSqlRaw(
            "create function [dbo].[SearchBlogs] (@searchTerm nvarchar(max))
                          returns @found table
                          (
                              BlogId int not null,
                              Url nvarchar(max),
                              Rating int
                          )
                          as
                          begin
                              insert into @found
                              select * from dbo.Blogs as b
                              where exists (
                                  select 1
                                  from [Post] as [p]
                                  where ([b].[BlogId] = [p].[BlogId]) and (charindex(@searchTerm, [p].[Title]) > 0))
                                  return
                          end")

            context.Database.ExecuteSqlRaw(
            "create procedure [dbo].[GetMostPopularBlogs] as
                          begin
                              select * from dbo.Blogs order by Rating
                          end")

            context.Database.ExecuteSqlRaw(
            "create procedure [dbo].[GetMostPopularBlogsForUser] @filterByUser nvarchar(max) as
                          begin
                              select * from dbo.Blogs order by Rating
                          end")
        End Using

#Region "FromSqlRaw"
        Using context As New BloggingContext
            Dim blogs1 = context.Blogs.
                                 FromSqlRaw("SELECT * FROM dbo.Blogs").
                                 ToList()
        End Using
#End Region

#Region "FromSqlRawStoredProcedure"
        Using context As New BloggingContext
            Dim blogs1 = context.Blogs.
                                 FromSqlRaw("EXECUTE dbo.GetMostPopularBlogs").
                                 ToList()
        End Using
#End Region

#Region "FromSqlRawStoredProcedureParameter"
        Using context As New BloggingContext
            Dim user As String = "johndoe"
            Dim blogs1 = context.Blogs.
                                 FromSqlRaw("EXECUTE dbo.GetMostPopularBlogsForUser {0}", user).
                                 ToList()
        End Using
#End Region

#Region "FromSqlInterpolatedStoredProcedureParameter"
        Using context As New BloggingContext
            Dim user As String = "johndoe"

            Dim blogs1 = context.Blogs.
                                 FromSqlInterpolated($"EXECUTE dbo.GetMostPopularBlogsForUser {user}").
                                 ToList()
        End Using
#End Region

#Region "FromSqlRawStoredProcedureSqlParameter"
        Using context As New BloggingContext
            Dim user As New SqlParameter("user", "johndoe")

            Dim blogs1 = context.Blogs.
                                 FromSqlRaw("EXECUTE dbo.GetMostPopularBlogsForUser @user", user).
                                 ToList()
        End Using
#End Region

#Region "FromSqlRawStoredProcedureNamedSqlParameter"
        Using context As New BloggingContext
            Dim user As New SqlParameter("user", "johndoe")

            Dim blogs1 = context.Blogs.
                                 FromSqlRaw("EXECUTE dbo.GetMostPopularBlogsForUser @filterByUser=@user", user).
                                 ToList()
        End Using
#End Region

#Region "FromSqlInterpolatedComposed"
        Using context As New BloggingContext
            Dim searchTerm As String = "Lorem ipsum"

            Dim blogs1 = context.Blogs.
                                 FromSqlInterpolated($"SELECT * FROM dbo.SearchBlogs({searchTerm})").
                                 Where(Function(b) b.Rating > 3).
                                 OrderByDescending(Function(b) b.Rating).
                                 ToList()
        End Using
#End Region

#Region "FromSqlInterpolatedAsNoTracking"
        Using context As New BloggingContext
            Dim searchTerm As String = "Lorem ipsum"

            Dim blogs1 = context.Blogs.
                                 FromSqlInterpolated($"SELECT * FROM dbo.SearchBlogs({searchTerm})").
                                 AsNoTracking().
                                 ToList()
        End Using
#End Region

#Region "FromSqlInterpolatedInclude"
        Using context As New BloggingContext
            Dim searchTerm As String = "Lorem ipsum"

            Dim blogs1 = context.Blogs.
                                 FromSqlInterpolated($"SELECT * FROM dbo.SearchBlogs({searchTerm})").
                                 Include(Function(b) b.Posts).
                                 ToList()
        End Using
#End Region

    End Sub
End Module
