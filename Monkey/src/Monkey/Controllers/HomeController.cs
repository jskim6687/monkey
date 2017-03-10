using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Monkey.Controllers;
using Monkey.Models;

namespace Monkey.Controllers
{
    public class HomeController : Controller
    {
        public string oFileName;

        public string nFileName;

        private IHostingEnvironment _environment;

        public HomeController(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(ICollection<IFormFile> files)
        {
            var uploads = Path.Combine(_environment.WebRootPath, "uploads");
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                        var fileName = file.FileName; 
                        if (fileName.EndsWith("o"))
                        {
                            oFileName = fileName;
/*                            new Ofiles
                            {
                                name = fileName,
                                stationName = fileName.Substring(0,4),
                                gpsday = int.Parse(fileName.Substring(4,3)),
                                order = int.Parse(fileName.Substring(7,1)),
                                year = int.Parse(fileName.Substring(9,2))
                            };*/
                        }
                        else if (fileName.EndsWith("n"))
                        {
                            nFileName = file.FileName;
/*                            new Nfiles
                            {
                                name = fileName,
                                stationName = fileName.Substring(0, 4),
                                gpsday = int.Parse(fileName.Substring(4, 3)),
                                order = int.Parse(fileName.Substring(7, 1)),
                                year = int.Parse(fileName.Substring(9, 2))
                            };*/
                        }
                        else
                        {

                        }
//                        await file.CopyToAsync(fileStream);
                    }
                }
            }
            if (files.Count > 0)
            {
                return RedirectToAction("Info","Upload", new {nFileName = nFileName, oFileName = oFileName /*uploadFiles = files*/});
            }
            else
            {
                return View();
            }
        }

        /*
        [HttpPost]
        public async Task<IActionResult> Index(ICollection<IFormFile> files) //2017.02.23 jskim add file upload system
        {
            var uploads = Path.Combine(_environment.WebRootPath, "uploads");
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
            }
            return Orbit(files);
        }*/

        /*
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(); 
        }
        *///jskim, 20170214 unnecessary information deleted
    }
}
