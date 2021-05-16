Imports System
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.Extensions.Logging

Namespace LazyLoading
    Public Class LazyBloggingContext
        Inherits DbContext
        Public Property Blogs As DbSet(Of Blog)
        Public Property Posts As DbSet(Of Post)

        Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
            Call optionsBuilder.
                    UseSqlServer("Server=(localdb)\mssqllocaldb;Database=sample_Blogging;Integrated Security=True").
                    LogTo(AddressOf Console.WriteLine, LogLevel.Information).
                    UseLazyLoadingProxies()
        End Sub
    End Class

    Public Class Blog
        Public Property BlogId As Integer
        Public Property Url As String
        Public Property Rating As Integer
        Public Overridable Property Posts As List(Of Post)
    End Class

    Public Class Post
        Public Property PostId As Integer
        Public Property Title As String
        Public Property Content As String
        Public Property BlogId As Integer
        Public Overridable Property Blog As Blog
    End Class
End Namespace
