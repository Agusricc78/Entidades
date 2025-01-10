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

        public int EliminarProducto(string id)
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

        public bool ValidarExistencia( string cod)
        {
            return mp.VerificarExistencia( cod);
        }

        public Productos ObtenerProducto(string cod)
        {
            try
            {
                
                return mp.ObtenerProducto(cod);

            }
            catch 
            {
                return null;
            }
        }

       public int EditarProducto(Productos Pro)
        {
            return mp.EditarPro(Pro);
        }

        public List<Productos> ListarPorCatalogo(int id)
        {
            DataTable dt = mp.ListarCatalogos(id);

            List<Productos> productos = Helper.DataTableToList<Productos>(dt);

            return productos;
        }

        public List<Productos> FiltrarProductos(int? categoriaId, int? lineaId, string codigo = null)
        {
            
            DataTable dt = mp.FiltrarProductos(categoriaId, lineaId, codigo);


            List<Productos> pro = Helper.DataTableToList<Productos>(dt);


            return pro;

        }





    }
}
