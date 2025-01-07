using DataAccessLayer;
using DataAccessLayer.Mappers;
using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class BLL_Categorias
    {
        MP_Categorias  mp = new MP_Categorias();

        public List<Categorias> listarCat()
        {
            DataTable dt = mp.ListarCategorias();

            List<Categorias> cat = Helper.DataTableToList<Categorias>(dt);

            return cat;

        }


    }
}
