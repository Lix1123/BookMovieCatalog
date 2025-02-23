using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookMovieCatalog.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookRatings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-12345",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3f62a2fb-8f27-48b0-a4af-086a831a18ae", "AQAAAAIAAYagAAAAEJ5oZT4yJWgw1IzzoRk6bSNiN3vR2cyLvqgSNqzYttwSg+jRFvM+p9CHtP88n91FCA==", "2090640b-5f62-43bc-aa73-0d1c8247929a" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-12345",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e6a9dad8-9a4b-413c-bc12-33faea884470", "AQAAAAIAAYagAAAAELjVrLmswAgUWmPbcimEVRu7jRp1NYN2LO/pXZ28ocMJCEYaW5BHKov3Ly5sktSrsg==", "511d89ec-1db3-4e6e-ae99-8d7a471ebac0" });
        }
    }
}
