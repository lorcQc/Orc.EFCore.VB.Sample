Imports Microsoft.EntityFrameworkCore

Namespace ViewNameAndSchema
    Friend Class MyContext
        Inherits DbContext

        Public Property Blogs As DbSet(Of Blog)

        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
#Region "ViewNameAndSchema"
            modelBuilder.
                Entity(Of Blog)().
                ToView("blogsView", schema:="blogging")
#End Region
        End Sub
    End Class

    Public Class Blog
        Public Property BlogId As Integer
        Public Property Url As String
    End Class
End Namespace
