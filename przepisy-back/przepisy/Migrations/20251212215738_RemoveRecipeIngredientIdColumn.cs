using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace przepisy.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRecipeIngredientIdColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IngredientId",
                table: "Recipes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IngredientId",
                table: "Recipes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
