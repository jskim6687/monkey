using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Monkey.Models;
using NPoco;
using System.Data.SqlClient;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Monkey.Models
{
    public class FileRepository
    {
        private string connectionString;

        public FileRepository()
        {
            //connectionString = "Server=localhost:3306;Uid=jskim;Pwd=7yearswar;Database=monkey";
            connectionString = @"Server=localhost;Database=monkey;User Id=jskim;Password=7yearswar";
        }

        public IDatabase Connection
        {
            get
            {
                return new Database(connectionString, DatabaseType.SqlServer2012, SqlClientFactory.Instance);
            }
        }

        public void Add(eachEpoch epoch)
        {
            using (IDatabase db = Connection)
            {
                db.Insert<eachEpoch>(epoch);
            }
        }

        public void AddNfile(navigation nav)
        {
            using (IDatabase db = Connection)
            {
                db.Insert<navigation>(nav);
            }

        }

        public void DeleteAll()
        {
            using (IDatabase db = Connection)
            {
                db.Delete("monkey");
            }
        }

        public List<commonSV> selectEachSV(int year, int month, int day, int hour, int minute, int second)
        {
            var commonSV = new List<commonSV>();

            using (IDatabase db = Connection)
            {
                //각 에포크마다 반복시작
                List<epochSV> baseSV = db.Fetch<epochSV>("select satNum, satType where year=" + year.ToString() + " and month=" + month.ToString() + " and day=" + day.ToString() + " and hour=" + hour.ToString() + " and minute=" + minute.ToString() + " and second =" + second.ToString() + " and type = 'base'");
                List<epochSV> roverSV = db.Fetch<epochSV>("select satNum, satType where year=" + year.ToString() + " and month=" + month.ToString() + " and day=" + day.ToString() + " and hour=" + hour.ToString() + " and minute=" + minute.ToString() + " and second =" + second.ToString() + " and type = 'rover'");

                for (int i = 0 ; i< baseSV.Count ; i++)
                {
                    for(int j =0; j < roverSV.Count; j++)
                    {
                        if (baseSV[i].num == roverSV[j].num && baseSV[i].type == roverSV[j].type)
                        {
                            var SVitem = new commonSV();
                            SVitem.year = year;
                            SVitem.month = month;
                            SVitem.day = day;
                            SVitem.hour = hour;
                            SVitem.minute = minute;
                            SVitem.second = second;
                            SVitem.num = baseSV[i].num;
                            SVitem.type = baseSV[i].type;
                            commonSV.Add(SVitem);
                        }
                    }
                }

            //각 에포크 반복 종료
            }
            return commonSV;
        }
        /*
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
                */
    }
}
