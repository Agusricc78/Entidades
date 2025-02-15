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
    public class BLL_Categoria
    {

        Mp_Categoria obj = new Mp_Categoria();


        public DataTable GetAllCategorias()
        {
            return obj.ListarCategorias();
        }

        public int RegistrarCategoria(Categorias ct, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(ct.Nombre) || string.IsNullOrWhiteSpace(ct.Nombre))
            {
                Mensaje = "El nombre de la categoria no puede estar vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
              
                    return obj.RegistrarCategoria(ct, out Mensaje);
              
            }
            else
            {
                return 0;
            }

        }
        public int UpdateCategoria(Categorias ct, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(ct.Nombre) || string.IsNullOrWhiteSpace(ct.Nombre))
            {
                Mensaje = "El nombre de la categoria no puede estar vacio";
            }
   
            if (string.IsNullOrEmpty(Mensaje))
            {
                return obj.UpdateCategoria(ct, out Mensaje);
            }
            else
            {
                return 0;
            }
        }

        public int EliminarCategoria(int id, out string Mensaje)
        {
            Mensaje = string.Empty;

            

            if (string.IsNullOrEmpty(Convert.ToInt32(id).ToString()) || string.IsNullOrWhiteSpace(Convert.ToInt32(id).ToString()))
            {
                Mensaje = "El id de la categoria no puede estar vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return obj.EliminarCategoria(id, out Mensaje);
            }
            else
            {
                return 0;
            }
        }
    }
}
