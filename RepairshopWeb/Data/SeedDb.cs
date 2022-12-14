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

            await _userHelper.CheckRoleAsync("ADMIN");
            await _userHelper.CheckRoleAsync("CLIENT");
            await _userHelper.CheckRoleAsync("MECHANIC");
            await _userHelper.CheckRoleAsync("RECEPTIONIST");

            var user = await _userHelper.GetUserByEmailAsync("debora.avelar.21695@formandos.cinel.pt");
            if (user == null)
            {
                user = new User
                {
                    FirstName = "Débora",
                    LastName = "Avelar",
                    Email = "debora.avelar.21695@formandos.cinel.pt",
                    UserName = "debora.avelar.21695@formandos.cinel.pt",
                    Role = "ADMIN"
                };

                var result = await _userHelper.AddUserAsync(user, "123456");
                if (result != IdentityResult.Success)
                    throw new InvalidOperationException("Could not create the user in seeder.");

                //Adicionar o role ao user
                await _userHelper.AddUserToRoleAsync(user, "ADMIN");

                var token = await _userHelper.GenerateConfirmEmailTokenAsync(user);
                await _userHelper.EmailConfirmAsync(user, token);
            }

            //Verifica se o user tem o role que quero verificar --> No caso o "ADMIN"
            var isInRole = await _userHelper.IsUserInRoleAsync(user, "ADMIN");

            //Se o user não tem a permissão Admin, adiciona ele no Admin
            if (!isInRole)
                await _userHelper.AddUserToRoleAsync(user, "ADMIN");

            if (!_context.Clients.Any())
            {
                AddClient("Pedro", "Rodrigues", "pedro@gmail.com", 27383728, user);
                AddClient("Debora", "Avelar", "debs@gmail.com", 27384738, user);
                AddClient("Amélie", "Poulain", "amelie@gmail.com", 26373849, user);
                await _context.SaveChangesAsync();
            }

            if (!_context.Mechanics.Any())
            {
                AddMechanic("João António", "Sanches", 928372829, 29384736, user);
                await _context.SaveChangesAsync();
            }

            if (!_context.Receptionists.Any())
            {
                AddReceptionist("Juliana", "Lopes", 938273829, 273625267, user);
                AddReceptionist("Maria", "Rodrigues", 938283728, 263762662, user);
                await _context.SaveChangesAsync();
            }

            if (!_context.Services.Any())
            {
                AddService("Troca de motor", 600, user);
                AddService("Troca de amortecedores", 100, user);
                AddService("Troca de freios", 100, user);
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
    }
}
