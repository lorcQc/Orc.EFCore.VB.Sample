Imports System.ComponentModel.DataAnnotations.Schema
Imports NetTopologySuite.Geometries

Namespace Models

    <Table("StateProvinces", Schema:="Application")>
    Friend Class StateProvince
        Public Property StateProvinceID As Integer

        Public Property StateProvinceName As String

        Public Property Border As Geometry
    End Class

End Namespace
