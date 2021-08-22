using Microsoft.EntityFrameworkCore.Migrations;

namespace PortLaMontagne.Migrations
{
    public partial class FixId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Partners",
                newName: "PartnerId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Comment",
                newName: "CommentId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Articles",
                newName: "ArticleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PartnerId",
                table: "Partners",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "CommentId",
                table: "Comment",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ArticleId",
                table: "Articles",
                newName: "Id");
        }
    }
}
