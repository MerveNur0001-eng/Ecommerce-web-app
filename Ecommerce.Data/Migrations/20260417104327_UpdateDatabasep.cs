using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabasep : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "UserGuid" },
                values: new object[] { new DateTime(2026, 4, 17, 13, 43, 25, 818, DateTimeKind.Local).AddTicks(829), new Guid("e9ff4e64-d671-42eb-927f-acecd89597a1") });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2026, 4, 17, 13, 43, 25, 818, DateTimeKind.Local).AddTicks(8127));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2026, 4, 17, 13, 43, 25, 818, DateTimeKind.Local).AddTicks(8146));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "UserGuid" },
                values: new object[] { new DateTime(2026, 4, 17, 13, 37, 39, 931, DateTimeKind.Local).AddTicks(383), new Guid("17147f9b-397e-46f9-8336-2594ea5550b8") });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2026, 4, 17, 13, 37, 39, 931, DateTimeKind.Local).AddTicks(4840));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2026, 4, 17, 13, 37, 39, 931, DateTimeKind.Local).AddTicks(4852));
        }
    }
}
