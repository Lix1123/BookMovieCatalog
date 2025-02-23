using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookMovieCatalog.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRatingsFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-12345",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d0907597-4477-4e1a-abdf-93fcace35dd4", "AQAAAAIAAYagAAAAEPpLqegS6Hh1iUaYHLWC8pckOeg/zhaJq8CzQkLD9Q283cuG7LPx3sOEp+pZhYDfug==", "f9ffe01c-1579-4377-a629-14fab3dfe960" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-12345",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "304cfd8b-dfa2-4868-b491-ff4aa72acc01", "AQAAAAIAAYagAAAAEEiLTVoEX0SihKwpPVZWhEoaWnOIawUv2xYkXdycOzr18plNwAWxHjoTV8ItWk03lg==", "c3527fda-e4af-48b7-a1c4-fcf7a03351cd" });
        }
    }
}
