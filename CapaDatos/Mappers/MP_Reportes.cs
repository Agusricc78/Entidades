using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Mappers
{
    public class MP_Reportes
    {
        private readonly Conexion cn = new Conexion();

        public DataTable GetReportes()
        {
            return cn.Leer("Reportes");
        }

        public DataTable GetPedidosPendientes(string fechaRegistro, string fechafin, string nropedido,int idEstado)
        {
            SqlParameter[] sp = new SqlParameter[]
            {
                new SqlParameter("@FechaCreacion",fechaRegistro),
                new SqlParameter("@FechaFin",fechafin),
                new SqlParameter("@NroPedido",nropedido),
                new SqlParameter("@Id_Estado",idEstado)
            };

            return cn.Leer("ReportesPedidos", sp);
        }


        public int UpdateEstado(string nropedido, out string Mensaje)
        {

            Mensaje = string.Empty;


            SqlParameter parametroMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 500)
            {
                Direction = ParameterDirection.Output
            };

            SqlParameter parametroResultado = new SqlParameter("@Resultado", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            SqlParameter[] sp = new SqlParameter[]
            {
                new SqlParameter("@NroPedido",nropedido),
                parametroMensaje,
                parametroResultado
            };

            cn.Escribir("UpadateEstadoCarrito", sp);

            Mensaje = parametroMensaje.Value?.ToString(); // Evitar `null`
            return (parametroResultado.Value != DBNull.Value) ? Convert.ToInt32(parametroResultado.Value) : 0;
        }

       
    }
}
