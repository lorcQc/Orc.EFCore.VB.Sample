Imports Microsoft.EntityFrameworkCore

Namespace ShadowForeignKey
#Region "Conventions"
    Friend Class MyContext
        Inherits DbContext

        Public Property Blogs As DbSet(Of Blog)
        Public Property Posts As DbSet(Of Post)
    End Class

    Public Class Blog
        Public Property BlogId As Integer
        Public Property Url As String

        Public Property Posts As List(Of Post)
    End Class

    Public Class Post
        Public Property PostId As Integer
        Public Property Title As String
        Public Property Content As String

        ' Since there is no CLR property which holds the foreign
        ' key for this relationship, a shadow property is created.
        Public Property Blog As Blog
    End Class
#End Region

End Namespace
