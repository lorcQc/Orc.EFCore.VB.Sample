Imports System

Module Program
    Sub Main(args As String())
        Using context As New OwnedEntityContext
            context.Database.EnsureDeleted()
            context.Database.EnsureCreated()

            context.Add(
                New DetailedOrder With {
                    .Status = OrderStatus.Pending,
                    .OrderDetails = New OrderDetails With {
                        .ShippingAddress = New StreetAddress With {
                            .City = "London",
                            .Street = "221 B Baker St"
                        },
                        .BillingAddress = New StreetAddress With {
                            .City = "New York",
                            .Street = "11 Wall Street"}
                    }
                })

            context.SaveChanges()
        End Using

        Using context As New OwnedEntityContext
#Region "DetailedOrderQuery"
            Dim order = context.DetailedOrders.First(Function(o) o.Status = OrderStatus.Pending)
            Console.WriteLine($"First pending order will ship to: {order.OrderDetails.ShippingAddress.City}")
#End Region
        End Using

    End Sub
End Module
