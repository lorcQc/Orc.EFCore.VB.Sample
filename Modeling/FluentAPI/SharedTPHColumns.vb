Imports Microsoft.EntityFrameworkCore

Namespace SharedTPHColumns
#Region "SharedTPHColumns"
    Public Class MyContext
        Inherits DbContext

        Public Property Blogs As DbSet(Of BlogBase)

        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.
                Entity(Of Blog)().
                Property(Function(b) b.Url).
                HasColumnName("Url")

            modelBuilder.
                Entity(Of RssBlog)().
                Property(Function(b) b.Url).
                HasColumnName("Url")
        End Sub
    End Class

    Public MustInherit Class BlogBase
        Public Property BlogId As Integer
    End Class

    Public Class Blog
        Inherits BlogBase
        Public Property Url As String
    End Class

    Public Class RssBlog
        Inherits BlogBase
        Public Property Url As String
    End Class
#End Region

End Namespace
