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
    public partial class ErrorPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["ERROR"] != null)
                {
                    Exception exception = (Exception)Session["ERROR"];

                    // Mensaje de error principal
                    lbErrorText.Text = exception.Message;

                    // Detalle del error
                    if (exception.InnerException != null)
                    {
                        lbDetailErrorText.Text = exception.InnerException.Message;
                        pnlErrorDetail.Visible = true;
                    }
                }
                else
                {
                    try
                    {
                        Response.Redirect(Constants.DefaultPagePath);
                    }
                    catch (ThreadAbortException) { }
                }
            }
        }
    }
}