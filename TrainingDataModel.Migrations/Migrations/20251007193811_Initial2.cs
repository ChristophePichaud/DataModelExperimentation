using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainingDataModel.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class Initial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 7, 19, 38, 10, 786, DateTimeKind.Utc).AddTicks(5883));

            migrationBuilder.UpdateData(
                table: "admin_users",
                keyColumn: "id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 10, 7, 19, 38, 10, 786, DateTimeKind.Utc).AddTicks(263));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 7, 19, 35, 58, 402, DateTimeKind.Utc).AddTicks(8945));

            migrationBuilder.UpdateData(
                table: "admin_users",
                keyColumn: "id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 10, 7, 19, 35, 58, 402, DateTimeKind.Utc).AddTicks(2380));
        }
    }
}
