using BusinessLogic;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UserInterface.Pages.Global
{
    public partial class ProductDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    try
                    {
                        // Alerta sobre producto favorito
                        pnlFavoriteAlert.Visible = false;

                        Product product = ProductBLL.GetProduct(int.Parse(Request.QueryString["id"]));

                        // Cargar los detalles del producto
                        LoadProductDetails(product);

                        // Si hay sesión de usuario, verificar si el producto actual es uno
                        // de sus favoritos.
                        CheckProductFavoriteStatus(product);
                    }
                    catch (Exception ex)
                    {
                        HandleException(ex);
                    }
                }
                else
                {
                    try
                    {
                        Response.Redirect(Constants.ProductsPagePath);
                    }
                    catch (ThreadAbortException) { }
                }
            }
        }

        /// <summary>
        /// Cargar visualmente la información del producto en la interfaz de usuario.
        /// </summary>
        /// <param name="product">Producto seleccionado para su visualización</param>
        private void LoadProductDetails(Product product)
        {
            try
            {
                if (product != null)
                {
                    productImage.ImageUrl = product.Image;
                    productImage.AlternateText = $"Imagen del producto {product.Name}";
                    lbCode.Text = product.Code;
                    lbTitle.Text = product.Name;
                    lbPrice.Text = $"$ {product.Price.ToString("N2")}";
                    lbDescription.Text = product.Description;
                    lbBrand.Text = product.Brand.Description;
                    lbCategory.Text = product.Category.Description;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Verificar si el producto actual es uno de los favoritos del usuario (sesión
        /// activa).
        /// </summary>
        /// <param name="product">Producto seleccionado para su visualización</param>
        private void CheckProductFavoriteStatus(Product product)
        {
            try
            {
                if (Session["USER"] != null)
                {
                    if (ProductBLL.IsFavoriteProduct(((Domain.User)Session["USER"]).ID, product.ID))
                    {
                        btnFavorite.CssClass = Constants.FavoriteProductButton;
                        pnlFavoriteIcon.CssClass = Constants.FavoriteProductIcon;
                        pnlProductDetail.CssClass = Constants.FavoriteProductDetail;
                    }
                    else
                    {
                        btnFavorite.CssClass = Constants.UnfavoriteProductButton;
                        pnlFavoriteIcon.CssClass = Constants.UnfavoriteProductIcon;
                        pnlProductDetail.CssClass = Constants.UnfavoriteProductDetail;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Manejar la excepción guardándola en sesión para después rederigirla a la
        /// página de error.
        /// </summary>
        private void HandleException(Exception ex)
        {
            try
            {
                Session["ERROR"] = ex;
                Response.Redirect(Constants.ErrorPagePath);
            }
            catch (ThreadAbortException) { }
        }

        /// <summary>
        /// Cuando se hace click en el botón de favorito (hay sesión de usuario).
        /// </summary>
        protected void btnFavorite_Click(object sender, EventArgs e)
        {
            try
            {
                int userID = ((Domain.User)Session["USER"]).ID;
                int productID = int.Parse(Request.QueryString["id"]);

                // Si el producto es favorito, se elimina su referencia en la base de datos
                if (IsFavoriteProduct())
                {
                    RemoveFavoriteProduct(userID, productID);
                }
                // Si el producto no es favorito, se agrega en la base de datos
                else
                {
                    AddFavoriteProduct(userID, productID);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Verificar si el producto es favorito según la clase que tenga el botón
        /// de favorito.
        /// </summary>
        /// <returns></returns>
        private bool IsFavoriteProduct()
        {
            return btnFavorite.CssClass.Contains("btn-fav");
        }

        /// <summary>
        /// Agregar el producto a los favoritos del usuario en la base de datos.
        /// </summary>
        /// <param name="userID">ID del usuario</param>
        /// <param name="productID">ID del producto</param>
        private void AddFavoriteProduct(int userID, int productID)
        {
            try
            {
                ProductBLL.AddFavoriteProduct(userID, productID);
                btnFavorite.CssClass = Constants.FavoriteProductButton;
                pnlFavoriteIcon.CssClass = Constants.FavoriteProductIcon;
                pnlProductDetail.CssClass = Constants.FavoriteProductDetail;
                lbFavoriteAlertText.Text = "Producto añadido como favorito";
                pnlFavoriteAlert.Visible = true;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Remover el producto a los favoritos del usuario en la base de datos.
        /// </summary>
        /// <param name="userID">ID del usuario</param>
        /// <param name="productID">ID del producto</param>
        private void RemoveFavoriteProduct(int userID, int productID)
        {
            try
            {
                ProductBLL.RemoveFavoriteProduct(userID, productID);
                btnFavorite.CssClass = Constants.UnfavoriteProductButton;
                pnlFavoriteIcon.CssClass = Constants.UnfavoriteProductIcon;
                pnlProductDetail.CssClass = Constants.UnfavoriteProductDetail;
                lbFavoriteAlertText.Text = "Producto removido como favorito";
                pnlFavoriteAlert.Visible = true;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
    }
}