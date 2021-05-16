Imports System.ComponentModel.DataAnnotations
Imports Microsoft.EntityFrameworkCore

Namespace MaxLength
    Friend Class MyContext
        Inherits DbContext
        Public Property Blogs As DbSet(Of Blog)
    End Class


#Region "MaxLength"
    Public Class Blog
        Public Property BlogId As Integer

        <MaxLength(500)>
        Public Property Url As String
    End Class
#End Region

End Namespace
