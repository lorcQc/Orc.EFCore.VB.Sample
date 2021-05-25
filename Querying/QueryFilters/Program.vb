Imports System
Imports Microsoft.EntityFrameworkCore

Module Program

    Sub Main()
        QueryFiltersBasicExample()
        QueryFiltersWithNavigationsExample()
        QueryFiltersWithRequiredNavigationExample()
        QueryFiltersUsingNavigationExample()
    End Sub

    Private Sub QueryFiltersBasicExample()
        Using db As New BloggingContext("diego")

            If db.Database.EnsureCreated() Then
                db.Blogs.Add(
                    New Blog With {
                        .Url = "http://sample.com/blogs/fish",
                        .Posts = New List(Of Post) From {
                            New Post With {
                                .Title = "Fish care 101"},
                            New Post With {
                                .Title = "Caring for tropical fish"},
                            New Post With {
                                .Title = "Types of ornamental fish"}
                        }
                    })

                db.Blogs.Add(
                    New Blog With {
                        .Url = "http://sample.com/blogs/cats",
                        .Posts = New List(Of Post) From {
                            New Post With {
                                .Title = "Cat care 101"},
                            New Post With {
                                .Title = "Caring for tropical cats"},
                            New Post With {
                                .Title = "Types of ornamental cats"}
                        }
                    })

                db.SaveChanges()

                Using andrewDb As New BloggingContext("andrew")
                    andrewDb.Blogs.Add(
                        New Blog With {
                            .Url = "http://sample.com/blogs/catfish",
                            .Posts = New List(Of Post) From {
                                New Post With {
                                    .Title = "Catfish care 101"},
                                New Post With {
                                    .Title = "History of the catfish name"}
                            }
                        })

                    andrewDb.SaveChanges()
                End Using

                db.Posts.Where(Function(p) p.Title = "Caring for tropical fish" OrElse
                                           p.Title = "Cat care 101").
                         ToList().
                         ForEach(Function(p) db.Posts.Remove(p))

                db.SaveChanges()
            End If
        End Using

        Using db As New BloggingContext("Diego")
            Dim blogs1 = db.Blogs.Include(Function(b) b.Posts).ToList()

            For Each blog1 In blogs1
                Console.WriteLine($"{blog1.Url,-33} [Tenant: {db.Entry(blog1).[Property]("_tenantId").CurrentValue}]")

                For Each post1 In blog1.Posts
                    Console.WriteLine($" - {post1.Title,-30} [IsDeleted: {post1.IsDeleted}]")
                Next

                Console.WriteLine()
            Next

#Region "IgnoreFilters"
            blogs1 = db.Blogs.Include(Function(b) b.Posts).IgnoreQueryFilters().ToList()
#End Region

            For Each blog1 In blogs1
                Console.WriteLine($"{blog1.Url,-33} [Tenant: {db.Entry(blog1).[Property]("_tenantId").CurrentValue}]")

                For Each post1 In blog1.Posts
                    Console.WriteLine($" - {post1.Title,-30} [IsDeleted: {post1.IsDeleted}]")
                Next
            Next
        End Using
    End Sub

    Private Sub QueryFiltersWithNavigationsExample()

        Dim DisplayResults As Action(Of List(Of Person)) =
            Sub(people As List(Of Person))
                For Each person1 In people
                    Console.WriteLine($"{person1.Name}")
                    If person1.Pets IsNot Nothing Then
                        For Each pet In person1.Pets
                            Console.Write($" - {pet.Name} [{pet.[GetType]().Name}] ")
                            Dim TempVar As Boolean = TypeOf pet Is Dog
                            Dim dog As Dog = pet
                            Dim TempVar1 As Boolean = TypeOf pet Is Cat
                            Dim cat As Cat = pet
                            If TempVar1 Then
                                Console.Write($"| Prefers cardboard boxes: {(If(cat.PrefersCardboardBoxes, "Yes", "No"))} ")
                                Console.WriteLine($"| Tolerates: {(If(cat.Tolerates IsNot Nothing, cat.Tolerates.Name, "No one"))}")
                            ElseIf TempVar Then
                                Console.Write($"| Favorite toy: {(If(dog.FavoriteToy IsNot Nothing, dog.FavoriteToy.Name, "None"))} ")
                                Console.WriteLine($"| Friend: {(If(dog.FriendsWith IsNot Nothing, dog.FriendsWith.Name, "The Owner"))}")
                            End If
                        Next
                    End If
                Next
            End Sub

        Using animalContext As New AnimalContext
            animalContext.Database.EnsureDeleted()
            animalContext.Database.EnsureCreated()

            Dim janice As New Person With {
                .Name = "Janice"}
            Dim jamie As New Person With {
                .Name = "Jamie"}
            Dim cesar As New Person With {
                .Name = "Cesar"}
            Dim paul As New Person With {
                .Name = "Paul"}
            Dim dominic As New Person With {
                .Name = "Dominic"}

            Dim kibbles As New Cat With {
                .Name = "Kibbles",
                .PrefersCardboardBoxes = False,
                .Owner = janice}
            Dim sammy As New Cat With {
                .Name = "Sammy",
                .PrefersCardboardBoxes = True,
                .Owner = janice}
            Dim puffy As New Cat With {
                .Name = "Puffy",
                .PrefersCardboardBoxes = True,
                .Owner = jamie}
            Dim hati As New Dog With {
                .Name = "Hati",
                .FavoriteToy = New Toy With {
                    .Name = "Squeeky duck"},
                .Owner = dominic,
                .FriendsWith = puffy}
            Dim simba As New Dog With {
                .Name = "Simba",
                .FavoriteToy = New Toy With {
                    .Name = "Bone"},
                .Owner = cesar,
                .FriendsWith = sammy}

            puffy.Tolerates = hati
            sammy.Tolerates = simba

            animalContext.People.AddRange(janice, jamie, cesar, paul, dominic)
            animalContext.Animals.AddRange(kibbles, sammy, puffy, hati, simba)
            animalContext.SaveChanges()
        End Using

        Using animalContext As New AnimalContext
            Console.WriteLine("*****************")
            Console.WriteLine("* Animal lovers *")
            Console.WriteLine("*****************")

            ' Jamie and Paul are filtered out.
            ' Paul doesn't own any pets. Jamie owns Puffy, but her pet has been filtered out.
            Dim animalLovers = animalContext.People.ToList()
            DisplayResults(animalLovers)

            Console.WriteLine("**************************************************")
            Console.WriteLine("* Animal lovers and their pets - filters enabled *")
            Console.WriteLine("**************************************************")

            ' Jamie and Paul are filtered out.
            ' Paul doesn't own any pets. Jamie owns Puffy, but her pet has been filtered out.
            ' Simba's favorite toy has also been filtered out.
            ' Puffy is filtered out so he doesn't show up as Hati's friend.
            Dim ownersAndTheirPets = animalContext.People.
                                                   Include(Function(p) p.Pets).
                                                   ThenInclude(Function(p) CType(p, Dog).FavoriteToy).
                                                   ToList()

            DisplayResults(ownersAndTheirPets)

            Console.WriteLine("*********************************************************")
            Console.WriteLine("* Animal lovers and their pets - query filters disabled *")
            Console.WriteLine("*********************************************************")

            Dim ownersAndTheirPetsUnfiltered = animalContext.People.
                                                             IgnoreQueryFilters().Include(Function(p) p.Pets).
                                                             ThenInclude(Function(p) CType(p, Dog).FavoriteToy).
                                                             ToList()

            DisplayResults(ownersAndTheirPetsUnfiltered)
        End Using

    End Sub

    Private Sub QueryFiltersWithRequiredNavigationExample()
        Using db As New FilteredBloggingContextRequired
            db.Database.EnsureDeleted()
            db.Database.EnsureCreated()

#Region "SeedData"
            db.Blogs.Add(
                New Blog With {
                    .Url = "http://sample.com/blogs/fish",
                    .Posts = New List(Of Post) From {
                        New Post With {
                            .Title = "Fish care 101"},
                        New Post With {
                            .Title = "Caring for tropical fish"},
                        New Post With {
                            .Title = "Types of ornamental fish"}
                    }
                })

            db.Blogs.Add(
                New Blog With {
                    .Url = "http://sample.com/blogs/cats",
                    .Posts = New List(Of Post) From {
                        New Post With {
                            .Title = "Cat care 101"},
                        New Post With {
                            .Title = "Caring for tropical cats"},
                        New Post With {
                            .Title = "Types of ornamental cats"}
                    }
                })
#End Region

            db.SaveChanges()
        End Using

        Console.WriteLine("Use of required navigations to access entity with query filter demo")

        Using db As New FilteredBloggingContextRequired

#Region "Queries"
            Dim allPosts = db.Posts.ToList()
            Dim allPostsWithBlogsIncluded = db.Posts.Include(Function(p) p.Blog).ToList()
#End Region

            If allPosts.Count = allPostsWithBlogsIncluded.Count Then
                Console.WriteLine($"Query filters set up correctly. Result count for both queries: {allPosts.Count}.")
            Else
                Console.WriteLine("Unexpected discrepancy due to query filters and required navigations interaction.")
                Console.WriteLine($"All posts count: {allPosts.Count}.")
                Console.WriteLine($"All posts with blogs included count: {allPostsWithBlogsIncluded.Count}.")
            End If
        End Using
    End Sub

    Private Sub QueryFiltersUsingNavigationExample()
        Using db As New FilteredBloggingContextRequired
            db.Database.EnsureDeleted()
            db.Database.EnsureCreated()

#Region "SeedDataNavigation"
            db.Blogs.Add(
                New Blog With {
                    .Url = "http://sample.com/blogs/fish",
                    .Posts = New List(Of Post) From {
                        New Post With {
                            .Title = "Fish care 101"},
                        New Post With {
                            .Title = "Caring for tropical fish"},
                        New Post With {
                            .Title = "Types of ornamental fish"}
                    }
                })

            db.Blogs.Add(
                New Blog With {
                    .Url = "http://sample.com/blogs/cats",
                    .Posts = New List(Of Post) From {
                        New Post With {
                            .Title = "Cat care 101"},
                        New Post With {
                            .Title = "Caring for tropical cats"},
                        New Post With {
                            .Title = "Types of ornamental cats"}}
                })

            db.Blogs.Add(
                New Blog With {
                    .Url = "http://sample.com/blogs/catfish",
                    .Posts = New List(Of Post) From {
                        New Post With {
                            .Title = "Catfish care 101"},
                        New Post With {
                            .Title = "History of the catfish name"}}
                })
#End Region

            db.SaveChanges()
        End Using

        Console.WriteLine("Query filters using navigations demo")

        Using db As New FilteredBloggingContextRequired

#Region "QueriesNavigation"
            Dim filteredBlogs = db.Blogs.ToList()
#End Region

            Dim filteredBlogsInclude = db.Blogs.Include(Function(b) b.Posts).ToList()

            If filteredBlogs.Count = 2 AndAlso filteredBlogsInclude.Count = 2 Then
                Console.WriteLine("Blogs without any Posts are also filtered out. Posts must contain 'fish' in title.")
                Console.WriteLine(
                "Filters are applied recursively, so Blogs that do have Posts, but those Posts don't contain 'fish' in the title will also be filtered out.")
            End If

        End Using
    End Sub

End Module
