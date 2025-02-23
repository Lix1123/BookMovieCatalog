using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookMovieCatalog.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-12345",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "55a08371-75ad-4732-ba23-769410c1b304", "AQAAAAIAAYagAAAAEGJe6piQgNN6NfVvuCcAeyA0E0UT5daaayenRL2HgtBrphpab/YSzkpuxVorUs6pfw==", "d016a64b-1f43-4436-a7fb-0fd7fb3c61fe" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-12345",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "20034380-8839-4860-9019-7f924c50dee7", "AQAAAAIAAYagAAAAEG6/6pyobXiPn6wA82+6lawbQzuPwIaW8myNSCpjsBZ0d6zxwjjq62hHnKz8XYjupw==", "b179f446-de42-4707-88b2-aebbaaab591a" });
        }
    }
}
