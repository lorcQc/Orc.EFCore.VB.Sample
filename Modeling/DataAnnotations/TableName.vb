Imports System.ComponentModel.DataAnnotations.Schema
Imports Microsoft.EntityFrameworkCore

Namespace Relational.TableName
    Friend Class MyContext
        Inherits DbContext
        Public Property Blogs As DbSet(Of Blog)
    End Class

#Region "TableName"
    <Table("blogs")>
    Public Class Blog
        Public Property BlogId As Integer
        Public Property Url As String
    End Class
#End Region

End Namespace
