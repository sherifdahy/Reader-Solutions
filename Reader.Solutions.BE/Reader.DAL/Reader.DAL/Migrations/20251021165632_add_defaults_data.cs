using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reader.DAL.Migrations
{
    /// <inheritdoc />
    public partial class add_defaults_data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGpZHN4AmbEKx2cxfESTo648C0RSryQoD/zLoM6Pu0LcbH463S90dkQygz3tl2VJPQ==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHKQKLdBW0Gg2FrAAwWsRr/5J6c3ec3ZwsWZB0xiXe6VNNcvyfU/uNaA0NdHedFeUQ==");
        }
    }
}
