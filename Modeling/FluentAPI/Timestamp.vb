Imports Microsoft.EntityFrameworkCore

Namespace Timestamp
#Region "Timestamp"
    Friend Class MyContext
        Inherits DbContext

        Public Property Blogs As DbSet(Of Blog)

        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.
                Entity(Of Blog)().
                Property(Function(p) p.Timestamp).
                IsRowVersion()
        End Sub
    End Class

    Public Class Blog
        Public Property BlogId As Integer
        Public Property Url As String
        Public Property Timestamp As Byte()
    End Class
#End Region

End Namespace
