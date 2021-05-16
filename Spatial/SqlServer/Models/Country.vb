Imports System.ComponentModel.DataAnnotations.Schema
Imports NetTopologySuite.Geometries

Namespace Models

#Region "snippet_Country"
    <Table("Countries", Schema:="Application")>
    Friend Class Country
        Public Property CountryID As Integer

        Public Property CountryName As String

        ' Database includes both Polygon and MultiPolygon values
        Public Property Border As Geometry
    End Class

#End Region

End Namespace
