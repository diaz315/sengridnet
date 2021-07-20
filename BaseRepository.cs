using Dapper;
using Integration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendGridNet
{
    public class BaseRepository
    {
        public string CadenaConexion { get; set; }

        protected BaseRepository()
        {
            CadenaConexion = Conexion.getConnection();
        }

        protected SqlConnection OpenConnection()
        {
            var connection = new SqlConnection();
            connection.ConnectionString = CadenaConexion;
            return connection;
        }

        protected void Execute(string procedimiento, DynamicParameters parametros)
        {
            using (SqlConnection conn = OpenConnection())
            {
                conn.Open();
                conn.Execute(procedimiento, parametros, commandType: CommandType.StoredProcedure);
            }
        }

        protected T Get<T>(string procedimiento, DynamicParameters parametros)
        {
            using (SqlConnection conn = OpenConnection())
            {
                conn.Open();
                return conn.Query<T>(procedimiento, parametros,
                    commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        protected List<T> List<T>(string procedimiento, DynamicParameters parametros) where T : class
        {
            using (SqlConnection conn = OpenConnection())
            {
                conn.Open();
                return conn.Query<T>(procedimiento, parametros, commandType: CommandType.StoredProcedure,
                    buffered: true, commandTimeout: 240)
                    .ToList();
            }
        }

        protected T ExecuteOutput<T>(string procedimiento, DynamicParameters parametros) where T : class
        {
            using (SqlConnection conn = OpenConnection())
            {
                conn.Open();
                return conn.Query<T>(procedimiento, parametros, commandType: CommandType.StoredProcedure)
                    .Single();
            }
        }

        private DataTable ConvertToDataTable<T>(IList<T> data)

        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));

            DataTable table = new DataTable();

            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (T item in data)

            {

                DataRow row = table.NewRow();

                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;

                }

                table.Rows.Add(row);
            }

            return table;

        }

        protected void InsertSet<T>(string procedure, IList<T> tableList, string tableType)
        {
            var dt = ConvertToDataTable(tableList);

            using (IDbConnection connection = new SqlConnection(CadenaConexion)) {
                var p = new
                {
                    TableType = dt.AsTableValuedParameter(tableType)
                };

                connection.Execute(procedure, p, commandType: CommandType.StoredProcedure);
            };

        }
    }
}
