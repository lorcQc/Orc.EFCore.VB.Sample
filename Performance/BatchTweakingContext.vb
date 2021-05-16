Imports Microsoft.EntityFrameworkCore

Public Class BatchTweakingContext
    Inherits DbContext

#Region "BatchTweaking"
    Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
        optionsBuilder.
            UseSqlServer("Server=(localdb)\mssqllocaldb;Database=sample_Blogging;Integrated Security=True",
                Function(o) o.MinBatchSize(1).MaxBatchSize(100)
            )
    End Sub
#End Region
End Class
