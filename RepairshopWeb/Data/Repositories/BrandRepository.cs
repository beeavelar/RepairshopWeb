using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RepairshopWeb.Data.Entities;
using RepairshopWeb.Data.Repository;
using RepairshopWeb.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepairshopWeb.Data.Repositories
{
    public class BrandRepository //: GenericRepository<Brand>, IBrandRepository
    {
        //private readonly DataContext _context;

        //public BrandRepository(DataContext context) : base(context)
        //{
        //    _context = context;
        //}
        //public async Task AddModelAsync(ModelViewModel model)
        //{
        //    var brand = await this.GetBrandWithModelsAsync(model.BrandId);
        //    if (brand == null)
        //    {
        //        return;
        //    }

        //    brand.Models.Add(new Model { Name = model.Name });
        //    _context.Brands.Update(brand);
        //    await _context.SaveChangesAsync();
        //}

        //public async Task<int> DeleteModelAsync(Model modelBrand)
        //{
        //    var brand = await _context.Brands
        //       .Where(m => m.Models.Any(mo => mo.Id == modelBrand.Id))
        //       .FirstOrDefaultAsync();
        //    if (brand == null)
        //    {
        //        return 0;
        //    }

        //    _context.Models.Remove(modelBrand);
        //    await _context.SaveChangesAsync();
        //    return brand.Id;
        //}

        //public async Task<Brand> GetBrandAsync(Model modelBrand)
        //{
        //    return await _context.Brands
        //         .Where(m => m.Models.Any(mo => mo.Id == modelBrand.Id))
        //         .FirstOrDefaultAsync();
        //}

        //public IQueryable GetBrandsWithModels()
        //{
        //    return _context.Brands
        //        .Include(m => m.Models)
        //        .OrderBy(m => m.Name);
        //}

        //public async Task<Brand> GetBrandWithModelsAsync(int id)
        //{
        //    return await _context.Brands
        //       .Include(m => m.Models)
        //       .Where(m => m.Id == id)
        //       .FirstOrDefaultAsync();
        //}

        //public IEnumerable<SelectListItem> GetComboBrands()
        //{
        //    var list = _context.Brands.Select(b => new SelectListItem
        //    {
        //        Text = b.Name,
        //        Value = b.Id.ToString()

        //    }).OrderBy(l => l.Text).ToList();

        //    list.Insert(0, new SelectListItem
        //    {
        //        Text = "(Select a brand...)",
        //        Value = "0"
        //    });
        //    return list;
        //}

        //public IEnumerable<SelectListItem> GetComboModels(int brandId)
        //{
        //    var brand = _context.Brands.Find(brandId);
        //    var list = new List<SelectListItem>();
        //    if (brand != null)
        //    {
        //        list = _context.Models.Select(m => new SelectListItem
        //        {
        //            Text = m.Name,
        //            Value = m.Id.ToString()

        //        }).OrderBy(l => l.Text).ToList();

        //        list.Insert(0, new SelectListItem
        //        {
        //            Text = "(Select a model...)",
        //            Value = "0"
        //        });
        //    }
        //    return list;
        //}

        //public async Task<Model> GetModelAsync(int id)
        //{
        //    return await _context.Models.FindAsync(id);
        //}

        //public async Task<int> UpdateModelAsync(Model modelBrand)
        //{
        //    var brand = await _context.Brands
        //       .Where(b => b.Models.Any(mo => mo.Id == modelBrand.Id)).FirstOrDefaultAsync();
        //    if (brand == null)
        //    {
        //        return 0;
        //    }

        //    _context.Models.Update(modelBrand);
        //    await _context.SaveChangesAsync();
        //    return brand.Id;
        //}
    }
}
