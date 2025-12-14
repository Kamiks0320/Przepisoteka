using Microsoft.AspNetCore.Mvc;
using przepisy.Data;

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

        [HttpGet("GetAll")]
        public IActionResult GetRecipes()
        {
            try
            {
                var recipes = this.context.Recipes.ToList();
                return Ok(recipes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /*[HttpGet("GetById/{RecipeId: Guid}")]
        public IActionResult GetRecipeById(Guid RecipeId)
        {
            var recipe = ;
            return Ok(recipe);
        }*/
    }
}
