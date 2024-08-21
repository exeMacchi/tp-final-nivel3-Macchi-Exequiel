using DataAccess;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class CategoryBLL
    {
        /// <summary>
        /// Obtener todas las <see cref="Category"/> desde la base de datos.
        /// </summary>
        public static List<Category> GetCategories()
        {
            DataBase db = new DataBase();
            List<Category> categories = new List<Category>();
            string query = "SELECT Id AS CategoryID, " +
                           "       Descripcion AS CategoryDescription " +
                           "FROM CATEGORIAS;";

            try
            {
                db.SetQuery(query);
                db.ExecuteRead();

                while (db.Reader.Read())
                {
                    Category category = new Category((int)db.Reader["CategoryID"], db.Reader["CategoryDescription"].ToString());
                    categories.Add(category);
                }
                return categories;
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
