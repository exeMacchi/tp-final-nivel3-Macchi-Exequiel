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
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    // Estilos controles de formulario
                    txbxEmail.CssClass = Constants.FormControlNormal;
                    txbxFirstPassword.CssClass = Constants.FormControlNormal;
                    txbxSecondPassword.CssClass = Constants.FormControlNormal;
                }
                catch (Exception ex)
                {
                    // TODO: manejar error
                    throw ex;
                }
            }

        }

        protected void txbxEmail_TextChanged(object sender, EventArgs e)
        {
            if (txbxEmail.Text.Length > 0 && txbxEmail.Text.Length <= 100)
            {
                if (UserBBL.EmailExistsInDB(txbxEmail.Text.Trim()))
                {
                    txbxEmail.Text = string.Empty;
                    txbxEmail.CssClass = Constants.FormControlInvalid;
                    invalidEmail.Visible = true;
                    invalidEmailLength.Visible = false;
                    txbxEmail.Focus();
                }
                else
                {
                    txbxEmail.CssClass = Constants.FormControlValid;
                    invalidEmail.Visible = false;
                    invalidEmailLength.Visible = false;
                    VerifyInformation();
                }
            }
            else
            {
                txbxEmail.Text = string.Empty;
                txbxEmail.CssClass = Constants.FormControlInvalid;
                invalidEmailLength.Visible = true;
                invalidEmail.Visible = false;
                txbxEmail.Focus();
            }
        }
        protected void btnRegister_Click(object sender, EventArgs e)
        {

        }

        private void VerifyInformation()
        {
            if (!string.IsNullOrEmpty(txbxEmail.Text))
            {
            }
            else
            {

            }
        }

    }
}