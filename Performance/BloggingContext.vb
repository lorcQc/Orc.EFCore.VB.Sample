Imports System
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.Extensions.Logging

Public Class BloggingContext
    Inherits DbContext

    Public Property Blogs As DbSet(Of Blog)
    Public Property Posts As DbSet(Of Post)
#Region "SimpleLogging"
    Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
        optionsBuilder.
            UseSqlServer("Server=(localdb)\mssqllocaldb;Database=sample_Blogging;Integrated Security=True").
            LogTo(AddressOf Console.WriteLine, LogLevel.Information)
    End Sub
#End Region

    Protected Overrides Sub OnModelCreating(modelBuilder1 As ModelBuilder)
        modelBuilder1.Entity(Of Post)().HasIndex(Function(p) p.Title)
    End Sub
End Class

Public Class Blog
    Public Property BlogId As Integer
    Public Property Url As String
    Public Property Rating As Integer
    Public Property Posts As List(Of Post)
End Class

Public Class Post
    Public Property PostId As Integer
    Public Property Title As String
    Public Property Content As String
    Public Property BlogId As Integer
    Public Property Blog As Blog
End Class
