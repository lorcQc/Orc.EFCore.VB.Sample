Imports System

Module Program
    Sub Main()
        Using context As New DynamicContext With {.UseIntProperty = True}

            context.Entities.Add(New ConfigurableEntity With {.IntProperty = 44,
                .StringProperty = "Aloha"})

            context.SaveChanges()
        End Using

        Using context As New DynamicContext With {.UseIntProperty = False}

            context.Entities.Add(New ConfigurableEntity With {.IntProperty = 43,
                .StringProperty = "Hola"})

            context.SaveChanges()
        End Using

        Using context As New DynamicContext With {.UseIntProperty = True}

            Dim entity = context.Entities.Single()

            ' Writes 44 and an empty string
            Console.WriteLine($"{entity.IntProperty} {entity.StringProperty}")
        End Using

        Using context As New DynamicContext With {.UseIntProperty = False}

            Dim entity = context.Entities.Single()

            ' Writes 0 and an "Hola"
            Console.WriteLine($"{entity.IntProperty} {entity.StringProperty}")
        End Using

    End Sub
End Module
