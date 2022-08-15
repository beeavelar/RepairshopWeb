using RepairshopWeb.Data.Entities;
using RepairshopWeb.Models;
using System;

namespace RepairshopWeb.Helpers
{
    public class ConverterHelper : IConverterHelper

    {
        public Client ToClient(ClientViewModel model, Guid imageId, bool isNew)
        {
            return new Client
            {
                Id = isNew ? 0 : model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Address = model.Address,
                PostalCode = model.PostalCode,
                Phone = model.Phone,
                Email = model.Email,
                Nif = model.Nif,
                ImageId = imageId,
                User = model.User
            };
        }

        public ClientViewModel ToClientViewModel(Client client)
        {
            return new ClientViewModel
            {
                Id = client.Id,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Address = client.Address,
                PostalCode = client.PostalCode,
                Phone = client.Phone,
                Email = client.Email,
                Nif = client.Nif,
                ImageId = client.ImageId,
                User = client.User
            };
        }
    }
}
