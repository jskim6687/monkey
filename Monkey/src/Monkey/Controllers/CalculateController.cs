﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Monkey.Controllers
{
    public class CalculateController : Controller
    {
        private IHostingEnvironment _environment;

        public CalculateController(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        // GET: /<controller>/
        public IActionResult Select()
        {
            var baseRoot = Path.Combine(_environment.WebRootPath, "base");
            var roverRoot = Path.Combine(_environment.WebRootPath, "rover");

            if (System.IO.Directory.Exists(baseRoot))
            {
                var baseList = new DirectoryInfo(baseRoot).GetFiles();
                ViewData["baseLIst"] = baseList;
            }

            if (System.IO.Directory.Exists(roverRoot))
            {
                var roverList = new DirectoryInfo(roverRoot).GetFiles();
                ViewData["roverLIst"] = roverList;
            }

            return View();
        }
    }
}
