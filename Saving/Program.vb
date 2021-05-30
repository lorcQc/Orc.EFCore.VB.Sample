Module Program
    Sub Main()
        Basics.Sample.Run()
        RelatedData.Sample.Run()
        CascadeDelete.Sample.Run()
        Concurrency.Run()
        Transactions.AmbientTransaction.Run()
        Transactions.ControllingTransaction.Run()
        Transactions.ManagingSavepoints.Run()
        Transactions.SharingTransaction.Run()
        Transactions.ExternalDbTransaction.Run()
        Disconnected.Run()
    End Sub
End Module
