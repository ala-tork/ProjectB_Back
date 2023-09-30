using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectB.Migrations
{
    /// <inheritdoc />
    public partial class AddIsVerticalAlignToContainersVariablesType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isVerticalAlign",
                table: "Containers__Variables__Types",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isVerticalAlign",
                table: "Containers__Variables__Types");
        }
    }
}
