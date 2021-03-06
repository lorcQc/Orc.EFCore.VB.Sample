Imports System.Net.Http
Imports Microsoft.EntityFrameworkCore

Namespace BackingFieldNoProperty

#Region "Sample"
    Friend Class MyContext
        Inherits DbContext

        Public Property Blogs As DbSet(Of Blog)

        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.Entity(Of Blog)().Property("_validatedUrl")
        End Sub
    End Class

    Public Class Blog
        Private _validatedUrl As String

        Public Property BlogId As Integer

        Public Function GetUrl() As String
            Return _validatedUrl
        End Function

        Public Sub SetUrl(url As String)
            'Remarks
            'HttpClient is intended to be instantiated once and re-used throughout the life of an application.
            'Instantiating an HttpClient class for every request will exhaust the number of sockets available under heavy loads.
            'This will result in SocketException errors.
            Using client As New HttpClient
                Dim response As HttpResponseMessage = client.GetAsync(url).Result
                response.EnsureSuccessStatusCode()
            End Using

            _validatedUrl = url
        End Sub
    End Class
#End Region

End Namespace
