using CapaDatos.Mappers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.BLL
{
    public class BLL_Ubicacion
    {
        MP_Ubicacion obj = new MP_Ubicacion();


        public DataTable GetProvincias()
        {
            return obj.ListarProvincias();
        }

        public DataTable GetPartidosXProvincia(int idprovincia)
        {
            return obj.ListarPartidosXProvincia(idprovincia);
        }

        public DataTable GetLocalidadXPartido(int idpartido)
        {
            return obj.ListarLocalidadesXPartidos(idpartido);
        }


    }
}
