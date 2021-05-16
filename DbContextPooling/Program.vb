Imports System.Threading
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.Extensions.DependencyInjection

Module Program

    Const Threads As Integer = 32
    Const Seconds As Integer = 10
    Private _requestsProcessed As Long

    Sub Main()
        MainAsync().GetAwaiter().GetResult()
    End Sub

    Async Function MainAsync() As Task
        Dim serviceCollection1 As New ServiceCollection

        With New Startup
            .ConfigureServices(serviceCollection1)
        End With

        Dim serviceProvider = serviceCollection1.BuildServiceProvider()

        SetupDatabase(serviceProvider)

        Dim sw As Stopwatch = New Stopwatch

        Dim monitorTask As Task = MonitorResults(TimeSpan.FromSeconds(Seconds), sw)

        Await Task.WhenAll(
            Enumerable.Range(0, Threads).Select(Function(x) SimulateRequestsAsync(serviceProvider, sw)))

        Await monitorTask
    End Function

    Public Class Blog
        Public Property BlogId As Integer
        Public Property Name As String
        Public Property Url As String
    End Class

    Public Class BloggingContext
        Inherits DbContext

        Public Shared InstanceCount As Long

        Public Sub New(options As DbContextOptions)
            MyBase.New(options)
            Interlocked.Increment(InstanceCount)
        End Sub

        Public Property Blogs As DbSet(Of Blog)
    End Class

    Public Class BlogController
        Private ReadOnly _context As BloggingContext

        Public Sub New(context As BloggingContext)
            _context = context
        End Sub

        Public Async Function ActionAsync() As Task(Of Blog)
            Return Await _context.Blogs.FirstAsync()
        End Function
    End Class

    Public Class Startup
        Private Const ConnectionString As String = "Server=(localdb)\mssqllocaldb;Database=sampleContextPooling;Integrated Security=True;ConnectRetryCount=0"
        Public Sub ConfigureServices(services As IServiceCollection)
            'Switch the lines below to compare pooling with the traditional instance-per-request approach.
            'services.AddDbContext(Of BloggingContext)(Function(c) c.UseSqlServer(ConnectionString))
            services.AddDbContextPool(Of BloggingContext)(Function(c) c.UseSqlServer(ConnectionString))
        End Sub
    End Class

    Private Sub SetupDatabase(serviceProvider As IServiceProvider)
        Using serviceScope = serviceProvider.CreateScope()
            Dim context = serviceScope.ServiceProvider.GetService(Of BloggingContext)()

            If context.Database.EnsureCreated() Then
                context.Blogs.Add(New Blog With {
                    .Name = "The Dog Blog",
                    .Url = "http://sample.com/dogs"})
                context.Blogs.Add(New Blog With {
                    .Name = "The Cat Blog",
                    .Url = "http://sample.com/cats"})
                context.SaveChanges()
            End If
        End Using
    End Sub

    Private Async Function SimulateRequestsAsync(serviceProvider As IServiceProvider, stopwatch1 As Stopwatch) As Task
        While stopwatch1.IsRunning
            Using serviceScope = serviceProvider.CreateScope()
                Await New BlogController(serviceScope.ServiceProvider.GetService(Of BloggingContext)()).ActionAsync()
            End Using

            Interlocked.Increment(_requestsProcessed)
        End While
    End Function

    Private Async Function MonitorResults(duration As TimeSpan, sw As Stopwatch) As Task
        Dim lastInstanceCount As Long = 0L
        Dim lastRequestCount As Long = 0L
        Dim lastElapsed As TimeSpan = TimeSpan.Zero

        sw.Start()

        While sw.Elapsed < duration
            Await Task.Delay(TimeSpan.FromSeconds(1))

            Dim instanceCount1 As Long = BloggingContext.InstanceCount
            Dim requestCount As Long = _requestsProcessed
            Dim elapsed1 As TimeSpan = sw.Elapsed
            Dim currentElapsed As TimeSpan = elapsed1 - lastElapsed
            Dim currentRequests As Long = requestCount - lastRequestCount

            Console.WriteLine(
                    $"[{DateTime.Now:HH:mm:ss.fff}] " &
                    $"Context creations/second: {instanceCount1 - lastInstanceCount} | " &
                    $"Requests/second: {Math.Round(currentRequests / currentElapsed.TotalSeconds)}")

            lastInstanceCount = instanceCount1
            lastRequestCount = requestCount
            lastElapsed = elapsed1
        End While

        Console.WriteLine()
        Console.WriteLine($"Total context creations: {BloggingContext.InstanceCount}")
        Console.WriteLine($"Requests per second: {Math.Round(_requestsProcessed / sw.Elapsed.TotalSeconds)}")

        sw.Stop()
    End Function
End Module
