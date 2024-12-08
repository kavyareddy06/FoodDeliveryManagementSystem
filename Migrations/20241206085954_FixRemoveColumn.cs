using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDeliveryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class FixRemoveColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_Restaurants_RestaurantId",
                table: "Inventories");

            

         

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Restaurants",
                type: "int",
                nullable: true);


            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_UserId",
                table: "Restaurants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_ItemId",
                table: "Inventories",
                column: "ItemId");

    

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_Restaurants_RestaurantId",
                table: "Inventories",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "RestaurantId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Users_OwnerId",
                table: "Restaurants",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Users_UserId",
                table: "Restaurants",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
         

            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_Restaurants_RestaurantId",
                table: "Inventories");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurant_Users_OwnerId",
                table: "Restaurants");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurant_Users_UserId",
                table: "Restaurants");

            migrationBuilder.DropIndex(
                name: "IX_Restaurants_UserId",
                table: "Restaurants");

            migrationBuilder.DropIndex(
                name: "IX_Inventories_ItemId",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Inventories");

            migrationBuilder.AddColumn<string>(
                name: "IngredientName",
                table: "Inventories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_Restaurants_RestaurantId",
                table: "Inventories",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "RestaurantId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Users_OwnerId",
                table: "Restaurants",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
