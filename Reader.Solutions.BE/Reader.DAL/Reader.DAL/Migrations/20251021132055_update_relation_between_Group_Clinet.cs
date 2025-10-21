using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reader.DAL.Migrations
{
    /// <inheritdoc />
    public partial class update_relation_between_Group_Clinet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientGroup");

            migrationBuilder.CreateTable(
                name: "GroupClient",
                columns: table => new
                {
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupClient", x => new { x.GroupId, x.ClientId });
                    table.ForeignKey(
                        name: "FK_GroupClient_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupClient_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFXX0jw6fzOSVDb+3r5Pa29Bl0mks6orfV1bkYGLIoumLaxrrhZC8mmqSSJ8mS6akQ==");

            migrationBuilder.CreateIndex(
                name: "IX_GroupClient_ClientId",
                table: "GroupClient",
                column: "ClientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupClient");

            migrationBuilder.CreateTable(
                name: "ClientGroup",
                columns: table => new
                {
                    ClientsId = table.Column<int>(type: "int", nullable: false),
                    GroupsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientGroup", x => new { x.ClientsId, x.GroupsId });
                    table.ForeignKey(
                        name: "FK_ClientGroup_Clients_ClientsId",
                        column: x => x.ClientsId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientGroup_Groups_GroupsId",
                        column: x => x.GroupsId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMD1ScIHeCmueEegucj4CVYj8u73NPoLIBbYGvwrVsUdG/fv3KXF2GwIzkV38KtXDQ==");

            migrationBuilder.CreateIndex(
                name: "IX_ClientGroup_GroupsId",
                table: "ClientGroup",
                column: "GroupsId");
        }
    }
}
