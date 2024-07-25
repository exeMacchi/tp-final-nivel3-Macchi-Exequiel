using BusinessLogic;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UserInterface.Pages.Auth
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    txbxEmail.CssClass = Constants.FormControlNormal;
                    txbxPassword.CssClass = Constants.FormControlNormal;
                }
                catch (Exception ex)
                {
                    // TODO: manejar error
                    throw ex;
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (txbxEmail.Text.Length > 100 || txbxEmail.Text == "")
            {
                txbxEmail.Text = string.Empty;
                txbxEmail.CssClass = Constants.FormControlInvalid;
                txbxEmail.Focus();
                return;
            }

            if (txbxPassword.Text.Length > 20 || txbxPassword.Text == "")
            {
                txbxPassword.Text = string.Empty;
                txbxPassword.CssClass = Constants.FormControlInvalid;
                txbxPassword.Focus();
                return;
            }

            try
            {
                Domain.User user = UserBBL.GetUser(txbxEmail.Text, txbxPassword.Text);
                if (user.ID != 0)
                {
                    Session["USER"] = user;
                    if (user.IsAdmin)
                        Response.Redirect(Constants.AdminPagePath, false);
                    else
                        Response.Redirect(Constants.ProductsPagePath, false);
                }
                else
                {
                    alertWrongUserCredentials.Visible = true;
                    txbxEmail.Focus();
                }
            }
            catch (Exception ex)
            {
                // TODO: Manejar error
                throw ex;
            }
        }
    }
}