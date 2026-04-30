using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBrandEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "UserGuid" },
                values: new object[] { new DateTime(2026, 3, 13, 23, 36, 34, 829, DateTimeKind.Local).AddTicks(8529), new Guid("b22b792c-3f74-423b-ac70-a2382d318431") });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2026, 3, 13, 23, 36, 34, 830, DateTimeKind.Local).AddTicks(1730));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2026, 3, 13, 23, 36, 34, 830, DateTimeKind.Local).AddTicks(1738));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "UserGuid" },
                values: new object[] { new DateTime(2026, 3, 12, 14, 44, 35, 685, DateTimeKind.Local).AddTicks(627), new Guid("d7aac95c-bb59-4dd4-8516-96ccecb406b5") });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2026, 3, 12, 14, 44, 35, 685, DateTimeKind.Local).AddTicks(4004));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2026, 3, 12, 14, 44, 35, 685, DateTimeKind.Local).AddTicks(4013));
        }
    }
}
