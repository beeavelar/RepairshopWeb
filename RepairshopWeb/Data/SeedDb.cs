using Microsoft.AspNetCore.Identity;
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
            await _context.Database.EnsureCreatedAsync();

            //await _userHelper.CheckRoleAsync("Admin");
            //await _userHelper.CheckRoleAsync("Owner");

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
            }

            //Verifica se o user tem o role que quero verificar --> No caso o "AdmiN"
            //var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");

            //Se o user não tem a permissão Admin, adiciona ele no Admin
            //if (!isInRole)
            //    await _userHelper.AddUserToRoleAsync(user, "Admin");

            if (!_context.Clients.Any()) 
            {
                AddClient("Miguel", "Mendes", "miguel@gmail.com", user);
                AddClient("Pedro", "Rodrigues", "pedro@gmail.com", user);
                AddClient("Karen", "Borges", "karen@gmail.com", user);
                AddClient("Maria", "Lopes", "maria@gmail.com", user);
                AddClient("Renata", "Fernandes", "renata@gmail.com", user);
                AddClient("Amélie", "Poulain", "amelie@gmail.com", user);
                await _context.SaveChangesAsync();
            }
        }
        private void AddClient(string firstname, string lastname, string email, User user)
        {
            _context.Clients.Add(new Client
            {
                FirstName = firstname,
                LastName = lastname,
                Email = email,
                User = user
            });
        }

    }
}
