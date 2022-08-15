using RepairshopWeb.Data.Entities;
using RepairshopWeb.Models;
using System;

namespace RepairshopWeb.Helpers
{
    public interface IConverterHelper
    {
        Client ToClient(ClientViewModel model, Guid imageId, bool isNew);

        ClientViewModel ToClientViewModel(Client client);
    }
}
