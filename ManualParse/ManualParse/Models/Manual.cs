using System.ComponentModel.DataAnnotations;

namespace ManualParse.Models
{
    public class Strings
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Email { get; set; }
    }
}
