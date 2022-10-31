using System.ComponentModel.DataAnnotations;

namespace PetApi.Models
{
    public class PetPriceChangeDto
    {
        [Required]
        public double Price { get; set; }
    }
}
