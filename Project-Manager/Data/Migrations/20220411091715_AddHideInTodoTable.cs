using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_Manager.Data.Migrations
{
    public partial class AddHideInTodoTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Hide",
                table: "Todos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hide",
                table: "Todos");
        }
    }
}
