using Microsoft.AspNetCore.Mvc;
using przepisy.Data;

namespace przepisy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly AppDbContext context;
        public IngredientController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult GetIngredients()
        {
            var ingredients = this.context.Ingredients.ToList();
            return Ok(ingredients);
        }
    }
}
