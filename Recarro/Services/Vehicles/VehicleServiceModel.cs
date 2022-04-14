using System;

namespace Recarro.Services.Vehicles
{
    public class VehicleServiceModel
    {
        public int Id { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public int Year { get; set; }

        public string ImageURL { get; set; }

        public string RentedUntil { get; set; }
    }
}
