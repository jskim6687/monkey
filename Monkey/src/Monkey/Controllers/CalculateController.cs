using System;
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
        [HttpGet]
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

        [HttpPost]
        public IActionResult Orbit(string baseFile, string roverFile)
        {
            var baseRoot = Path.Combine(_environment.WebRootPath, "base\\", baseFile);
            var roverRoot = Path.Combine(_environment.WebRootPath, "rover\\", roverFile);

            var baseStream = new FileStream(baseRoot, FileMode.Open);
            var roverStream = new FileStream(roverRoot, FileMode.Open);
            
            //여기서 부터 위성 궤도 올리고 보정신호 처리

            return View();
        }

    }
}
