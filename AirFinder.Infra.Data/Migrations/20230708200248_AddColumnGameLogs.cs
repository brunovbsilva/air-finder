using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirFinder.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnGameLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ValidateDate",
                table: "GameLogs",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValidateDate",
                table: "GameLogs");
        }
    }
}
