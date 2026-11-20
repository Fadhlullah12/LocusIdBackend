using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocusIDBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddDirector : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Directors",
                keyColumn: "Id",
                keyValue: "d145f196-0896-41b7-80b3-e71d40bfffc6");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "507b91a1-8783-4a3a-9402-d3ebd09eb8e3");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FirstName", "IsDeleted", "LastName", "Password", "PhotoUrl", "Role" },
                values: new object[] { "9624ee9b-771e-400f-bf31-1fae4c5aee2c", new DateTime(2026, 11, 19, 23, 57, 25, 581, DateTimeKind.Local).AddTicks(7116), "director@unilorin.edu.ng", "Balogun", false, "Fadhlullah", "$2a$11$8Sw1ZfhzMCw0oOoI4WD0y.TCXPCsNRO5.K6UEs05B4DPd6/hWFBNK", null, "Director" });

            migrationBuilder.InsertData(
                table: "Directors",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "StaffId", "UserId" },
                values: new object[] { "5ca29ae5-9bba-4c20-adc5-b38bb72572b9", new DateTime(2026, 11, 19, 22, 57, 25, 584, DateTimeKind.Utc).AddTicks(1110), false, "DIR001", "9624ee9b-771e-400f-bf31-1fae4c5aee2c" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Directors",
                keyColumn: "Id",
                keyValue: "5ca29ae5-9bba-4c20-adc5-b38bb72572b9");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "9624ee9b-771e-400f-bf31-1fae4c5aee2c");

            migrationBuilder.InsertData(
                table: "Directors",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "StaffId", "UserId" },
                values: new object[] { "d145f196-0896-41b7-80b3-e71d40bfffc6", new DateTime(2026, 11, 19, 22, 51, 30, 415, DateTimeKind.Utc).AddTicks(4887), false, "DIR001", "424fc615-ff10-4b4d-9e0b-5819b0168ba2" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FirstName", "IsDeleted", "LastName", "Password", "PhotoUrl", "Role" },
                values: new object[] { "507b91a1-8783-4a3a-9402-d3ebd09eb8e3", new DateTime(2026, 11, 19, 23, 51, 30, 409, DateTimeKind.Local).AddTicks(6565), "director@unilorin.edu.ng", "Balogun", false, "Fadhlullah", "$2a$11$2vl1FiNasns3bUzpqjWUJuUWwVUiwB6VFH2Lxf.9x1oIw/sxcyB6C", null, "Director" });
        }
    }
}
