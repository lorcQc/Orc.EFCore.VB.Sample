Imports Microsoft.Data.SqlClient
Imports Microsoft.EntityFrameworkCore

Namespace Transactions
    Public NotInheritable Class ExternalDbTransaction
        Public Shared Sub Run()
            Dim connectionString As String = "Server=(localdb)\mssqllocaldb;Database=sample_EFSaving.Transactions;Trusted_Connection=True;ConnectRetryCount=0"

            Dim Options = (New DbContextOptionsBuilder(Of BloggingContext)).
                            UseSqlServer(connectionString).
                            Options

            Using context As New BloggingContext(Options)
                context.Database.EnsureDeleted()
                context.Database.EnsureCreated()
            End Using

#Region "Transaction"
            Using connection As New SqlConnection(connectionString)
                connection.Open()

                Using transaction = connection.BeginTransaction()
                    Try
                        ' Run raw ADO.NET command in the transaction
                        Dim Command = connection.CreateCommand()
                        Command.Transaction = transaction
                        command.CommandText = "DELETE FROM dbo.Blogs"
                        command.ExecuteNonQuery()

                        ' Run an EF Core command in the transaction
                        Dim Options2 = (New DbContextOptionsBuilder(Of BloggingContext)).
                                        UseSqlServer(connection).
                                        Options

                        Using context As New BloggingContext(options2)
                            context.Database.UseTransaction(transaction)
                            context.Blogs.Add(New Blog With {
                                .Url = "http://blogs.msdn.com/dotnet"})
                            context.SaveChanges()
                        End Using

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
