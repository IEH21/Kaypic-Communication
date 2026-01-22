using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace web3_kaypic.Migrations
{
    /// <inheritdoc />
    public partial class profil : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfileImageUrl",
                table: "TUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "comment_user_profile_image_url",
                table: "TMessageComment",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "msg_user_profile_image_url",
                table: "TMessage",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImageUrl",
                table: "TUser");

            migrationBuilder.DropColumn(
                name: "comment_user_profile_image_url",
                table: "TMessageComment");

            migrationBuilder.DropColumn(
                name: "msg_user_profile_image_url",
                table: "TMessage");
        }
    }
}
