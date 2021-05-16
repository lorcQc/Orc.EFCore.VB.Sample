Imports System
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.Extensions.Logging

Public Class EmployeeContext
    Inherits DbContext
    Public Property Employees As DbSet(Of Employee)
    Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
        optionsBuilder.
            UseSqlServer("Server=(localdb)\mssqllocaldb;Database=sample_Blogging;Integrated Security=True").
            LogTo(AddressOf Console.WriteLine, LogLevel.Information)
    End Sub
End Class

Public Class Employee
    Public Property Id As Integer
    Public Property Name As String
    Public Property Salary As Integer
End Class
