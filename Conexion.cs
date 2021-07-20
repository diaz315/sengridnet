using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.IO;

namespace Integration
{
    public class Conexion
    {
        static SqlConnection con;

        public static IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }

        public static string getConnection()
        {
            var configuration = GetConfiguration();
            con = new SqlConnection(configuration.GetSection("ConnectionStrings").GetSection("Integration").Value);
            return con.ConnectionString;
        }

    }
}

