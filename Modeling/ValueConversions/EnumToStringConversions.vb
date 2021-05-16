Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Linq
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.EntityFrameworkCore.Diagnostics
Imports Microsoft.EntityFrameworkCore.Storage.ValueConversion

Public Class EnumToStringConversions

    Public Sub Run()
        ConsoleWriteLines("Sample showing explicitly configured value converter")

        Using context As New SampleDbContextExplicit
            CleanDatabase(context)

            context.Add(New Rider With {
                .Mount = EquineBeast.Horse
            })

            context.SaveChanges()

            context.SaveChanges()
        End Using

        Using context As New SampleDbContextExplicit
            ConsoleWriteLines($"Enum value read as '{context.Set(Of Rider)().Single().Mount}'.")
        End Using

        ConsoleWriteLines("Sample showing conversion configured by CLR type")
        Using context As New SampleDbContextByClrType
            CleanDatabase(context)

            context.Add(New Rider With {
                .Mount = EquineBeast.Horse
            })

            context.SaveChanges()
        End Using

        Using context As SampleDbContextByClrType = New SampleDbContextByClrType
            ConsoleWriteLines($"Enum value read as '{context.Set(Of Rider)().Single().Mount}'.")
        End Using

        ConsoleWriteLines("Sample showing conversion configured by database type")
        Using context As New SampleDbContextByDatabaseType
            CleanDatabase(context)

            context.Add(New Rider2 With {
                .Mount = EquineBeast.Horse
            })

            context.SaveChanges()

            context.SaveChanges()
        End Using
        Using context As New SampleDbContextByDatabaseType
            ConsoleWriteLines($"Enum value read as '{context.Set(Of Rider2)().Single().Mount}'.")
        End Using

        ConsoleWriteLines("Sample showing conversion configured by a ValueConverter instance")
        Using context As New SampleDbContextByConverterInstance
            CleanDatabase(context)

            context.Add(New Rider With {
                .Mount = EquineBeast.Horse
            })

            context.SaveChanges()
        End Using

        Using context As New SampleDbContextByConverterInstance
            ConsoleWriteLines($"Enum value read as '{context.Set(Of Rider)().Single().Mount}'.")
        End Using

        ConsoleWriteLines("Sample showing conversion configured by a built-in ValueConverter instance")
        Using context As New SampleDbContextByBuiltInInstance
            CleanDatabase(context)

            context.Add(New Rider With {
                .Mount = EquineBeast.Horse
            })
            context.SaveChanges()
        End Using

        Using context As New SampleDbContextByBuiltInInstance
            ConsoleWriteLines($"Enum value read as '{context.Set(Of Rider)().Single().Mount}'.")
        End Using

        ConsoleWriteLines("Sample showing conversion configured by CLR type with per-property facets")
        Using context As New SampleDbContextByClrTypeWithFacets
            CleanDatabase(context)

            context.Add(New Rider With {
                .Mount = EquineBeast.Horse
            })

            context.SaveChanges()
        End Using

        Using context As New SampleDbContextByClrTypeWithFacets
            ConsoleWriteLines($"Enum value read as '{context.Set(Of Rider)().Single().Mount}'.")
        End Using

        ConsoleWriteLines("Sample showing conversion configured by a ValueConverter instance with per-property facets")
        Using context As New SampleDbContextByConverterInstanceWithFacets
            CleanDatabase(context)

            context.Add(New Rider With {
                .Mount = EquineBeast.Horse
            })

            context.SaveChanges()
        End Using

        Using context As New SampleDbContextByConverterInstanceWithFacets
            ConsoleWriteLines($"Enum value read as '{context.Set(Of Rider)().Single().Mount}'.")
        End Using
    End Sub
    Public Class SampleDbContextExplicit
        Inherits SampleDbContextBase

#Region "ExplicitConversion"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder _
                .Entity(Of Rider)().Property(Function(e) e.Mount) _
                .HasConversion(
                    Function(v) v.ToString(),
                    Function(v) CType([Enum].Parse(GetType(EquineBeast), v), EquineBeast))
        End Sub
    End Class
#End Region

    Public Class SampleDbContextByClrType
        Inherits SampleDbContextBase
#Region "ConversionByClrType"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder _
                .Entity(Of Rider)().Property(Function(e) e.Mount) _
                .HasConversion(Of String)()
        End Sub
    End Class
#End Region

    Public Class SampleDbContextByDatabaseType
        Inherits SampleDbContextBase
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.Entity(Of Rider2)()
        End Sub
    End Class

    Public Class SampleDbContextByConverterInstance
        Inherits SampleDbContextBase
#Region "ConversionByConverterInstance"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            Dim converter As New ValueConverter(Of EquineBeast, String)(
                Function(v) v.ToString(),
                Function(v) CType([Enum].Parse(GetType(EquineBeast), v), EquineBeast))

            modelBuilder _
                .Entity(Of Rider)().Property(Function(e) e.Mount) _
                .HasConversion(converter)
        End Sub
    End Class
#End Region

    Public Class SampleDbContextByClrTypeWithFacets
        Inherits SampleDbContextBase
#Region "ConversionByClrTypeWithFacets"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder _
                .Entity(Of Rider)().Property(Function(e) e.Mount) _
                .HasConversion(Of String)().HasMaxLength(20) _
                .IsUnicode(False)
        End Sub
    End Class
#End Region

    Public Class SampleDbContextByConverterInstanceWithFacets
        Inherits SampleDbContextBase
#Region "ConversionByConverterInstanceWithFacets"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            Dim converter As New ValueConverter(Of EquineBeast, String)(
                Function(v) v.ToString(),
                Function(v) CType([Enum].Parse(GetType(EquineBeast), v), EquineBeast))

            modelBuilder _
                .Entity(Of Rider)().Property(Function(e) e.Mount) _
                .HasConversion(converter) _
                .HasMaxLength(20) _
                .IsUnicode(False)
        End Sub
    End Class
#End Region

    Public Class SampleDbContextByConverterInstanceWithMappingHints
        Inherits SampleDbContextBase
#Region "ConversionByConverterInstanceWithMappingHints"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            Dim converter As New ValueConverter(Of EquineBeast, String)(
                Function(v) v.ToString(),
                Function(v) CType([Enum].Parse(GetType(EquineBeast), v), EquineBeast),
                New ConverterMappingHints(size:=20, unicode:=False))

            modelBuilder _
                .Entity(Of Rider)().Property(Function(e) e.Mount) _
                .HasConversion(converter)
        End Sub
    End Class
#End Region

    Public Class SampleDbContextByBuiltInInstance
        Inherits SampleDbContextBase
#Region "ConversionByBuiltInInstance"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            Dim converter As New EnumToStringConverter(Of EquineBeast)

            modelBuilder _
                .Entity(Of Rider)().Property(Function(e) e.Mount) _
                .HasConversion(converter)
        End Sub
    End Class
#End Region

    Public Class SampleDbContextBoolToInt
        Inherits SampleDbContextBase
#Region "ConversionByBuiltInBoolToInt"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder _
                .Entity(Of User)().Property(Function(e) e.IsActive) _
                .HasConversion(Of Integer)()
        End Sub
    End Class
#End Region

    Public Class SampleDbContextBoolToIntExplicit
        Inherits SampleDbContextBase
#Region "ConversionByBuiltInBoolToIntExplicit"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            Dim converter As New BoolToZeroOneConverter(Of Integer)

            modelBuilder _
                .Entity(Of User)().Property(Function(e) e.IsActive) _
                .HasConversion(converter)
        End Sub
    End Class
#End Region

    Public Class SampleDbContextRider2
        Inherits SampleDbContextBase
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
#Region "ConversionByDatabaseTypeFluent"
            modelBuilder _
                .Entity(Of Rider2)().Property(Function(e) e.Mount) _
                .HasColumnType("nvarchar(24)")
#End Region
        End Sub
    End Class

    Public Class SampleDbContextBase
        Inherits DbContext
        Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
            Call optionsBuilder.LogTo(AddressOf Console.WriteLine, {RelationalEventId.CommandExecuted}).
                                UseSqlite("Data Source=sample.db").
                                EnableSensitiveDataLogging()
        End Sub
    End Class

#Region "BeastAndRider"
    Public Class Rider
        Public Property Id As Integer
        Public Property Mount As EquineBeast
    End Class

    Public Enum EquineBeast
        Donkey
        Mule
        Horse
        Unicorn
    End Enum
#End Region

#Region "ConversionByDatabaseType"
    Public Class Rider2
        Public Property Id As Integer

        <Column(TypeName:="nvarchar(24)")>
        Public Property Mount As EquineBeast
    End Class

#End Region
    Public Class User
        Public Property Id As Integer
        Public Property IsActive As Boolean
    End Class
End Class
