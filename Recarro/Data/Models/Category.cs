using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static Recarro.Data.DataConstants;

namespace Recarro.Data.Models
{
    public class Category
    {
        public Category()
        {
            this.Vehicles = new List<Vehicle>();
        }

        [Key]
        public int Id { get; init; }

        [Required]
        public string Name { get; init; }

        public IEnumerable<Vehicle> Vehicles { get; init; }
    }
}
