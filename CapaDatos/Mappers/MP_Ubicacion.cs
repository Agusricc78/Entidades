using Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Mappers
{
    public class MP_Ubicacion
    {

        private readonly Conexion cn = new Conexion();

        public DataTable ListarProvincias()
        {
            return cn.Leer("GetProvincias");
        }

        public DataTable ListarPartidosXProvincia(int idprovincia)
        {
            SqlParameter[] sp = new SqlParameter[]
            {
                new SqlParameter("@Id_Provincia",idprovincia)
            };

            return cn.Leer("GetPartidosXProvincia",sp);
        }

        public DataTable ListarLocalidadesXPartidos(int idpartido)
        {

            SqlParameter[] sp = new SqlParameter[]
         {
                new SqlParameter("@Id_Partido",idpartido)
         };

            return cn.Leer("GetLocalidadXPartido",sp);
        }


    }
}
