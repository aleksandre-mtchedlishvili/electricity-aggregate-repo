using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Electricity.Api.Migrations
{
    public partial class CreateElecticityEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ElectricityAggregates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tinklas = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    PSum = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElectricityAggregates", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ElectricityAggregates_Tinklas",
                table: "ElectricityAggregates",
                column: "Tinklas",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ElectricityAggregates");
        }
    }
}
