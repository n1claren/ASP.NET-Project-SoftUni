using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static Recarro.Data.DataConstants;

namespace Recarro.Data.Models
{
    public class Renter
    {
        public Renter()
        {
            this.Vehicles = new List<Vehicle>();
        }

        [Key]
        public int Id { get; init; }

        [Required]
        [MaxLength(RenterNameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(RenterPhoneNumberMaxLength)]
        public string PhoneNumber { get; set; }

        [Required]
        public string UserId { get; init; }

        public IdentityUser User { get; init; }

        public IEnumerable<Vehicle> Vehicles { get; init; }
    }
}
