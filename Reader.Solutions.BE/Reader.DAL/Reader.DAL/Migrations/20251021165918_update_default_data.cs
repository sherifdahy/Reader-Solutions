using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reader.DAL.Migrations
{
    /// <inheritdoc />
    public partial class update_default_data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { 1, 1 });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKrs6Ekw7IywN+PxisRW9xcajdE6POAsTCXxXu4cMR4ZOHi1coPin3dVlWsJIvR4mA==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGpZHN4AmbEKx2cxfESTo648C0RSryQoD/zLoM6Pu0LcbH463S90dkQygz3tl2VJPQ==");
        }
    }
}
