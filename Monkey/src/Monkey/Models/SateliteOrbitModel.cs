using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monkey.Models
{
    public class SatelliteOrbitModel
    {
        public string id { get; set; }
        public int year { get; set; }
        public int month { get; set; }
        public int day { get; set; }
        public int hour { get; set; }
        public int minute { get; set; }
        public int second { get; set; }
        public int satNum { get; set; }
        public string satType { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
        public string station { get; set; }
    }

    [TableName("eachEpoch")]
    public class eachEpoch
    {
        public string id { get; set; }
        public int year { get; set; }
        public int month { get; set; }
        public int day { get; set; }
        public int hour { get; set; }
        public int minute { get; set; }
        public int second { get; set; }
        public int satNum { get; set; }
        public string satType { get; set; }
        public string station { get; set; }
        public double observation1 { get; set; }
        public double observation2 { get; set; }
    }
}
