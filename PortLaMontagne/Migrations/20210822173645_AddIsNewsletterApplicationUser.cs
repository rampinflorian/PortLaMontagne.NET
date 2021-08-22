using Microsoft.EntityFrameworkCore.Migrations;

namespace PortLaMontagne.Migrations
{
    public partial class AddIsNewsletterApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsNewsletter",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNewsletter",
                table: "AspNetUsers");
        }
    }
}
