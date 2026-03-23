using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRM_v2.Migrations
{
    /// <inheritdoc />
    public partial class AddNgaySinhToNhanVien : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "NgaySinh",
                table: "NhanViens",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "NhanVienResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    TenNhanVien = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenChucVu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NhanVienResponses");

            migrationBuilder.DropColumn(
                name: "NgaySinh",
                table: "NhanViens");
        }
    }
}
