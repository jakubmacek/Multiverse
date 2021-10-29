using System.ComponentModel.DataAnnotations;

namespace Multiverse.WebApp.Models
{
    public class Login
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

        public int? PlayerId { get; set; }
    }
}
