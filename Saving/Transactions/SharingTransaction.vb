Imports Microsoft.Data.SqlClient
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.EntityFrameworkCore.Storage

Namespace Transactions
    Public NotInheritable Class SharingTransaction
        Public Shared Sub Run()
            Dim ConnectionString As String = "Server=(localdb)\mssqllocaldb;Database=sample_EFSaving.Transactions;Trusted_Connection=True;ConnectRetryCount=0"

            Dim Options = (New DbContextOptionsBuilder(Of BloggingContext)).
                            UseSqlServer(ConnectionString).
                            Options

            Using Context As New BloggingContext(Options)
                Context.Database.EnsureDeleted()
                Context.Database.EnsureCreated()
            End Using

#Region "Transaction"
            Using Connection As New SqlConnection(ConnectionString)
                Dim Options2 = (New DbContextOptionsBuilder(Of BloggingContext)).
                                    UseSqlServer(Connection).
                                    Options

                Using Context As New BloggingContext(Options2)
                    Using transaction = Context.Database.BeginTransaction()
                        Try
                            Context.Blogs.Add(New Blog With {
                                .Url = "http://blogs.msdn.com/dotnet"})

                            Context.SaveChanges()

                            Using Context2 As New BloggingContext(Options2)
                                Context2.Database.UseTransaction(transaction.GetDbTransaction())

                                Dim blogs1 = Context2.Blogs.
                                                OrderBy(Function(b) b.Url).
                                                ToList()
                            End Using

                            ' Commit transaction if all commands succeed, transaction will auto-rollback
                            ' when disposed if either commands fails
                            transaction.Commit()
                        Catch ex As Exception
                            ' TODO: Handle failure
                        End Try
                    End Using
                End Using
            End Using
#End Region
        End Sub

#Region "Context"
        Public Class BloggingContext
            Inherits DbContext
            Public Sub New(options As DbContextOptions(Of BloggingContext))
                MyBase.New(options)
            End Sub

            Public Property Blogs As DbSet(Of Blog)
        End Class
#End Region

        Public Class Blog
            Public Property BlogId As Integer
            Public Property Url As String
        End Class
    End Class
End Namespace
