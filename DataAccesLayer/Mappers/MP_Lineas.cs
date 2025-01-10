using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesLayer.Mappers
{
    public class MP_Lineas
    {
        private readonly Conexion cn = new Conexion();

        public DataTable ListarLi()
        {
            return cn.Leer("ListarLineas");
        }

    }
}
