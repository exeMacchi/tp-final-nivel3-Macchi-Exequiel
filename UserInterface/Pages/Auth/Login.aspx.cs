using BusinessLogic;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
                InitializeLoginForm();
            }
        }

        /// <summary>
        /// Configurar la interfaz de usuario predeterminada del formulario de logueo.
        /// </summary>
        private void InitializeLoginForm()
        {
            txbxEmail.CssClass = Constants.FormControlNormal;
            txbxPassword.CssClass = Constants.FormControlNormal;
            // Se utiliza el atributo 'disabled' en lugar de la propiedad 'Enabled'
            // porque la habilitación del botón se gestiona desde el cliente.
            btnLogin.Attributes["disabled"] = "true";
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
        /// Loguear una sesión de usuario.
        /// </summary>
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txbxEmail.Text.Trim();
            string pass = txbxPassword.Text.Trim();

            if (IsValidEmailFormat(email) && IsValidPassFormat(pass))
            {
                LogIn(email, pass);
            }
        }

        /// <summary>
        /// Verificar que el correo electrónico ingresado respete el formato esperado.
        /// </summary>
        /// <param name="email">Correo electrónico ingresado en <see cref="txbxEmail"/></param>
        private bool IsValidEmailFormat(string email)
        {
            if (string.IsNullOrEmpty(email) || email.Length > 100)
            {
                txbxEmail.Text = string.Empty;
                txbxEmail.CssClass = Constants.FormControlInvalid;
                txbxEmail.Focus();
                return false;
            }
            return true;
        }

        /// <summary>
        /// Verificar que la contraseña ingresada respete el formato esperado.
        /// </summary>
        /// <param name="pass">Contraseña ingresada en <see cref="txbxPassword"/></param>
        private bool IsValidPassFormat(string pass)
        {
            if (string.IsNullOrEmpty(pass) || pass.Length > 20)
            {
                txbxPassword.Text = string.Empty;
                txbxPassword.CssClass = Constants.FormControlInvalid;
                txbxPassword.Focus();
                return false;
            }
            return true;
        }

        /// <summary>
        /// Buscar las credenciales de usuario y crear una nueva sesión.
        /// </summary>
        /// <param name="email">Correo electrónico del usuario</param>
        /// <param name="pass">Contraseña de la cuenta del usuario</param>
        private void LogIn(string email, string pass)
        {
            try
            {
                Domain.User user = UserBLL.GetUser(email, pass);
                if (user.ID != 0)
                {
                    Session["USER"] = user;

                    // Dependiendo del rol de usuario, se redirige a una página.
                    if (user.IsAdmin)
                        Response.Redirect(Constants.AdminPagePath);
                    else
                        Response.Redirect(Constants.ProductsPagePath);
                }
                else
                {
                    alertWrongUserCredentials.Visible = true;
                    txbxEmail.Focus();
                }
            }
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
    }
}