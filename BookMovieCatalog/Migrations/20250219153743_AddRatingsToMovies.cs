using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookMovieCatalog.Migrations
{
    /// <inheritdoc />
    public partial class AddRatingsToMovies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-12345",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a746a430-7ab7-44b6-9b96-6c397c3265dc", "AQAAAAIAAYagAAAAEONZ9ikROyreRlx31sorPs3OldZPjdNUMWz5Bfh1u8uUmE2ceKwqhyUJVCtggVtdcA==", "52ab94ad-2bfe-4f56-99c8-cc1e26e1e56e" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-12345",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1851dfe4-7c07-42ee-baba-cf0f17ba5985", "AQAAAAIAAYagAAAAEKIYGIztTF1Jy7iPxWDD5+3A9pZcTUxRPqFXz6qf08ayXRZfreM/ElJSKU+fntlMxA==", "869da7e8-f3ec-4bac-bee7-163bd5229da9" });
        }
    }
}
