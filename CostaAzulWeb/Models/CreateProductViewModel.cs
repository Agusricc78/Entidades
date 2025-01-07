using System.ComponentModel.DataAnnotations;

namespace CostaAzulWeb.Models
{
    public class CreateProductViewModel
    {
        
            [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
            public string Nombre { get; set; }

            [Required(ErrorMessage = "La descripción es obligatoria.")]
            public string Descripcion { get; set; }

            [Required(ErrorMessage = "El precio es obligatorio.")]
            public decimal Precio { get; set; }

            [Required(ErrorMessage = "El código es obligatorio.")]
            public int Codigo { get; set; }

            [Required(ErrorMessage = "La categoría es obligatoria.")]
            public string Id_Categoria { get; set; }

            [Required(ErrorMessage = "El estado activo es obligatorio.")]
            public bool Activo { get; set; }

            public string Imagen { get; set; }
            public int Stock { get; set; }
        

    }
}
