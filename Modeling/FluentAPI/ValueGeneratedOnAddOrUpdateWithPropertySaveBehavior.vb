Imports Microsoft.EntityFrameworkCore
Imports Microsoft.EntityFrameworkCore.Metadata

Namespace ValueGeneratedOnAddOrUpdateWithPropertySaveBehavior
    Friend Class MyContext
        Inherits DbContext

        Public Property Blogs As DbSet(Of Blog)

#Region "ValueGeneratedOnAddOrUpdateWithPropertySaveBehavior"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.
                Entity(Of Blog)().
                Property(Function(b) b.LastUpdated).ValueGeneratedOnAddOrUpdate().
                Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Save)
        End Sub
    End Class
#End Region

    Public Class Blog
        Public Property BlogId As Integer
        Public Property Url As String
        Public Property LastUpdated As DateTime
    End Class
End Namespace
