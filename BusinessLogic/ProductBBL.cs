using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Domain;

namespace BusinessLogic
{
    public class ProductBBL
    {
        /// <summary>
        /// Obtener todos los productos desde la base de datos.
        /// </summary>
        /// <returns>Lista con todos los productos en la base de datos.</returns>
        public static List<Product> GetProducts()
        {
            List<Product> products = new List<Product>();
            DataBase db = new DataBase();
            string query = "SELECT A.Id AS ProductID, " +
                           "       A.Codigo AS ProductCode, " +
                           "       A.Nombre AS ProductName," +
                           "       A.Descripcion AS ProductDescription, " +
                           "       A.ImagenUrl AS ProductImage, " +
                           "       A.Precio AS ProductPrice, " +
                           "       C.Id AS CategoryID, " +
                           "       C.Descripcion AS CategoryDescription, " +
                           "       M.Id AS BrandID, " +
                           "       M.Descripcion AS BrandDescription " +
                           "FROM ARTICULOS AS A " +
                           "INNER JOIN CATEGORIAS AS C ON A.IdCategoria = C.Id " +
                           "INNER JOIN MARCAS AS M ON A.IdMarca = M.Id; ";

            try
            {
                db.SetQuery(query);
                db.ExecuteRead();
                while (db.Reader.Read())
                {
                    Product product = new Product();

                    product.ID = (int)db.Reader["ProductID"];

                    if (!(db.Reader["ProductCode"] is DBNull))
                        product.Code = db.Reader["ProductCode"].ToString();

                    if (!(db.Reader["ProductName"] is DBNull))
                        product.Name = db.Reader["ProductName"].ToString();

                    if (!(db.Reader["ProductDescription"] is DBNull))
                        product.Description = db.Reader["ProductDescription"].ToString();

                    if (!(db.Reader["ProductImage"] is DBNull))
                        product.Image = Auxiliary.VerifyImage(db.Reader["ProductImage"].ToString());

                    if (!(db.Reader["ProductPrice"] is DBNull))
                        product.Price = (decimal)db.Reader["ProductPrice"];

                    if (!(db.Reader["CategoryID"] is DBNull || db.Reader["CategoryDescription"] is DBNull))
                        product.Category = new Category((int)db.Reader["CategoryID"],
                                                         db.Reader["CategoryDescription"].ToString());

                    if (!(db.Reader["BrandID"] is DBNull || db.Reader["BrandDescription"] is DBNull))
                        product.Brand = new Brand((int)db.Reader["BrandID"],
                                                   db.Reader["BrandDescription"].ToString());

                    products.Add(product);
                }

                return products;
            }
            catch (Exception ex)
            {
                // TODO: Manejar Error
                throw ex;
            }
            finally
            {
                db.CloseConnection();
            }
        }

        /// <summary>
        /// Obtener un <see cref="Product"/> desde la base de datos según un ID.
        /// </summary>
        /// <param name="id">ID del producto que se desea obtener en la base de datos.</param>
        /// <returns>Producto seleccionado.</returns>
        public static Product GetProduct(int id)
        {
            DataBase db = new DataBase();
            string query = "SELECT A.Id AS ProductID, " +
                           "       A.Codigo AS ProductCode, " +
                           "       A.Nombre AS ProductName, " +
                           "       A.Descripcion AS ProductDescription, " +
                           "       A.ImagenUrl AS ProductImage, " +
                           "       A.Precio AS ProductPrice, " +
                           "       C.Id AS CategoryID, " +
                           "       C.Descripcion AS CategoryDescription, " +
                           "       M.Id AS BrandID, " +
                           "       M.Descripcion AS BrandDescription " +
                           "FROM ARTICULOS AS A " +
                           "INNER JOIN CATEGORIAS AS C ON A.IdCategoria = C.Id " +
                           "INNER JOIN MARCAS AS M ON A.IdMarca = M.Id " +
                           "WHERE A.Id = @ProductID; ";

            try
            {
                db.SetQuery(query);
                db.SetParam("@ProductID", id);
                db.ExecuteRead();
                Product product = new Product();
                if (db.Reader.Read())
                {

                    product.ID = (int)db.Reader["ProductID"];

                    if (!(db.Reader["ProductCode"] is DBNull))
                        product.Code = db.Reader["ProductCode"].ToString();

                    if (!(db.Reader["ProductName"] is DBNull))
                        product.Name = db.Reader["ProductName"].ToString();

                    if (!(db.Reader["ProductDescription"] is DBNull))
                        product.Description = db.Reader["ProductDescription"].ToString();

                    if (!(db.Reader["ProductImage"] is DBNull))
                        product.Image = Auxiliary.VerifyImage(db.Reader["ProductImage"].ToString());

                    if (!(db.Reader["ProductPrice"] is DBNull))
                        product.Price = (decimal)db.Reader["ProductPrice"];

                    if (!(db.Reader["CategoryID"] is DBNull || db.Reader["CategoryDescription"] is DBNull))
                        product.Category = new Category((int)db.Reader["CategoryID"],
                                                         db.Reader["CategoryDescription"].ToString());

                    if (!(db.Reader["BrandID"] is DBNull || db.Reader["BrandDescription"] is DBNull))
                        product.Brand = new Brand((int)db.Reader["BrandID"],
                                                   db.Reader["BrandDescription"].ToString());
                }
                return product;
            }
            catch (Exception ex)
            {
                // TODO: Manejar Error
                throw ex;
            }
            finally
            {
                db.CloseConnection();
            }

        }

        /// <summary>
        /// Crear un nuevo <see cref="Product"/> en la base de datos.
        /// </summary>
        /// <param name="newProduct"><see cref="Product"/> con toda la información cargada.</param>
        public static void CreateProduct(Product newProduct)
        {
            DataBase db = new DataBase();
            string query = "INSERT INTO ARTICULOS " +
                           "VALUES (@ProductCode, @ProductName, @ProductDescription, @BrandID, @CategoryID, @ProductImage, @ProductPrice);";
            try
            {
                db.SetQuery(query);
                db.SetParam("@ProductCode", newProduct.Code);
                db.SetParam("@ProductName", newProduct.Name);
                db.SetParam("@ProductDescription", newProduct.Description);
                db.SetParam("@BrandID", newProduct.Brand.ID);
                db.SetParam("@CategoryID", newProduct.Category.ID);
                db.SetParam("@ProductImage", newProduct.Image);
                db.SetParam("@ProductPrice", newProduct.Price);
                db.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // TODO: Manejar Error
                throw ex;
            }
            finally
            {
                db.CloseConnection();
            }
        }


        /// <summary>
        /// Modificar un <see cref="Product"/> seleccionado en la base de datos.
        /// </summary>
        /// <param name="product"><see cref="Product"/> con la información cargada.</param>
        public static void UpdateProduct(Product product)
        {
            DataBase db = new DataBase();
            string query = "UPDATE ARTICULOS SET Codigo = @ProductCode, " +
                           "                     Nombre = @ProductName, " +
                           "                     Descripcion = @ProductDescription, " +
                           "                     IdMarca = @BrandID, " +
                           "                     IdCategoria = @CategoryID, " +
                           "                     ImagenUrl = @ProductImage, " +
                           "                     Precio = @ProductPrice " +
                           "WHERE Id = @ProductID;";

            try
            {
                db.SetQuery(query);
                db.SetParam("@ProductCode", product.Code);
                db.SetParam("@ProductName", product.Name);
                db.SetParam("@ProductDescription", product.Description);
                db.SetParam("@BrandID", product.Brand.ID);
                db.SetParam("@CategoryID", product.Category.ID);
                db.SetParam("@ProductImage", product.Image);
                db.SetParam("@ProductPrice", product.Price);
                db.SetParam("@ProductID", product.ID);
                db.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // TODO: Manejar Error
                throw ex;
            }
            finally
            {
                db.CloseConnection();
            }
        }


        /// <summary>
        /// Eliminar de forma física de la base de datos un producto seleccionado.
        /// </summary>
        /// <param name="id">ID del producto que se desea eliminar.</param>
        public static void DeleteProduct(int id)
        {
            DataBase db = new DataBase();
            string query = "DELETE FROM ARTICULOS WHERE Id = @ProductID;";
            try
            {
                db.SetQuery(query);
                db.SetParam("@ProductID", id);
                db.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // TODO: Manejar Error
                throw ex;
            }
            finally 
            { 
                db.CloseConnection(); 
            }
        }

        public static bool CodeExistsInDB(string code)
        {
            DataBase db = new DataBase();
            string query = "DECLARE @Code VARCHAR(50) = @ProductCode; " +
                           "DECLARE @Exists BIT; " +
                           "SET @Exists = 0; " +
                           "IF EXISTS (SELECT 1 FROM ARTICULOS WHERE Codigo = @Code) " +
                           "BEGIN " +
                           "    SET @Exists = 1; " + 
                           "END " +
                           "SELECT @Exists AS ExisteElCodigo;";

            try
            {
                db.SetQuery(query);
                db.SetParam("@ProductCode", code);
                return db.ExecuteScalar();
            }
            catch (Exception ex)
            {
                // TODO: Manejar error
                throw ex;
            }
            finally
            {
                db.CloseConnection();
            }
        }
    }
}
