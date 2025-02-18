using Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Mappers
{
    public class MP_Venta
    {
        private readonly Conexion cn = new Conexion();
        public int Registrar(Ventas vt, DataTable detalleVenta, out string Mensaje)
        {
            Mensaje = string.Empty;

            // Validación del parámetro 'detalleVenta'
            if (detalleVenta == null || detalleVenta.Rows.Count == 0)
            {
                Mensaje = "El detalle de la venta está vacío o es inválido.";
                return 0; // Indica que no se puede registrar sin un detalle válido
            }

            try
            {
                SqlParameter parametroMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 500)
                {
                    Direction = ParameterDirection.Output
                };

                SqlParameter parametroResultado = new SqlParameter("@Resultado", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                SqlParameter[] parametros = new SqlParameter[]
                {
                new SqlParameter("@Id_Cliente", vt.Id_Cliente),
                new SqlParameter("@Ip_Cliente", vt.Ip_Cliente),
                new SqlParameter("@TotalProducto", vt.TotalProductos),
                new SqlParameter("@MontoTotal", vt.MontoTotal),
                new SqlParameter("@Direccion", vt.Direccion),
                new SqlParameter("@Id_Transaccion", vt.Id_Transaccion),
                new SqlParameter("@FormaPago", vt.FormaPago),
                new SqlParameter("@FormaRetiro", vt.FormaRetiro),
                new SqlParameter("@Id_Estado", vt.Id_Estado),
                new SqlParameter("@NroPedido", vt.NroPedido),
                new SqlParameter("@Id_Localidad", vt.Id_Localidad),

                // Verificar si el DataTable es null
                new SqlParameter("@DetalleVenta", detalleVenta),

                parametroMensaje,
                parametroResultado,
                };

                // Ejecución del procedimiento almacenado
                cn.Escribir("RegistrarVenta", parametros);

                // Obtener el mensaje y resultado de la operación
                Mensaje = parametroMensaje.Value?.ToString();

                // Devolver el resultado
                return (parametroResultado.Value != DBNull.Value) ? Convert.ToInt32(parametroResultado.Value) : 0;
            }
            catch (Exception ex)
            {
                // Manejo de errores y log
                Mensaje = "Error en producción: " + ex.Message;
                Console.WriteLine("Error en producción: " + ex.ToString());
                return 0; // Indica que hubo un error al registrar la venta
            }
        }




        public DataTable GetComprasCliente(int idcliente)
        {
            SqlParameter[] sp = new SqlParameter[]
            {
                new SqlParameter("@Id_Cliente",idcliente)
            };

            return cn.Leer("GetComprasCliente", sp);
        }

        public object ObtenerCorelativo()
        {
            return cn.ObetenerDatos("ObtenerCorrelativo");
        }

    }
}
