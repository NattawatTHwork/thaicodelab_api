using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace thaicodelab_api.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePermissionModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "module",
                schema: "sc_center_users",
                table: "tb_permissions",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                schema: "sc_equipments",
                table: "tb_equipment_status",
                keyColumn: "equipment_status_id",
                keyValue: 1,
                column: "equipment_status",
                value: "Returned");

            migrationBuilder.UpdateData(
                schema: "sc_equipments",
                table: "tb_equipment_status",
                keyColumn: "equipment_status_id",
                keyValue: 2,
                column: "equipment_status",
                value: "Borrowed");

            migrationBuilder.UpdateData(
                schema: "sc_center_users",
                table: "tb_permissions",
                keyColumn: "permission_id",
                keyValue: 1,
                column: "module",
                value: "");

            migrationBuilder.UpdateData(
                schema: "sc_center_users",
                table: "tb_users",
                keyColumn: "user_id",
                keyValue: 1,
                column: "user_password",
                value: "$2a$11$JzKHa.72.5Oz1e6ranjXSe72IjMLWDPOSQdSIoGuugQcme32TSVKi");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "module",
                schema: "sc_center_users",
                table: "tb_permissions");

            migrationBuilder.UpdateData(
                schema: "sc_equipments",
                table: "tb_equipment_status",
                keyColumn: "equipment_status_id",
                keyValue: 1,
                column: "equipment_status",
                value: "Borrowed");

            migrationBuilder.UpdateData(
                schema: "sc_equipments",
                table: "tb_equipment_status",
                keyColumn: "equipment_status_id",
                keyValue: 2,
                column: "equipment_status",
                value: "Returned");

            migrationBuilder.UpdateData(
                schema: "sc_center_users",
                table: "tb_users",
                keyColumn: "user_id",
                keyValue: 1,
                column: "user_password",
                value: "$2a$11$Pq6oY4SQS2fYeG7zrGZfM.cT17LfNyxHnReu8fDV0LWEwuFDz4Q5C");
        }
    }
}
