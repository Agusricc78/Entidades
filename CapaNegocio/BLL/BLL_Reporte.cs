using CapaDatos.Mappers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.BLL
{
    public class BLL_Reporte
    {

        MP_Reportes obj = new MP_Reportes();

        public DataTable GetReporte()
        {
            return obj.GetReportes();
        }

        public DataTable GetPedidosPendientes(string fechacreacion,string fechafin, string nropedido,int idestado)
        {
            return obj.GetPedidosPendientes(fechacreacion, fechafin, nropedido,idestado);
        }


        public int UpdateEstado(string nropedido, out string Mensaje)
        {

            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(nropedido) || string.IsNullOrWhiteSpace(nropedido))
            {
                Mensaje = "El Nro de Pedido no puede estar vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return obj.UpdateEstado(nropedido, out Mensaje);
            }
            else
            {
                return 0;
            }

       
        }
    }
}
