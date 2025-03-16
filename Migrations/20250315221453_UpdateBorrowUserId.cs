using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace thaicodelab_api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBorrowUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "sc_center_users",
                table: "tb_users",
                keyColumn: "user_id",
                keyValue: 1,
                column: "user_password",
                value: "$2a$11$5WNfh7s1yE8GzycdcaDJI.vdCA26YegYu44DiIz7koaRFDAG8JjVq");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "sc_center_users",
                table: "tb_users",
                keyColumn: "user_id",
                keyValue: 1,
                column: "user_password",
                value: "$2a$11$Rw5M4VJZ.MwNbyPkmt00quozhUcfkPsgJ4d7cZrPDFD6GVTsll1HK");
        }
    }
}
