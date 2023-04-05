using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bist.Migrations
{
    /// <inheritdoc />
    public partial class AddModel3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "hisses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HisseAdi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HisseninPazari = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VeriSetiDosyaAdi = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hisses", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "hisses");
        }
    }
}
