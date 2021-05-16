Imports Microsoft.EntityFrameworkCore

Namespace Relationships.OneNavigation
    Friend Class MyContext
        Inherits DbContext
        Public Property Blogs As DbSet(Of Blog)
        Public Property Posts As DbSet(Of Post)
    End Class

#Region "OneNavigation"
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
#End Region

End Namespace
