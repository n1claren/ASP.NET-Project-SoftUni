namespace Recarro.Services.Vehicles
{
    public class VehicleServiceFullModel : VehicleServiceModel
    {
        public string Description { get; init; }

        public decimal PricePerDay { get; set; }

        public int CategoryId { get; init; }

        public string CategoryName { get; init; }

        public int EngineTypeId { get; init; }

        public string EngineTypeName { get; init; }

        public int RenterId { get; init; }
    }
}
