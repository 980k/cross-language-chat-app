using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrossLangChat.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedUserModelLanguage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatRoomUser_User_ParticipantsId",
                table: "ChatRoomUser");

            migrationBuilder.RenameColumn(
                name: "ParticipantsId",
                table: "ChatRoomUser",
                newName: "UsersId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatRoomUser_ParticipantsId",
                table: "ChatRoomUser",
                newName: "IX_ChatRoomUser_UsersId");

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "User",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRoomUser_User_UsersId",
                table: "ChatRoomUser",
                column: "UsersId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatRoomUser_User_UsersId",
                table: "ChatRoomUser");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "ChatRoomUser",
                newName: "ParticipantsId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatRoomUser_UsersId",
                table: "ChatRoomUser",
                newName: "IX_ChatRoomUser_ParticipantsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRoomUser_User_ParticipantsId",
                table: "ChatRoomUser",
                column: "ParticipantsId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
