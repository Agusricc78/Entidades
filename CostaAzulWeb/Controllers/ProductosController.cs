using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entities;
using CostaAzulWeb.Models;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Grpc.Core;
using System.Reflection;

namespace CostaAzulWeb.Controllers
{
    public class ProductosController : Controller
    {
        private readonly BLL_Productos _pro;
        private readonly BLL_Categorias _cat;

        public ProductosController()
        {
            _pro = new BLL_Productos();
            _cat = new BLL_Categorias();
        }

        [HttpGet]
        public IActionResult Productos()
        {
            try
            {
                var categorias = _cat.listarCat();
                var productos = _pro.ObtenerProductos();

                var model = new ProductoViewModel
                {
                    Categorias = categorias,
                    ListaProductos = productos,
                };

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Error al cargar las categorías: {ex.Message}";
                return View(new ProductoViewModel());
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new ProductoViewModel());
        }

        [HttpPost]
        public IActionResult Create(ProductoViewModel producto, IFormFile Imagen)
        {
            try
            {
                ModelState.Remove(nameof(producto.ListaProductos)); // No validar ListaProductos
                ModelState.Remove(nameof(producto.CategoriasSelectList));
                ModelState.Remove(nameof(producto.Id_Producto));
                ModelState.Remove(nameof(producto.NombreCategoria));
                ModelState.Remove(nameof(producto.Categorias));
                ModelState.Remove(nameof(producto.Imagen));

                if (ModelState.IsValid)
                {
                    // Verificar existencia
                    if (_pro.ValidarExistencia(producto.Nombre, producto.Codigo))
                    {
                        TempData["Message"] = "El producto ya existe.";
                        return RedirectToAction("Productos");
                    }

                    // Guardar imagen si se subió
                    if (Imagen != null && Imagen.Length > 0)
                    {
                        var fileName = Path.GetFileName(Imagen.FileName);
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Img", fileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            Imagen.CopyTo(stream);
                        }
                        producto.Imagen = fileName;
                    }

                    // Crear entidad Producto y guardar en la base de datos
                    var prod = new Productos
                    {
                        Nombre = producto.Nombre,
                        Cod_Producto = producto.Codigo,
                        Descripcion = producto.Descripcion,
                        Id_Categoria = producto.Id_Categoria,
                        Activo = producto.Activo,
                        Imagen = producto.Imagen,
                        Precio = producto.Precio,
                        stock = producto.Stock,
                    };

                    _pro.AgregarProducto(prod);
                    TempData["Message"] = "Producto creado correctamente.";
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Error al crear el producto: {ex.Message}";
            }

            return RedirectToAction("Productos");
        }

       

        [HttpPost]
        public IActionResult Edit(ProductoViewModel model, IFormFile Imagen)
        {
            try
            {
                var objeto = _pro.ObtenerProducto(model.Codigo);


                ModelState.Remove(nameof(model.ListaProductos)); // No validar ListaProductos
                ModelState.Remove(nameof(model.CategoriasSelectList));
                ModelState.Remove(nameof(model.NombreCategoria));
                ModelState.Remove(nameof(model.Categorias));
                ModelState.Remove(nameof(model.Imagen));


                if (ModelState.IsValid)
                {
                    // Guardar nueva imagen si se sube
                    if (Imagen != null && Imagen.Length > 0)
                    {
                        var fileName = Path.GetFileName(Imagen.FileName);
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Img", fileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            Imagen.CopyTo(stream);
                        }
                        objeto.Imagen = fileName;
                    }

                    // Actualizar producto
                    var prod = new Productos
                    {
                        Id_Producto = objeto.Id_Producto,
                        Nombre = objeto.Nombre,
                        Cod_Producto = objeto.Cod_Producto,
                        Descripcion = objeto.Descripcion,
                        Id_Categoria = objeto.Id_Categoria,
                        Activo = objeto.Activo,
                        Imagen = objeto.Imagen,
                        Precio = objeto.Precio,
                        stock = objeto.stock,
                    };

                    _pro.EditarProducto(objeto);

                    TempData["Message"] = "Producto actualizado correctamente.";
                    return RedirectToAction("Productos");
                }

                TempData["Message"] = "Por favor, completa todos los campos requeridos.";
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Error al actualizar el producto: {ex.Message}";
            }

            return RedirectToAction("Productos");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                var producto = _pro.ObtenerProducto(id);

                if (producto == null)
                {
                    TempData["Message"] = "El producto no existe.";
                    return RedirectToAction("Productos");
                }

                _pro.EliminarProducto(id);
                TempData["Message"] = "Producto eliminado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Error al eliminar el producto: {ex.Message}";
            }

            return RedirectToAction("Productos");
        }

        [HttpPost]
        public IActionResult ObtenerProductos()
        {
            try
            {
                var productos = _pro.ObtenerProductos();
                return PartialView("ProductosPartial", productos);
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Error al cargar los productos: {ex.Message}";
                return StatusCode(500, ex.Message);
            }
        }
    }
}
