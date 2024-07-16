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
                    Product product = ProductBBL.GetProduct(int.Parse(Request.QueryString["id"]));

                    if (product != null)
                    {
                        productImage.ImageUrl = product.Image;
                        productImage.AlternateText = $"Imagen del producto {product.Name}";
                        productCode.Text = product.Code;
                        productName.Text = product.Name;
                        productDescription.Text = product.Description;
                        productBrand.Text = product.Brand.Description;
                        productCategory.Text = product.Category.Description;
                        productPrice.Text = $"$ {product.Price.ToString("N2")}" ;
                    }
                }
                else
                {
                    Response.Redirect("/Pages/Global/Products.aspx");
                }
            }
        }
    }
}