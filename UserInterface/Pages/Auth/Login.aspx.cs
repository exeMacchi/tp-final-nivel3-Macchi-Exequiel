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
            string email = txbxEmail.Text.Trim();
            string pass = txbxPassword.Text.Trim();

            if (email.Length > 100 || email == "")
            {
                txbxEmail.Text = string.Empty;
                txbxEmail.CssClass = Constants.FormControlInvalid;
                txbxEmail.Focus();
                return;
            }

            if (pass.Length > 20 || pass == "")
            {
                txbxPassword.Text = string.Empty;
                txbxPassword.CssClass = Constants.FormControlInvalid;
                txbxPassword.Focus();
                return;
            }

            try
            {
                Domain.User user = UserBBL.GetUser(email, pass);
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