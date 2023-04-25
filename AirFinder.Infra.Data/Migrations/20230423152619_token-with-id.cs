using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirFinder.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class tokenwithid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TokenControl",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TokenControl",
                table: "TokenControl",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TokenControl",
                table: "TokenControl");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TokenControl");
        }
    }
}
