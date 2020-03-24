using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CarDealershipApp.DAL.Migrations
{
    public partial class orderChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("e336f43c-a45e-4a89-92b5-56e2fb503af9"));

            migrationBuilder.AddColumn<decimal>(
                name: "TotalCost",
                table: "Orders",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalSellingPrice",
                table: "Orders",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[] { new Guid("22b98769-1d57-4af2-aca5-ea9e03e8723f"), "ZA", "South Africa" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("22b98769-1d57-4af2-aca5-ea9e03e8723f"));

            migrationBuilder.DropColumn(
                name: "TotalCost",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TotalSellingPrice",
                table: "Orders");

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[] { new Guid("e336f43c-a45e-4a89-92b5-56e2fb503af9"), "ZA", "South Africa" });
        }
    }
}
