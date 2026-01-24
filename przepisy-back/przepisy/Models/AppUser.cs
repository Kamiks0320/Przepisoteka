using Microsoft.AspNetCore.Identity;

namespace przepisy.Models
{
    public class AppUser : IdentityUser
    {
        public string Nick { get; set; } = "";
        public List<Recipe> Recipes { get; set; } = new();
    }
}
