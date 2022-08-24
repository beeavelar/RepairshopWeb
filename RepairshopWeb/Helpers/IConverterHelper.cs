using RepairshopWeb.Data.Entities;
using RepairshopWeb.Models;
using System;

namespace RepairshopWeb.Helpers
{
    public interface IConverterHelper
    {
        Client ToClient(ClientViewModel model, Guid imageId, bool isNew);

        ClientViewModel ToClientViewModel(Client client);

        Mechanic ToMechanic(MechanicViewModel model, Guid imageId, bool isNew);

        MechanicViewModel ToMechanicViewModel(Mechanic mechanic);

        Vehicle ToVehicle(VehicleViewModel model, Guid imageId, bool isNew);

        VehicleViewModel ToVehicleViewModel(Vehicle vehicle);
    }
}
