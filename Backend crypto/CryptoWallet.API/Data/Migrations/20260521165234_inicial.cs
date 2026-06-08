using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoWallet.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cryptos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cryptos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "transactionActions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transactionActions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CryptoId = table.Column<int>(type: "int", nullable: false),
                    TransactionActionId = table.Column<int>(type: "int", nullable: false),
                    CryptoAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Money = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_transactions_cryptos_CryptoId",
                        column: x => x.CryptoId,
                        principalTable: "cryptos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_transactions_transactionActions_TransactionActionId",
                        column: x => x.TransactionActionId,
                        principalTable: "transactionActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_transactions_CryptoId",
                table: "transactions",
                column: "CryptoId");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_TransactionActionId",
                table: "transactions",
                column: "TransactionActionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "transactions");

            migrationBuilder.DropTable(
                name: "cryptos");

            migrationBuilder.DropTable(
                name: "transactionActions");
        }
    }
}
