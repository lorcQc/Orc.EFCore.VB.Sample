Imports System.ComponentModel.DataAnnotations.Schema
Imports Microsoft.EntityFrameworkCore

Namespace Relational.ColumnDataType
    Friend Class MyContext
        Inherits DbContext
        Public Property Blogs As DbSet(Of Blog)
    End Class

#Region "ColumnDataType"
    Public Class Blog
        Public Property BlogId As Integer

        <Column(TypeName:="varchar(200)")>
        Public Property Url As String

        <Column(TypeName:="decimal(5, 2)")>
        Public Property Rating As Decimal
    End Class
#End Region

End Namespace
