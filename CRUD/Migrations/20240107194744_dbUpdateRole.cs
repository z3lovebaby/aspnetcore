using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD.Migrations
{
    public partial class dbUpdateRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "28d65a5b-a7db-4850-b380-83591f7d7531", "28d65a5b-a7db-4850-b380-83591f7d7531", "Reader", "READER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9740f16c-24a1-4224-a7be-1bb00b7c6892", "9740f16c-24a1-4224-a7be-1bb00b7c6892", "Writer", "WRITER" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "edc267ec-d43c-4e3b-8108-a1a1f819906d", 0, "a4df7824-498f-4c0f-9aed-61ad00bdf998", "admin@crud.com", false, false, null, "ADMIN@CRUD.COM", "ADMIN@CRUD.COM", "AQAAAAEAACcQAAAAELV3e7K7tofGtjgy6zVHov1SR0aS8HcvOAbtRSYCWCLMzNr0S6cvXWlH3jgaQiMpLQ==", null, false, "516cf2a0-1e76-429b-b6c7-fe1a1739b78f", false, "admin@crud.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "28d65a5b-a7db-4850-b380-83591f7d7531", "edc267ec-d43c-4e3b-8108-a1a1f819906d" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "9740f16c-24a1-4224-a7be-1bb00b7c6892", "edc267ec-d43c-4e3b-8108-a1a1f819906d" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "28d65a5b-a7db-4850-b380-83591f7d7531", "edc267ec-d43c-4e3b-8108-a1a1f819906d" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "9740f16c-24a1-4224-a7be-1bb00b7c6892", "edc267ec-d43c-4e3b-8108-a1a1f819906d" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "28d65a5b-a7db-4850-b380-83591f7d7531");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9740f16c-24a1-4224-a7be-1bb00b7c6892");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "edc267ec-d43c-4e3b-8108-a1a1f819906d");
        }
    }
}
