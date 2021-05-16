Imports Microsoft.EntityFrameworkCore
Imports Microsoft.Extensions.Logging

Public Class ExtensionsLoggingContext
    Inherits DbContext

#Region "ExtensionsLogging"
    Private Shared ReadOnly Property ContextLoggerFactory As ILoggerFactory
        Get
            Return LoggerFactory.Create(Function(b) b.AddConsole().AddFilter("", LogLevel.Information))
        End Get
    End Property

    Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
        optionsBuilder.
            UseSqlServer("Server=(localdb)\mssqllocaldb;Database=sample_Blogging;Integrated Security=True").
            UseLoggerFactory(ContextLoggerFactory)
    End Sub
#End Region

End Class
