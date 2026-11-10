using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMMS.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddWoPartsUsed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WoPartsUsed",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WorkOrderId = table.Column<int>(type: "INTEGER", nullable: false),
                    SparePartId = table.Column<int>(type: "INTEGER", nullable: false),
                    Qty = table.Column<int>(type: "INTEGER", nullable: false),
                    UnitCost = table.Column<decimal>(type: "TEXT", nullable: false),
                    UsedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WoPartsUsed", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WoPartsUsed");
        }
    }
}
