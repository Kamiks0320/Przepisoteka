using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using przepisy.Data;
using przepisy.DTO.Ingredient;

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
            var ingredients = context.Ingredient.ToList();
            
            if(ingredients.Count == 0) return NotFound(); //pewnie cos innego niz notfound, ale pozniej to zmienie

            var dtos = ingredients.Select(ingredient => new IngredientReadDTO
            {
                Id = ingredient.PublicId,
                Name = ingredient.Name
            }).ToList();

            return Ok(dtos);
        }

        [HttpGet("{IngredientId}")]
        public IActionResult GetIngredientsById(Guid IngredientId)
        {
            var ingredient = context.Ingredient.FirstOrDefault(i => i.PublicId == IngredientId);

            if (ingredient == null) return NotFound();

            var dto = new IngredientReadDTO
            {
                Id = ingredient.PublicId,
                Name = ingredient.Name
            };

            return Ok(dto);
        }
    }
}
