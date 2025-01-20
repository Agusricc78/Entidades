using Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CostaAzulWeb.Models
{
    public class ProductoViewModel
    {
        public int Id_Producto { get; set; }

        public int Id_Linea { get; set; }

        public string NombreLinea { get; set; }

        public string Descripcion { get; set; }

        public bool Electrico { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser un valor positivo.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El código es obligatorio.")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        public string Id_Categoria { get; set; }

        public string NombreCategoria { get; set; } // Nombre de la categoría asociada

        public bool Activo { get; set; }

        public int Id_Catalogo { get; set; }    

        public string Imagen { get; set; } // Ruta o nombre del archivo de la imagen

        [Required(ErrorMessage = "El stock es obligatorio.")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock debe ser un valor positivo.")]
        public int Stock { get; set; }

        // Lista de categorías
        public List<Categorias> Categorias { get; set; }

        // SelectList para Dropdown
        public IEnumerable<SelectListItem> CategoriasSelectList =>
            Categorias?.Select(c => new SelectListItem
            {
                Value = c.Id_Categoria.ToString(),
                Text = c.Nombre
            });

        // Lista de productos
        public List<Productos> ListaProductos { get; set; }


        public List<Catalogos> Catalogos { get; set; }

        // SelectList para Dropdown
        public IEnumerable<SelectListItem> CatalogosSelectList =>
            Catalogos?.Select(c => new SelectListItem
            {
                Value = c.Id_Catalogo.ToString(),
                Text = c.Nombre
            });


        public List<Linea> Lineas{ get; set; }

        // SelectList para Dropdown
        public IEnumerable<SelectListItem> LineasSelectList =>
            Lineas?.Select(c => new SelectListItem
            {
                Value = c.Id_Linea.ToString(),
                Text = c.Nombre
            });


    }
}