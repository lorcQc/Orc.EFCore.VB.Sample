Imports System.ComponentModel.DataAnnotations
Imports Microsoft.EntityFrameworkCore

Namespace Concurrency
    Friend Class MyContext
        Inherits DbContext

        Public Property People As DbSet(Of Person)
    End Class

#Region "Concurrency"
    Public Class Person
        Public Property PersonId As Integer

        <ConcurrencyCheck>
        Public Property LastName As String

        Public Property FirstName As String
    End Class
#End Region

End Namespace
