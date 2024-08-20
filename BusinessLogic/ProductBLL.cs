using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Domain;

namespace BusinessLogic
{
    public class ProductBLL
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
                throw ex; // El error se propaga hacia arriba
            }
            finally
            {
                db.CloseConnection();
            }
        }

        /// <summary>
        /// Obtener un <see cref="Product"/> desde la base de datos según un ID.
        /// </summary>
        /// <param name="productID">ID del producto que se desea obtener en la base de datos.</param>
        /// <returns>Producto seleccionado.</returns>
        public static Product GetProduct(int productID)
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
                           "WHERE A.Id = @ProductID;";

            try
            {
                db.SetQuery(query);
                db.SetParam("@ProductID", productID);
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
                throw ex; // El error se propaga hacia arriba
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
                db.SetParam("@ProductImage", newProduct.Image ?? Constants.PlaceholderImagePath);
                db.SetParam("@ProductPrice", newProduct.Price);
                db.ExecuteNonQuery();
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
                db.SetParam("@ProductImage", product.Image ?? Constants.PlaceholderImagePath);
                db.SetParam("@ProductPrice", product.Price);
                db.SetParam("@ProductID", product.ID);
                db.ExecuteNonQuery();
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

        /// <summary>
        /// Eliminar de forma física de la base de datos un producto seleccionado.
        /// Además, se eliminan las posibles referencias huérfanas en la tabla
        /// de favoritos.
        /// </summary>
        /// <param name="productID">ID del producto que se desea eliminar.</param>
        public static void DeleteProduct(int productID)
        {
            DataBase db = new DataBase();
            string query = "DELETE FROM ARTICULOS WHERE Id = @ProductID;";
            try
            {
                // Se inicia una transacción porque, además de eliminar el producto
                // en la base de datos, también se tiene que eliminar las posibles
                // referencias huérfanas en la tabla de favoritos.
                db.BeginTransaction();

                db.SetQuery(query);
                db.SetParam("@ProductID", productID);
                db.ExecuteNonQuery();

                db.ClearParams();
                DeleteFavoritesReferences(db, productID);

                db.CommitTransaction();
            }
            catch (Exception ex)
            {
                db.RollbackTransaction();
                throw ex; // El error se propaga hacia arriba
            }
            finally 
            { 
                db.CloseConnection(); 
            }
        }

        /// <summary>
        /// Eliminar las posibles referencias huérfanas en la tabla de favoritos
        /// luego de que se elimine físicamente el producto referenciado.
        /// </summary>
        /// <param name="db">Instancia de la conexión abierta con la DB</param>
        /// <param name="productID">ID del producto que se eliminó</param>
        private static void DeleteFavoritesReferences(DataBase db, int productID)
        {
            string query = "DELETE FROM FAVORITOS WHERE IdArticulo = @ProductID;";
            try
            {
                db.SetQuery(query);
                db.SetParam("@ProductID", productID);
                db.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                throw ex; // El error se propaga hacia arriba
            }
            // No se cierra la conexión porque es parte de la transacción.
        }

        /// <summary>
        /// Verificar si en la base de datos existe un código prexistente al que se quiere
        /// agregar en la base de datos. 
        /// </summary>
        /// <param name="code">Cadena que representa el código a verificar</param>
        /// <returns>Valor booleano que indica la existencia o no del código introducido.</returns>
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
                           "SELECT @Exists AS CodeExists;";

            try
            {
                db.SetQuery(query);
                db.SetParam("@ProductCode", code);
                return db.ExecuteScalar();
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

        /// <summary>
        /// Buscar y devolver productos de la base de datos según una condición especificada.
        /// </summary>
        /// <param name="condition">Condición de búsqueda después del WHERE</param>
        /// <returns>Lista de productos filtrados</returns>
        public static List<Product> SearchProducts(string condition)
        {
            DataBase db = new DataBase();
            List<Product> filteredProducts = new List<Product>();
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
                           "WHERE " + condition;

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

                    filteredProducts.Add(product);
                }

                return filteredProducts;
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

        /// <summary>
        /// Devolver la URL de imagen del producto especificado.
        /// </summary>
        /// <param name="id">ID del producto</param>
        /// <returns>URL de imagen del producto especificado si existe</returns>
        public static string GetProductImage(int id)
        {
            DataBase db = new DataBase();
            string query = "SELECT ImagenUrl AS ProductImage " +
                           "FROM ARTICULOS " +
                           "WHERE Id = @ID";

            try
            {
                db.SetQuery(query);
                db.SetParam("@ID", id);
                db.ExecuteRead();

                if (db.Reader.Read())
                {
                    return db.Reader["ProductImage"].ToString();
                }
                return string.Empty;
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

        /// <summary>
        /// Devolver los productos favoritos de un usuario desde la base de datos.
        /// </summary>
        /// <param name="userID">ID del usuario</param>
        /// <returns>Lista de <see cref="Product"/> con la información cargada</returns>
        public static List<Product> GetFavoriteProducts(int userID)
        {
            DataBase db = new DataBase();
            List<Product> favoriteProducts = new List<Product>();
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
                           "INNER JOIN FAVORITOS AS F ON A.Id = F.IdArticulo " +
                           "WHERE F.IdUser = @UserID;";

            try
            {
                db.SetQuery(query);
                db.SetParam("@UserID", userID);
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

                    favoriteProducts.Add(product);
                }

                return favoriteProducts;
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
        
        /// <summary>
        /// Buscar y devolver desde la base de datos los productos favoritos del usuario
        /// según una condición especificada.
        /// </summary>
        /// <param name="userID">ID del usuario</param>
        /// <param name="condition">Condición que se aplica después del WHERE</param>
        /// <returns>Lista de productos favoritos filtrados.</returns>
        public static List<Product> SearchFavoriteProducts(int userID, string condition)
        {
            DataBase db = new DataBase();
            List<Product> filteredFavoriteProducts = new List<Product>();
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
                           "INNER JOIN FAVORITOS AS F ON A.Id = F.IdArticulo " +
                           "WHERE F.IdUser = @UserID AND " + condition;

            try
            {
                db.SetQuery(query);
                db.SetParam("@UserID", userID);
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

                    filteredFavoriteProducts.Add(product);
                }

                return filteredFavoriteProducts;
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

        /// <summary>
        /// Agregar un producto como favorito de un usuario en la base de datos.
        /// </summary>
        /// <param name="userID">ID del usuario</param>
        /// <param name="productID">ID del producto</param>
        public static void AddFavoriteProduct(int userID, int productID)
        {
            DataBase db = new DataBase();
            string query = "INSERT INTO FAVORITOS (IdUser, IdArticulo) VALUES (@UserID, @ProductID);";

            try
            {
                db.SetQuery(query);
                db.SetParam("@UserID", userID);
                db.SetParam("@ProductID", productID);
                db.ExecuteNonQuery();
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

        /// <summary>
        /// Quitar un producto como favorito de un usuario en la base de datos.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="productID"></param>
        public static void RemoveFavoriteProduct(int userID, int productID)
        {
            DataBase db = new DataBase();
            string query = "DELETE FROM FAVORITOS WHERE IdUser = @UserID AND IdArticulo = @ProductID;";

            try
            {
                db.SetQuery(query);
                db.SetParam("@UserID", userID);
                db.SetParam("@ProductID", productID);
                db.ExecuteNonQuery();
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

        /// <summary>
        /// Devolver un valor booleano que indica si el producto especificado es favorito
        /// para el usuario determinado.
        /// </summary>
        /// <param name="userID">ID del usuario</param>
        /// <param name="productID">ID del producto</param>
        /// <returns>Valor booleano que indica si el producto es favorito o no</returns>
        public static bool IsFavoriteProduct(int userID, int productID)
        {
            DataBase db = new DataBase();
            string query = "DECLARE @UserID INTEGER = @MyUserID; " +
                           "DECLARE @ProductID INTEGER = @MyProductID; " +
                           "DECLARE @Favorite BIT; " +
                           "SET @Favorite = 0; " +
                           "IF EXISTS (SELECT 1 FROM FAVORITOS WHERE IdUser = @UserID AND IdArticulo = @ProductID) " +
                           "BEGIN " +
                           "    SET @Favorite = 1; " +
                           "END " +
                           "SELECT @Favorite AS IsFavorite;";

            try
            {
                db.SetQuery(query);
                db.SetParam("@MyUserID", userID);
                db.SetParam("@MyProductID", productID);
                return db.ExecuteScalar();
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
