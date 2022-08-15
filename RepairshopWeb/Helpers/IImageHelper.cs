﻿using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace RepairshopWeb.Helpers
{
    public interface IImageHelper
    {
        Task<string> UploadImageAsync(IFormFile imageFile, string folder);
    }
}
