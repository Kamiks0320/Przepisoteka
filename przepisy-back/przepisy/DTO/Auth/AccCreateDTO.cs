using System.ComponentModel.DataAnnotations;

namespace przepisy.DTO.Auth
{
    public class AccCreateDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Nick {  get; set; }
    }
}
