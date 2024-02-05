using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD.Migrations
{
    public partial class updatedbv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationTime",
                table: "UserRefreshToken",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "edc267ec-d43c-4e3b-8108-a1a1f819906d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6d351eec-734e-46f8-b67b-1d56bdbb1cec", "AQAAAAEAACcQAAAAEPaxYRExcxc6tfoWajRWT6wua7PfPHiTUescE0DxfhB85+MCXfcqoR8vANnMd+rRCw==", "d118e794-5490-4779-82f1-ddde1a00b74b" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpirationTime",
                table: "UserRefreshToken");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "edc267ec-d43c-4e3b-8108-a1a1f819906d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a4df7824-498f-4c0f-9aed-61ad00bdf998", "AQAAAAEAACcQAAAAELV3e7K7tofGtjgy6zVHov1SR0aS8HcvOAbtRSYCWCLMzNr0S6cvXWlH3jgaQiMpLQ==", "516cf2a0-1e76-429b-b6c7-fe1a1739b78f" });
        }
    }
}
