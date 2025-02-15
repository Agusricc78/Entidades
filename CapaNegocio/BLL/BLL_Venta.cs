using CapaDatos.Mappers;
using Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.BLL
{
    public class BLL_Venta
    {
        MP_Venta obj = new MP_Venta();

        public int Registrar(Ventas vt, DataTable detalleVenta,string correo ,out string Mensaje)
        {
            Mensaje = string.Empty;
            int resultado = obj.Registrar(vt, detalleVenta, out Mensaje);

            if(resultado > 0)
            {
                string asunto = "Compra realizada con Exito!!";
                string Mensaje_correo = "<h3> Gracias por comprar en Costa Azul!!</h3></br><P> Su Nro de Pedido es: !pedido!</P> </br>";
                Mensaje_correo = Mensaje_correo.Replace("!pedido!", vt.NroPedido);

                string tablaDetalle = "<h4>Detalles de la compra:</h4><table border='1' style='border-collapse: collapse;'>";
                tablaDetalle += "<tr><th>Nombre</th><th>Cantidad</th><th>Total</th></tr>";

                CultureInfo culturaArgentina = new CultureInfo("es-AR");

                foreach (DataRow row in detalleVenta.Rows)
                {
                    tablaDetalle += "<tr>";
                    tablaDetalle += "<td>" + row["NombreProducto"].ToString() + "</td>";
                    tablaDetalle += "<td>" + row["Cantidad"].ToString() + "</td>";
                    tablaDetalle += "<td>" + Convert.ToDecimal(row["Total"]).ToString("C2", culturaArgentina) + "</td>";
                    tablaDetalle += "</tr>";
                }

                tablaDetalle += "</table> </br>";

                string Total = "<h3> Total: $ !total!</h3>";
                Total = Total.Replace("!total!", vt.MontoTotal.ToString());

                Mensaje_correo += tablaDetalle + Total;

                bool respuesta = BLL_Recursos.EnviarCorreo(correo, asunto, Mensaje_correo);


                string asunto2 = "Compra Realizada";
                string Mensaje_2 = "<h3> Nro de pedidod: !pedido!</h3> </br>";
                Mensaje_2 = Mensaje_2.Replace("!pedido!", vt.NroPedido);

                string Idtrans = "<h3> Id_Transaccion: !TR!</h3>";
                Idtrans = Idtrans.Replace("!TR!", vt.Id_Transaccion);

                Mensaje_2 += Idtrans;

                BLL_Recursos.EnviarCorreo("nicolasspada9@gmail.com", asunto2, Mensaje_2);

                if (respuesta)
                {
                    return 1;
                }
                else
                {
                    Mensaje = "No se pudo restablecer la contraseña";
                    return 0;
                }
            }
            else
            {
                return 0;
            }
    
        }


        public DataTable GetComprasCliente(int idcliente)
        {
            return obj.GetComprasCliente(idcliente);
        }


        public object ObtenerCorrelativo()
        {
            return obj.ObtenerCorelativo();
        }
        
    }
}
