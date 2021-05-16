Imports Microsoft.EntityFrameworkCore

Namespace IndexerProperty
    Friend Class MyContext
        Inherits DbContext

        Public Property Blogs As DbSet(Of Blog)

#Region "ShadowProperty"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.
                Entity(Of Blog)().
                IndexerProperty(Of Date)("LastUpdated")
        End Sub
    End Class
#End Region

    Public Class Blog

        Private ReadOnly _data As New Dictionary(Of String, Object)
        Public Property BlogId As Integer

        Default Public Property item(key As String) As Object
            Get
                Return _data(key)
            End Get
            Set
                _data(key) = Value
            End Set
        End Property
    End Class
End Namespace
