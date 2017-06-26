using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class FieldRenameClientComputer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationsInfos_ClientComputers_ClientComputerId",
                table: "ApplicationsInfos");

            migrationBuilder.DropColumn(
                name: "PSComputerId",
                table: "ApplicationsInfos");

            migrationBuilder.AlterColumn<int>(
                name: "ClientComputerId",
                table: "ApplicationsInfos",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationsInfos_ClientComputers_ClientComputerId",
                table: "ApplicationsInfos",
                column: "ClientComputerId",
                principalTable: "ClientComputers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationsInfos_ClientComputers_ClientComputerId",
                table: "ApplicationsInfos");

            migrationBuilder.AlterColumn<int>(
                name: "ClientComputerId",
                table: "ApplicationsInfos",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "PSComputerId",
                table: "ApplicationsInfos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationsInfos_ClientComputers_ClientComputerId",
                table: "ApplicationsInfos",
                column: "ClientComputerId",
                principalTable: "ClientComputers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
