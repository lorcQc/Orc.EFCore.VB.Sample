﻿Imports Microsoft.EntityFrameworkCore

Namespace Basics
    Public Class BloggingContext
        Inherits DbContext

        Public Property Blogs As DbSet(Of Blog)
        Public Property Posts As DbSet(Of Post)

        Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\mssqllocaldb;Database=sample_EFSaving.Basics;Trusted_Connection=True;ConnectRetryCount=0")
        End Sub
    End Class
End Namespace
