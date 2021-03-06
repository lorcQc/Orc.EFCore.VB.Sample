Imports System
Imports System.Collections.Generic
Imports Microsoft.EntityFrameworkCore.Migrations

Namespace Global.GetStarted.Migrations
    Partial Public Class InitialCreate
        Inherits Migration

        Protected Overrides Sub Up(migrationBuilder As MigrationBuilder)
            migrationBuilder.CreateTable(
                name:="Blogs",
                columns:=Function(table) New With
                {
                    .BlogId = table.Column(Of Integer)(type:="INTEGER", nullable:=False).
                            Annotation("Sqlite:Autoincrement", True),
                    .Url = table.Column(Of String)(type:="TEXT", nullable:=True)
                },
                constraints:=Sub(table)
                    table.PrimaryKey("PK_Blogs", Function(e) e.BlogId)
                End Sub)

            migrationBuilder.CreateTable(
                name:="Posts",
                columns:=Function(table) New With
                {
                    .PostId = table.Column(Of Integer)(type:="INTEGER", nullable:=False).
                            Annotation("Sqlite:Autoincrement", True),
                    .Title = table.Column(Of String)(type:="TEXT", nullable:=True),
                    .Content = table.Column(Of String)(type:="TEXT", nullable:=True),
                    .BlogId = table.Column(Of Integer)(type:="INTEGER", nullable:=False)
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
