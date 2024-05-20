using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Messenger.Migrations
{
    public partial class V2_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MessageText",
                table: "messages",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "SendDate",
                table: "messages",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "SenderId",
                table: "messages",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Base64Image",
                table: "chats",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "chats",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MessageText",
                table: "messages");

            migrationBuilder.DropColumn(
                name: "SendDate",
                table: "messages");

            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "messages");

            migrationBuilder.DropColumn(
                name: "Base64Image",
                table: "chats");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "chats");
        }
    }
}
