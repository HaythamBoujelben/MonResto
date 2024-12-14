using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonResto.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class addIdentifier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Identifier",
                table: "Carts",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Identifier",
                table: "Carts");
        }
    }
}
