using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMGUyeSiparis.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class IlkKurulum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Uyeler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Isim = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Unvan = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    KanGrubu = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uyeler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SiparisOzetleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UyeId = table.Column<int>(type: "int", nullable: false),
                    Tarih = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiparisOzetleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SiparisOzetleri_Uyeler_UyeId",
                        column: x => x.UyeId,
                        principalTable: "Uyeler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SiparisDetaylari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiparisOzetId = table.Column<int>(type: "int", nullable: false),
                    Urun = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Adet = table.Column<int>(type: "int", nullable: false),
                    Beden = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    KanGrubu = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiparisDetaylari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SiparisDetaylari_SiparisOzetleri_SiparisOzetId",
                        column: x => x.SiparisOzetId,
                        principalTable: "SiparisOzetleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SiparisDetaylari_SiparisOzetId",
                table: "SiparisDetaylari",
                column: "SiparisOzetId");

            migrationBuilder.CreateIndex(
                name: "IX_SiparisOzetleri_UyeId",
                table: "SiparisOzetleri",
                column: "UyeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SiparisDetaylari");

            migrationBuilder.DropTable(
                name: "SiparisOzetleri");

            migrationBuilder.DropTable(
                name: "Uyeler");
        }
    }
}
