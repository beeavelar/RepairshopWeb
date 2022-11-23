using RepairshopWeb.Data.Entities;
using System;

namespace RepairshopWeb.Data.Repositories.DataTransferObjects
{
    public class AppointmentDetailsDto
    {
        public DateTime? AppointmentDate { get; set; }

        public string ClientName { get; set; }

        public string Email { get; set; }

        public string VehiclePlate { get; set; }

        public bool IsSuccess { get; set; }
    }
}
