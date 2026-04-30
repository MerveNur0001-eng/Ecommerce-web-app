using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                table: "Categories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "UserGuid" },
                values: new object[] { new DateTime(2026, 3, 22, 20, 35, 3, 434, DateTimeKind.Local).AddTicks(2198), new Guid("b4624b41-c71e-4a63-a3a1-d3132f8e8f44") });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "ParentId" },
                values: new object[] { new DateTime(2026, 3, 22, 20, 35, 3, 434, DateTimeKind.Local).AddTicks(5355), null });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreateDate", "ParentId" },
                values: new object[] { new DateTime(2026, 3, 22, 20, 35, 3, 434, DateTimeKind.Local).AddTicks(5364), null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "UserGuid" },
                values: new object[] { new DateTime(2026, 3, 22, 13, 29, 30, 889, DateTimeKind.Local).AddTicks(1009), new Guid("13c78911-4243-43b6-94f8-f341bbe6dbf8") });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "ParentId" },
                values: new object[] { new DateTime(2026, 3, 22, 13, 29, 30, 889, DateTimeKind.Local).AddTicks(4799), 0 });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreateDate", "ParentId" },
                values: new object[] { new DateTime(2026, 3, 22, 13, 29, 30, 889, DateTimeKind.Local).AddTicks(4812), 0 });
        }
    }
}
