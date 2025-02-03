using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace thaicodelab_api.Migrations
{
    /// <inheritdoc />
    public partial class ChangeBirthdateToDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "birthdate",
                schema: "sc_center_users",
                table: "tb_users",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.UpdateData(
                schema: "sc_center_users",
                table: "tb_users",
                keyColumn: "user_id",
                keyValue: 1,
                columns: new[] { "birthdate", "user_password" },
                values: new object[] { new DateOnly(2024, 1, 1), "$2a$11$tfK5h9B0tIRWH1pkTbX6ke3rk4AOvaFhiHqbOvqQeMQdao5YyBSb." });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "birthdate",
                schema: "sc_center_users",
                table: "tb_users",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.UpdateData(
                schema: "sc_center_users",
                table: "tb_users",
                keyColumn: "user_id",
                keyValue: 1,
                columns: new[] { "birthdate", "user_password" },
                values: new object[] { new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "$2a$11$pLvW7AwwhWFEOld8e813VOi0Jsb.kg08zCac53HEJidYUEbRzfP42" });
        }
    }
}
