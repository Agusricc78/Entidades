using CapaNegocio;
using CapaNegocio.BLL;
using ClosedXML.Excel;
using Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace CapaPresentacionAdmin.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        BLL_Login log = new BLL_Login();
        BLL_Reporte rep = new BLL_Reporte();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Usuarios()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAllUsers()
        {
            DataTable dt = log.GetAllUsers();

            // Convierte el DataTable a una lista de objetos anónimos o DTOs
            var users = dt.AsEnumerable().Select(row => new
            {
                Id = row["Id_Usuario"],
                Nombre = row["Nombre"],
                Apellido = row["Apellido"],
                Telefono = row["Telefono"],
                Correo = row["Correo"],
                Activo = row["Activo"],
                Tipo = row["Tipo"]
            }).ToList();

            return Json(new { data = users }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarUsuario(Usuario us)
        {
            object Resultado;
            string Mensaje = string.Empty;

            if (us.Id_Usuario == 0)
            {

                Resultado = new BLL_Login().RegistrarUsuario(us, out Mensaje);

            }
            else
            {
                Resultado = new BLL_Login().UpdateUsuario(us, out Mensaje);
            }


            return Json(new { resultado = Resultado, mensaje = Mensaje}, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]

        public JsonResult GetReportes()
        {
            DataTable dt = rep.GetReporte();

            var objeto = dt.AsEnumerable().Select(row => new
            {
                TotalProductos = row["TotalProductos"],
                TotalVentas = row["TotalVentas"],
                TotalClientes = row["TotalClientes"]
            }).ToList();

            return Json(new { resultado = objeto }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]

        public JsonResult GetPedidosPendientes(string fechacreacion, string fechafin, string nropedido,int idestado)
        {
            try
            {
                DataTable dt = rep.GetPedidosPendientes(fechacreacion, fechafin, nropedido,idestado);

                var pedidos = dt.AsEnumerable().Select(row => new
                {
                    FechaCreacion = row["FechaCreacion"],
                    NroPedido = row["NroPedido"],
                    Nombre = row["Nombre"],
                    Producto = row["Productos"],
                    Cantidad = row["Cantidad"],
                    Totales = row["Total"],
                    Estado = row["Estado"],
                }).ToList();

                return Json(new { data = pedidos }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    
        [HttpPost]
        public JsonResult UpdateEstado(string nropedido)
        {

            object Resultado;
            string Mensaje = string.Empty;

            Resultado = new BLL_Reporte().UpdateEstado(nropedido, out Mensaje);

            return Json(new { resultado = Resultado, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public FileResult ExportarPedidos(string fechacreacion, string fechafin, string nropedido, int idestado) {

           
            DataTable dt = new BLL_Reporte().GetPedidosPendientes(fechacreacion, fechafin, nropedido,idestado);

            dt.TableName = "Datos";

            using(XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    return File(ms.ToArray(), "application/vnd.openxmlformats-officedocuments.spreadsheetml.sheet", "ReportePedidos" + DateTime.Now.ToString() + ".xlsx");

                }
            }
        }
    }
}