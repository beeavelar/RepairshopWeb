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

        public Mechanic ToMechanic(MechanicViewModel model, Guid imageId, bool isNew)
        {
            return new Mechanic
            {
                Id = isNew ? 0 : model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Address = model.Address,
                PostalCode = model.PostalCode,
                Phone = model.Phone,
                Email = model.Email,
                Nif = model.Nif,
                Niss = model.Niss,
                IdentityDocument = model.IdentityDocument,
                ImageId = imageId,
                User = model.User
            };
        }

        public MechanicViewModel ToMechanicViewModel(Mechanic mechanic)
        {
            return new MechanicViewModel
            {
                Id = mechanic.Id,
                FirstName = mechanic.FirstName,
                LastName = mechanic.LastName,
                Address = mechanic.Address,
                PostalCode = mechanic.PostalCode,
                Phone = mechanic.Phone,
                Email = mechanic.Email,
                Nif = mechanic.Nif,
                Niss = mechanic.Niss,
                IdentityDocument = mechanic.IdentityDocument,
                ImageId = mechanic.ImageId,
                User = mechanic.User
            };
        }

    }
}
