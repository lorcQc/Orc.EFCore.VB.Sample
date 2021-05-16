Imports System.IO
Imports NetTopologySuite.Geometries
Imports NetTopologySuite.IO
Imports SqlServer.Models

Module Program
    Sub Main(args As String())

        Using context As New WideWorldImportersContext
            context.Database.EnsureDeleted()
            context.Database.EnsureCreated()

            context.AddRange(
                New City With {
                    .CityName = "Bellemondville",
                    .Location = New Point(-122.128822, 47.643703) With {
                    .SRID = 4326}},
                New Country With {
                    .CountryName = "'Merica",
                    .Border = New Polygon(
                                New LinearRing({
                                    New Coordinate(-123.128822, 46.643703), New Coordinate(-121.128822, 46.643703),
                                    New Coordinate(-121.128822, 48.643703), New Coordinate(-123.128822, 48.643703),
                                    New Coordinate(-123.128822, 46.643703)})) With {.SRID = 4326}
                })

            context.SaveChanges()
        End Using

        Dim currentLocation As New Point(-122.128822, 47.643703) With {.SRID = 4326}
        Dim db = New WideWorldImportersContext

#Region "snippet_Distance"
        ' Find the nearest city
        Dim nearestCity = db.Cities.
                             OrderBy(Function(c) c.Location.Distance(currentLocation)).
                             FirstOrDefault()
#End Region

        Console.WriteLine($"Nearest city: {nearestCity.CityName}")

#Region "snippet_Contains"
        ' Find the containing country
        Dim currentCountry = db.Countries.FirstOrDefault(Function(c) c.Border.Contains(currentLocation))
#End Region

        Console.WriteLine($"Current country: {currentCountry.CountryName}")

        ' Find which states/provinces a route intersects
        Dim route = (New GeoJsonReader).Read(Of LineString)(File.ReadAllText("seattle-to-new-york.json"))
        route.SRID = 4326

        Dim statePorvincesIntersected = (From s In db.StateProvinces
                                         Where s.Border.Intersects(route)
                                         Order By s.Border.Distance(currentLocation)
                                         Select s).ToList()

        Console.WriteLine("States/provinces intersected:")

        For Each state In statePorvincesIntersected
            Console.WriteLine($"	{state.StateProvinceName}")
        Next

    End Sub
End Module
