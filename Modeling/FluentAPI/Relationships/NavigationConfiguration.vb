Imports Microsoft.EntityFrameworkCore

Namespace Relationships.NavigationConfiguration
    Friend Class MyContext
        Inherits DbContext

        Public Property Blogs As DbSet(Of Blog)
        Public Property Posts As DbSet(Of Post)

#Region "NavigationConfiguration"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.
                Entity(Of Blog)().
                HasMany(Function(b) b.Posts).
                WithOne()

            modelBuilder.
                Entity(Of Blog)().
                Navigation(Function(b) b.Posts).
                UsePropertyAccessMode(PropertyAccessMode.Property)
        End Sub
    End Class
#End Region

    Public Class Blog
        Public Property BlogId As Integer
        Public Property Url As String

        Public Property Posts As List(Of Post)
    End Class

    Public Class Post
        Public Property PostId As Integer
        Public Property Title As String
        Public Property Content As String
    End Class
End Namespace
