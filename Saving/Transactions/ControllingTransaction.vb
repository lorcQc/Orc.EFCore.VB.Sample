Imports Microsoft.EntityFrameworkCore

Namespace Transactions
    Public NotInheritable Class ControllingTransaction
        Public Shared Sub Run()
            Using setupContext As BloggingContext = New BloggingContext
                setupContext.Database.EnsureDeleted()
                setupContext.Database.EnsureCreated()
            End Using

#Region "Transaction"
            Using context As BloggingContext = New BloggingContext
                Using transaction = context.Database.BeginTransaction()

                    Try
                        context.Blogs.Add(New Blog With {
                            .Url = "http://blogs.msdn.com/dotnet"})
                        context.SaveChanges()

                        context.Blogs.Add(New Blog With {
                            .Url = "http://blogs.msdn.com/visualstudio"})
                        context.SaveChanges()

                        Dim blogs1 = context.Blogs.
                            OrderBy(Function(b) b.Url).
                            ToList()

                        ' Commit transaction if all commands succeed, transaction will auto-rollback
                        ' when disposed if either commands fails
                        transaction.Commit()
                    Catch ex As Exception
                        ' TODO: Handle failure
                    End Try
                End Using
            End Using
#End Region
        End Sub

        Public Class BloggingContext
            Inherits DbContext
            Public Property Blogs As DbSet(Of Blog)
            Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
                optionsBuilder.UseSqlServer(
                    "Server=(localdb)\mssqllocaldb;Database=sample_EFSaving.Transactions;Trusted_Connection=True;ConnectRetryCount=0")
            End Sub
        End Class

        Public Class Blog
            Public Property BlogId As Integer
            Public Property Url As String
        End Class
    End Class
End Namespace
