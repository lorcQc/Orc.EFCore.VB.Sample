Imports System.ComponentModel.DataAnnotations.Schema
Imports Microsoft.EntityFrameworkCore

Namespace Relational.ColumnName
    Friend Class MyContext
        Inherits DbContext
        Public Property Blogs As DbSet(Of Blog)
    End Class

#Region "ColumnName"
    Public Class Blog
        <Column("blog_id")>
        Public Property BlogId As Integer

        Public Property Url As String
    End Class
#End Region

End Namespace
