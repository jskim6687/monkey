using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Monkey.Models;

namespace Monkey.Models
{
    public class FileRepository : IFileRepository
    {
        public void getOfileInfo(string oFileName)
        {
            new Ofiles
            {
                name = oFileName,
                stationName = oFileName.Substring(0, 4),
                gpsday = int.Parse(oFileName.Substring(4, 3)),
                order = int.Parse(oFileName.Substring(7, 1)),
                year = int.Parse(oFileName.Substring(9, 2))
            };
        }
        public void getNfileInfo(string nFileName)
        {
            new Nfiles
            {
                name = nFileName,
                stationName = nFileName.Substring(0, 4),
                gpsday = int.Parse(nFileName.Substring(4, 3)),
                order = int.Parse(nFileName.Substring(7, 1)),
                year = int.Parse(nFileName.Substring(9, 2))
            };
        }
    }
}
