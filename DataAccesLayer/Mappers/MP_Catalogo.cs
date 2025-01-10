using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DataAccesLayer.Mappers
{
    public class MP_Catalogo
    {
        private readonly Conexion cn = new Conexion();

        public DataTable ListarCatalogos()
        {
            return cn.Leer("ListarCatalogos");

        }


    }
}
