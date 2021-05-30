Imports Microsoft.EntityFrameworkCore

Namespace Transactions
    Public NotInheritable Class ManagingSavepoints
        Public Shared Sub Run()
            Using setupContext As New BloggingContext
                setupContext.Database.EnsureDeleted()
                setupContext.Database.EnsureCreated()
            End Using

#Region "Savepoints"
            Using context As New BloggingContext
                Using transaction = context.Database.BeginTransaction()

                    Try
                        context.Blogs.Add(New Blog With {
                            .Url = "https://devblogs.microsoft.com/dotnet/"})
                        context.SaveChanges()

                        transaction.CreateSavepoint("BeforeMoreBlogs")

                        context.Blogs.Add(New Blog With {
                            .Url = "https://devblogs.microsoft.com/visualstudio/"})

                        context.Blogs.Add(New Blog With {
                            .Url = "https://devblogs.microsoft.com/aspnet/"})

                        context.SaveChanges()

                        transaction.Commit()
                    Catch ex As Exception
                        ' If a failure occurred, we rollback to the savepoint and can continue the transaction
                        transaction.RollbackToSavepoint("BeforeMoreBlogs")
                        ' TODO: Handle failure, possibly retry inserting blogs
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
