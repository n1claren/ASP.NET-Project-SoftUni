using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static Recarro.Data.DataConstants;

namespace Recarro.Models.Vehicles
{
    public class CreateVehicleModel
    {
        [Required(ErrorMessage = "Vehicle Make is required!")]
        [StringLength(MakeMaxLength, MinimumLength = MakeMinLength, ErrorMessage = "Vehicle Make should be between {2} and {1}!")]
        public string Make { get; init; }

        [Required(ErrorMessage = "Vehicle Model is required!")]
        [StringLength(ModelMaxLength, MinimumLength = ModelMinLength, ErrorMessage = "Vehicle Model should be between {2} and {1}!")]
        public string Model { get; init; }

        [Required(ErrorMessage = "Vehicle Year is required!")]
        [Range(YearMin, YearMax, ErrorMessage = "Vehicle year should be between {1} and {2}!")]
        public int Year { get; init; }

        [Required(ErrorMessage = "Vehicle Description is required!")]
        public string Description { get; init; }

        [Required(ErrorMessage = "Vehicle Picture is required!")]
        [Display(Name = "Image")]
        [Url]
        public string ImageURL { get; init; }

        [Display(Name = "Price Per Day")]
        [BindRequired]
        public decimal PricePerDay { get; init; }

        public int CategoryId { get; init; }

        public int EngineTypeId { get; init; }

        public IEnumerable<CreateCategoryModel> Categories { get; set; }

        public IEnumerable<CreateEngineTypeModel> EngineTypes { get; set; }
    }
}
