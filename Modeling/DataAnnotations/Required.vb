Imports System.ComponentModel.DataAnnotations
Imports Microsoft.EntityFrameworkCore

Namespace Required
    Friend Class MyContext
        Inherits DbContext
        Public Property Blogs As DbSet(Of Blog)
    End Class

#Region "Required"
    Public Class Blog
        Public Property BlogId As Integer

        <Required>
        Public Property Url As String
    End Class
#End Region

End Namespace
