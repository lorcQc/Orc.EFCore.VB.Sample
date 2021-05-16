Imports Microsoft.EntityFrameworkCore

Namespace SharedType
#Region "SharedType"
    Friend Class MyContext
        Inherits DbContext

        Public ReadOnly Property Blogs As DbSet(Of Dictionary(Of String, Object))
            Get
                Return Me.Set(Of Dictionary(Of String, Object))("Blog")
            End Get
        End Property

        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.SharedTypeEntity(Of Dictionary(Of String, Object))(
                "Blog", Sub(bb)
                            bb.Property(Of Integer)("BlogId")
                            bb.Property(Of String)("Url")
                            bb.Property(Of DateTime)("LastUpdated")
                        End Sub)
        End Sub
    End Class
#End Region

End Namespace
