using System.ComponentModel.DataAnnotations;

namespace przepisy.DTO.Recipe
{
    public class RecipeCreateDTO
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        [MinLength(3)]
        public string Description { get; set; }

        [Required]
        [MinLength(1)]
        public List<string> IngredientNames { get; set; }
    }
}
