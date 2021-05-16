Imports Microsoft.EntityFrameworkCore

Namespace Keyless
    Friend Class BlogsContext
        Inherits DbContext

        Public Property BlogPostCounts As DbSet(Of BlogPostsCount)

#Region "Keyless"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.
                Entity(Of BlogPostsCount)().
                HasNoKey()
        End Sub
#End Region
    End Class

    Public Class BlogPostsCount
        Public Property BlogName As String
        Public Property PostCount As Integer
    End Class
End Namespace
