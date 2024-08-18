using BusinessLogic;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UserInterface
{
    public partial class Default : System.Web.UI.Page
    {
        public int ProductIndex { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (((List<Product>)Session["PRODUCTS"]).Count > 0)
                    {
                        ProductIndex = 0; // Índice para el carousel de productos
                        ProductCards.DataSource = (List<Product>)Session["PRODUCTS"];
                        ProductCards.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    Session["ERROR"] = ex;
                    Response.Redirect(Constants.ErrorPagePath);
                }
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                if (!(Session["PRODUCTS"] != null))
                {
                    Session["PRODUCTS"] = ProductBBL.GetProducts();
                }
            }
            catch (Exception ex)
            {
                Session["ERROR"] = ex;
                Response.Redirect(Constants.ErrorPagePath);
            }
        }
    }
}