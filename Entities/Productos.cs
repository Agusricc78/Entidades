using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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
        public int Id_Categoria { get; set; }   
        public string NombreCategoria { get; set; }  
        public bool Activo { get; set; }
        public string Electrico{ get; set; }
        public byte[] Imagen { get; set; }
        public string ExtImagen { get; set; }
        public int stock { get; set; }

        public int? cant {  get; set; }
        public int Id_Catalogo { get; set; }  
        
        public string Nombre {  get; set; }

    }
}
