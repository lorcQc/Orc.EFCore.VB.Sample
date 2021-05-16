Imports Microsoft.EntityFrameworkCore

Namespace EntityTypes
#Region "EntityTypes"
    Friend Class MyContext
        Inherits DbContext

        Public Property Blogs As DbSet(Of Blog)

        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.Entity(Of AuditEntry)()
        End Sub
    End Class

    Public Class Blog
        Public Property BlogId As Integer
        Public Property Url As String

        Public Property Posts As List(Of Post)
    End Class

    Public Class Post
        Public Property PostId As Integer
        Public Property Title As String
        Public Property Content As String

        Public Property Blog As Blog
    End Class

    Public Class AuditEntry
        Public Property AuditEntryId As Integer
        Public Property Username As String
        Public Property Action As String
    End Class

#End Region

#Region "BlogWithMultiplePostsEntity"
    Public Class BlogWithMultiplePosts
        Public Property Url As String
        Public Property PostCount As Integer
    End Class

#End Region

    Public Class MyContextWithFunctionMapping
        Inherits DbContext

        Public Property Blogs As DbSet(Of Blog)
        Public Property Posts As DbSet(Of Post)

        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
#Region "QueryableFunctionConfigurationToFunction"
            modelBuilder.
                Entity(Of BlogWithMultiplePosts)().
                HasNoKey().
                ToFunction("BlogsWithMultiplePosts")
#End Region
        End Sub

        Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\mssqllocaldb;Database=sample_EFModeling.EntityTypeToFunctionMapping;Trusted_Connection=True;ConnectRetryCount=0")
        End Sub
    End Class
End Namespace
