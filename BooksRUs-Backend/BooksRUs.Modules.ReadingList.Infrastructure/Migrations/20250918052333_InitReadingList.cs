using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BooksRUs.Modules.ReadingList.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitReadingList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "readinglist");

            migrationBuilder.CreateTable(
                name: "Items",
                schema: "readinglist",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    BookId = table.Column<Guid>(type: "uuid", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_UserId_BookId",
                schema: "readinglist",
                table: "Items",
                columns: new[] { "UserId", "BookId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Items",
                schema: "readinglist");
        }
    }
}
