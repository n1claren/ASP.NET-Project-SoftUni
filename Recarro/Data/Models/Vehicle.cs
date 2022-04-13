using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static Recarro.Data.DataConstants;

namespace Recarro.Data.Models
{
    public class Vehicle
    {
        public Vehicle()
        {
            this.Rents = new List<Rent>();
        }

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

        public string CurrentUser { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        [Required]
        public int EngineTypeId { get; set; }

        public EngineType EngineType { get; set; }

        public int RenterId { get; set; }

        public Renter Renter { get; set; }

        public IEnumerable<Rent> Rents { get; set; }
    }
}
