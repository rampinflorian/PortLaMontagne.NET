using Microsoft.EntityFrameworkCore.Migrations;

namespace PortLaMontagne.Migrations
{
    public partial class IsAlertOnArticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAlert",
                table: "Articles",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAlert",
                table: "Articles");
        }
    }
}
