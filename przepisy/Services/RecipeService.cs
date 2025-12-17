using przepisy.Data;
using Microsoft.EntityFrameworkCore;
using przepisy.Models;

namespace przepisy.Services
{
    public class RecipeService
    {
        private readonly AppDbContext context;
        public RecipeService(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Recipe> CreateRecipeAsync(string name, string description, List<string> ingredientNames)
        {
            //normalizacja nazw skladnikow
            var normalizedNames = ingredientNames.Select(n => n.Trim().ToLowerInvariant()).Where(n => n != "").Distinct().ToList();

            //pobranie istniejacych skladnikow
            var existingIngredients = await context.Ingredient.Where(i => normalizedNames.Contains(i.Name)).ToListAsync();

            //sprawdzenie, ktore skladniki juz istnieja
            var existingNames = existingIngredients.Select(i => i.Name).ToHashSet();

            //stworzenie nowych skladnikow
            var newIngredients = normalizedNames.Where(n => !existingNames.Contains(n)).Select(n => new Ingredient
            {
                Name = n
            }).ToList();

            //dodanie nowych skladnikow do kontekstu
            context.Ingredient.AddRange(newIngredients);

            //stworzenie nowego przepisu
            var recipe = new Recipe
            {
                PublicId = Guid.NewGuid(),
                Name = name.Trim(),
                Description = description,
                Ingredients = existingIngredients.Concat(newIngredients).ToList()
            };

            //dodanie przepisu do kontekstu
            context.Recipes.Add(recipe);

            //zapis
            await context.SaveChangesAsync();

            return recipe;
        }
    }
}
