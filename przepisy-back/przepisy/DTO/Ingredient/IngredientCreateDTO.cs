using System.ComponentModel.DataAnnotations;

namespace przepisy.DTO.Ingredient
{
    public class IngredientCreateDTO
    {
        [Required]
        [MinLength(2)]
        public string Name { get; set; }
    }
}
