Imports Microsoft.EntityFrameworkCore

Namespace DiscriminatorMappingIncomplete
    Public Class MyContext
        Inherits DbContext

        Public Property Blogs As DbSet(Of Blog)

#Region "DiscriminatorMappingIncomplete"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.Entity(Of Blog)().HasDiscriminator().IsComplete(False)
        End Sub
#End Region
    End Class

    Public Class Blog
        Public Property BlogId As Integer
        Public Property Url As String
    End Class

    Public Class RssBlog
        Inherits Blog
        Public Property RssUrl As String
    End Class
End Namespace
