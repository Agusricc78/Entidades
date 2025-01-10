using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicalLayer
{
    public class BLL_Lineas
    {
        DataAccesLayer.Mappers.MP_Lineas mp = new DataAccesLayer.Mappers.MP_Lineas();

        public List<Entities.Linea> listarCat()
        {
            DataTable dt = mp.ListarLi();

            List<Entities.Linea> cat = DataAccessLayer.Helper.DataTableToList<Entities.Linea>(dt);

            return cat;

        }




    }
}
