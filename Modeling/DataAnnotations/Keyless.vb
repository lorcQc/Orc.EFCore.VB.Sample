Imports Microsoft.EntityFrameworkCore

Namespace Keyless
    Friend Class BlogsContext
        Inherits DbContext
        Public Property BlogPostCounts As DbSet(Of BlogPostsCount)
    End Class

#Region "Keyless"
    <Keyless>
    Public Class BlogPostsCount
        Public Property BlogName As String
        Public Property PostCount As Integer
    End Class
#End Region

End Namespace
