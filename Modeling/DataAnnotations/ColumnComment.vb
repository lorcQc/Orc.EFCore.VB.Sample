Imports Microsoft.EntityFrameworkCore

Namespace Relational.ColumnComment
    Friend Class MyContext
        Inherits DbContext
        Public Property Blogs As DbSet(Of Blog)
    End Class

#Region "ColumnComment"
    Public Class Blog
        Public Property BlogId As Integer

        <Comment("The URL of the blog")>
        Public Property Url As String
    End Class
#End Region

End Namespace
