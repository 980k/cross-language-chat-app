using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrossLangChat.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigrate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_ChatRoom_ChatRoomId",
                table: "Message");

            migrationBuilder.AlterColumn<int>(
                name: "ChatRoomId",
                table: "Message",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_ChatRoom_ChatRoomId",
                table: "Message",
                column: "ChatRoomId",
                principalTable: "ChatRoom",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_ChatRoom_ChatRoomId",
                table: "Message");

            migrationBuilder.AlterColumn<int>(
                name: "ChatRoomId",
                table: "Message",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_ChatRoom_ChatRoomId",
                table: "Message",
                column: "ChatRoomId",
                principalTable: "ChatRoom",
                principalColumn: "Id");
        }
    }
}
