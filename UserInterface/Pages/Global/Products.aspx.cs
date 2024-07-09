using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Domain;
using BusinessLogic;

namespace UserInterface.Pages.Global
{
    public partial class Products : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (((List<Product>)Session["PRODUCTS"]).Count > 0)
                    {
                        ProductCards.DataSource = (List<Product>)Session["PRODUCTS"];
                        ProductCards.DataBind();
                    }
                    else
                    {
                        // TODO: Alerta de base de datos vacía
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de errores
                    throw ex;
                }
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!(Session["PRODUCTS"] != null))
            {
                Session["PRODUCTS"] = ProductBBL.GetProducts();
            }
        }
    }
}