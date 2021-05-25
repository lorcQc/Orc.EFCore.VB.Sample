Imports Microsoft.EntityFrameworkCore

Public Class SplitQueriesBloggingContext
    Inherits BloggingContext

#Region "QuerySplittingBehaviorSplitQuery"
    Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
        optionsBuilder.
            UseSqlServer(
                "Server=(localdb)\mssqllocaldb;Database=sample_EFQuerying;Trusted_Connection=True;ConnectRetryCount=0",
                Function(o) o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
    End Sub
#End Region

End Class
