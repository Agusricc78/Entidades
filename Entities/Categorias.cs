using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Categorias
    {
		private int id_Categoria;

		public int Id_Categoria
		{
			get { return id_Categoria; }
			set { id_Categoria = value; }
		}

		private string nombre;

		public string Nombre
		{
			get { return nombre; }
			set { nombre = value; }
		}


	}
}
