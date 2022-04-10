using Microsoft.AspNetCore.Identity;
using Recarro.Data.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Recarro.Models.Rent
{
    public class CreateRentFormModel
    {
        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; init; }

        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; init; }

        [Required]
        public int VehicleId { get; init; }

        public Vehicle Vehicle { get; init; }

        [Required]
        public string UserId { get; init; }

        public IdentityUser User { get; init; }
    }
}
