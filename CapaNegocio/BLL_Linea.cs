using CapaDatos.Mappers;
using Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class BLL_Linea
    {
        Mp_Linea obj = new Mp_Linea();

        public DataTable GetAllLineas()
        {
            return obj.ListarLinea();
        }

        public DataTable GetLineaCategoria(int idcategoria)
        {
            return obj.GetLineaCategoria(idcategoria);
        }

        public int RegistrarLinea(Linea ln, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(ln.Nombre) || string.IsNullOrWhiteSpace(ln.Nombre))
            {
                Mensaje = "El nombre de la linea no puede estar vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {

                return obj.RegistrarLinea(ln, out Mensaje);

            }
            else
            {
                return 0;
            }

        }
        public int UpdateLinea(Linea ln, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(ln.Nombre) || string.IsNullOrWhiteSpace(ln.Nombre))
            {
                Mensaje = "El nombre de la categoria no puede estar vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return obj.UpdateLinea(ln, out Mensaje);
            }
            else
            {
                return 0;
            }
        }

        public int EliminarLinea(int id, out string Mensaje)
        {
            Mensaje = string.Empty;



            if (string.IsNullOrEmpty(Convert.ToInt32(id).ToString()) || string.IsNullOrWhiteSpace(Convert.ToInt32(id).ToString()))
            {
                Mensaje = "El id de la categoria no puede estar vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return obj.EliminarLinea(id, out Mensaje);
            }
            else
            {
                return 0;
            }

        }
    }
}
