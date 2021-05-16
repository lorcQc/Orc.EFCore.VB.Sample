Imports Microsoft.EntityFrameworkCore

Namespace IndexUnique
    Friend Class MyContext
        Inherits DbContext
        Public Property Blogs As DbSet(Of Blog)
    End Class

#Region "IndexUnique"
    <Index(NameOf(Blog.Url), IsUnique:=True)>
    Public Class Blog
        Public Property BlogId As Integer
        Public Property Url As String
    End Class
#End Region

End Namespace
