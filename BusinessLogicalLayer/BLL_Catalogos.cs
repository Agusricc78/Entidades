using DataAccesLayer.Mappers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicalLayer
{
    public class BLL_Catalogos
    {
        MP_Catalogo mp = new MP_Catalogo();

        public List<Entities.Catalogos> Listar()
        {
            DataTable dt = mp.ListarCatalogos();

            List<Entities.Catalogos> cat = DataAccessLayer.Helper.DataTableToList<Entities.Catalogos>(dt);

            return cat;
        }


    }
}
