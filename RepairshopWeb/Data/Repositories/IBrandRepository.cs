using Microsoft.AspNetCore.Mvc.Rendering;
using RepairshopWeb.Data.Entities;
using RepairshopWeb.Data.Repository;
using RepairshopWeb.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepairshopWeb.Data.Repositories
{
    public interface IBrandRepository : IGenericRepository<Brand>
    {
        //IQueryable GetBrandsWithModels();

        //Task<Brand> GetBrandWithModelsAsync(int id);

        //Task<Model> GetModelAsync(int id);

        //Task AddModelAsync(ModelViewModel model);

        //Task<int> UpdateModelAsync(Model modelBrand);

        //Task<int> DeleteModelAsync(Model modelBrand);

        //IEnumerable<SelectListItem> GetComboBrands();

        //IEnumerable<SelectListItem> GetComboModels(int brandId);

        //Task<Brand> GetBrandAsync(Model modelBrand);
    }
}
