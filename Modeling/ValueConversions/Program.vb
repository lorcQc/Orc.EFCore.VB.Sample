Imports Microsoft.EntityFrameworkCore

'https://docs.microsoft.com/en-us/ef/core/modeling/value-conversions?tabs=data-annotations

''' <summary>
''' Samples for value conversions and comparisons.
''' </summary>
Module Program
    Sub Main()
        Call New MappingImmutableClassProperty().Run()
        Call New MappingImmutableStructProperty().Run()
        Call New MappingListProperty().Run()
        Call New MappingListPropertyOld().Run()
        Call New OverridingByteArrayComparisons().Run()
        Call New EnumToStringConversions().Run()
        Call New KeyValueObjects().Run()
        Call New SimpleValueObject().Run()
        Call New CompositeValueObject().Run()
        Call New PrimitiveCollection().Run()
        Call New ValueObjectCollection().Run()
        Call New ULongConcurrency().Run()
        Call New PreserveDateTimeKind().Run()
        Call New CaseInsensitiveStrings().Run()
        Call New FixedLengthStrings().Run()
        Call New EncryptPropertyValues().Run()
        Call New WithMappingHints().Run()

    End Sub

    Friend Sub ConsoleWriteLines(ParamArray values() As String)
        Console.WriteLine()
        For Each value In values
            Console.WriteLine(value)
        Next
        Console.WriteLine()
    End Sub

    Friend Sub CleanDatabase(context As DbContext)
        ConsoleWriteLines("Deleting and re-creating database...")
        context.Database.EnsureDeleted()
        context.Database.EnsureCreated()
        ConsoleWriteLines("Done. Database is clean and fresh.")
    End Sub
End Module
