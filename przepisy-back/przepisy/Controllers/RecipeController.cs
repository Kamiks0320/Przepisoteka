using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using przepisy.Data;
using przepisy.DTO.Recipe;
using przepisy.Models;
using System.Security.Claims;

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

        private bool WhoCanModify(Recipe recipe)
        {
            if (User.IsInRole("Administrator") || User.IsInRole("Moderator")) return true;

            return recipe.OwnerId == User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetRecipes()
        {
            var recipes = context.Recipes.Include(r => r.Ingredients).ToList();

            if(recipes.Count == 0) return NotFound();

            var dtos = recipes.Select(recipe => new RecipeReadDTO
            {
                Id = recipe.PublicId,
                Name = recipe.Name,
                Description = recipe.Description,
                Ingredients = recipe.Ingredients.Select(i => i.Name).ToList()
            }).ToList();

            return Ok(dtos);
        }

        [AllowAnonymous]
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

        [HttpGet("mine")]
        public async Task<IActionResult> GetMine()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(userId == null) return Unauthorized();

            var recipes = await context.Recipes.Where(r => r.OwnerId == userId).ToListAsync();

            return Ok(recipes);
        }

        [Authorize]
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
            var existingIngredients = await context.Ingredients.Where(i => normalizedNames.Contains(i.Name)).ToListAsync();

            //sprawdzenie, ktore skladniki juz istnieja
            var existingNames = existingIngredients.Select(i => i.Name).ToHashSet();

            //stworzenie nowych skladnikow
            var newIngredients = normalizedNames.Where(n => !existingNames.Contains(n)).Select(n => new Ingredient
            {
                PublicId = Guid.NewGuid(),
                Name = n
            }).ToList();

            //dodanie nowych skladnikow do kontekstu
            context.Ingredients.AddRange(newIngredients);

            //uzytkownik wlascicielem
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            //stworzenie nowego przepisu
            var recipe = new Recipe
            {
                PublicId = Guid.NewGuid(),
                Name = dto.Name.Trim(),
                Description = dto.Description,
                Ingredients = existingIngredients.Concat(newIngredients).ToList(),
                OwnerId = userId
            };

            //dodanie przepisu do kontekstu
            context.Recipes.Add(recipe);

            //zapis
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRecipeById), 
                new { RecipeId = recipe.PublicId }, 
                null);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecipe(Guid id, [FromBody] RecipeCreateDTO dto)
        {
            var recipe = await context.Recipes.Include(r => r.Ingredients).FirstOrDefaultAsync(r => r.PublicId == id);

            if (recipe == null) return NotFound();
            if (!WhoCanModify(recipe)) return Forbid();

            //walidacja nazwy przepisu
            var recipeName = dto.Name.Trim();
            if (string.IsNullOrWhiteSpace(recipeName)) return BadRequest("Recipe name cannot be empty.");

            //normalizacja nazw skladnikow i walidacja
            var normalizedNames = dto.IngredientNames.Select(n => n.Trim().ToLowerInvariant()).Where(n => n != "").Distinct().ToList();
            if (normalizedNames.Count == 0) return BadRequest("Recipe must contain at least one ingredient.");

            //pobranie istniejacych skladnikow
            var existingIngredients = await context.Ingredients.Where(i => normalizedNames.Contains(i.Name)).ToListAsync();

            //sprawdzenie, ktore skladniki juz istnieja
            var existingNames = existingIngredients.Select(i => i.Name).ToHashSet();

            //stworzenie nowych skladnikow
            var newIngredients = normalizedNames.Where(n => !existingNames.Contains(n)).Select(n => new Ingredient
            {
                PublicId = Guid.NewGuid(),
                Name = n
            }).ToList();

            recipe.Name = recipeName;
            recipe.Description = dto.Description;
            recipe.Ingredients = existingIngredients.Concat(newIngredients).ToList();

            await context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(Guid id)
        {
            var recipe = await context.Recipes.Include(r => r.Ingredients).FirstOrDefaultAsync(i => i.PublicId == id);

            if (recipe == null) return NotFound();
            if (!WhoCanModify(recipe)) return Forbid();

            context.Recipes.Remove(recipe);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
