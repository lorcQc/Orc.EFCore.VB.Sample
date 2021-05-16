Imports System.ComponentModel.DataAnnotations.Schema
Imports Microsoft.EntityFrameworkCore

Namespace IgnoreProperty
    Friend Class MyContext
        Inherits DbContext
        Public Property Blogs As DbSet(Of Blog)
    End Class

#Region "IgnoreProperty"
    Public Class Blog
        Public Property BlogId As Integer
        Public Property Url As String

        <NotMapped>
        Public Property LoadedFromDatabase As DateTime
    End Class
#End Region

End Namespace
