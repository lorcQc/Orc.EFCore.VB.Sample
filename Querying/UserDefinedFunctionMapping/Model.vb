Imports System.Data
Imports System.Linq.Expressions
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.EntityFrameworkCore.Query.SqlExpressions
Imports Microsoft.EntityFrameworkCore.Storage

#Region "Entities"

Public Class Blog
    Public Property BlogId As Integer
    Public Property Url As String
    Public Property Rating As Integer?

    Public Property Posts As List(Of Post)
End Class

Public Class Post
    Public Property PostId As Integer
    Public Property Title As String
    Public Property Content As String
    Public Property Rating As Integer
    Public Property BlogId As Integer

    Public Property Blog As Blog
    Public Property Comments As List(Of Comment)
End Class

Public Class Comment
    Public Property CommentId As Integer
    Public Property Text As String
    Public Property Likes As Integer
    Public Property PostId As Integer

    Public Property Post As Post
End Class

#End Region

Public Class BloggingContext
    Inherits DbContext

    Public Property Blogs As DbSet(Of Blog)
    Public Property Posts As DbSet(Of Post)
    Public Property Comments As DbSet(Of Comment)

#Region "BasicFunctionDefinition"
    Public Function ActivePostCountForBlog(blogId As Integer) As Integer
        Throw New NotSupportedException
    End Function
#End Region

#Region "HasTranslationFunctionDefinition"
    Public Function PercentageDifference(first As Double, second As Integer) As Double
        Throw New NotSupportedException
    End Function
#End Region

#Region "QueryableFunctionDefinition"
    Public Function PostsWithPopularComments(likeThreshold As Integer) As IQueryable(Of Post)
        Return FromExpression(Function() PostsWithPopularComments(likeThreshold))
    End Function
#End Region

#Region "NullabilityPropagationFunctionDefinition"
    Public Function ConcatStrings(prm1 As String, prm2 As String) As String
        Throw New InvalidOperationException
    End Function
    Public Function ConcatStringsOptimized(prm1 As String, prm2 As String) As String
        Throw New InvalidOperationException
    End Function
#End Region

    Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
#Region "EntityConfiguration"
        modelBuilder.Entity(Of Blog)().HasMany(Function(b) b.Posts) _
            .WithOne(Function(p) p.Blog)

        modelBuilder.Entity(Of Post)().HasMany(Function(p) p.Comments) _
            .WithOne(Function(c) c.Post)
#End Region

        modelBuilder.Entity(Of Blog)().HasData(
            New Blog With {
                .BlogId = 1,
                .Url = "https://devblogs.microsoft.com/dotnet",
                .Rating = 5},
            New Blog With {
                .BlogId = 2,
                .Url = "https://mytravelblog.com/",
                .Rating = 4})

        modelBuilder.Entity(Of Post)().HasData(
            New Post With {
                .PostId = 1,
                .BlogId = 1,
                .Title = "What's new",
                .Content = "Lorem ipsum dolor sit amet",
                .Rating = 5},
            New Post With {
                .PostId = 2,
                .BlogId = 2,
                .Title = "Around the World in Eighty Days",
                .Content = "consectetur adipiscing elit",
                .Rating = 5},
            New Post With {
                .PostId = 3,
                .BlogId = 2,
                .Title = "Glamping *is* the way",
                .Content = "sed do eiusmod tempor incididunt",
                .Rating = 4},
            New Post With {
                .PostId = 4,
                .BlogId = 2,
                .Title = "Travel in the time of pandemic",
                .Content = "ut labore et dolore magna aliqua",
                .Rating = 3})

        modelBuilder.Entity(Of Comment)().HasData(
            New Comment With {
                .CommentId = 1,
                .PostId = 1,
                .Text = "Exciting!",
                .Likes = 3},
            New Comment With {
                .CommentId = 2,
                .PostId = 1,
                .Text = "Dotnet is useless - why use C# when you can write super fast assembly code instead?",
                .Likes = 0},
            New Comment With {
                .CommentId = 3,
                .PostId = 2,
                .Text = "Didn't think you would make it!",
                .Likes = 3},
            New Comment With {
                .CommentId = 4,
                .PostId = 2,
                .Text = "Are you going to try 70 days next time?",
                .Likes = 5},
            New Comment With {
                .CommentId = 5,
                .PostId = 2,
                .Text = "Good thing the earth is round :)",
                .Likes = 5},
            New Comment With {
                .CommentId = 6,
                .PostId = 3,
                .Text = "I couldn't agree with you more",
                .Likes = 2})

#Region "BasicFunctionConfiguration"
        modelBuilder.HasDbFunction(GetType(BloggingContext).GetMethod(NameOf(ActivePostCountForBlog), {GetType(Integer)})) _
            .HasName("CommentedPostCountForBlog")
#End Region

#Region "HasTranslationFunctionConfiguration"
        ' 100 * ABS(first - second) / ((first + second) / 2)
        modelBuilder.HasDbFunction(
                GetType(BloggingContext).GetMethod(NameOf(PercentageDifference), {GetType(Double), GetType(Integer)})) _
            .HasTranslation(
                Function(args) As SqlBinaryExpression
                    Return New SqlBinaryExpression(
                        ExpressionType.Multiply,
                        New SqlConstantExpression(
                            Expression.Constant(100),
                            New IntTypeMapping("int", DbType.Int32)),
                        New SqlBinaryExpression(
                            ExpressionType.Divide,
                            New SqlFunctionExpression(
                                "ABS",
                                New SqlExpression() {
                                    New SqlBinaryExpression(
                                        ExpressionType.Subtract,
                                        args.First(),
                                        args.Skip(1).First(),
                                        args.First().Type,
                                        args.First().TypeMapping)
                                },
                                nullable:=True,
                                argumentsPropagateNullability:={True, True},
                                type:=args.First().Type,
                                typeMapping:=args.First().TypeMapping),
                            New SqlBinaryExpression(
                                ExpressionType.Divide,
                                New SqlBinaryExpression(
                                    ExpressionType.Add,
                                    args.First(),
                                    args.Skip(1).First(),
                                    args.First().Type,
                                    args.First().TypeMapping),
                                New SqlConstantExpression(
                                    Expression.Constant(2),
                                    New IntTypeMapping("int", DbType.Int32)),
                                args.First().Type,
                                args.First().TypeMapping),
                            args.First().Type,
                            args.First().TypeMapping),
                        args.First().Type,
                        args.First().TypeMapping)
                End Function)
#End Region

#Region "NullabilityPropagationModelConfiguration"
        modelBuilder _
            .HasDbFunction(GetType(BloggingContext).GetMethod(NameOf(ConcatStrings), {GetType(String), GetType(String)})) _
            .HasName("ConcatStrings")

        modelBuilder.HasDbFunction(
            GetType(BloggingContext).GetMethod(NameOf(ConcatStringsOptimized), {GetType(String), GetType(String)}),
            Sub(b)
                b.HasName("ConcatStrings")
                b.HasParameter("prm1").PropagatesNullability()
                b.HasParameter("prm2").PropagatesNullability()
            End Sub)
#End Region

#Region "QueryableFunctionConfigurationHasDbFunction"
        modelBuilder.Entity(Of Post)().ToTable("Posts")
        modelBuilder.HasDbFunction(GetType(BloggingContext).GetMethod(NameOf(PostsWithPopularComments), {GetType(Integer)}))
#End Region
    End Sub

    Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
        optionsBuilder.UseSqlServer(
            "Server=(localdb)\mssqllocaldb;Database=sample_EFQuerying.UserDefinedFunctionMapping;Trusted_Connection=True;ConnectRetryCount=0")
    End Sub
End Class
