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

        [HttpGet]
        public IActionResult GetRecipes()
        {
            var recipes = this.context.Recipes.ToList();
            return Ok(recipes);
        }
    }
}
