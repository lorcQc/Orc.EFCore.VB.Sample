#Region "Order"
Public Class Order
    Public Property Id As Integer
    Public Property Status As OrderStatus?
    Public Property DetailedOrder As DetailedOrder
End Class

#End Region
