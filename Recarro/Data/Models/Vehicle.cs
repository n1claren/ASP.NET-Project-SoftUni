using System.ComponentModel.DataAnnotations;

using static Recarro.Data.DataConstants;

namespace Recarro.Data.Models
{
    public class Vehicle
    {
        [Key]
        public int Id { get; init; }

        [Required]
        public string Make { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public string ImageURL { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal PricePerDay { get; set; }

        public bool IsAvailable { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public Category Category { get; init; }

        [Required]
        public int EngineTypeId { get; set; }

        public EngineType EngineType { get; init; }

        public int RenterId { get; set; }

        public Renter Renter { get; init; }
    }
}
