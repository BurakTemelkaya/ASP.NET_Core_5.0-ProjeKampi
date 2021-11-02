using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class Blogs_BlogName_Rename_BlogTitle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BlogName",
                table: "Blogs",
                newName: "BlogTitle");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BlogName",
                table: "Blogs",
                newName: "BlogName");
        }
    }
}
