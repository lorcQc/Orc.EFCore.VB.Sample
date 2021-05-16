Imports System.ComponentModel.DataAnnotations
Imports Microsoft.EntityFrameworkCore

Namespace KeySingle
    Friend Class MyContext
        Inherits DbContext

        Public Property Cars As DbSet(Of Car)
    End Class

#Region "KeySingle"
    Friend Class Car
        <Key>
        Public Property LicensePlate As String

        Public Property Make As String
        Public Property Model As String
    End Class
#End Region

End Namespace
