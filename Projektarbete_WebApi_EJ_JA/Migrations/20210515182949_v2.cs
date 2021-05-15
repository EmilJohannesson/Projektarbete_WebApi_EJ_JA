using Microsoft.EntityFrameworkCore.Migrations;

namespace Projektarbete_WebApi_EJ_JA.Migrations
{
    public partial class v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "16c9587f-71b3-4312-9cb7-457bde8bc52d");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "GeoMessages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Body",
                table: "GeoMessages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "GeoMessages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "ce61d5bf-fb74-4684-a4be-b0e44a9863f4", 0, "8f9d0277-6ebe-425e-9ee5-4ec0784fc969", null, false, "Emil", "Johannesson", false, null, null, "EMIL", "AQAAAAEAACcQAAAAEM1vxJocIgIk/oZObrK++c6TPlhlAD1KU+LauD6cdpPKAYSA8gFH6VYagByQ0NN0sw==", null, false, "9d91e9a4-2ff0-47bb-b47c-2b6f2aba7f9b", false, "Emil" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ce61d5bf-fb74-4684-a4be-b0e44a9863f4");

            migrationBuilder.DropColumn(
                name: "Author",
                table: "GeoMessages");

            migrationBuilder.DropColumn(
                name: "Body",
                table: "GeoMessages");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "GeoMessages");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "16c9587f-71b3-4312-9cb7-457bde8bc52d", 0, "2b62309b-d179-43f2-a9ac-e918fc954445", null, false, "Emil", "Johannesson", false, null, null, "EMIL", "AQAAAAEAACcQAAAAEI4g14aVvl8rYdgTQwEG+EJVHcv9FeZXfRFLtLNPP3XV44O2M6y3AEYlFf6gxewL9w==", null, false, "22806377-2f00-4804-b828-aab8ce097267", false, "Emil" });
        }
    }
}
