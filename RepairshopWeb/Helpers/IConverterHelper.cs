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

        Receptionist ToReceptionist(ReceptionistViewModel model, Guid imageId, bool isNew);

        ReceptionistViewModel ToReceptionistViewModel(Receptionist receptionist);

        User ToUser(RegisterNewUserViewModel model, Guid imageId, bool isNew);

        RegisterNewUserViewModel ToUserViewModel(User user);

    }
}
