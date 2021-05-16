Imports Microsoft.EntityFrameworkCore

Namespace KeyId
    Friend Class MyContext
        Inherits DbContext
        Public Property Cars As DbSet(Of Car)
        Public Property Trucks As DbSet(Of Truck)
    End Class

#Region "KeyId"
    Friend Class Car
        Public Property Id As String

        Public Property Make As String
        Public Property Model As String
    End Class

    Friend Class Truck
        Public Property TruckId As String

        Public Property Make As String
        Public Property Model As String
    End Class
#End Region

End Namespace
