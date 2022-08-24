using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RepairshopWeb.Data.Entities;

namespace RepairshopWeb.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<Client> Clients { get; set; }

        public DbSet<Mechanic> Mechanics { get; set; }

        //public DbSet<MechanicSpeciality> MechanicSpecialities { get; set; }

        public DbSet<Vehicle> Vehicles { get; set; }

        //public DbSet<Brand> Brands { get; set; }

        //public DbSet<Model> Models { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

    }
}
