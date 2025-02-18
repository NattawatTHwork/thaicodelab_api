using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace thaicodelab_api.Migrations
{
    /// <inheritdoc />
    public partial class AddEquipmentUniqueCodeToEquipment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "equipment_unique_code",
                schema: "sc_equipments",
                table: "tb_equipments",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                schema: "sc_center_users",
                table: "tb_users",
                keyColumn: "user_id",
                keyValue: 1,
                column: "user_password",
                value: "$2a$11$LbtheWxmMSAAULvHlhzZoO0o9o9z4rRXcR8PBmX1bmGaCxj4y7.Zm");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "equipment_unique_code",
                schema: "sc_equipments",
                table: "tb_equipments");

            migrationBuilder.UpdateData(
                schema: "sc_center_users",
                table: "tb_users",
                keyColumn: "user_id",
                keyValue: 1,
                column: "user_password",
                value: "$2a$11$K3VJ957UmdS.0lnnqBeMRuT2Kc/vqQnIb6k2/brQ/2FOscVIgKMoW");
        }
    }
}
