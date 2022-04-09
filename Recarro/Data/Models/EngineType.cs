using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static Recarro.Data.DataConstants;

namespace Recarro.Data.Models
{
    public class EngineType
    {
        public EngineType()
        {
            this.Vehicles = new List<Vehicle>();
        }

        [Key]
        public int Id { get; init; }

        [Required]
        public string Type { get; init; }

        public IEnumerable<Vehicle> Vehicles { get; init; }
    }
}
