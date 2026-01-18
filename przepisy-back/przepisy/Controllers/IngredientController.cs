using Microsoft.AspNetCore.Authorization;
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

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetIngredients()
        {
            var ingredients = context.Ingredients.ToList();
            
            if(ingredients.Count == 0) return NotFound();

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
            var ingredient = context.Ingredients.FirstOrDefault(i => i.PublicId == IngredientId);

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
