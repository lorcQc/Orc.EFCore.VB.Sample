Imports System

Module Program
    Sub Main(args As String())
#Region "Usage"
        Using context As New TableSplittingContext
            context.Database.EnsureDeleted()
            context.Database.EnsureCreated()

            context.Add(New Order With {
                .Status = OrderStatus.Pending,
                .DetailedOrder = New DetailedOrder With {
                    .Status = OrderStatus.Pending,
                    .ShippingAddress = "221 B Baker St, London",
                    .BillingAddress = "11 Wall Street, New York"
                }
            })

            context.SaveChanges()
        End Using

        Using context As New TableSplittingContext
            Dim pendingCount = context.Orders.Count(Function(o) o.Status = OrderStatus.Pending)
            Console.WriteLine($"Current number of pending orders: {pendingCount}")
        End Using

        Using context As New TableSplittingContext
            Dim order = context.DetailedOrders.First(Function(o) o.Status = OrderStatus.Pending)
            Console.WriteLine($"First pending order will ship to: {order.ShippingAddress}")
        End Using
#End Region
    End Sub
End Module
