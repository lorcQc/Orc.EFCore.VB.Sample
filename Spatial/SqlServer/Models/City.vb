Imports System.ComponentModel.DataAnnotations.Schema
Imports NetTopologySuite.Geometries

Namespace Models

#Region "snippet_City"

    <Table("Cities", Schema:="Application")>
    Friend Class City
        Public Property CityID As Integer
        Public Property CityName As String
        Public Property Location As Point
    End Class

#End Region

End Namespace
