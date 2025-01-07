using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebFormLayer.Models
{
    public class ProductoViewModel
    {
       public int Id_Producto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Codigo { get; set; }
        public string Id_Categoria { get; set; }

        public string NombreCategoria { get; set; } 
        public bool Activo { get; set; }
        public string Imagen { get; set; }
        public int Stock { get; set; }

        
        public List<Categorias> categorias { get; set; }

        public IEnumerable<SelectListItem> CategoriasSelectList =>
        categorias?.Select(c => new SelectListItem
        {
            Value = c.Id_Categoria.ToString(),
            Text = c.Nombre
        });


        public List<Productos> ListaProductos { get; set; }





    }
}