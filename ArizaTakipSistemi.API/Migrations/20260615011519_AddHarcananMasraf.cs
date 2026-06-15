using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArizaTakipSistemi.API.Migrations
{
    /// <inheritdoc />
    public partial class AddHarcananMasraf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "HarcananMasraf",
                table: "Arizalar",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HarcananMasraf",
                table: "Arizalar");
        }
    }
}
