Imports Microsoft.EntityFrameworkCore
Imports Microsoft.EntityFrameworkCore.Metadata

Namespace TPTConfiguration
    Public Class MyContext
        Inherits DbContext
        Public Sub New(options As DbContextOptions(Of MyContext))
            MyBase.New(options)
        End Sub

        Public Property Blogs As DbSet(Of Blog)
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
#Region "TPTConfiguration"
            modelBuilder.Entity(Of Blog)().ToTable("Blogs")
            modelBuilder.Entity(Of RssBlog)().ToTable("RssBlogs")
#End Region

#Region "Metadata"
            For Each entityType In modelBuilder.Model.GetEntityTypes()
                Dim tableIdentifier = StoreObjectIdentifier.Create(entityType, StoreObjectType.Table)

                Console.WriteLine($"{entityType.DisplayName()}		{tableIdentifier}")
                Console.WriteLine(" Property" & vbTab & "Column")

                For Each prop In entityType.GetProperties()
                    Dim columnName = prop.GetColumnName(tableIdentifier.Value)
                    Console.WriteLine($" {prop.Name,-10}	{columnName}")
                Next

                Console.WriteLine()
            Next
#End Region
        End Sub
    End Class

    Public Class Blog
        Public Property BlogId As Integer
        Public Property Url As String
    End Class

    Public Class RssBlog
        Inherits Blog
        Public Property RssUrl As String
    End Class
End Namespace
