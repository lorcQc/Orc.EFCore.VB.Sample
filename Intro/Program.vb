Imports Microsoft.EntityFrameworkCore

Module Program
    Public Sub Main()

        Using db As New BloggingContext
            ' Remove these lines if you are running migrations from the command line
            db.Database.EnsureDeleted()
            db.Database.Migrate()
        End Using

#Region "Querying"
        Using db As New BloggingContext
            Dim blogs = db.Blogs.Where(Function(b) b.Rating > 3).
                                 OrderBy(Function(b) b.Url).
                                 ToList()
        End Using

        Using db As New BloggingContext
            Dim Blog As New Blog With {
                .Url = "http://sample.com"}

            db.Blogs.Add(Blog)
            db.SaveChanges()
        End Using
#End Region
    End Sub

End Module
