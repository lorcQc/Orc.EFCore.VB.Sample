Imports System

Module Program

#Region "ClientMethod"
    Public Function StandardizeUrl(url As String) As String
        url = url.ToLower()

        If Not url.StartsWith("http://") Then
            url = String.Concat("http://", url)
        End If

        Return url
    End Function
#End Region

    Sub Main()

        Using context As New BloggingContext
            context.Database.EnsureDeleted()
            context.Database.EnsureCreated()
        End Using

#Region "ClientProjection"
        Using context As New BloggingContext
            Dim blogs1 = context.Blogs.
                                 OrderByDescending(Function(blog) blog.Rating).
                                 Select(Function(blog) New With {.Id = blog.BlogId, .Url = StandardizeUrl(blog.Url)}).
                                 ToList()
        End Using
#End Region

#Region "ClientWhere"
        Using context As New BloggingContext
            Try
                Dim blogs1 = context.Blogs.
                                     Where(Function(blog) StandardizeUrl(blog.Url).Contains("dotnet")).
                                     ToList()

            Catch e As Exception
                Console.WriteLine(e.Message)
            End Try
        End Using
#End Region

#Region "ExplicitClientEvaluation"
        Using context As New BloggingContext
            Dim blogs1 = context.Blogs.
                                 AsEnumerable().
                                 Where(Function(blog) StandardizeUrl(blog.Url).
                                 Contains("dotnet")).
                                 ToList()
        End Using
#End Region

    End Sub

End Module
