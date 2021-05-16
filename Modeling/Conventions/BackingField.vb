Imports Microsoft.EntityFrameworkCore

Namespace BackingField
    Friend Class MyContext
        Inherits DbContext

        Public Property Blogs As DbSet(Of Blog)
    End Class


#Region "Sample"
    Public Class Blog
        Private _url As String

        Public Property BlogId As Integer
        Public Property Url As String
            Get
                Return _url
            End Get

            Set(Value As String)
                _url = Value
            End Set
        End Property
    End Class
#End Region

End Namespace
