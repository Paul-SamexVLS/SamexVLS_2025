using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SamexVLS_2025.Data.Migrations
{
    /// <inheritdoc />
    public partial class initialDBcreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Quotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Customer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SalesRep = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsEstimate = table.Column<bool>(type: "bit", nullable: false),
                    IsPrequote = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quotes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuotesDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Class = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Profile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BillingContainer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectionContainer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContainerDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Treatment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Destination = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransportationCost = table.Column<float>(type: "real", nullable: true),
                    Overcharges = table.Column<float>(type: "real", nullable: true),
                    Minimum = table.Column<float>(type: "real", nullable: true),
                    Charge = table.Column<bool>(type: "bit", nullable: false),
                    Maxweight = table.Column<float>(type: "real", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShowInQuote = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuotesDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuotesDetails_Quotes_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Quotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuotesDetails_ParentId",
                table: "QuotesDetails",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuotesDetails");

            migrationBuilder.DropTable(
                name: "Quotes");
        }
    }
}
