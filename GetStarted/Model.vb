Imports Microsoft.EntityFrameworkCore

Public Class BloggingContext
    Inherits DbContext

    Public Property Blogs As DbSet(Of Blog)
    Public Property Posts As DbSet(Of Post)

    ' The following configures EF to create a Sqlite database file as `C:\sample_blogging.db`.
    ' For Mac or Linux, change this to `/tmp/blogging.db` or any other absolute path.
    Protected Overrides Sub OnConfiguring(options As DbContextOptionsBuilder)
        Call options.UseSqlite("Data Source=C:\sample_blogging.db")
    End Sub
End Class

Public Class Blog
    Public Property BlogId As Integer
    Public Property Url As String
    Public ReadOnly Property Posts As List(Of Post) = New List(Of Post)
End Class

Public Class Post
    Public Property PostId As Integer
    Public Property Title As String
    Public Property Content As String
    Public Property BlogId As Integer
    Public Property Blog As Blog
End Class
