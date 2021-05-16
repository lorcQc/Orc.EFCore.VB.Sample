Imports Microsoft.EntityFrameworkCore

Namespace Relationships.ManyToManyShared
    Public Class MyContext
        Inherits DbContext

        Public Sub New(options As DbContextOptions(Of MyContext))
            MyBase.New(options)
        End Sub

        Public Property Posts As DbSet(Of Post)
        Public Property Tags As DbSet(Of Tag)

        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
#Region "SharedConfiguration"
            modelBuilder.
                Entity(Of Post)().HasMany(Function(p) p.Tags).
                WithMany(Function(p) p.Posts).
                UsingEntity(Function(j) j.ToTable("PostTags"))
#End Region

#Region "Metadata"
            For Each entityType In modelBuilder.Model.GetEntityTypes()
                For Each skipNavigation In entityType.GetSkipNavigations()
                    Console.WriteLine(entityType.DisplayName() & "." + skipNavigation.Name)
                Next
            Next
#End Region

#Region "Seeding"
            modelBuilder.
                Entity(Of Post)().HasData(New Post With {
                    .PostId = 1,
                    .Title = "First"})

            modelBuilder.
                Entity(Of Tag)().HasData(New Tag With {
                    .TagId = "ef"})

            modelBuilder.
                Entity(Of Post)().
                    HasMany(Function(p) p.Tags).
                    WithMany(Function(p) p.Posts).
                    UsingEntity(Function(j) j.HasData(New With {.PostsPostId = 1, .TagsTagId = "ef"}))
#End Region

#Region "Components"
            modelBuilder.
                Entity(Of Post)().
                    HasMany(Function(p) p.Tags).
                    WithMany(Function(p) p.Posts).
                    UsingEntity(Of Dictionary(Of String, Object))(
                        "PostTag",
                        Function(j)
                            Return j.HasOne(Of Tag)().
                                     WithMany().HasForeignKey("TagId").
                                     HasConstraintName("FK_PostTag_Tags_TagId").
                                     OnDelete(DeleteBehavior.Cascade)
                        End Function,
                        Function(j)
                            Return j.HasOne(Of Post)().
                                     WithMany().
                                     HasForeignKey("PostId").
                                     HasConstraintName("FK_PostTag_Posts_PostId").
                                     OnDelete(DeleteBehavior.ClientCascade)
                        End Function)
#End Region
        End Sub
    End Class


#Region "ManyToManyShared"
    Public Class Post
        Public Property PostId As Integer
        Public Property Title As String
        Public Property Content As String

        Public Property Tags As ICollection(Of Tag)
    End Class

    Public Class Tag
        Public Property TagId As String

        Public Property Posts As ICollection(Of Post)
    End Class
#End Region

End Namespace
