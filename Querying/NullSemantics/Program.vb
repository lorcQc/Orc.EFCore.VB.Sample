Imports System
Imports Microsoft.EntityFrameworkCore

Module Program

    Sub Main(args As String())
        Using context As New NullSemanticsContext
            context.Database.EnsureDeleted()
            context.Database.EnsureCreated()

            '#Region "FunctionSqlRaw"
            '            context.Database.ExecuteSqlRaw(
            '                "create function [dbo].[ConcatStrings] (@prm1 nvarchar(max), @prm2 nvarchar(max))
            '                    returns nvarchar(max)
            '                    as
            '                    begin
            '                        return @prm1 + @prm2;
            '                    end")
            '#End Region

            'BasicExamples()
            Functions()
        End Using
        'ManualOptimization()
    End Sub

    Private Sub BasicExamples()
        Using context As New NullSemanticsContext
#Region "BasicExamples"
            Dim query1 = context.Entities.Where(Function(e) e.Id = e.Int)
            Dim query2 = context.Entities.Where(Function(e) e.Id = e.NullableInt)
            Dim query3 = context.Entities.Where(Function(e) e.Id <> e.NullableInt)
            Dim query4 = context.Entities.Where(Function(e) e.String1 = e.String2)
            Dim query5 = context.Entities.Where(Function(e) e.String1 <> e.String2)
#End Region

            Dim result1 = query1.ToList()
            Dim result2 = query2.ToList()
            Dim result3 = query3.ToList()
            Dim result4 = query4.ToList()
            Dim result5 = query5.ToList()
        End Using
    End Sub

    Private Sub Functions()
        Using context As New NullSemanticsContext

#Region "Functions"
            Dim query = context.Entities.Where(Function(e) e.String1.Substring(0, e.String2.Length) Is Nothing)
#End Region

            Dim result = query.ToList()
        End Using
    End Sub

    Private Sub ManualOptimization()
        Using context As New NullSemanticsContext

#Region "ManualOptimization"
            Dim query1 = context.Entities.Where(Function(e) e.String1 <> e.String2 OrElse e.String1.Length = e.String2.Length)
            Dim query2 = context.Entities.Where(
        Function(e) e.String1 IsNot Nothing AndAlso e.String2 IsNot Nothing AndAlso (e.String1 <> e.String2 OrElse e.String1.Length = e.String2.Length))
#End Region

            Dim result1 = query1.ToList()
            Dim result2 = query2.ToList()
        End Using
    End Sub

End Module
