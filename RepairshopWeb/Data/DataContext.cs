using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RepairshopWeb.Data.Entities;

namespace RepairshopWeb.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<Client> Clients { get; set; }

        public DbSet<Mechanic> Mechanics { get; set; }

        public DbSet<Receptionist> Receptionists { get; set; }

        public DbSet<Service> Services { get; set; }

        public DbSet<Vehicle> Vehicles { get; set; }

        public DbSet<RepairOrder> RepairOrders { get; set; }

        public DbSet<RepairOrderDetail> RepairOrderDetails { get; set; }

        public DbSet<RepairOrderDetailTemp> RepairOrderDetailsTemp { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

    }
}
