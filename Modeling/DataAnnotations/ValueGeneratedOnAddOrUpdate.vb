Imports System.ComponentModel.DataAnnotations.Schema
Imports Microsoft.EntityFrameworkCore

Namespace ValueGeneratedOnAddOrUpdate
    Friend Class MyContext
        Inherits DbContext
        Public Property Blogs As DbSet(Of Blog)
    End Class

#Region "ValueGeneratedOnAddOrUpdate"
    Public Class Blog
        Public Property BlogId As Integer
        Public Property Url As String

        <DatabaseGenerated(DatabaseGeneratedOption.Computed)>
        Public Property LastUpdated As DateTime
    End Class
#End Region

End Namespace
