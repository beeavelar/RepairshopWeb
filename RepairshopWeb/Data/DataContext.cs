using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RepairshopWeb.Data.Entities;
using System.Linq;

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

        public DbSet<AppointmentDetail> AppointmentDetails { get; set; }

        public DbSet<AppointmentDetailTemp> AppointmentDetailsTemp { get; set; }

        public DbSet<Billing> Billings { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Billing>().HasOne(x => x.Client).WithMany(x => x.Billings).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Billing>().HasOne(x => x.Vehicle).WithMany(x => x.Billings).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Appointment>().HasOne(x => x.Vehicle).WithMany(x => x.Appointments).OnDelete(DeleteBehavior.NoAction); //O agendamento possui um veiculo com muitos agendamentos
            builder.Entity<AppointmentDetail>().HasOne(x => x.Vehicle).WithMany(x => x.AppointmentDetails).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<AppointmentDetailTemp>().HasOne(x => x.Vehicle).WithOne(x => x.AppointmentDetailTemp).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Appointment>().HasOne(x => x.RepairOrder).WithOne(x => x.Appointment).OnDelete(DeleteBehavior.NoAction);
            base.OnModelCreating(builder);
        }
    }
}
