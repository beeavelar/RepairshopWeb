using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepairshopWeb.Data;
using RepairshopWeb.Data.Entities;
using RepairshopWeb.Data.Repositories;
using RepairshopWeb.Helpers;
using System.Linq;
using System.Threading.Tasks;

namespace RepairshopWeb.Controllers
{
    public class RepairsController : Controller
    {
        private readonly IRepairRepository _repairRepository;
        private readonly IUserHelper _userHelper;

        public RepairsController(IRepairRepository repairRepository, IUserHelper userHelper)
        {
            _repairRepository = repairRepository;
            _userHelper = userHelper;
        }

        // GET: Repairs
        public IActionResult Index()
        {
            return View(_repairRepository.GetAll().OrderBy(r => r.Description));
        }

        // GET: Repairs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return new NotFoundViewResult("RepairNotFound");

            var repair = await _repairRepository.GetByIdAsync(id.Value);

            if (repair == null)
                return new NotFoundViewResult("RepairNotFound");

            return View(repair);
        }

        // GET: Repairs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Repairs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Price,Description,LaborPrice")] Repair repair)
        {
            if (ModelState.IsValid)
            {
                repair.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                await _repairRepository.CreateAsync(repair);
                return RedirectToAction(nameof(Index));
            }
            return View(repair);
        }

        // GET: Repairs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return new NotFoundViewResult("RepairNotFound");

            var repair = await _repairRepository.GetByIdAsync(id.Value);

            if (repair == null)
                return new NotFoundViewResult("RepairNotFound");

            return View(repair);
        }

        // POST: Repairs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Price,Description,LaborPrice")] Repair repair)
        {
            if (id != repair.Id)
                return new NotFoundViewResult("RepairNotFound");

            if (ModelState.IsValid)
            {
                try
                {
                    repair.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                    await _repairRepository.UpdateAsync(repair);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _repairRepository.ExistAsync(repair.Id))
                        return new NotFoundViewResult("RepairNotFound");
                    else

                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(repair);
        }

        // GET: Repairs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return new NotFoundViewResult("RepairNotFound");

            var repair = await _repairRepository.GetByIdAsync(id.Value);

            if (repair == null)
                return new NotFoundViewResult("RepairNotFound");

            return View(repair);
        }

        // POST: Repairs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var repair = await _repairRepository.GetByIdAsync(id);
            await _repairRepository.DeleteAsync(repair);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RepairNotFound()
        {
            return View();
        }
    }
}
