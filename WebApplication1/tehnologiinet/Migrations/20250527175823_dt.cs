using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tehnologiinet.Migrations
{
    public partial class dt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalQuantity",
                table: "Constructions",
                newName: "Quantity");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Constructions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "Constructions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Constructions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Constructions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Constructions_ItemId",
                table: "Constructions",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Constructions_Items_ItemId",
                table: "Constructions",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Constructions_Items_ItemId",
                table: "Constructions");

            migrationBuilder.DropIndex(
                name: "IX_Constructions_ItemId",
                table: "Constructions");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Constructions");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "Constructions");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Constructions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Constructions");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Constructions",
                newName: "TotalQuantity");
        }
    }
}
