Imports Microsoft.EntityFrameworkCore

Namespace Relational.IndexName
    Friend Class MyContext
        Inherits DbContext
        Public Property Blogs As DbSet(Of Blog)
    End Class

#Region "IndexName"
    <Index(NameOf(Blog.Url), Name:="Index_Url")>
    Public Class Blog
        Public Property BlogId As Integer
        Public Property Url As String
    End Class
#End Region

End Namespace
