Public Class Blog

    Private _tenantId As String

    Public Property BlogId As Integer
    Public Property Name As String
    Public Property Url As String

    Public Property Posts As List(Of Post)
End Class

Public Class Post
    Public Property PostId As Integer
    Public Property Title As String
    Public Property Content As String
    Public Property IsDeleted As Boolean

    Public Property Blog As Blog
End Class

Public Class Person
    Public Property Id As Integer
    Public Property Name As String
    Public Property Pets As List(Of Animal)
End Class

Public MustInherit Class Animal
    Public Property Id As Integer
    Public Property Name As String
    Public Property Owner As Person
End Class

Public Class Cat
    Inherits Animal
    Public Property PrefersCardboardBoxes As Boolean

    Public Property ToleratesId As Integer?

    Public Property Tolerates As Dog
End Class

Public Class Dog
    Inherits Animal
    Public Property FavoriteToy As Toy
    Public Property FriendsWith As Cat
End Class

Public Class Toy
    Public Property Id As Integer
    Public Property Name As String
    Public Property BelongsToId As Integer?
    Public Property BelongsTo As Dog
End Class
