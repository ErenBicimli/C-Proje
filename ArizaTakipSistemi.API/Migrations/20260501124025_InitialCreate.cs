using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ArizaTakipSistemi.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cihazlar",
                columns: table => new
                {
                    CihazId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Marka = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Model = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    SeriNumarasi = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CihazTuru = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    MusteriAdi = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    MusteriTelefon = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    MusteriAdres = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    MusteriEmail = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now','localtime')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cihazlar", x => x.CihazId);
                });

            migrationBuilder.CreateTable(
                name: "Kullanicilar",
                columns: table => new
                {
                    KullaniciId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ad = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Soyad = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Sifre = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Rol = table.Column<int>(type: "INTEGER", nullable: false),
                    Telefon = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    AktifMi = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now','localtime')"),
                    SonGirisTarihi = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanicilar", x => x.KullaniciId);
                });

            migrationBuilder.CreateTable(
                name: "Arizalar",
                columns: table => new
                {
                    ArizaId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CihazId = table.Column<int>(type: "INTEGER", nullable: false),
                    KullaniciId = table.Column<int>(type: "INTEGER", nullable: true),
                    ArizaTanimi = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Kategori = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Durum = table.Column<int>(type: "INTEGER", nullable: false),
                    OncelikDurumu = table.Column<int>(type: "INTEGER", nullable: false),
                    YapilanIslem = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    TahminiMaliyet = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now','localtime')"),
                    GuncellemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: true),
                    TamamlanmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Arizalar", x => x.ArizaId);
                    table.ForeignKey(
                        name: "FK_Arizalar_Cihazlar_CihazId",
                        column: x => x.CihazId,
                        principalTable: "Cihazlar",
                        principalColumn: "CihazId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Arizalar_Kullanicilar_KullaniciId",
                        column: x => x.KullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "KullaniciId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Loglar",
                columns: table => new
                {
                    LogId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IslemTuru = table.Column<int>(type: "INTEGER", nullable: false),
                    TabloAdi = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    KayitId = table.Column<int>(type: "INTEGER", nullable: false),
                    YapilanIslem = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    EskiDegerler = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    YeniDegerler = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    KullaniciId = table.Column<int>(type: "INTEGER", nullable: false),
                    IslemTarihi = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now','localtime')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loglar", x => x.LogId);
                    table.ForeignKey(
                        name: "FK_Loglar_Kullanicilar_KullaniciId",
                        column: x => x.KullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "KullaniciId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Kullanicilar",
                columns: new[] { "KullaniciId", "Ad", "AktifMi", "Email", "OlusturulmaTarihi", "Rol", "Sifre", "SonGirisTarihi", "Soyad", "Telefon" },
                values: new object[,]
                {
                    { 1, "Admin", true, "admin@arizatakip.com", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "admin123", null, "Yönetici", "05001234567" },
                    { 2, "Ahmet", true, "ahmet@arizatakip.com", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "ahmet123", null, "Tekniker", "05009876543" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Arizalar_CihazId",
                table: "Arizalar",
                column: "CihazId");

            migrationBuilder.CreateIndex(
                name: "IX_Arizalar_KullaniciId",
                table: "Arizalar",
                column: "KullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_Cihazlar_SeriNumarasi",
                table: "Cihazlar",
                column: "SeriNumarasi",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Kullanicilar_Email",
                table: "Kullanicilar",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Loglar_IslemTarihi",
                table: "Loglar",
                column: "IslemTarihi");

            migrationBuilder.CreateIndex(
                name: "IX_Loglar_KullaniciId",
                table: "Loglar",
                column: "KullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_Loglar_TabloAdi",
                table: "Loglar",
                column: "TabloAdi");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Arizalar");

            migrationBuilder.DropTable(
                name: "Loglar");

            migrationBuilder.DropTable(
                name: "Cihazlar");

            migrationBuilder.DropTable(
                name: "Kullanicilar");
        }
    }
}
