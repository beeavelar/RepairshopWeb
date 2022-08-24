using Microsoft.AspNetCore.Identity;
using RepairshopWeb.Data.Entities;
using RepairshopWeb.Helpers;
using System;
using System.Collections.Generic;
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
            await _context.Database.EnsureCreatedAsync();

            //await _userHelper.CheckRoleAsync("Admin");
            //await _userHelper.CheckRoleAsync("Client");

            //if (!_context.Brands.Any())
            //{
            //    var models = new List<Model>();
            //    models.Add(new Model { Name = "Panda" });
            //    models.Add(new Model { Name = "Tipo" });
            //    models.Add(new Model { Name = "500" });

            //    _context.Brands.Add(new Brand
            //    {
            //        Models = models,
            //        Name = "Fiat"
            //    });

            //    await _context.SaveChangesAsync();
            //}

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
                //await _userHelper.AddUserToRoleAsync(user, "Admin");
                
                //var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                //await _userHelper.ConfirmEmailAsync(user, token);
            }

            //Verifica se o user tem o role que quero verificar --> No caso o "AdmiN"
            //var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");

            //Se o user não tem a permissão Admin, adiciona ele no Admin
            //if (!isInRole)
            //    await _userHelper.AddUserToRoleAsync(user, "Admin");

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
                AddMechanic("João António", "Sanches", 928372829, 29384736, "1928392839", user);
                AddMechanic("Lindovaldo", "Silva", 92837282, 28374837, "1928392839", user);
                AddMechanic("Cleyton", "Sousa", 92937483, 28392837, "1928392839", user);
                await _context.SaveChangesAsync();
            }

            if (!_context.Vehicles.Any())
            {
                AddVehicle("000D111", user);
                AddVehicle("2222G333", user);
                AddVehicle("3333J888", user);
                await _context.SaveChangesAsync();
            }
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
        private void AddMechanic(string firstname, string lastname, int phone, int nif, string ídentitydocument, User user)
        {
            _context.Mechanics.Add(new Mechanic
            {
                FirstName = firstname,
                LastName = lastname,
                Phone = phone,
                Nif = nif,
                IdentityDocument = ídentitydocument,
                User = user
            });
        }

        private void AddVehicle(string licensePlate, User user)
        {
            _context.Vehicles.Add(new Vehicle
            {
                LicensePlate = licensePlate,
                User = user
            });
        }
    }
}
