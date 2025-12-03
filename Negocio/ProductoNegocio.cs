using ConexionBD;
using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class ProductoNegocio
    {
        public List<Producto> Listar()
        {
            List<Producto> lista = new List<Producto>();
            ConexionBD.AccesoDatos datos = new ConexionBD.AccesoDatos();

            try
            {
                datos.SetearConsulta(
                    @"SELECT A.Id, A.Codigo, A.Nombre, A.Descripcion, A.Precio,
                             M.Id AS IdMarca, M.Descripcion AS MarcaDesc,
                             C.Id AS IdCategoria, C.Descripcion AS CatDesc
                      FROM ARTICULOS A
                      LEFT JOIN MARCAS M ON A.IdMarca = M.Id
                      LEFT JOIN CATEGORIAS C ON A.IdCategoria = C.Id");

                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Producto p = new Producto();

                    p.Id = (int)datos.Lector["Id"];
                    p.Codigo = datos.Lector["Codigo"].ToString();
                    p.Nombre = datos.Lector["Nombre"].ToString();
                    p.Descripcion = datos.Lector["Descripcion"].ToString();
                    p.Precio = (decimal)datos.Lector["Precio"];

                    p.Marca = new Marca
                    {
                        Id = datos.Lector["IdMarca"] != DBNull.Value ? (int)datos.Lector["IdMarca"] : 0,
                        Descripcion = datos.Lector["MarcaDesc"]?.ToString()
                    };

                    p.Categoria = new Categoria
                    {
                        Id = datos.Lector["IdCategoria"] != DBNull.Value ? (int)datos.Lector["IdCategoria"] : 0,
                        Descripcion = datos.Lector["CatDesc"]?.ToString()
                    };

                    lista.Add(p);
                }

                return lista;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }


        public int Agregar(Producto p)
        {
            ConexionBD.AccesoDatos datos = new ConexionBD.AccesoDatos();

            try
            {
                datos.SetearConsulta(
                    @"INSERT INTO ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, Precio)
                      VALUES (@codigo, @nombre, @desc, @marca, @cat, @precio);
                      SELECT SCOPE_IDENTITY();");

                datos.SetearParametro("@codigo", p.Codigo);
                datos.SetearParametro("@nombre", p.Nombre);
                datos.SetearParametro("@desc", p.Descripcion);
                datos.SetearParametro("@marca", p.Marca.Id);
                datos.SetearParametro("@cat", p.Categoria.Id);
                datos.SetearParametro("@precio", p.Precio);

                datos.EjecutarLectura();

                if (datos.Lector.Read())
                    return Convert.ToInt32(datos.Lector[0]);

                return 0;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void AgregarDto(ProductoDto dto)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(
                    "INSERT INTO ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, Precio) " +
                    "VALUES (@cod, @nom, @desc, @marca, @cat, @precio)");

                datos.SetearParametro("@cod", dto.Codigo);
                datos.SetearParametro("@nom", dto.Nombre);
                datos.SetearParametro("@desc", dto.Descripcion);
                datos.SetearParametro("@marca", dto.IdMarca);
                datos.SetearParametro("@cat", dto.IdCategoria);
                datos.SetearParametro("@precio", dto.Precio);

                datos.EjecutarAccion();
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void ModificarDto(int id, ProductoDto dto)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(
                    "UPDATE ARTICULOS SET Codigo = @cod, Nombre = @nom, Descripcion = @desc, " +
                    "IdMarca = @marca, IdCategoria = @cat, Precio = @precio " +
                    "WHERE Id = @id");

                datos.SetearParametro("@cod", dto.Codigo);
                datos.SetearParametro("@nom", dto.Nombre);
                datos.SetearParametro("@desc", dto.Descripcion);
                datos.SetearParametro("@marca", dto.IdMarca);
                datos.SetearParametro("@cat", dto.IdCategoria);
                datos.SetearParametro("@precio", dto.Precio);
                datos.SetearParametro("@id", id);

                datos.EjecutarAccion();
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void EliminarDto(int id)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("DELETE FROM ARTICULOS WHERE Id = @id");
                datos.SetearParametro("@id", id);
                datos.EjecutarAccion();
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void AgregarImagenes(int idProducto, List<string> urls)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                foreach (var url in urls)
                {
                    datos.SetearConsulta(
                        "INSERT INTO IMAGENES (IdArticulo, ImagenUrl) VALUES (@id, @url)");
                    datos.SetearParametro("@id", idProducto);
                    datos.SetearParametro("@url", url);
                    datos.EjecutarAccion();
                }
            }
            finally
            {
                datos.CerrarConexion();
            }
        }


        public void Modificar(Producto p)
        {
            ConexionBD.AccesoDatos datos = new ConexionBD.AccesoDatos();

            try
            {
                datos.SetearConsulta(
                    @"UPDATE ARTICULOS SET 
                        Codigo = @codigo,
                        Nombre = @nombre,
                        Descripcion = @desc,
                        IdMarca = @marca,
                        IdCategoria = @cat,
                        Precio = @precio
                      WHERE Id = @id");

                datos.SetearParametro("@codigo", p.Codigo);
                datos.SetearParametro("@nombre", p.Nombre);
                datos.SetearParametro("@desc", p.Descripcion);
                datos.SetearParametro("@marca", p.Marca.Id);
                datos.SetearParametro("@cat", p.Categoria.Id);
                datos.SetearParametro("@precio", p.Precio);
                datos.SetearParametro("@id", p.Id);

                datos.EjecutarAccion();
            }
            finally
            {
                datos.CerrarConexion();
            }
        }


        public void Eliminar(int id)
        {
            ConexionBD.AccesoDatos datos = new ConexionBD.AccesoDatos();

            try
            {
                datos.SetearConsulta("DELETE FROM ARTICULOS WHERE Id = @id");
                datos.SetearParametro("@id", id);
                datos.EjecutarAccion();
            }
            finally
            {
                datos.CerrarConexion();
            }
        }


        public Producto Buscar(int id)
        {
            List<Producto> lista = Listar();
            return lista.Find(x => x.Id == id);
        }
    }
}
