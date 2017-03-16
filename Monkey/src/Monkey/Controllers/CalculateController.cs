using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Monkey.Models;

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

            makeEpoch(roverRoot, "rover");
            makeEpoch(baseRoot, "base");

            //여기서 부터 위성 궤도 올리고 보정신호 처리

            return View();
        }

        //o파일 입출력 클래스 생성
        public void makeEpoch(string root, string station)
        {
            var addEpoch = new FileRepository();
            var eachEpoch = new eachEpoch();

            var stream = new FileStream(root, FileMode.Open);
            var reader = new StreamReader(stream, System.Text.Encoding.ASCII);

            for(int i = 0; i < 21 ; i++ )
            {
                reader.ReadLine();
            }

            int j = 1;
            int row = 1;
            int numSat = 1;
            string[] sat = new string[30];
            int year = 1;
            int month = 1;
            int day = 1;
            int hour = 1;
            int minute = 1;
            int second = 1;

            for (int i = 0; i < 2880; i++)
            {
                string fileLine = "";

                if(j == 1)
                {
                    fileLine = reader.ReadLine();
                    row = row + 1;

                    if(fileLine.Length > 0)
                    {
                        year = int.Parse(fileLine.Substring(1, 2));
                        month = int.Parse(fileLine.Substring(4, 2));
                        day = int.Parse(fileLine.Substring(7, 2));
                        hour = int.Parse(fileLine.Substring(10, 2));
                        minute = int.Parse(fileLine.Substring(13, 2));
                        second = int.Parse(fileLine.Substring(16, 2));
                        numSat = int.Parse(fileLine.Substring(30, 2));

                        for (int k = 0; k < 12; k++)
                        {
                            var satName = fileLine.Substring((3 * k) + 32, 3);
                            sat[k] = satName;
                        }

                        fileLine = reader.ReadLine();
                        row = row + 1;
                        fileLine = fileLine.Trim();
                        //numSat = 12 + (fileLine.Length / 3);

                        for (int k = 0; k < fileLine.Length / 3; k++)
                        {
                            var satName = fileLine.Substring(k * 3, 3);
                            sat[12 + k] = satName;
                        }
                    }
                    j = j + 2;
                }
                else
                {
                    for (int k = 0; k < numSat; k++)
                    {
                        fileLine = reader.ReadLine();
                        row = row + 1;
                        if (fileLine.Length > 0)
                        {
                            //여기서 부터 L1, L2 기록
                            /*
                             * Phase  : Units in whole cycles of carrier
                             * Code   : Units in meters
                             */
                            var satName = sat[k];
                            double observation1 = 0;
                            double observation2 = 0;
                            if (fileLine.Substring(2, 12).Length > 0)
                            {
                                observation1 = double.Parse(fileLine.Substring(2, 12));
                            }
                            if (fileLine.Length > 16)
                            {
                                if (fileLine.Substring(17,12).Trim().Length > 0)
                                {
                                    observation2 = double.Parse(fileLine.Substring(17, 12));
                                }
                            }

                            eachEpoch.id = (year*10000000000+month*100000000+day*1000000+hour*10000+minute*100+second).ToString()+satName;
                            eachEpoch.year = year;
                            eachEpoch.month = month;
                            eachEpoch.day = day;
                            eachEpoch.hour = hour;
                            eachEpoch.minute = minute;
                            eachEpoch.second = second;
                            eachEpoch.satNum = int.Parse(satName.Substring(1, 2));
                            eachEpoch.satType = satName.Substring(0, 1);
                            eachEpoch.station = station;
                            eachEpoch.observation1 = observation1;
                            eachEpoch.observation2 = observation2;

                            addEpoch.Add(eachEpoch);
                        }
                        reader.ReadLine();
                        row = row + 1;
                        reader.ReadLine();
                        row = row + 1;
                        j = j + 1;
                    }
                    j = 1;
                }
            }
        }

        public IActionResult Delete()
        {
            var fileRepo = new FileRepository();
            fileRepo.DeleteAll();
            return View();
        }
    }
}
