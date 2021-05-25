Imports System
Imports Microsoft.EntityFrameworkCore
Imports NetTopologySuite.Geometries

Module Program

    Sub Main()

        Using context As New SpatialContext
            context.Database.EnsureDeleted()
            context.Database.EnsureCreated()
        End Using

#Region "BasicQueryTag"
        Using context As New SpatialContext
            Dim myLocation As New Point(1, 2)
            Dim nearestPeople = (From f In context.People.TagWith("This is my spatial query!")
                                 Order By f.Location.Distance(myLocation)
                                 Select f).Take(5).ToList()
        End Using
#End Region

#Region "ChainedQueryTags"
        Using context As New SpatialContext
            Dim results = Limit(GetNearestPeople(context, New Point(1, 2)), 25).ToList()
        End Using
#End Region

#Region "MultilineQueryTag"
        Using context As New SpatialContext
            Dim results = Limit(GetNearestPeople(context, New Point(1, 2)), 25).TagWith(
            "This is a multi-line
string").ToList()
        End Using
#End Region

    End Sub

#Region "QueryableMethods"
    Private Function GetNearestPeople(context As SpatialContext, myLocation As Point) As IQueryable(Of Person)
        Return From f In context.People.TagWith("GetNearestPeople")
               Order By f.Location.Distance(myLocation)
               Select f
    End Function

    Private Function Limit(Of T)(source As IQueryable(Of T), limit1 As Integer) As IQueryable(Of T)
        Return source.TagWith("Limit").Take(limit1)
    End Function
#End Region

End Module
