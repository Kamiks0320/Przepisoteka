namespace przepisy.DTO.Recipe
{
    public class RecipeCreateDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Guid> IngredientIds { get; set; }
    }
}
