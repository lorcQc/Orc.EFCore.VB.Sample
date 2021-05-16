Imports Microsoft.EntityFrameworkCore

Namespace BackingField
    Friend Class MyContext
        Inherits DbContext

        Public Property Blogs As DbSet(Of Blog)
    End Class

#Region "BackingField"
    Public Class Blog
        Private _validatedUrl As String

        Public Property BlogId As Integer

        <BackingField(NameOf(_validatedUrl))>
        Public ReadOnly Property Url As String
            Get
                Return _validatedUrl
            End Get
        End Property

        Public Sub SetUrl(url As String)
            ' put your validation code here

            _validatedUrl = url
        End Sub
    End Class
#End Region

End Namespace
