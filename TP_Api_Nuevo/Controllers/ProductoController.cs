using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dominio;
using Negocio;

namespace TP_Api_Nuevo.Controllers
{
    public class ProductoController : ApiController
    {
        ProductoNegocio negocio = new ProductoNegocio();

        // LISTAR TODOS
        [HttpGet]
        [Route("api/Producto")]
        public List<Producto> Get()
        {
            return negocio.Listar();
        }

        // BUSCAR POR ID 
        [HttpGet]
        [Route("api/Producto/{id}")]
        public object Get(int id)
        {
            Producto producto = negocio.Buscar(id);

            if (producto == null)
                return "No existe un producto con ese ID.";

            return producto;
        }

        // AGREGAR PRODUCTO 
        [HttpPost]
        [Route("api/Producto")]
        public string Post(ProductoDto dto)
        {
            if (dto == null)
                return "Datos inválidos.";

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                return "El nombre es obligatorio.";

            if (string.IsNullOrWhiteSpace(dto.Codigo))
                return "El código es obligatorio.";

            if (dto.Precio <= 0)
                return "El precio debe ser mayor a 0.";

            MarcaNegocio marcaNegocio = new MarcaNegocio();
            Marca marca = marcaNegocio.ObtenerPorId(dto.IdMarca);
            if (marca == null)
                return "La marca no existe.";

            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
            Categoria categoria = categoriaNegocio.ObtenerPorId(dto.IdCategoria);
            if (categoria == null)
                return "La categoría no existe.";

            try
            {
                negocio.AgregarDto(dto);
                return "Producto agregado correctamente.";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        // AGREGAR IMAGENES UNA POR VEZ  
        [HttpPost]
        [Route("api/Producto/{id}/imagenes")]
        public string AgregarImagenes(int id, List<string> imagenes)
        {
            if (imagenes == null || imagenes.Count == 0)
                return "Debe enviar al menos una imagen.";

            Producto producto = negocio.Buscar(id);
            if (producto == null)
                return "No existe un producto con ese ID.";

            try
            {
                negocio.AgregarImagenes(id, imagenes);
                return "Imágenes agregadas correctamente.";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        // MODIFICAR PRODUCTO
        [HttpPut]
        [Route("api/Producto/{id}")]
        public string Put(int id, ProductoDto dto)
        {
            if (dto == null)
                return "Datos inválidos.";

            Producto existente = negocio.Buscar(id);
            if (existente == null)
                return "No existe un producto con ese ID.";

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                return "El nombre es obligatorio.";

            if (dto.Precio <= 0)
                return "El precio debe ser mayor a 0.";

            try
            {
                negocio.ModificarDto(id, dto);
                return "Producto modificado correctamente.";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        // ELIMINAR PRODUCTO
        [HttpDelete]
        [Route("api/Producto/{id}")]
        public string Delete(int id)
        {
            Producto producto = negocio.Buscar(id);
            if (producto == null)
                return "No existe un producto con ese ID.";

            try
            {
                negocio.Eliminar(id);
                return "Producto eliminado correctamente.";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
    }
}