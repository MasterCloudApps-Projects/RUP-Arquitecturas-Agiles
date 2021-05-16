using Microsoft.EntityFrameworkCore.Migrations;

namespace ShareThings.Data.Migrations
{
    public partial class deleteLockedProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Locked",
                table: "Product");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Locked",
                table: "Product",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
