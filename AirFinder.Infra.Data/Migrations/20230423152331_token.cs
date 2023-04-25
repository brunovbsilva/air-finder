using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirFinder.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class token : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TokenControl",
                columns: table => new
                {
                    IdUser = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "varchar(6)", nullable: false),
                    Valid = table.Column<bool>(type: "bit", nullable: false),
                    SentDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TokenControl");
        }
    }
}
