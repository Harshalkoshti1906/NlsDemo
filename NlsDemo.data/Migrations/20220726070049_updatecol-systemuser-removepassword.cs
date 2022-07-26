using Microsoft.EntityFrameworkCore.Migrations;

namespace NlsDemo.data.Migrations
{
    public partial class updatecolsystemuserremovepassword : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "SystemUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "SystemUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
