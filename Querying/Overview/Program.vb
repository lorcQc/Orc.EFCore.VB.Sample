Imports System

Module Program
    Sub Main(args As String())

        Using context As New BloggingContext
            context.Database.EnsureDeleted()
            context.Database.EnsureCreated()
        End Using

#Region "LoadingAllData"
        Using context As New BloggingContext
            Dim blogs1 = context.Blogs.ToList()
        End Using
#End Region

#Region "LoadingSingleEntity"
        Using context As New BloggingContext
            Dim blog = context.Blogs.
                               Single(Function(b) b.BlogId = 1)
        End Using
#End Region

#Region "Filtering"
        Using context As New BloggingContext
            Dim blogs1 = context.Blogs.
                                 Where(Function(b) b.Url.Contains("dotnet")).
                                 ToList()
        End Using
#End Region

    End Sub
End Module
