using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Recarro.Models.Vehicles
{
    public class SearchQueryModel
    {
        public const int VehiclesPerPage = 6;

        public int CurrentPage { get; set; } = 1;

        public int TotalCars { get; set; }

        public string Make { get; set; }

        public IEnumerable<string> Makes { get; set; }

        [Display(Name = "Search:")]
        public string SearchTerm { get; set; }

        public VehicleSorting Sorting { get; set; }

        public IEnumerable<ListingViewModel> Vehicles { get; set; }
    }
}
