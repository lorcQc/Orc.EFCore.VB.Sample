#Region "DetailedOrder"
Public Class DetailedOrder
    Public Property Id As Integer
    Public Property Status As OrderStatus?
    Public Property BillingAddress As String
    Public Property ShippingAddress As String
    Public Property Version As Byte()
End Class
#End Region
