Imports System.ComponentModel.DataAnnotations
Imports Microsoft.EntityFrameworkCore

Namespace Timestamp
    Friend Class MyContext
        Inherits DbContext
        Public Property Blogs As DbSet(Of Blog)
    End Class

#Region "Timestamp"
    Public Class Blog
        Public Property BlogId As Integer

        Public Property Url As String

        <Timestamp>
        Public Property Timestamp As Byte()
    End Class
#End Region

End Namespace
