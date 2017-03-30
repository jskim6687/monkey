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

        public void AddSVcoord(commonSVcoordinate coord)
        {
            using (IDatabase db = Connection)
            {
                db.Insert<commonSVcoordinate>(coord);
            }
        }

        public void AddDOP(dop dop)
        {
            using (IDatabase db = Connection)
            {
                db.Insert<dop>(dop);
            }
        }

        public void DeleteAll()
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var queryString = "delete from commonSVcoordinate delete from eachepoch delete from navigation";
                var command = new SqlCommand(queryString, sqlConnection);
                command.ExecuteNonQuery();
            }
        }

        public List<commonSV> selectEachSV(int year, int month, int day)
        {
            var commonSV = new List<commonSV>();

            int hour = 0;
            int minute = 0;
            int second = 0;

            using (IDatabase db = Connection)
            {
                while (hour < 24)
                {
                    var queryString = "select [satNum], [satType] from eachEpoch where year=" + year.ToString() + " and month=" + month.ToString() + " and day=" + day.ToString() + " and hour=" + hour.ToString() + " and minute=" + minute.ToString() + " and second =" + second.ToString() + " and satType = 'G'";
                    //각 에포크마다 반복시작
                    List<epochSV> baseSV = db.Fetch<epochSV>(queryString + " and station = 'base'");
                    List<epochSV> roverSV = db.Fetch<epochSV>(queryString + " and station = 'rover'");

                    for (int i = 0; i < baseSV.Count; i++)
                    {
                        for (int j = 0; j < roverSV.Count; j++)
                        {
                            if (baseSV[i].satNum == roverSV[j].satNum && baseSV[i].satType == roverSV[j].satType)
                            {
                                var SVitem = new commonSV();
                                SVitem.year = year;
                                SVitem.month = month;
                                SVitem.day = day;
                                SVitem.hour = hour;
                                SVitem.minute = minute;
                                SVitem.second = second;
                                SVitem.num = baseSV[i].satNum;
                                SVitem.type = baseSV[i].satType;
                                commonSV.Add(SVitem);
                            }
                        }
                    }

                    if (second == 0)
                    {
                        second = 30;
                    }
                    else
                    {
                        second = 0;
                        if (minute == 59)
                        {
                            minute = 0;
                            hour = hour + 1;
                        }
                        else
                        {
                            minute = minute + 1;
                        }
                    }
                    //각 에포크 반복 종료
                }
            }
            return commonSV;
        }

        public List<navigation> commonNavList(int year, int month, int day, int svNum)
        {
            var commonNavList = new List<navigation>();

            using (IDatabase db = Connection)
            {
                var queryString = "select * from navigation where year=" + year.ToString() + " and month=" + month.ToString() + " and day=" + day.ToString() + " and prn=" + svNum.ToString() + " order by toe";
                commonNavList = db.Fetch<navigation>(queryString);
            }

            return commonNavList;
        }

        public List<commonSVcoordinate> getCommonSV(int year, int month, int day, int hour, int minute, int second)
        {
            var commonList = new List<commonSVcoordinate>();
            using (IDatabase db = Connection)
            {
                var queryString = "select * from commonSVcoordinate where year=" + year.ToString() + " and month=" + month.ToString() + " and day=" + day.ToString() + " and hour=" + hour.ToString() + " and minute=" + minute.ToString() + " and second=" + second.ToString() + " order by num";
                commonList = db.Fetch<commonSVcoordinate>(queryString);
            }
            return commonList;
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
