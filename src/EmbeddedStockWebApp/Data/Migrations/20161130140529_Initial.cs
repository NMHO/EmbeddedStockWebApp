using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EmbeddedStockWebApp.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "ESImage",
                columns: table => new
                {
                    ESImageId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ImageData = table.Column<byte[]>(nullable: true),
                    ImageMimeType = table.Column<string>(maxLength: 128, nullable: true),
                    Thumbnail = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESImage", x => x.ESImageId);
                });

            migrationBuilder.CreateTable(
                name: "ComponentType",
                columns: table => new
                {
                    ComponentTypeId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AdminComment = table.Column<string>(nullable: true),
                    CategoryId = table.Column<int>(nullable: true),
                    ComponentInfo = table.Column<string>(nullable: true),
                    ComponentName = table.Column<string>(nullable: true),
                    Datasheet = table.Column<string>(nullable: true),
                    ImageESImageId = table.Column<long>(nullable: true),
                    ImageUrl = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    Manufacturer = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    WikiLink = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentType", x => x.ComponentTypeId);
                    table.ForeignKey(
                        name: "FK_ComponentType_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ComponentType_ESImage_ImageESImageId",
                        column: x => x.ImageESImageId,
                        principalTable: "ESImage",
                        principalColumn: "ESImageId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Component",
                columns: table => new
                {
                    ComponentId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AdminComment = table.Column<string>(nullable: true),
                    ComponentNumber = table.Column<int>(nullable: false),
                    ComponentTypeId = table.Column<long>(nullable: false),
                    CurrentLoanInformationId = table.Column<long>(nullable: true),
                    SerialNo = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    UserComment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Component", x => x.ComponentId);
                    table.ForeignKey(
                        name: "FK_Component_ComponentType_ComponentTypeId",
                        column: x => x.ComponentTypeId,
                        principalTable: "ComponentType",
                        principalColumn: "ComponentTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Component_ComponentTypeId",
                table: "Component",
                column: "ComponentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentType_CategoryId",
                table: "ComponentType",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentType_ImageESImageId",
                table: "ComponentType",
                column: "ImageESImageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Component");

            migrationBuilder.DropTable(
                name: "ComponentType");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "ESImage");
        }
    }
}
