using ConexionBD;
using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class ImagenNegocio
    {
        public List<Imagen> ListarPorProducto(int id)
        {
            List<Imagen> lista = new List<Imagen>();
            ConexionBD.AccesoDatos datos = new ConexionBD.AccesoDatos();

            try
            {
                datos.SetearConsulta("SELECT Id, IdArticulo, ImagenUrl FROM IMAGENES WHERE IdArticulo = @id");
                datos.SetearParametro("@id", id);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Imagen img = new Imagen
                    {
                        Id = (int)datos.Lector["Id"],
                        IdArticulo = (int)datos.Lector["IdArticulo"],
                        ImagenUrl = datos.Lector["ImagenUrl"].ToString()
                    };

                    lista.Add(img);
                }

                return lista;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void AgregarImagenes(int idProducto, List<string> urls)
        {
            ConexionBD.AccesoDatos datos = new ConexionBD.AccesoDatos();

            try
            {
                foreach (string url in urls)
                {
                    datos.SetearConsulta("INSERT INTO IMAGENES (IdArticulo, ImagenUrl) VALUES (@id, @url)");
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
    }
}
