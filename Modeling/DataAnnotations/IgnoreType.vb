Imports System.ComponentModel.DataAnnotations.Schema
Imports Microsoft.EntityFrameworkCore

Namespace IgnoreType
    Friend Class MyContext
        Inherits DbContext
        Public Property Blogs As DbSet(Of Blog)
    End Class

    Public Class Blog
        Public Property BlogId As Integer
        Public Property Url As String

        Public Property Metadata As BlogMetadata
    End Class

#Region "IgnoreType"
    <NotMapped>
    Public Class BlogMetadata
        Public Property LoadedFromDatabase As DateTime
    End Class
#End Region

End Namespace
