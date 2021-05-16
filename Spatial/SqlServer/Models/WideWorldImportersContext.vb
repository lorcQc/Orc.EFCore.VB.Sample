Imports Microsoft.EntityFrameworkCore

Namespace Models

    ' This DbContext maps to the Wide World Importers sample database which can be
    ' found at https://github.com/microsoft/sql-server-samples
    Friend Class WideWorldImportersContext
        Inherits DbContext

        Public Property Cities As DbSet(Of City)
        Public Property StateProvinces As DbSet(Of StateProvince)
        Public Property Countries As DbSet(Of Country)

        Protected Overrides Sub OnConfiguring(options As DbContextOptionsBuilder)
#Region "snippet_UseNetTopologySuite"
            Call options.UseSqlServer(
            "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=sample_WideWorldImporters",
            Function(x) x.UseNetTopologySuite())
        End Sub
    End Class
#End Region

End Namespace
