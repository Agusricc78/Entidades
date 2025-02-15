using CapaNegocio;
using CapaNegocio.BLL;
using Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    [Authorize]
    public class MantenedorController : Controller
    {
        BLL_Categoria cat = new BLL_Categoria();
        BLL_Linea lin = new BLL_Linea();
        BLL_Producto prod = new BLL_Producto();
        // GET: Mantenedor
        public ActionResult Categoria()
        {
            return View();
        }
        public ActionResult Linea()
        {
            return View();
        }
        public ActionResult Producto()
        {
            return View();
        }


        #region Categoria
        [HttpGet]
        public JsonResult GetAllCategorias()
        {
            try
            {
                DataTable dt = cat.GetAllCategorias(); // Considera agregar lógica de paginación en tu método

                var categorias = dt.AsEnumerable().Select(row => new
                {
                    Id_Categoria = row["Id_Categoria"],
                    Nombre = row["Nombre"],
                    Activo = Convert.ToBoolean(row["Activo"]) // Asegúrate de que sea un booleano
                }).ToList();

                return Json(new { data = categorias }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Si hay un error, lo logueas y lo devuelves en la respuesta
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpPost]
        public JsonResult GuardarCategoria(Categorias ct)
        {
            object Resultado;
            string Mensaje = string.Empty;

            if (ct.Id_Categoria == 0)
            {

                Resultado = new BLL_Categoria().RegistrarCategoria(ct, out Mensaje);

            }
            else
            {
                Resultado = new BLL_Categoria().UpdateCategoria(ct, out Mensaje);
            }


            return Json(new { resultado = Resultado, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarCategoria(int Id_Categoria)
        {
            object Resultado;
            string Mensaje = string.Empty;

            Resultado = new BLL_Categoria().EliminarCategoria(Id_Categoria, out Mensaje);

            return Json(new { resultado = Resultado, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Linea
        [HttpGet]
        public JsonResult GetAllLineas()
        {
            try
            {
                DataTable dt = lin.GetAllLineas(); 

                var lineas = dt.AsEnumerable().Select(row => new
                {
                    Id_Linea = row["Id_Linea"],
                    Nombre = row["Nombre"],
                    Activo = Convert.ToBoolean(row["Activo"]) // Asegúrate de que sea un booleano
                }).ToList();

                return Json(new { data = lineas }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Loguea el error y lo devuelves en la respuesta
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult GuardarLinea(Linea ln)
        {
            object Resultado;
            string Mensaje = string.Empty;

            if (ln.Id_Linea == 0)
            {

                Resultado = new BLL_Linea().RegistrarLinea(ln, out Mensaje);

            }
            else
            {
                Resultado = new BLL_Linea().UpdateLinea(ln, out Mensaje);
            }


            return Json(new { resultado = Resultado, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarLinea(int Id_Linea)
        {
            object Resultado;
            string Mensaje = string.Empty;

            Resultado = new BLL_Linea().EliminarLinea(Id_Linea, out Mensaje);

            return Json(new { resultado = Resultado, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion



        #region Productos

        [HttpGet]

        public JsonResult GetAllProductos()
        {
            DataTable dt = prod.GetAllProductos();

            // Convierte el DataTable a una lista de objetos anónimos o DTOs
            var Productos = dt.AsEnumerable().Select(row => new
            {
                Id_Producto = row["Id_Producto"],
                Nombre = row["Nombre"],
                Descripcion = row["Descripcion"],
                Precio = row["Precio"],
                Cod_Producto = row["Cod_Producto"],
                Id_Categoria = row["Id_Categoria"],
                Categoria = row["Categoria"],
                Id_Catalogo = row["Id_Catalogo"],
                Catalogo = row["Catalogo"],
                Id_Linea = row["Id_Linea"],
                Linea = row["Linea"],
                Activo = row["Activo"],
                Electrico = row["Electrico"],
                Stock = row["Stock"],
                ImagenBase64 = row["Imagen"] != DBNull.Value ? Convert.ToBase64String((byte[])row["Imagen"]) : null,
                ExtImagen = row["ExtImagen"] != DBNull.Value ? row["ExtImagen"].ToString() : ""
            }).ToList();

            return Json(new { data = Productos }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarProducto(Productos pr, HttpPostedFileBase fileProducto)
        {
            try
            {
                int Resultado = 0;
                string Mensaje = string.Empty;

                // Si hay imagen, convertirla a byte[]
                if (fileProducto != null)
                {
                    using (var binaryReader = new BinaryReader(fileProducto.InputStream))
                    {
                        pr.Imagen = binaryReader.ReadBytes(fileProducto.ContentLength);
                    }
                    pr.ExtImagen = fileProducto.FileName;
                }

                // Registrar o actualizar producto
                if (pr.Id_Producto == 0)
                {
                    Resultado = new BLL_Producto().RegistrarProducto(pr, out Mensaje);
                    if (Resultado != 0)
                    {
                        pr.Id_Producto = Resultado;
                    }
                }
                else
                {
                    Resultado = new BLL_Producto().UpdateProducto(pr, out Mensaje);
                }

                return Json(new { resultado = Resultado, idGenerado = pr.Id_Producto, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Log de error
                return Json(new { resultado = 0, idGenerado = 0, mensaje = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult EliminarProducto(int Id_Producto)
        {
            object Resultado;
            string Mensaje = string.Empty;

            Resultado = new BLL_Producto().EliminarProducto(Id_Producto, out Mensaje);

            return Json(new { resultado = Resultado, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}