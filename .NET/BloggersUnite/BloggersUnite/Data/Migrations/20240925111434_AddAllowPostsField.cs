using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BloggersUnite.Data.Migrations
{
    public partial class AddAllowPostsField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AllowPosts",
                table: "Blogs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowPosts",
                table: "Blogs");
        }
    }
}
