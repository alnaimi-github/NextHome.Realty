using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NextHome.Realty.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AmentiyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Amenities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VillaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amenities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Amenities_Villas_VillaId",
                        column: x => x.VillaId,
                        principalTable: "Villas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Amenities",
                columns: new[] { "Id", "Description", "Name", "VillaId" },
                values: new object[,]
                {
                    { 1, "", "Private Pool", 1 },
                    { 2, "", "Microwave", 1 },
                    { 3, "", "Private Balcony", 1 },
                    { 4, "", "1 king bed and 1 sofa bed", 1 },
                    { 5, "", "Private Plunge Pool", 2 },
                    { 6, "", "Microwave and Mini Refrigerator", 2 },
                    { 7, "", "Private Balcony", 2 },
                    { 8, "", "king bed or 2 double beds", 2 },
                    { 9, "", "Private Pool", 7 },
                    { 10, "", "Jacuzzi", 7 },
                    { 11, "", "Private Balcony", 7 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Amenities_VillaId",
                table: "Amenities",
                column: "VillaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Amenities");
        }
    }
}
