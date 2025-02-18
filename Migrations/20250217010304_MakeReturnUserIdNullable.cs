using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace thaicodelab_api.Migrations
{
    /// <inheritdoc />
    public partial class MakeReturnUserIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "return_user_id",
                schema: "sc_equipments",
                table: "tb_equipment_transaction_details",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.UpdateData(
                schema: "sc_center_users",
                table: "tb_users",
                keyColumn: "user_id",
                keyValue: 1,
                column: "user_password",
                value: "$2a$11$4vQghuzXFFS2TkTWW.5pxe10zsji/JWLwbNPEetQtT120ctmfFnhC");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "return_user_id",
                schema: "sc_equipments",
                table: "tb_equipment_transaction_details",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.UpdateData(
                schema: "sc_center_users",
                table: "tb_users",
                keyColumn: "user_id",
                keyValue: 1,
                column: "user_password",
                value: "$2a$11$7w1etq6kGo6BAah9sPps/O7K/oewJ/Gh4WXzsuaLQVHdpZGEmyklK");
        }
    }
}
