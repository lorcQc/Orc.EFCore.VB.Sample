Imports Microsoft.EntityFrameworkCore

Namespace IgnoreProperty
    Friend Class MyContext
        Inherits DbContext

        Public Property Blogs As DbSet(Of Blog)

#Region "IgnoreProperty"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.
                Entity(Of Blog)().
                Ignore(Function(b) b.LoadedFromDatabase)
        End Sub
#End Region
    End Class

    Public Class Blog
        Public Property BlogId As Integer
        Public Property Url As String
        Public Property LoadedFromDatabase As DateTime
    End Class
End Namespace
