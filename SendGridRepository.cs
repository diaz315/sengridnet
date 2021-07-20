using Dapper;
using System.Collections.Generic;
using System.Data;

namespace SendGridNet
{
    class SendGridRepository: BaseRepository
    {
        public void Insertar(List<Persona> tTBOConnections)
        {
            InsertSet(StoredProcedures.Insertar, tTBOConnections, "TT_Persona");
        }

        public void Actualizar(Persona persona) {
            var param = new DynamicParameters();
            param.Add("@id", persona.id, DbType.Int32, ParameterDirection.Input);
            Execute(StoredProcedures.Actualizar, param);
        }

        public void Asignar(int Cantidad,string Mac)
        {
            var param = new DynamicParameters();
            param.Add("@Cantidad", Cantidad, DbType.Int32, ParameterDirection.Input);
            param.Add("@Mac", Mac, DbType.String, ParameterDirection.Input);
            Execute(StoredProcedures.Asignar, param);
        }

        public List<Persona> Seleccionar(int Rango,string Mac)
        {
            var param = new DynamicParameters();
            param.Add("@Mac", Mac, DbType.String, ParameterDirection.Input);
            param.Add("@Rango", Rango, DbType.Int32, ParameterDirection.Input);
            return List<Persona>(StoredProcedures.Seleccionar, param);
            //return List<Persona>("reintentar", param);
        }

        public Config GetConfig()
        {
            return Get<Config>(StoredProcedures.Getconfig, null);
        }

        public Estado GetEstado()
        {
            return Get<Estado>(StoredProcedures.Estado, null);
        }
    }
}
