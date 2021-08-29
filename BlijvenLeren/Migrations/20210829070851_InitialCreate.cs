using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlijvenLeren.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LearnResource",
                columns: table => new
                {
                    LearnResourceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Link = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearnResource", x => x.LearnResourceId);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    CommentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommentText = table.Column<string>(nullable: false),
                    CommentDate = table.Column<DateTime>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    LearnResourceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_Comment_LearnResource_LearnResourceId",
                        column: x => x.LearnResourceId,
                        principalTable: "LearnResource",
                        principalColumn: "LearnResourceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comment_LearnResourceId",
                table: "Comment",
                column: "LearnResourceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "LearnResource");
        }
    }
}
