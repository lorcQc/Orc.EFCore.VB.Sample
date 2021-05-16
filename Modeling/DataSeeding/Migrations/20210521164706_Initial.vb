Imports System
Imports System.Collections.Generic
Imports Microsoft.EntityFrameworkCore.Migrations

Namespace Global.DataSeeding.Migrations
    Partial Public Class Initial
        Inherits Migration

        Protected Overrides Sub Up(migrationBuilder As MigrationBuilder)
            migrationBuilder.CreateTable(
                name:="Blogs",
                columns:=Function(table) New With
                {
                    .BlogId = table.Column(Of Integer)(type:="int", nullable:=False).
                            Annotation("SqlServer:Identity", "1, 1"),
                    .Url = table.Column(Of String)(type:="nvarchar(max)", nullable:=False)
                },
                constraints:=Sub(table)
                    table.PrimaryKey("PK_Blogs", Function(e) e.BlogId)
                End Sub)

            migrationBuilder.CreateTable(
                name:="Posts",
                columns:=Function(table) New With
                {
                    .PostId = table.Column(Of Integer)(type:="int", nullable:=False).
                            Annotation("SqlServer:Identity", "1, 1"),
                    .Content = table.Column(Of String)(type:="nvarchar(max)", nullable:=True),
                    .Title = table.Column(Of String)(type:="nvarchar(max)", nullable:=True),
                    .BlogId = table.Column(Of Integer)(type:="int", nullable:=False),
                    .AuthorName_First = table.Column(Of String)(type:="nvarchar(max)", nullable:=True),
                    .AuthorName_Last = table.Column(Of String)(type:="nvarchar(max)", nullable:=True)
                },
                constraints:=Sub(table)
                    table.PrimaryKey("PK_Posts", Function(e) e.PostId)
                    table.ForeignKey(
                        name:="FK_Posts_Blogs_BlogId",
                        column:=Function(e) e.BlogId,
                        principalTable:="Blogs",
                        principalColumn:="BlogId",
                        onDelete:=ReferentialAction.Cascade)
                End Sub)

            migrationBuilder.InsertData(
                table:="Blogs",
                columns:={"BlogId", "Url"},
                values:=New Object() {1, "http://sample.com"})

            migrationBuilder.InsertData(
                table:="Posts",
                columns:={"PostId", "BlogId", "Content", "Title", "AuthorName_First", "AuthorName_Last"},
                values:=New Object() {1, 1, "Test 1", "First post", "Andriy", "Svyryd"})

            migrationBuilder.InsertData(
                table:="Posts",
                columns:={"PostId", "BlogId", "Content", "Title", "AuthorName_First", "AuthorName_Last"},
                values:=New Object() {2, 1, "Test 2", "Second post", "Diego", "Vega"})

            migrationBuilder.CreateIndex(
                name:="IX_Posts_BlogId",
                table:="Posts",
                column:="BlogId")
        End Sub

        Protected Overrides Sub Down(migrationBuilder As MigrationBuilder)
            migrationBuilder.DropTable(
                name:="Posts")

            migrationBuilder.DropTable(
                name:="Blogs")
        End Sub
    End Class
End Namespace
