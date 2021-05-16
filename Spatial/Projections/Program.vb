Imports NetTopologySuite.Geometries

Module Program
    Sub Main()

#Region "snippet_ProjectTo"
        Dim seattle As New Point(-122.333056, 47.609722) With {
            .SRID = 4326}

        Dim redmond As New Point(-122.123889, 47.669444) With {
            .SRID = 4326}

        ' In order to get the distance in meters, we need to project to an appropriate
        ' coordinate system. In this case, we're using SRID 2855 since it covers the
        ' geographic area of our data
        Dim distanceInDegrees = seattle.Distance(redmond)
        Dim distanceInMeters = seattle.ProjectTo(2855).Distance(redmond.ProjectTo(2855))
#End Region

        Console.WriteLine($"Degrees: {distanceInDegrees:N4}")
        Console.WriteLine($"Meters:  {distanceInMeters:N0}")

    End Sub
End Module
