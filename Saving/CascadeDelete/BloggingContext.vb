Imports Microsoft.EntityFrameworkCore
Imports Microsoft.EntityFrameworkCore.Diagnostics
Imports Microsoft.EntityFrameworkCore.Infrastructure
Imports Microsoft.Extensions.Logging


Namespace CascadeDelete
    Public NotInheritable Class BloggingContext
        Inherits DbContext

        Public Sub New(deleteBehavior1 As DeleteBehavior, requiredRelationship As Boolean)
            Me.DeleteBehavior = deleteBehavior1
            Me.RequiredRelationship = requiredRelationship

            If LogMessages Is Nothing Then
                LogMessages = New List(Of String)
                Me.GetService(Of ILoggerFactory)().AddProvider(New MyLoggerProvider)
            End If
        End Sub

        Public ReadOnly Property DeleteBehavior As DeleteBehavior
        Public ReadOnly Property RequiredRelationship As Boolean

        Public Property Blogs As DbSet(Of Blog)
        Public Property Posts As DbSet(Of Post)
        Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
            Call optionsBuilder.
                    ReplaceService(Of IModelCacheKeyFactory, DeleteBehaviorCacheKeyFactory)().EnableSensitiveDataLogging().UseSqlServer(
                                "Server=(localdb)\mssqllocaldb;Database=sample_EFSaving.CascadeDelete;Trusted_Connection=True;ConnectRetryCount=0",
                                Function(b) b.MaxBatchSize(1))
        End Sub

        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            Call modelBuilder.
                    Entity(Of Blog)().HasMany(Function(e) e.Posts).
                    WithOne(Function(e) e.Blog).
                    OnDelete(DeleteBehavior).
                    IsRequired(RequiredRelationship)
        End Sub

        Public Overrides Function SaveChanges() As Integer
            LogMessages.Clear()

            Return MyBase.SaveChanges()
        End Function

        Public Class DeleteBehaviorCacheKeyFactory
            Implements IModelCacheKeyFactory

            Public Overridable Function Create(context As DbContext) As Object Implements IModelCacheKeyFactory.Create
                Dim BloggingContext As BloggingContext = CType(context, BloggingContext)

                Return (BloggingContext.DeleteBehavior, BloggingContext.RequiredRelationship)
            End Function
        End Class

        Public Shared LogMessages As IList(Of String)
        Private Class MyLoggerProvider
            Implements ILoggerProvider

            Public Function CreateLogger(categoryName As String) As ILogger Implements ILoggerProvider.CreateLogger
                Return New SampleLogger
            End Function

            Public Sub Dispose() Implements IDisposable.Dispose
            End Sub

            Private Class SampleLogger
                Implements ILogger

                Public Function IsEnabled(logLevel As LogLevel) As Boolean Implements ILogger.IsEnabled
                    Return True
                End Function

                Public Sub Log(Of TState)(
                    logLevel1 As LogLevel, eventId1 As EventId, state As TState, exception As Exception,
                    formatter As Func(Of TState, Exception, String)) Implements ILogger.Log
                    If eventId1.Id = RelationalEventId.CommandExecuting.Id Then
                        Dim message = formatter(state, exception)
                        Dim commandIndex = Math.Max(message.IndexOf("UPDATE"), message.IndexOf("DELETE"))
                        If commandIndex >= 0 Then
                            Dim truncatedMessage = message.Substring(commandIndex, message.IndexOf(";", commandIndex) - commandIndex).Replace(Environment.NewLine, " ")

                            For i = 0 To 4 - 1
                                Dim paramIndex = message.IndexOf($"@p{i}='")
                                If paramIndex >= 0 Then
                                    Dim paramValue = message.Substring(paramIndex + 5, 1)
                                    If paramValue = "'" Then
                                        paramValue = "NULL"
                                    End If

                                    truncatedMessage = truncatedMessage.Replace($"@p{i}", paramValue)
                                End If
                            Next

                            LogMessages.Add(truncatedMessage)
                        End If
                    End If
                End Sub

                Public Function BeginScope(Of TState)(state As TState) As IDisposable Implements ILogger.BeginScope
                    Return Nothing
                End Function
            End Class
        End Class
    End Class
End Namespace
