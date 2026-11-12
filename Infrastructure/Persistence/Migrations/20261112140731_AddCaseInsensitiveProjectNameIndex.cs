using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCaseInsensitiveProjectNameIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropIndex(
            name: "IX_Projects_Name",
            table: "Projects");

            migrationBuilder.Sql(
            """
            CREATE UNIQUE INDEX "IX_Projects_Name_Lower"
            ON "Projects" (LOWER("Name"));
            """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """DROP INDEX "IX_Projects_Name_Lower";""");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Name",
                table: "Projects",
                column: "Name",
                unique: true);
        }
    }
}
