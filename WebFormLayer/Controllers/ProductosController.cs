using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entities;
using WebFormLayer.Models;

namespace WebFormLayer.Controllers
{
    public class ProductosController : Controller
    {
        private readonly BLL_Productos Pro;


        public ActionResult Index()
        {
            try
            { 
                var productos = Pro.ObtenerProductos();

                if(productos.Count > 0)
                {
                    return View(productos);
                }
           
            }
            catch(Exception ex)
            {
                TempData["Message"] = ex.Message;
            }
            return View("Productos", "Home");
        }

        public ProductosController()
        {
            Pro = new BLL_Productos();
        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]

        public ActionResult Create(ProductoViewModel producto)
        {
            try
            {
                if(ModelState.IsValid)
                {

                    Productos prod = new Productos
                    {
                        Id_Producto = producto.Id_Producto,
                        Nombre = producto.Nombre,
                        Codigo = producto.Codigo,
                        Descripcion = producto.Descripcion,
                        Categoria = producto.Categoria,
                        Activo = producto.Activo,
                        Imagen = producto.Imagen,
                        Precio = producto.Precio,

                    };



                    if (prod != null)
                    {
                        Pro.AgregarProducto(prod);
                    }


                    TempData["Message"] = "Producto Creado Correctamente";

                    return View("Productos","Home");


                }
                

            }
            catch(Exception ex) 
            {
                TempData["Message"] = ex.Message;
            }

            return View(producto);
        }

     
    }
}
