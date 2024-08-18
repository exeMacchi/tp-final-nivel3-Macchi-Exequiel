using DataAccess;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class BrandBBL
    {
        /// <summary>
        /// Obtener todas las <see cref="Brand"/> desde la base de datos.
        /// </summary>
        /// <returns>Lista de <see cref="Brand"/></returns>
        public static List<Brand> GetBrands()
        {
            DataBase db = new DataBase();
            List<Brand> brands = new List<Brand>();
            string query = "SELECT Id AS BrandID, " +
                           "       Descripcion AS BrandDescription " +
                           "FROM MARCAS;";

            try
            {
                db.SetQuery(query);
                db.ExecuteRead();

                while (db.Reader.Read())
                {
                    Brand brand = new Brand((int)db.Reader["BrandID"], db.Reader["BrandDescription"].ToString());
                    brands.Add(brand);
                }
                return brands;
            }
            catch (Exception ex)
            {
                throw ex; // El error se propaga hacia arriba
            }
            finally
            {
                db.CloseConnection();
            }
        }
    }
}
