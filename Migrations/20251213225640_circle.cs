using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace web3_kaypic.Migrations
{
    /// <inheritdoc />
    public partial class circle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TMessage",
                columns: table => new
                {
                    msg_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    msg_username = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    msg_content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    msg_created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    msg_image_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    msg_likes_count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TMessage", x => x.msg_id);
                });

            migrationBuilder.CreateTable(
                name: "TMessageComment",
                columns: table => new
                {
                    comment_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    msg_id = table.Column<int>(type: "int", nullable: false),
                    comment_username = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    comment_content = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    comment_created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TMessageComment", x => x.comment_id);
                    table.ForeignKey(
                        name: "FK_TMessageComment_TMessage_msg_id",
                        column: x => x.msg_id,
                        principalTable: "TMessage",
                        principalColumn: "msg_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TMessageComment_msg_id",
                table: "TMessageComment",
                column: "msg_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TMessageComment");

            migrationBuilder.DropTable(
                name: "TMessage");
        }
    }
}
