Imports System.ComponentModel.DataAnnotations.Schema
Imports Microsoft.EntityFrameworkCore

Namespace Relationships.InverseProperty
    Friend Class MyContext
        Inherits DbContext
        Public Property Posts As DbSet(Of Post)
        Public Property Users As DbSet(Of User)
    End Class

#Region "InverseProperty"
    Public Class Post
        Public Property PostId As Integer
        Public Property Title As String
        Public Property Content As String

        Public Property AuthorUserId As Integer
        Public Property Author As User

        Public Property ContributorUserId As Integer
        Public Property Contributor As User
    End Class

    Public Class User
        Public Property UserId As String
        Public Property FirstName As String
        Public Property LastName As String

        <InverseProperty("Author")>
        Public Property AuthoredPosts As List(Of Post)

        <InverseProperty("Contributor")>
        Public Property ContributedToPosts As List(Of Post)
    End Class
#End Region

End Namespace
