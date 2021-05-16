Imports System.ComponentModel.DataAnnotations.Schema
Imports Microsoft.EntityFrameworkCore

Namespace ValueGeneratedOnAdd
    Friend Class MyContext
        Inherits DbContext
        Public Property Blogs As DbSet(Of Blog)
    End Class


#Region "ValueGeneratedOnAdd"
    Public Class Blog
        Public Property BlogId As Integer
        Public Property Url As String

        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property Inserted As Date
    End Class
#End Region

End Namespace
