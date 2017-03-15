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

            makeEpoch(roverRoot);

            //여기서 부터 위성 궤도 올리고 보정신호 처리

            return View();
        }

        //o파일 입출력 클래스 생성
        public void makeEpoch(string root)
        {
            var stream = new FileStream(root, FileMode.Open);
            var reader = new StreamReader(stream, System.Text.Encoding.ASCII);

            for(int i = 0; i < 21 ; i++ )
            {
                reader.ReadLine();
            }

            for(int i = 0; i < 2880; i++)
            {
                int j = 1;
                string fileLine = "";
                int numSat = 1;
                int year = 1;
                int month = 1;
                int day = 1;
                int hour = 1;
                int minute = 1;
                int second = 1;
                string[] sat = new string[30];

                if(j == 1)
                {
                    fileLine = reader.ReadLine();
                    year = int.Parse(fileLine.Substring(1, 2));
                    month = int.Parse(fileLine.Substring(4, 2));
                    day = int.Parse(fileLine.Substring(7, 2));
                    hour = int.Parse(fileLine.Substring(10, 2));
                    minute = int.Parse(fileLine.Substring(13, 2));
                    second = int.Parse(fileLine.Substring(16, 2));

                    for (int k = 0; k < 12; k++)
                    {
                        var satName = fileLine.Substring((3*k)+30, 3);
                        sat[k] = satName;
                    }

                    fileLine = reader.ReadLine();
                    fileLine = fileLine.Trim();
                    numSat = 12 + (fileLine.Length / 3);

                    for (int k = 0; k < fileLine.Length/3; k++)
                    {
                        var satName = fileLine.Substring(k*3,3);
                        sat[12 + k] = satName;
                    }

                    j = j + 2;
                }
                else
                {
                    for (int k = 0; k < numSat; k++)
                    {
                        fileLine = reader.ReadLine();
                        var satName = sat[k];
                        //여기서 부터 기록

                        j = j + 1;
                    }
                    j = 1;
                }
            }
        }
    }
}
