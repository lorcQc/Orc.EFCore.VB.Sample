Imports Microsoft.EntityFrameworkCore

Namespace Relational.TableComment
    Friend Class MyContext
        Inherits DbContext

        Public Property Blogs As DbSet(Of Blog)
    End Class

#Region "TableComment"
    <Comment("Blogs managed on the website")>
    Public Class Blog
        Public Property BlogId As Integer
        Public Property Url As String
    End Class
#End Region

End Namespace
