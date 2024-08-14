using BusinessLogic;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
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
                        // Alerta
                        pnlFavoriteAlert.Visible = false;

                        // Detalle
                        Product product = ProductBBL.GetProduct(int.Parse(Request.QueryString["id"]));
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

                        // Si hay sesión de usuario, verificar si el producto actual es uno
                        // de sus favoritos.
                        if (Session["USER"] != null)
                        {
                            if (ProductBBL.IsFavoriteProduct(((Domain.User)Session["USER"]).ID, product.ID))
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
                        // TODO: manejar error
                        throw ex;
                    }
                }
                else
                {
                    Response.Redirect(Constants.ProductsPagePath);
                }
            }
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

                // Si el producto no es favorito, se agrega en la base de datos
                if (!btnFavorite.CssClass.Contains("btn-fav"))
                {
                    ProductBBL.AddFavoriteProduct(userID, productID);
                    btnFavorite.CssClass = Constants.FavoriteProductButton;
                    pnlFavoriteIcon.CssClass = Constants.FavoriteProductIcon;
                    pnlProductDetail.CssClass = Constants.FavoriteProductDetail;
                    lbFavoriteAlertText.Text = "Producto añadido como favorito";
                    pnlFavoriteAlert.Visible = true;
                }
                // Si el producto es favorito, se elimina su referencia en la base de datos
                else
                {
                    ProductBBL.RemoveFavorite(userID, productID);
                    btnFavorite.CssClass = Constants.UnfavoriteProductButton;
                    pnlFavoriteIcon.CssClass = Constants.UnfavoriteProductIcon;
                    pnlProductDetail.CssClass = Constants.UnfavoriteProductDetail;
                    lbFavoriteAlertText.Text = "Producto removido como favorito";
                    pnlFavoriteAlert.Visible = true;
                }
            }
            catch (Exception ex)
            {
                // TODO: manejar error
                throw ex;
            }
        }
    }
}