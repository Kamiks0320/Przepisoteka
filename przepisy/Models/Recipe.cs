namespace przepisy.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public Users? User { get; set; }
    }
}
