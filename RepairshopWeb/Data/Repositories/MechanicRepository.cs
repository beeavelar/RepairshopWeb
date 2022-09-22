using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RepairshopWeb.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace RepairshopWeb.Data.Repositories
{
    public class MechanicRepository : GenericRepository<Mechanic>, IMechanicRepository
    {
        private readonly DataContext _context;

        public MechanicRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable GetAllWithUsers()
        {
            return _context.Mechanics.Include(m => m.User);
        }

        public IEnumerable<SelectListItem> GetComboMechanics()
        {
            var list = _context.Mechanics.Select(m => new SelectListItem
            {
                Text = m.FullName,
                Value = m.Id.ToString()
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a mechanic...)",
                Value = "0"
            });

            return list;

        }
    }
}
