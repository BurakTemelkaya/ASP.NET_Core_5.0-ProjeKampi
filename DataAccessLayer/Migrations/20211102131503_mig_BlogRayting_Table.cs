using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class mig_BlogRayting_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlogRaytings",
                columns: table => new
                {
                    BlogRaytingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlogID = table.Column<int>(type: "int", nullable: false),
                    BlogTotalScore = table.Column<int>(type: "int", nullable: false),
                    BlogRaytingCount = table.Column<int>(type: "int", nullable: false),
                    BlogRaytingAverage = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogRaytings", x => x.BlogRaytingID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogRaytings");
        }
    }
}
