using Recarro.Models.Vehicles;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Recarro.Services.Vehicles
{
    public class VehicleQueryServiceModel
    {
        public int VehiclesPerPage { get; set; } = 6;

        public int CurrentPage { get; set; } = 1;

        public int TotalCars { get; set; }

        public string Make { get; set; }

        public IEnumerable<string> Makes { get; set; }

        [Display(Name = "Search:")]
        public string SearchTerm { get; set; }

        public VehicleSorting Sorting { get; set; }

        public IEnumerable<VehicleServiceModel> Vehicles { get; set; }
    }
}
