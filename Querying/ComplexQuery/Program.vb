Imports System

Module Program

    Sub Main()

#Region "Join"
        Using context As New BloggingContext

            Dim query = From photo In context.Set(Of PersonPhoto)()
                        Join person In context.Set(Of Person)() On photo.PersonPhotoId Equals person.PhotoId
                        Select New With {person, photo}
        End Using
#End Region

#Region "JoinComposite"
        Using context As New BloggingContext
            Dim query = From photo In context.Set(Of PersonPhoto)()
                        Join person In context.Set(Of Person)()
                            On New With {Key .Id = CType(photo.PersonPhotoId, Integer?), Key photo.Caption} Equals
                            New With {Key .Id = person.PhotoId, Key .Caption = "SN"}
                        Select New With {person, photo}
        End Using
#End Region

#Region "GroupJoin"
        Using context As New BloggingContext
            Dim query = From b In context.Set(Of Blog)()
                        Group Join p In context.Set(Of Post)() On b.BlogId Equals p.PostId Into Group
                        Select New With {b, Group}
        End Using
#End Region

#Region "GroupJoinComposed"
        Using context As New BloggingContext
            Dim query = From b In context.Set(Of Blog)()
                        Group Join p In context.Set(Of Post)() On b.BlogId Equals p.PostId Into Group
                        Select New With {b, Key .Posts = Group.Where(Function(p) p.Content.Contains("EF")).ToList()}
        End Using
#End Region

#Region "SelectManyConvertedToCrossJoin"
        Using context As New BloggingContext

            Dim query = From b In context.Set(Of Blog)()
                        From p In context.Set(Of Post)()
                        Select New With {b, p}
        End Using
#End Region

#Region "SelectManyConvertedToJoin"
        Using context As New BloggingContext

            Dim query = From b In context.Set(Of Blog)()
                        From p In context.Set(Of Post)().Where(Function(p) b.BlogId = p.BlogId)
                        Select New With {b, p}

            Dim query2 = From b In context.Set(Of Blog)()
                         From p In context.Set(Of Post)().Where(Function(p) b.BlogId = p.BlogId).DefaultIfEmpty()
                         Select New With {b, p}
        End Using
#End Region

#Region "SelectManyConvertedToApply"
        Using context As New BloggingContext

            Dim query = From b In context.Set(Of Blog)()
                        From p In context.Set(Of Post)().Select(Function(p) b.Url & "=>" & p.Title)
                        Select New With {b, p}

            Dim query2 = From b In context.Set(Of Blog)()
                         From p In context.Set(Of Post)().Select(Function(p) b.Url & "=>" & p.Title).DefaultIfEmpty()
                         Select New With {b, p}
        End Using
#End Region

#Region "GroupBy"
        Using context As New BloggingContext
            Dim query = From p In context.Set(Of Post)()
                        Group p By myKey = p.AuthorId Into g = Group
                        Select New With {myKey, g.Count()}
        End Using
#End Region

#Region "GroupByFilter"
        Using context As New BloggingContext

            Dim query = From p In context.Set(Of Post)()
                        Group p By myKey = p.AuthorId Into g = Group
                        Select New With {myKey, g.Count()}
        End Using
#End Region

#Region "LeftJoin"
        Using context As New BloggingContext

            Dim query = From b In context.Set(Of Blog)()
                        Group Join p In context.Set(Of Post)() On b.BlogId Equals p.BlogId Into grouping = Group
                        From p In grouping.DefaultIfEmpty()
                        Select New With {b, p}
        End Using
#End Region

    End Sub

End Module
