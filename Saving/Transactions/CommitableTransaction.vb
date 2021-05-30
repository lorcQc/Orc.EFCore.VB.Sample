Imports System.Transactions
Imports Microsoft.Data.SqlClient
Imports Microsoft.EntityFrameworkCore

Namespace Transactions
    Public NotInheritable Class CommitableTransaction
        Public Shared Sub Run()
            Dim ConnectionString As String = "Server=(localdb)\mssqllocaldb;Database=sample_EFSaving.Transactions;Trusted_Connection=True;ConnectRetryCount=0"

            Dim Options = (New DbContextOptionsBuilder(Of BloggingContext)).
                            UseSqlServer(connectionString).
                            Options

            Using Context As New BloggingContext(Options)
                Context.Database.EnsureDeleted()
                Context.Database.EnsureCreated()
            End Using

#Region "Transaction"
            Using Transaction As New CommittableTransaction(
                New TransactionOptions With {
                    .IsolationLevel = IsolationLevel.ReadCommitted})

                Dim connection As New SqlConnection(connectionString)

                Try
                    Dim Options2 = (New DbContextOptionsBuilder(Of BloggingContext)).
                                        UseSqlServer(connection).
                                        Options

                    Using Context As New BloggingContext(Options2)
                        Context.Database.OpenConnection()
                        Context.Database.EnlistTransaction(Transaction)

                        ' Run raw ADO.NET command in the transaction
                        Dim Command = connection.CreateCommand()
                        Command.CommandText = "DELETE FROM dbo.Blogs"
                        Command.ExecuteNonQuery()

                        ' Run an EF Core command in the transaction
                        Context.Blogs.Add(New Blog With {
                            .Url = "http://blogs.msdn.com/dotnet"})

                        Context.SaveChanges()
                        Context.Database.CloseConnection()
                    End Using

                    ' Commit transaction if all commands succeed, transaction will auto-rollback
                    ' when disposed if either commands fails
                    Transaction.Commit()
                Catch ex As Exception
                    'TODO: Handle failure
                End Try
            End Using
#End Region

        End Sub
        Public Class BloggingContext
            Inherits DbContext
            Public Sub New(options As DbContextOptions(Of BloggingContext))
                MyBase.New(options)
            End Sub

            Public Property Blogs As DbSet(Of Blog)
        End Class

        Public Class Blog
            Public Property BlogId As Integer
            Public Property Url As String
        End Class
    End Class
End Namespace
