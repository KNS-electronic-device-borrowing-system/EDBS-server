using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EDBS_server.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDepartmentColumnFromUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "department",
                table: "users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "department",
                table: "users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
