using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RepairshopWeb.Data.Entities;
using RepairshopWeb.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RepairshopWeb.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync(); //Quando corre o seed corre tbm as migrações automaticamente

            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Client");
            await _userHelper.CheckRoleAsync("Mechanic");
            await _userHelper.CheckRoleAsync("Receptionist");

            var user = await _userHelper.GetUserByEmailAsync("debora.avelar.21695@formandos.cinel.pt");
            if (user == null)
            {
                user = new User
                {
                    FirstName = "Débora",
                    LastName = "Avelar",
                    Email = "debora.avelar.21695@formandos.cinel.pt",
                    UserName = "debora.avelar.21695@formandos.cinel.pt",
                };

                var result = await _userHelper.AddUserAsync(user, "123456");
                if (result != IdentityResult.Success)
                    throw new InvalidOperationException("Could not create the user in seeder.");

                //Adicionar o role ao user
                await _userHelper.AddUserToRoleAsync(user, "Admin");

                //var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                //await _userHelper.ConfirmEmailAsync(user, token);
            }

            //Verifica se o user tem o role que quero verificar --> No caso o "AdmiN"
            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");

            //Se o user não tem a permissão Admin, adiciona ele no Admin
            if (!isInRole)
                await _userHelper.AddUserToRoleAsync(user, "Admin");

            if (!_context.Clients.Any())
            {
                AddClient("Miguel", "Mendes", "miguel@gmail.com", 28373827, user);
                AddClient("Pedro", "Rodrigues", "pedro@gmail.com", 27383728, user);
                AddClient("Karen", "Borges", "karen@gmail.com", 27384738, user);
                AddClient("Maria", "Lopes", "maria@gmail.com", 26374892, user);
                AddClient("Renata", "Fernandes", "renata@gmail.com", 20394839, user);
                AddClient("Amélie", "Poulain", "amelie@gmail.com", 26373849, user);
                await _context.SaveChangesAsync();
            }

            if (!_context.Mechanics.Any())
            {
                AddMechanic("João António", "Sanches", 928372829, 29384736, user);
                AddMechanic("Lindovaldo", "Silva", 92837282, 28374837, user);
                AddMechanic("Cleyton", "Sousa", 92937483, 28392837, user);
                await _context.SaveChangesAsync();
            }

            if (!_context.Receptionists.Any())
            {
                AddReceptionist("Juliana", "Lopes", 938273829, 273625267, user);
                AddReceptionist("Gabriela", "Brito", 938283728, 263762662, user);
                AddReceptionist("Gustavo", "Almeida", 938273827, 282736222, user);
                await _context.SaveChangesAsync();
            }

            if (!_context.Services.Any())
            {
                AddService("Troca de motor", 600, user);
                AddService("Troca de amortecedores", 100, user);
                AddService("Troca de freios", 100, user);
                await _context.SaveChangesAsync();
            }

            //if (!_context.Vehicles.Any())
            //{
            //    AddVehicle("11-AA-11", user);
            //    AddVehicle("22-BB-22", user);
            //    AddVehicle("33-CC-33", user);
            //    await _context.SaveChangesAsync();
            //}

        }
        private void AddClient(string firstname, string lastname, string email, int nif, User user)
        {
            _context.Clients.Add(new Client
            {
                FirstName = firstname,
                LastName = lastname,
                Email = email,
                Nif = nif,
                User = user
            });
        }
        private void AddMechanic(string firstname, string lastname, int phone, int nif, User user)
        {
            _context.Mechanics.Add(new Mechanic
            {
                FirstName = firstname,
                LastName = lastname,
                Phone = phone,
                Nif = nif,
                User = user
            });
        }

        private void AddReceptionist(string firstname, string lastname, int phone, int nif, User user)
        {
            _context.Receptionists.Add(new Receptionist
            {
                FirstName = firstname,
                LastName = lastname,
                Phone = phone,
                Nif = nif,
                User = user
            });
        }

        private void AddService(string description, decimal repairPrice, User user)
        {
            _context.Services.Add(new Service
            {
                Description = description,
                RepairPrice = repairPrice,
                User = user
            });
        }

        //private void AddVehicle(string licensePlate, User user)
        //{
        //    _context.Vehicles.Add(new Vehicle
        //    {
        //        LicensePlate = licensePlate,
        //        User = user
        //    });
        //}
    }
}
