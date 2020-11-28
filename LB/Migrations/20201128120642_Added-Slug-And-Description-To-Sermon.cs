using Microsoft.EntityFrameworkCore.Migrations;

namespace LB.Migrations
{
    public partial class AddedSlugAndDescriptionToSermon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "Sermons");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Sermons",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileURL",
                table: "Sermons",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Sermons",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Sermons");

            migrationBuilder.DropColumn(
                name: "FileURL",
                table: "Sermons");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Sermons");

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "Sermons",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
