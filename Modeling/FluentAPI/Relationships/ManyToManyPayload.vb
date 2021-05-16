Imports Microsoft.EntityFrameworkCore

Namespace Relationships.ManyToManyPayload
#Region "ManyToManyPayload"
    Friend Class MyContext
        Inherits DbContext

        Public Sub New(options As DbContextOptions(Of MyContext))
            MyBase.New(options)
        End Sub

        Public Property Posts As DbSet(Of Post)
        Public Property Tags As DbSet(Of Tag)

        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.Entity(Of Post)().
                HasMany(Function(p) p.Tags).
                WithMany(Function(p) p.Posts).
                UsingEntity(Of PostTag)(
                    Function(j)
                        Return j.HasOne(Function(pt) pt.Tag).
                                 WithMany(Function(t) t.PostTags).
                                 HasForeignKey(Function(pt) pt.TagId)
                    End Function,
                    Function(j)
                        Return j.HasOne(Function(pt) pt.Post).
                                 WithMany(Function(p) p.PostTags).
                                 HasForeignKey(Function(pt) pt.PostId)
                    End Function,
                    Sub(j)
                        j.Property(Function(pt) pt.PublicationDate).HasDefaultValueSql("CURRENT_TIMESTAMP")
                        j.HasKey(Function(t) New With {t.PostId, t.TagId})
                    End Sub)
        End Sub
    End Class

    Public Class Post
        Public Property PostId As Integer
        Public Property Title As String
        Public Property Content As String

        Public Property Tags As ICollection(Of Tag)
        Public Property PostTags As List(Of PostTag)
    End Class

    Public Class Tag
        Public Property TagId As String

        Public Property Posts As ICollection(Of Post)
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
