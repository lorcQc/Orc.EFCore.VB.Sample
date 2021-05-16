Imports System.ComponentModel.DataAnnotations.Schema
Imports Microsoft.EntityFrameworkCore

Namespace ValueGeneratedNever
    Friend Class MyContext
        Inherits DbContext
        Public Property Blogs As DbSet(Of Blog)
    End Class


#Region "ValueGeneratedNever"
    Public Class Blog
        <DatabaseGenerated(DatabaseGeneratedOption.None)>
        Public Property BlogId As Integer

        Public Property Url As String
    End Class
#End Region

End Namespace
