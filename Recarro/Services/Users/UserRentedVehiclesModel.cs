using System;
using System.ComponentModel.DataAnnotations;

namespace Recarro.Services.Users
{
    public class UserRentedVehiclesModel
    {
        public int Id { get; init; }

        public string Make { get; set; }

        public string Model { get; set; }

        public int Year { get; set; }

        public DateTime RentedFrom { get; set; }

        public DateTime RentedUntil { get; set; }

        public string CurrentUser { get; set; } 
    }
}
