using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Data.Migrations
{
    /// <inheritdoc />
    public partial class addingenumm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderStatus",
                table: "Orders",
                newName: "OrderState");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "UserGuid" },
                values: new object[] { new DateTime(2026, 4, 20, 23, 38, 35, 48, DateTimeKind.Local).AddTicks(4648), new Guid("6eb49d22-b65e-4fcf-ba88-e9c598ece93d") });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2026, 4, 20, 23, 38, 35, 48, DateTimeKind.Local).AddTicks(8044));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2026, 4, 20, 23, 38, 35, 48, DateTimeKind.Local).AddTicks(8053));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderState",
                table: "Orders",
                newName: "OrderStatus");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "UserGuid" },
                values: new object[] { new DateTime(2026, 4, 20, 23, 34, 57, 811, DateTimeKind.Local).AddTicks(4389), new Guid("4c0b6003-f036-485c-b478-b4ae1ade439d") });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2026, 4, 20, 23, 34, 57, 811, DateTimeKind.Local).AddTicks(8045));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2026, 4, 20, 23, 34, 57, 811, DateTimeKind.Local).AddTicks(8054));
        }
    }
}
