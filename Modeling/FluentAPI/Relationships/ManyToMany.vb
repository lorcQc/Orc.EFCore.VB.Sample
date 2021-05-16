Imports Microsoft.EntityFrameworkCore

Namespace Relationships.ManyToMany

#Region "ManyToMany"
    Public Class MyContext
        Inherits DbContext

        Public Sub New(options As DbContextOptions(Of MyContext))
            MyBase.New(options)
        End Sub

        Public Property Posts As DbSet(Of Post)
        Public Property Tags As DbSet(Of Tag)
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.Entity(Of PostTag)().
                HasKey(Function(t) New With {t.PostId, t.TagId})

            modelBuilder.Entity(Of PostTag)().
                HasOne(Function(pt) pt.Post).
                WithMany(Function(p) p.PostTags).
                HasForeignKey(Function(pt) pt.PostId)

            modelBuilder.Entity(Of PostTag)().
                HasOne(Function(pt) pt.Tag).
                WithMany(Function(t) t.PostTags).
                HasForeignKey(Function(pt) pt.TagId)
        End Sub
    End Class

    Public Class Post
        Public Property PostId As Integer
        Public Property Title As String
        Public Property Content As String

        Public Property PostTags As List(Of PostTag)
    End Class

    Public Class Tag
        Public Property TagId As String

        Public Property PostTags As List(Of PostTag)
    End Class

    Public Class PostTag
        Public Property PublicationDate As DateTime

        Public Property PostId As Integer
        Public Property Post As Post

        Public Property TagId As String
        Public Property Tag As Tag
    End Class
#End Region

End Namespace
