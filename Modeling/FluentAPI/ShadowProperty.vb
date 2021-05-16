Imports Microsoft.EntityFrameworkCore

Namespace ShadowProperty
#Region "ShadowProperty"
    Friend Class MyContext
        Inherits DbContext

        Public Property Blogs As DbSet(Of Blog)
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.
                Entity(Of Blog)().
                Property(Of Date)("LastUpdated")
        End Sub
    End Class

    Public Class Blog
        Public Property BlogId As Integer
        Public Property Url As String
    End Class
#End Region

End Namespace
