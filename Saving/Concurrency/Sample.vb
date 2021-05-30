Imports System.ComponentModel.DataAnnotations
Imports Microsoft.EntityFrameworkCore

Namespace Concurrency
    Public Module Sample
        Public Sub Run()
            Using setupContext As New PersonContext
                setupContext.Database.EnsureDeleted()
                setupContext.Database.EnsureCreated()

                setupContext.People.Add(New Person With {
                    .FirstName = "John",
                    .LastName = "Doe"})

                setupContext.SaveChanges()
            End Using

#Region "ConcurrencyHandlingCode"
            Using context As New PersonContext
                ' Fetch a person from database and change phone number
                Dim person1 = context.People.Single(Function(p) p.PersonId = 1)
                person1.PhoneNumber = "555-555-5555"

                ' Change the person's name in the database to simulate a concurrency conflict
                context.Database.ExecuteSqlRaw(
                    "UPDATE dbo.People SET FirstName = 'Jane' WHERE PersonId = 1")

                Dim saved As Boolean = False
                While Not saved
                    Try
                        ' Attempt to save changes to the database
                        context.SaveChanges()
                        saved = True
                    Catch ex As DbUpdateConcurrencyException
                        For Each entry In ex.Entries
                            If TypeOf entry.Entity Is Person Then
                                Dim proposedValues = entry.CurrentValues
                                Dim databaseValues = entry.GetDatabaseValues()

                                For Each [property] In proposedValues.Properties
                                    Dim proposedValue = proposedValues([property])
                                    Dim databaseValue = databaseValues([property])

                                    ' TODO: decide which value should be written to database
                                    ' proposedValues([property]) = <value to be saved>

                                Next

                                ' Refresh original values to bypass next concurrency check
                                entry.OriginalValues.SetValues(databaseValues)
                            Else
                                Throw New NotSupportedException(
                                    "Don't know how to handle concurrency conflicts for " & entry.Metadata.Name)
                            End If
                        Next
                    End Try
                End While
            End Using
#End Region
        End Sub

        Public Class PersonContext
            Inherits DbContext
            Public Property People As DbSet(Of Person)

            Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
                ' Requires NuGet package Microsoft.EntityFrameworkCore.SqlServer
                optionsBuilder.UseSqlServer(
                    "Server=(localdb)\mssqllocaldb;Database=sample_EFSaving.Concurrency;Trusted_Connection=True;ConnectRetryCount=0")
            End Sub
        End Class

        Public Class Person
            Public Property PersonId As Integer

            <ConcurrencyCheck>
            Public Property FirstName As String

            <ConcurrencyCheck>
            Public Property LastName As String

            Public Property PhoneNumber As String
        End Class
    End Module
End Namespace
