Imports Microsoft.EntityFrameworkCore

Namespace TableExcludeFromMigrations
    Friend Class MyContext
        Inherits DbContext

        Public Property Users As DbSet(Of IdentityUser)

#Region "TableExcludeFromMigrations"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.
                Entity(Of IdentityUser)().
                ToTable("AspNetUsers", Function(t) t.ExcludeFromMigrations())
        End Sub
#End Region
    End Class


    Public Class IdentityUser
        Public Property Id As String
        Public Property UserName As String
    End Class
End Namespace
