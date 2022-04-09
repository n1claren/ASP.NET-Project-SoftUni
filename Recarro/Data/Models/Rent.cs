using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Recarro.Data.Models
{
    public class Rent
    {
        private decimal bill;

        [Key]
        public int Id { get; init; }

        [Required]
        public DateTime StartDate { get; init; }

        [Required]
        public DateTime EndDate { get; init; }

        [Required]
        public int VehicleId { get; init; }

        public Vehicle Vehicle { get; init; }

        [Required]
        public string UserId { get; init; }

        public IdentityUser User { get; init; }

        public decimal Bill
        {
            get { return this.bill; }

            private set
            {
                var days = (EndDate - StartDate).TotalDays;
                var price = this.Vehicle.PricePerDay;

                this.bill = (decimal)days * price;
            }
        }
    }
}
