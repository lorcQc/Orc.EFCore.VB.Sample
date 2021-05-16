Imports Microsoft.EntityFrameworkCore

Namespace Relationships.OneToOne
    Friend Class MyContext
        Inherits DbContext

        Public Property Blogs As DbSet(Of Blog)
        Public Property BlogImages As DbSet(Of BlogImage)
    End Class

#Region "OneToOne"
    Public Class Blog
        Public Property BlogId As Integer
        Public Property Url As String

        Public Property BlogImage As BlogImage
    End Class

    Public Class BlogImage
        Public Property BlogImageId As Integer
        Public Property Image As Byte()
        Public Property Caption As String

        Public Property BlogId As Integer
        Public Property Blog As Blog
    End Class
#End Region

End Namespace
