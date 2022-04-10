﻿namespace Recarro.Services.Users
{
    public interface IUserService
    {
        public bool UserIsRenter(string userId);

        public int GetRenterId(string userId);

        public bool VehicleBelongsToRenter(int renterId, int vehicleId);
    }
}