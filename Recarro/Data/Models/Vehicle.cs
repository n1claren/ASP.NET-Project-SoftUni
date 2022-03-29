using System.ComponentModel.DataAnnotations;

using static Recarro.Data.DataConstants;

namespace Recarro.Data.Models
{
    public class Vehicle
    {
        [Key]
        public int Id { get; init; }

        [Required]
        public string Make { get; init; }

        [Required]
        public string Model { get; init; }

        [Required]
        public int Year { get; init; }

        [Required]
        public string ImageURL { get; init; }

        [Required]
        public string Description { get; init; }

        [Required]
        public decimal PricePerDay { get; set; }

        public bool IsAvailable { get; set; }

        [Required]
        public int CategoryId { get; init; }

        public Category Category { get; init; }

        [Required]
        public int EngineTypeId { get; init; }

        public EngineType EngineType { get; init; }
    }
}
