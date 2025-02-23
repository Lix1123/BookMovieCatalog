using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookMovieCatalog.Migrations
{
    /// <inheritdoc />
    public partial class AddSumOfRatingsAndTotalVotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-12345",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1851dfe4-7c07-42ee-baba-cf0f17ba5985", "AQAAAAIAAYagAAAAEKIYGIztTF1Jy7iPxWDD5+3A9pZcTUxRPqFXz6qf08ayXRZfreM/ElJSKU+fntlMxA==", "869da7e8-f3ec-4bac-bee7-163bd5229da9" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-12345",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "55a08371-75ad-4732-ba23-769410c1b304", "AQAAAAIAAYagAAAAEGJe6piQgNN6NfVvuCcAeyA0E0UT5daaayenRL2HgtBrphpab/YSzkpuxVorUs6pfw==", "d016a64b-1f43-4436-a7fb-0fd7fb3c61fe" });
        }
    }
}
