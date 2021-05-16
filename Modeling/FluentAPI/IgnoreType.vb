Imports Microsoft.EntityFrameworkCore

Namespace IgnoreType
    Friend Class MyContext
        Inherits DbContext
        Public Property Blogs As DbSet(Of Blog)

#Region "IgnoreType"
        Protected Overrides Sub OnModelCreating(modelBuilder1 As ModelBuilder)
            modelBuilder1.Ignore(Of BlogMetadata)()
        End Sub
#End Region
    End Class

    Public Class Blog
        Public Property BlogId As Integer
        Public Property Url As String

        Public Property Metadata As BlogMetadata
    End Class

    Public Class BlogMetadata
        Public Property LoadedFromDatabase As DateTime
    End Class
End Namespace
