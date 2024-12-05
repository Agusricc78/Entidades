using DataAccessLayer;
using DataAccessLayer.Mappers;
using Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class BLL_Productos
    {
        Mp_Productos mp = new Mp_Productos();

        public int AgregarProducto(Productos ps) 
        {
            return mp.AgregarProducto(ps);
        }

        public int EliminarProducto(int id)
        {
            return mp.EliminarProducto(id);
        }

        public DataTable ListarProductos()
        {
            return mp.ListarProductos();
        }


        public List<Productos> ObtenerProductos()
        {
            DataTable dt = ListarProductos();

            List<Productos> productos = Helper.DataTableToList<Productos>(dt);

            return productos;

        }





    }
}
