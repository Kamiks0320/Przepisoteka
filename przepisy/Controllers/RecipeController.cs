using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using przepisy.Data;
using przepisy.DTO.Recipe;

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

        /*[HttpPost]
        public IActionResult CreateRecipe(RecipeCreateDTO dto)
        {

        }*/
    }
}
