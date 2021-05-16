Imports System.Net.Http
Imports Microsoft.EntityFrameworkCore

Namespace BackingField
    Friend Class MyContext
        Inherits DbContext

        Public Property Blogs As DbSet(Of Blog)
#Region "BackingField"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.
                Entity(Of Blog)().Property(Function(b) b.Url).
                HasField("_validatedUrl")
        End Sub
    End Class
#End Region

    Public Class Blog
        Public Property BlogId As Integer

        Private _Url As String
        Public Property Url As String
            Get
                Return _Url
            End Get
            Private Set
                _Url = Value
            End Set
        End Property

        Public Sub SetUrl(url As String)

            'Remarks
            'HttpClient is intended to be instantiated once and re-used throughout the life of an application.
            'Instantiating an HttpClient class for every request will exhaust the number of sockets available under heavy loads.
            'This will result in SocketException errors.
            Using client As New HttpClient
                Dim response As HttpResponseMessage = client.GetAsync(url).Result
                response.EnsureSuccessStatusCode()
            End Using

            Me.Url = url
        End Sub
    End Class
End Namespace
