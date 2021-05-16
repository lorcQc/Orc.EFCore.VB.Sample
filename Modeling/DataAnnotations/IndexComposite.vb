Imports Microsoft.EntityFrameworkCore

Namespace IndexComposite
    Friend Class MyContext
        Inherits DbContext
        Public Property People As DbSet(Of Person)
    End Class

#Region "Composite"
    <Index(NameOf(Person.FirstName), NameOf(Person.LastName))>
    Public Class Person
        Public Property PersonId As Integer
        Public Property FirstName As String
        Public Property LastName As String
    End Class
#End Region

End Namespace
