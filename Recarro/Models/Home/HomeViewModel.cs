using Recarro.Models.Vehicles;
using System.Collections.Generic;

namespace Recarro.Models.Home
{
    public class HomeViewModel
    {
        public HomeViewModel()
            => this.Vehicles = new List<ListingViewModel>();

        public List<ListingViewModel> Vehicles { get; set; }

        public int VehiclesLeft { get; set; }
    }
}
