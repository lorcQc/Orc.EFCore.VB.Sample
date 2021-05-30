Imports System.ComponentModel.DataAnnotations.Schema

Namespace Disconnected
    Public MustInherit Class EntityBase
        <NotMapped>
        Public Property IsNew As Boolean

        <NotMapped>
        Public Property IsDeleted As Boolean

        <NotMapped>
        Public Property IsChanged As Boolean
    End Class
End Namespace
