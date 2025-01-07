using DataAccesLayer;
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
        DataAccesLayer.Mappers.MP_Categorias  mp = new DataAccesLayer.Mappers.MP_Categorias();

        public List<Entities.Categorias> listarCat()
        {
            DataTable dt = mp.ListarCategorias();

            List<Entities.Categorias> cat = DataAccessLayer.Helper.DataTableToList<Entities.Categorias>(dt);

            return cat;

        }


    }
}
