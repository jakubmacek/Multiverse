using System.ComponentModel.DataAnnotations;

namespace Multiverse.WebApp.Models
{
    public class Login
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public int? PlayerId { get; set; }
    }
}
