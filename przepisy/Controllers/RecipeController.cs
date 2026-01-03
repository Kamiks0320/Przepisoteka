using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using przepisy.Data;
using przepisy.DTO.Recipe;
using przepisy.Models;

namespace przepisy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly AppDbContext context;
        public RecipeController(AppDbContext context)
        {
            this.context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetRecipes()
        {
            var recipes = context.Recipes.Include(r => r.Ingredients).ToList();

            if(recipes.Count == 0) return NotFound(); //pewnie cos innego niz notfound, ale pozniej to zmienie

            var dtos = recipes.Select(recipe => new RecipeReadDTO
            {
                Id = recipe.PublicId,
                Name = recipe.Name,
                Description = recipe.Description,
                Ingredients = recipe.Ingredients.Select(i => i.Name).ToList()
            }).ToList();

            return Ok(dtos);
        }

        [HttpGet("{RecipeId}")]
        public IActionResult GetRecipeById(Guid RecipeId)
        {
            var recipe = context.Recipes.Include(r => r.Ingredients).FirstOrDefault(r => r.PublicId == RecipeId);

            if(recipe == null) return NotFound();

            var dto = new RecipeReadDTO
            {
                Id = recipe.PublicId,
                Name = recipe.Name,
                Description = recipe.Description,
                Ingredients = recipe.Ingredients.Select(i => i.Name).ToList()
            };

            return Ok(dto);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateRecipe([FromBody] RecipeCreateDTO dto)
        {
            //walidacja nazwy przepisu
            var recipeName = dto.Name.Trim();
            if (string.IsNullOrWhiteSpace(recipeName)) return BadRequest("Recipe name cannot be empty.");

            //normalizacja nazw skladnikow i walidacja
            var normalizedNames = dto.IngredientNames.Select(n => n.Trim().ToLowerInvariant()).Where(n => n != "").Distinct().ToList();
            if (normalizedNames.Count == 0) return BadRequest("Recipe must contain at least one ingredient.");

            //pobranie istniejacych skladnikow
            var existingIngredients = await context.Ingredient.Where(i => normalizedNames.Contains(i.Name)).ToListAsync();

            //sprawdzenie, ktore skladniki juz istnieja
            var existingNames = existingIngredients.Select(i => i.Name).ToHashSet();

            //stworzenie nowych skladnikow
            var newIngredients = normalizedNames.Where(n => !existingNames.Contains(n)).Select(n => new Ingredient
            {
                PublicId = Guid.NewGuid(),
                Name = n
            }).ToList();

            //dodanie nowych skladnikow do kontekstu
            context.Ingredient.AddRange(newIngredients);

            //stworzenie nowego przepisu
            var recipe = new Recipe
            {
                PublicId = Guid.NewGuid(),
                Name = dto.Name.Trim(),
                Description = dto.Description,
                Ingredients = existingIngredients.Concat(newIngredients).ToList()
            };

            //dodanie przepisu do kontekstu
            context.Recipes.Add(recipe);

            //zapis
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRecipeById), 
                new { RecipeId = recipe.PublicId }, 
                null);
        }
    }
}
