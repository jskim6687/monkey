using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monkey.Models
{
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

    [TableName("navigation")]
    public class navigation
    {
        public int prn { get; set; }
        public int year { get; set; }
        public int month { get; set; }
        public int day { get; set; }
        public int hour { get; set; }
        public int minute { get; set; }
        public double second { get; set; }
        public double svClockBias { get; set; }
        public double svClockDrift { get; set; }
        public double svClockDriftRate { get; set; }
        public double IODE { get; set; }
        public double crs { get; set; }
        public double deltaN { get; set; }
        public double m0 { get; set; }
        public double cuc { get; set; }
        public double e { get; set; }
        public double cus { get; set; }
        public double rootA { get; set; }
        public double toe { get; set; }
        public double cic { get; set; }
        public double ohm0 { get; set; }
        public double cis { get; set; }
        public double i0 { get; set; }
        public double crc { get; set; }
        public double w { get; set; }
        public double ohmDot { get; set; }
        public double iDot { get; set; }
        public double L2Code { get; set; }
        public double gpsWeek { get; set; }
        public double L2PFlag { get; set; }
        public double svAccuracy { get; set; }
        public double svHealth { get; set; }
        public double TDG { get; set; }
        public double IODC { get; set; }
        public double Tx { get; set; }
        public double fitInterval { get; set; }
    }

    public class epochSV
    {
        public int satNum { get; set; }
        public string satType { get; set; }
    }

    public class commonSV
    {
        public int year { get; set; }
        public int month { get; set; }
        public int day { get; set; }
        public int hour { get; set; }
        public int minute { get; set; }
        public int second { get; set; }
        public int num { get; set; }
        public string type { get; set; }
    }

    [TableName("commonSVcoordinate")]
    public class commonSVcoordinate
    {
        public int year { get; set; }
        public int month { get; set; }
        public int day { get; set; }
        public int hour { get; set; }
        public int minute { get; set; }
        public int second { get; set; }
        public int num { get; set; }
        public string type { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
    }

    [TableName("dop")]
    public class dop
    {
        public int year { get; set; }
        public int month { get; set; }
        public int day { get; set; }
        public int hour { get; set; }
        public int minute { get; set; }
        public int second { get; set; }
        public int prn1 { get; set; }
        public int prn2 { get; set; }
        public int prn3 { get; set; }
        public int prn4 { get; set; }
        public double GDOP { get; set; }
        public double HDOP { get; set; }
        public double VDOP { get; set; }
        public double TDOP { get; set; }
        public double PDOP { get; set; }
    }

    [TableName("glonassNav")]
    public class glonassNav
    {
        public int prn { get; set; }
        public int year { get; set; }
        public int month { get; set; }
        public int day { get; set; }
        public int hour { get; set; }
        public int minute { get; set; }
        public double second { get; set; }
        public double clockBias { get; set; }
        public double relFreqBias { get; set; }
        public double msgFrameTime { get; set; }
        public double positionX { get; set; }
        public double velocityXdot { get; set; }
        public double accelX { get; set; }
        public double health { get; set; }
        public double positionY { get; set; }
        public double velocityYdot { get; set; }
        public double accelY { get; set; }
        public double freqNum { get; set; }
        public double positionZ { get; set; }
        public double velocityZdot { get; set; }
        public double accelZ { get; set; }
        public double ageOfOper { get; set; }
    }
}
