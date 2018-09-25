using System;
using MySql.Data.MySqlClient;
using UniversityRegistrar;
using static UniversityRegistrar.Startup;

namespace UniversityRegistrar.Models
{
    public class DB
    {
        public static MySqlConnection Connection()
        {
            MySqlConnection conn = new MySqlConnection(DBConfiguration.ConnectionString);
            return conn;
        }
    }
}
