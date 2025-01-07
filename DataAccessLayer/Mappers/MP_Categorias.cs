using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Mappers
{
    public class MP_Categorias
    {
        private readonly Conexion cn = new Conexion();

        public DataTable ListarCategorias()
        {
            return cn.Leer("ListarCategorias");
        }

    }
}
