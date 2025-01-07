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
        public string Nombre { get; set; }
        public string Descripcion{ get; set; }
        public decimal Precio { get; set; }
        public int Cod_Producto { get; set; } 
        public string Id_Categoria { get; set; }   

        public string NombreCategoria { get; set; }  
        public bool Activo { get; set; }
        public string Imagen { get; set; }
        public int stock { get; set; }



    }
}
