namespace przepisy.DTO.Recipe
{
    public class RecipeReadDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Ingredients { get; set; }
    }
}
