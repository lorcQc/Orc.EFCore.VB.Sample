Imports Microsoft.EntityFrameworkCore
Imports Microsoft.EntityFrameworkCore.Infrastructure

#Region "DynamicModel"
Public Class DynamicModelCacheKeyFactory
    Implements IModelCacheKeyFactory

    Public Function Create(context As DbContext) As Object Implements IModelCacheKeyFactory.Create

        If TypeOf context Is DynamicContext Then
            Dim dynamicContext = DirectCast(context, DynamicContext)
            Return (context.GetType(), dynamicContext.UseIntProperty)
        End If

        Return CObj(context.GetType())
    End Function
End Class
#End Region
