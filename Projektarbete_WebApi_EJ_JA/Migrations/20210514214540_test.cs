using Microsoft.EntityFrameworkCore.Migrations;

namespace Projektarbete_WebApi_EJ_JA.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "16c9587f-71b3-4312-9cb7-457bde8bc52d");

            migrationBuilder.CreateTable(
                name: "GeoMessagesV2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeoMessagesV2", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "20c3b1b1-5f1a-4539-889a-9044c8b1bfe8", 0, "6e91203b-b2ac-4bb5-9e71-1b7f8fcb2a7e", null, false, "Emil", "Johannesson", false, null, null, "EMIL", "AQAAAAEAACcQAAAAEL52Yy9/kWhirhTBx2r71tkkKVAjYmHWVYTg3bGZvl2RG0ohtWp/Ddw8m6rrt59xsg==", null, false, "9eaf5344-b81b-497f-b898-d18c85d132eb", false, "Emil" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeoMessagesV2");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "20c3b1b1-5f1a-4539-889a-9044c8b1bfe8");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "16c9587f-71b3-4312-9cb7-457bde8bc52d", 0, "2b62309b-d179-43f2-a9ac-e918fc954445", null, false, "Emil", "Johannesson", false, null, null, "EMIL", "AQAAAAEAACcQAAAAEI4g14aVvl8rYdgTQwEG+EJVHcv9FeZXfRFLtLNPP3XV44O2M6y3AEYlFf6gxewL9w==", null, false, "22806377-2f00-4804-b828-aab8ce097267", false, "Emil" });
        }
    }
}
