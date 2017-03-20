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
            var navFile = baseFile.Remove(11,1) + "n";
            var baseRoot = Path.Combine(_environment.WebRootPath, "base\\", baseFile);
            var roverRoot = Path.Combine(_environment.WebRootPath, "rover\\", roverFile);
            var navRoot = Path.Combine(_environment.WebRootPath, "base\\", navFile);

            makeEpoch(roverRoot, "rover"); //rover o파일 업로드
            makeEpoch(baseRoot, "base"); //base o파일 업로드
            svNavigation(navRoot); //navigation 파일 업로드

            //공통 위성 뽑아내는 알고리즘 시작

            var year = int.Parse(baseFile.Substring(9,2));
            var gpsday = int.Parse(baseFile.Substring(4,3));

            var monthDay = calculateDay(gpsday, year);

            var month = monthDay[0];
            var day = monthDay[1];

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
            string[] sat = new string[50];
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
                    
                        if(numSat > 12)
                        {
                            fileLine = reader.ReadLine();
                            row = row + 1;
                            fileLine = fileLine.Trim();
                            //numSat = 12 + (fileLine.Length / 3);

                            for (int k = 0; k < fileLine.Length / 3; k++)
                            {
                                var satName = fileLine.Substring(k * 3, 3);
                                sat[12 + k] = satName;
                            }
                            if(numSat > 24)
                            {
                                fileLine = reader.ReadLine();
                                row = row + 1;
                                fileLine = fileLine.Trim();

                                for(int k = 0; k < fileLine.Length / 3; k++)
                                {
                                    var satName = fileLine.Substring(k * 3, 3);
                                    sat[24 + k] = satName;
                                }
                            }
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

        public void svNavigation(string root)
        {
            var stream = new FileStream(root, FileMode.Open);
            var reader = new StreamReader(stream, System.Text.Encoding.ASCII);
            var addNfile = new FileRepository();

            var navigation = new navigation();

            for (int i = 0; i < 9; i++)
            {
                reader.ReadLine();
            }

            while (reader.EndOfStream == false)
            {
                var fileLine = reader.ReadLine(); //1st row

                navigation.prn = int.Parse(fileLine.Substring(0,2));
                navigation.year = int.Parse(fileLine.Substring(3, 2));
                navigation.month = int.Parse(fileLine.Substring(6, 2));
                navigation.day = int.Parse(fileLine.Substring(9,2));
                navigation.hour = int.Parse(fileLine.Substring(12,2));
                navigation.minute = int.Parse(fileLine.Substring(15,2));
                navigation.second = double.Parse(fileLine.Substring(18,4));
                navigation.svClockBias = double.Parse(fileLine.Substring(22, 15)) * Math.Pow(10,double.Parse(fileLine.Substring(38,3)));
                navigation.svClockDrift = double.Parse(fileLine.Substring(41, 15)) * Math.Pow(10, double.Parse(fileLine.Substring(57, 3)));
                navigation.svClockDriftRate = double.Parse(fileLine.Substring(60, 15)) * Math.Pow(10, double.Parse(fileLine.Substring(76, 3)));

                fileLine = reader.ReadLine(); //2nd row

                navigation.IODE = double.Parse(fileLine.Substring(3,15)) * Math.Pow(10, double.Parse(fileLine.Substring(19, 3)));
                navigation.crs = double.Parse(fileLine.Substring(22, 15)) * Math.Pow(10, double.Parse(fileLine.Substring(38, 3)));
                navigation.deltaN = double.Parse(fileLine.Substring(41, 15)) * Math.Pow(10, double.Parse(fileLine.Substring(57, 3)));
                navigation.m0 = double.Parse(fileLine.Substring(60, 15)) * Math.Pow(10, double.Parse(fileLine.Substring(76, 3)));

                fileLine = reader.ReadLine(); //3rd row

                navigation.cuc = double.Parse(fileLine.Substring(3, 15)) * Math.Pow(10, double.Parse(fileLine.Substring(19, 3)));
                navigation.e = double.Parse(fileLine.Substring(22, 15)) * Math.Pow(10, double.Parse(fileLine.Substring(38, 3)));
                navigation.cus = double.Parse(fileLine.Substring(41, 15)) * Math.Pow(10, double.Parse(fileLine.Substring(57, 3)));
                navigation.rootA = double.Parse(fileLine.Substring(60, 15)) * Math.Pow(10, double.Parse(fileLine.Substring(76, 3)));

                fileLine = reader.ReadLine(); //4th row

                navigation.toe = double.Parse(fileLine.Substring(3, 15)) * Math.Pow(10, double.Parse(fileLine.Substring(19, 3)));
                navigation.cic = double.Parse(fileLine.Substring(22, 15)) * Math.Pow(10, double.Parse(fileLine.Substring(38, 3)));
                navigation.ohm0 = double.Parse(fileLine.Substring(41, 15)) * Math.Pow(10, double.Parse(fileLine.Substring(57, 3)));
                navigation.cis = double.Parse(fileLine.Substring(60, 15)) * Math.Pow(10, double.Parse(fileLine.Substring(76, 3)));

                fileLine = reader.ReadLine(); //5th row

                navigation.i0 = double.Parse(fileLine.Substring(3, 15)) * Math.Pow(10, double.Parse(fileLine.Substring(19, 3)));
                navigation.crc = double.Parse(fileLine.Substring(22, 15)) * Math.Pow(10, double.Parse(fileLine.Substring(38, 3)));
                navigation.w = double.Parse(fileLine.Substring(41, 15)) * Math.Pow(10, double.Parse(fileLine.Substring(57, 3)));
                navigation.ohmDot = double.Parse(fileLine.Substring(60, 15)) * Math.Pow(10, double.Parse(fileLine.Substring(76, 3)));

                fileLine = reader.ReadLine(); //6th row

                navigation.iDot = double.Parse(fileLine.Substring(3, 15)) * Math.Pow(10, double.Parse(fileLine.Substring(19, 3)));
                navigation.L2Code = double.Parse(fileLine.Substring(22, 15)) * Math.Pow(10, double.Parse(fileLine.Substring(38, 3)));
                navigation.gpsWeek = double.Parse(fileLine.Substring(41, 15)) * Math.Pow(10, double.Parse(fileLine.Substring(57, 3)));
                navigation.L2PFlag = double.Parse(fileLine.Substring(60, 15)) * Math.Pow(10, double.Parse(fileLine.Substring(76, 3)));

                fileLine = reader.ReadLine(); //7th row

                navigation.svAccuracy = double.Parse(fileLine.Substring(3, 15)) * Math.Pow(10, double.Parse(fileLine.Substring(19, 3)));
                navigation.svHealth = double.Parse(fileLine.Substring(22, 15)) * Math.Pow(10, double.Parse(fileLine.Substring(38, 3)));
                navigation.TDG = double.Parse(fileLine.Substring(41, 15)) * Math.Pow(10, double.Parse(fileLine.Substring(57, 3)));
                navigation.IODC = double.Parse(fileLine.Substring(60, 15)) * Math.Pow(10, double.Parse(fileLine.Substring(76, 3)));

                fileLine = reader.ReadLine(); //8th row

                navigation.Tx = double.Parse(fileLine.Substring(3, 15)) * Math.Pow(10, double.Parse(fileLine.Substring(19, 3)));
                navigation.fitInterval = double.Parse(fileLine.Substring(22, 15)) * Math.Pow(10, double.Parse(fileLine.Substring(38, 3)));

                addNfile.AddNfile(navigation);
            }
        } //N파일 리딩 및 업로드

        public IActionResult Delete()
        {
            var fileRepo = new FileRepository();
            fileRepo.DeleteAll();
            return View();
        }//기록된 레코드 삭제

        public int[] calculateDay(int gpsday, int year) //GPSDay를 이용한 달과 일 계산
        {
            int[] calculatedMonthDay = new int[] { 1, gpsday };

            int[] monthDay = new int[] {31,28,31,30,31,30,31,31,30,31,30,31};

            if (year%4==0)
            {
                monthDay[1] = 29;
            }

            while (gpsday-monthDay[calculatedMonthDay[0]-1] > 0)
            {
                calculatedMonthDay[1] = calculatedMonthDay[1] - monthDay[calculatedMonthDay[0] - 1];
                calculatedMonthDay[0] = calculatedMonthDay[0] + 1;
            }
            return calculatedMonthDay;
        }

    }
}
