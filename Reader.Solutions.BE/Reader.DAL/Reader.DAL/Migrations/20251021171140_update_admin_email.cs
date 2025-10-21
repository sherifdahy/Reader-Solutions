using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reader.DAL.Migrations
{
    /// <inheritdoc />
    public partial class update_admin_email : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "UserName" },
                values: new object[] { "admin@reader-solutions.com", "ADMIN@READER-SOLUTIONS.COM", "ADMIN@READER-SOLUTIONS.COM", "AQAAAAIAAYagAAAAECWh/vioWNvE32xqQYMgDdh/v3H/RWh7nnXYM+VhxDCSn4BViz6NrN5AooRAQzZskA==", "admin@reader-solutions.com" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "UserName" },
                values: new object[] { "admin@note-solutions.com", "ADMIN@NOTE-SOLUTIONS.COM", "ADMIN@NOTE-SOLUTIONS.COM", "AQAAAAIAAYagAAAAEKrs6Ekw7IywN+PxisRW9xcajdE6POAsTCXxXu4cMR4ZOHi1coPin3dVlWsJIvR4mA==", "admin@note-solutions.com" });
        }
    }
}
