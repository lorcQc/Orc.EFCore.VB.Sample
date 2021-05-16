Imports System.Runtime.CompilerServices
Imports NetTopologySuite.Geometries
Imports ProjNet
Imports ProjNet.CoordinateSystems
Imports ProjNet.CoordinateSystems.Transformations

#Region "snippet_GeometryExtensions"

Friend Module GeometryExtensions

    Private ReadOnly _coordinateSystemServices As New CoordinateSystemServices(
            New Dictionary(Of Integer, String) From { _
 _ ' Coordinate systems:
                {4326, GeographicCoordinateSystem.WGS84.WKT},
 _ ' This coordinate system covers the area of our data.
 _ ' Different data requires a different coordinate system.
                {2855, "
                        PROJCS[""NAD83(HARN) / Washington North"",
                            GEOGCS[""NAD83(HARN)"",
                                DATUM[""NAD83_High_Accuracy_Regional_Network"",
                                    SPHEROID[""GRS 1980"",6378137,298.257222101,
                                        AUTHORITY[""EPSG"",""7019""]],
                                    AUTHORITY[""EPSG"",""6152""]],
                                PRIMEM[""Greenwich"",0,
                                    AUTHORITY[""EPSG"",""8901""]],
                                UNIT[""degree"",0.01745329251994328,
                                    AUTHORITY[""EPSG"",""9122""]],
                                AUTHORITY[""EPSG"",""4152""]],
                            PROJECTION[""Lambert_Conformal_Conic_2SP""],
                            PARAMETER[""standard_parallel_1"",48.73333333333333],
                            PARAMETER[""standard_parallel_2"",47.5],
                            PARAMETER[""latitude_of_origin"",47],
                            PARAMETER[""central_meridian"",-120.8333333333333],
                            PARAMETER[""false_easting"",500000],
                            PARAMETER[""false_northing"",0],
                            UNIT[""metre"",1,
                                AUTHORITY[""EPSG"",""9001""]],
                            AUTHORITY[""EPSG"",""2855""]]
                    "
                }
            })

    <Extension()>
    Public Function ProjectTo(geometry1 As Geometry, srid As Integer) As Geometry
        Dim transformation = _coordinateSystemServices.CreateTransformation(geometry1.SRID, srid)

        Dim result = geometry1.Copy()
        result.Apply(New MathTransformFilter(transformation.MathTransform))

        Return result
    End Function

    Private Class MathTransformFilter
        Implements ICoordinateSequenceFilter

        Private ReadOnly _Transform As MathTransform

        Public Sub New(transform As MathTransform)
            _Transform = transform
        End Sub

        Public ReadOnly Property Done As Boolean Implements ICoordinateSequenceFilter.Done
            Get
                Return False
            End Get
        End Property

        Public ReadOnly Property GeometryChanged As Boolean Implements ICoordinateSequenceFilter.GeometryChanged
            Get
                Return True
            End Get
        End Property

        Public Sub Filter(seq As CoordinateSequence, i As Integer) Implements ICoordinateSequenceFilter.Filter
            Dim x As Double = seq.GetX(i)
            Dim y As Double = seq.GetY(i)

            Dim t = _Transform.Transform({x, y})

            seq.SetX(i, t(0))
            seq.SetY(i, t(1))
        End Sub
    End Class
End Module
#End Region
