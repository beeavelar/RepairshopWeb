﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RepairshopWeb.Data.Repositories;
using RepairshopWeb.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RepairshopWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMechanicRepository _mechanicRepository;

        public HomeController(ILogger<HomeController> logger, IMechanicRepository mechanicRepository)
        {
            _logger = logger;
            _mechanicRepository = mechanicRepository;
        }

        public IActionResult Index()
        {
            return View(_mechanicRepository.GetAll());
        }
        public IActionResult BackOfficeIndex()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
    }
}
