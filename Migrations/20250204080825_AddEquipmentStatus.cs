using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace thaicodelab_api.Migrations
{
    /// <inheritdoc />
    public partial class AddEquipmentStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "user_status",
                schema: "sc_center_users",
                table: "tb_user_status",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<int>(
                name: "equipment_status_id",
                schema: "sc_equipments",
                table: "tb_equipments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "tb_equipment_status",
                schema: "sc_equipments",
                columns: table => new
                {
                    equipment_status_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    equipment_status_code = table.Column<string>(type: "text", nullable: false),
                    equipment_status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    updated_by = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_equipment_status", x => x.equipment_status_id);
                });

            migrationBuilder.UpdateData(
                schema: "sc_center_users",
                table: "tb_users",
                keyColumn: "user_id",
                keyValue: 1,
                column: "user_password",
                value: "$2a$11$HRHyq0hFkXEEABFhR6zNWe54ll.0428Es6lv5e1JlUg3wDtdhh7te");

            migrationBuilder.CreateIndex(
                name: "IX_tb_equipments_equipment_status_id",
                schema: "sc_equipments",
                table: "tb_equipments",
                column: "equipment_status_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_equipments_tb_equipment_status_equipment_status_id",
                schema: "sc_equipments",
                table: "tb_equipments",
                column: "equipment_status_id",
                principalSchema: "sc_equipments",
                principalTable: "tb_equipment_status",
                principalColumn: "equipment_status_id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_equipments_tb_equipment_status_equipment_status_id",
                schema: "sc_equipments",
                table: "tb_equipments");

            migrationBuilder.DropTable(
                name: "tb_equipment_status",
                schema: "sc_equipments");

            migrationBuilder.DropIndex(
                name: "IX_tb_equipments_equipment_status_id",
                schema: "sc_equipments",
                table: "tb_equipments");

            migrationBuilder.DropColumn(
                name: "equipment_status_id",
                schema: "sc_equipments",
                table: "tb_equipments");

            migrationBuilder.AlterColumn<string>(
                name: "user_status",
                schema: "sc_center_users",
                table: "tb_user_status",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.UpdateData(
                schema: "sc_center_users",
                table: "tb_users",
                keyColumn: "user_id",
                keyValue: 1,
                column: "user_password",
                value: "$2a$11$tfK5h9B0tIRWH1pkTbX6ke3rk4AOvaFhiHqbOvqQeMQdao5YyBSb.");
        }
    }
}
