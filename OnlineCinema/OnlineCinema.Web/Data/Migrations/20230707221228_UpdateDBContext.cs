using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineCinema.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDBContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketsInShoppingCart_ShoppingCarts_TicketId",
                table: "TicketsInShoppingCart");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketsInShoppingCart_Tickets_ShoppingCartId",
                table: "TicketsInShoppingCart");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketsInShoppingCart_ShoppingCarts_ShoppingCartId",
                table: "TicketsInShoppingCart",
                column: "ShoppingCartId",
                principalTable: "ShoppingCarts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketsInShoppingCart_Tickets_TicketId",
                table: "TicketsInShoppingCart",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketsInShoppingCart_ShoppingCarts_ShoppingCartId",
                table: "TicketsInShoppingCart");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketsInShoppingCart_Tickets_TicketId",
                table: "TicketsInShoppingCart");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketsInShoppingCart_ShoppingCarts_TicketId",
                table: "TicketsInShoppingCart",
                column: "TicketId",
                principalTable: "ShoppingCarts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketsInShoppingCart_Tickets_ShoppingCartId",
                table: "TicketsInShoppingCart",
                column: "ShoppingCartId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
