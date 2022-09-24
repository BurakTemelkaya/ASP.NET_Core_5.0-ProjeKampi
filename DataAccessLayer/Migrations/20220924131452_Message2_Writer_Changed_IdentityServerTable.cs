using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class Message2_Writer_Changed_IdentityServerTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages2_Writers_ReceiverID",
                table: "Messages2");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages2_Writers_SenderID",
                table: "Messages2");

            migrationBuilder.DropIndex(
                name: "IX_Messages2_ReceiverID",
                table: "Messages2");

            migrationBuilder.DropIndex(
                name: "IX_Messages2_SenderID",
                table: "Messages2");

            migrationBuilder.AddColumn<int>(
                name: "ReceiverUserId",
                table: "Messages2",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SenderUserId",
                table: "Messages2",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WriterID",
                table: "Messages2",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WriterID1",
                table: "Messages2",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OldPassword",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordAgain",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages2_ReceiverUserId",
                table: "Messages2",
                column: "ReceiverUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages2_SenderUserId",
                table: "Messages2",
                column: "SenderUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages2_WriterID",
                table: "Messages2",
                column: "WriterID");

            migrationBuilder.CreateIndex(
                name: "IX_Messages2_WriterID1",
                table: "Messages2",
                column: "WriterID1");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages2_AspNetUsers_ReceiverUserId",
                table: "Messages2",
                column: "ReceiverUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages2_AspNetUsers_SenderUserId",
                table: "Messages2",
                column: "SenderUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages2_Writers_WriterID",
                table: "Messages2",
                column: "WriterID",
                principalTable: "Writers",
                principalColumn: "WriterID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages2_Writers_WriterID1",
                table: "Messages2",
                column: "WriterID1",
                principalTable: "Writers",
                principalColumn: "WriterID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages2_AspNetUsers_ReceiverUserId",
                table: "Messages2");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages2_AspNetUsers_SenderUserId",
                table: "Messages2");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages2_Writers_WriterID",
                table: "Messages2");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages2_Writers_WriterID1",
                table: "Messages2");

            migrationBuilder.DropIndex(
                name: "IX_Messages2_ReceiverUserId",
                table: "Messages2");

            migrationBuilder.DropIndex(
                name: "IX_Messages2_SenderUserId",
                table: "Messages2");

            migrationBuilder.DropIndex(
                name: "IX_Messages2_WriterID",
                table: "Messages2");

            migrationBuilder.DropIndex(
                name: "IX_Messages2_WriterID1",
                table: "Messages2");

            migrationBuilder.DropColumn(
                name: "ReceiverUserId",
                table: "Messages2");

            migrationBuilder.DropColumn(
                name: "SenderUserId",
                table: "Messages2");

            migrationBuilder.DropColumn(
                name: "WriterID",
                table: "Messages2");

            migrationBuilder.DropColumn(
                name: "WriterID1",
                table: "Messages2");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OldPassword",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PasswordAgain",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_Messages2_ReceiverID",
                table: "Messages2",
                column: "ReceiverID");

            migrationBuilder.CreateIndex(
                name: "IX_Messages2_SenderID",
                table: "Messages2",
                column: "SenderID");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages2_Writers_ReceiverID",
                table: "Messages2",
                column: "ReceiverID",
                principalTable: "Writers",
                principalColumn: "WriterID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages2_Writers_SenderID",
                table: "Messages2",
                column: "SenderID",
                principalTable: "Writers",
                principalColumn: "WriterID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
