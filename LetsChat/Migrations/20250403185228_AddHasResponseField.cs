using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LetsChat.Migrations
{
    /// <inheritdoc />
    public partial class AddHasResponseField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasResponse",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasResponse",
                table: "Messages");
        }
    }
}
