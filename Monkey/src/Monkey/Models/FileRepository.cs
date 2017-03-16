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

        public void DeleteAll()
        {
            using (IDatabase db = Connection)
            {
                db.Delete("monkey");
            }
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
