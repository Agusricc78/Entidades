using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Productos
    {
        public int Id_Producto { get; set; }
        public int Id_Linea { get; set; }
        public string NombreLinea { get; set; }
        public string Descripcion{ get; set; }
        public decimal Precio { get; set; }
        public string Cod_Producto { get; set; } 
        public string Id_Categoria { get; set; }   
        public string NombreCategoria { get; set; }  
        public bool Activo { get; set; }
        public bool Electrico{ get; set; }
        public string Imagen { get; set; }
        public int stock { get; set; }

        public int? cant {  get; set; }
        public int Id_Catalogo { get; set; }    

    }
}
