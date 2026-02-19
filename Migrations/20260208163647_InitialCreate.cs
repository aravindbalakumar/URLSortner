using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace URLSortner.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "URLShort",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    OriginalURL = table.Column<string>(type: "text", nullable: false),
                    ShortenedURL = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_URLShort", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_URLShort_ShortenedURL",
                table: "URLShort",
                column: "ShortenedURL",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "URLShort");
        }
    }
}
