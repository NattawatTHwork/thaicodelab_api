using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace thaicodelab_api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEquipmentTransactionDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_equipment_transactions_tb_equipments_equipment_id",
                schema: "sc_equipments",
                table: "tb_equipment_transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_equipment_transactions_tb_users_equipment_transaction_us~",
                schema: "sc_equipments",
                table: "tb_equipment_transactions");

            migrationBuilder.DropIndex(
                name: "IX_tb_equipment_transactions_equipment_id",
                schema: "sc_equipments",
                table: "tb_equipment_transactions");

            migrationBuilder.DropColumn(
                name: "equipment_id",
                schema: "sc_equipments",
                table: "tb_equipment_transactions");

            migrationBuilder.RenameColumn(
                name: "status_equipment_transaction",
                schema: "sc_equipments",
                table: "tb_equipment_transactions",
                newName: "operator_borrow_user_id");

            migrationBuilder.RenameColumn(
                name: "equipment_transaction_user_id",
                schema: "sc_equipments",
                table: "tb_equipment_transactions",
                newName: "borrow_user_id");

            migrationBuilder.RenameColumn(
                name: "equipment_transaction_timestamp",
                schema: "sc_equipments",
                table: "tb_equipment_transactions",
                newName: "borrow_timestamp");

            migrationBuilder.RenameIndex(
                name: "IX_tb_equipment_transactions_equipment_transaction_user_id",
                schema: "sc_equipments",
                table: "tb_equipment_transactions",
                newName: "IX_tb_equipment_transactions_borrow_user_id");

            migrationBuilder.CreateTable(
                name: "tb_equipment_transaction_details",
                schema: "sc_equipments",
                columns: table => new
                {
                    equipment_transaction_detail_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    equipment_transaction_id = table.Column<int>(type: "integer", nullable: false),
                    equipment_transaction_detail_code = table.Column<string>(type: "text", nullable: false),
                    equipment_id = table.Column<int>(type: "integer", nullable: false),
                    operator_return_user_id = table.Column<int>(type: "integer", nullable: false),
                    return_user_id = table.Column<int>(type: "integer", nullable: false),
                    return_timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    note = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_by = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_equipment_transaction_details", x => x.equipment_transaction_detail_id);
                    table.ForeignKey(
                        name: "FK_tb_equipment_transaction_details_tb_equipment_transactions_~",
                        column: x => x.equipment_transaction_id,
                        principalSchema: "sc_equipments",
                        principalTable: "tb_equipment_transactions",
                        principalColumn: "equipment_transaction_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_equipment_transaction_details_tb_equipments_equipment_id",
                        column: x => x.equipment_id,
                        principalSchema: "sc_equipments",
                        principalTable: "tb_equipments",
                        principalColumn: "equipment_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_equipment_transaction_details_tb_users_operator_return_u~",
                        column: x => x.operator_return_user_id,
                        principalSchema: "sc_center_users",
                        principalTable: "tb_users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_equipment_transaction_details_tb_users_return_user_id",
                        column: x => x.return_user_id,
                        principalSchema: "sc_center_users",
                        principalTable: "tb_users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                schema: "sc_center_users",
                table: "tb_users",
                keyColumn: "user_id",
                keyValue: 1,
                column: "user_password",
                value: "$2a$11$K3VJ957UmdS.0lnnqBeMRuT2Kc/vqQnIb6k2/brQ/2FOscVIgKMoW");

            migrationBuilder.CreateIndex(
                name: "IX_tb_equipment_transactions_operator_borrow_user_id",
                schema: "sc_equipments",
                table: "tb_equipment_transactions",
                column: "operator_borrow_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_equipment_transaction_details_equipment_id",
                schema: "sc_equipments",
                table: "tb_equipment_transaction_details",
                column: "equipment_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_equipment_transaction_details_equipment_transaction_id",
                schema: "sc_equipments",
                table: "tb_equipment_transaction_details",
                column: "equipment_transaction_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_equipment_transaction_details_operator_return_user_id",
                schema: "sc_equipments",
                table: "tb_equipment_transaction_details",
                column: "operator_return_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_equipment_transaction_details_return_user_id",
                schema: "sc_equipments",
                table: "tb_equipment_transaction_details",
                column: "return_user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_equipment_transactions_tb_users_borrow_user_id",
                schema: "sc_equipments",
                table: "tb_equipment_transactions",
                column: "borrow_user_id",
                principalSchema: "sc_center_users",
                principalTable: "tb_users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_equipment_transactions_tb_users_operator_borrow_user_id",
                schema: "sc_equipments",
                table: "tb_equipment_transactions",
                column: "operator_borrow_user_id",
                principalSchema: "sc_center_users",
                principalTable: "tb_users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_equipment_transactions_tb_users_borrow_user_id",
                schema: "sc_equipments",
                table: "tb_equipment_transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_equipment_transactions_tb_users_operator_borrow_user_id",
                schema: "sc_equipments",
                table: "tb_equipment_transactions");

            migrationBuilder.DropTable(
                name: "tb_equipment_transaction_details",
                schema: "sc_equipments");

            migrationBuilder.DropIndex(
                name: "IX_tb_equipment_transactions_operator_borrow_user_id",
                schema: "sc_equipments",
                table: "tb_equipment_transactions");

            migrationBuilder.RenameColumn(
                name: "operator_borrow_user_id",
                schema: "sc_equipments",
                table: "tb_equipment_transactions",
                newName: "status_equipment_transaction");

            migrationBuilder.RenameColumn(
                name: "borrow_user_id",
                schema: "sc_equipments",
                table: "tb_equipment_transactions",
                newName: "equipment_transaction_user_id");

            migrationBuilder.RenameColumn(
                name: "borrow_timestamp",
                schema: "sc_equipments",
                table: "tb_equipment_transactions",
                newName: "equipment_transaction_timestamp");

            migrationBuilder.RenameIndex(
                name: "IX_tb_equipment_transactions_borrow_user_id",
                schema: "sc_equipments",
                table: "tb_equipment_transactions",
                newName: "IX_tb_equipment_transactions_equipment_transaction_user_id");

            migrationBuilder.AddColumn<int>(
                name: "equipment_id",
                schema: "sc_equipments",
                table: "tb_equipment_transactions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                schema: "sc_center_users",
                table: "tb_users",
                keyColumn: "user_id",
                keyValue: 1,
                column: "user_password",
                value: "$2a$11$HRHyq0hFkXEEABFhR6zNWe54ll.0428Es6lv5e1JlUg3wDtdhh7te");

            migrationBuilder.CreateIndex(
                name: "IX_tb_equipment_transactions_equipment_id",
                schema: "sc_equipments",
                table: "tb_equipment_transactions",
                column: "equipment_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_equipment_transactions_tb_equipments_equipment_id",
                schema: "sc_equipments",
                table: "tb_equipment_transactions",
                column: "equipment_id",
                principalSchema: "sc_equipments",
                principalTable: "tb_equipments",
                principalColumn: "equipment_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_equipment_transactions_tb_users_equipment_transaction_us~",
                schema: "sc_equipments",
                table: "tb_equipment_transactions",
                column: "equipment_transaction_user_id",
                principalSchema: "sc_center_users",
                principalTable: "tb_users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
