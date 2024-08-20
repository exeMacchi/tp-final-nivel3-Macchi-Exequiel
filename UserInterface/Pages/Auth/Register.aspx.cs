using BusinessLogic;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UserInterface.Pages.Auth
{
    public partial class Register : System.Web.UI.Page
    {
        /* ---------------------------------------------------------------------------- */
        /*                                    GENERAL                                   */
        /* ---------------------------------------------------------------------------- */
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitializeRegisterForm();
            }
        }

        /// <summary>
        /// Configurar la interfaz de usuario predeterminada para el formulario de registro.
        /// </summary>
        private void InitializeRegisterForm()
        {
            // Estilos controles de formulario
            txbxEmail.CssClass = Constants.FormControlNormal;
            txbxFirstPassword.CssClass = Constants.FormControlNormal;
            txbxSecondPassword.CssClass = Constants.FormControlNormal;

            // Se utiliza el atributo 'disabled' en lugar de la propiedad 'Enabled'
            // porque la habilitación del botón también se gestiona desde el cliente.
            btnRegister.Attributes["disabled"] = "true";
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

        /* ---------------------------------------------------------------------------- */
        /*                                     EMAIL                                    */
        /* ---------------------------------------------------------------------------- */
        protected void txbxEmail_TextChanged(object sender, EventArgs e)
        {
            if (IsValidEmailLength())
            {
                try
                {
                    // Verificar que el cliente se quiera registrar con un email
                    // que no esté ya en uso.
                    if (UserBLL.EmailExistsInDB(txbxEmail.Text.Trim()))
                    {
                        InvalidEmail();
                    }
                    else
                    {
                        ValidEmail();
                    }
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
            else
            {
                InvalidEmail();
            }
        }

        /// <summary>
        /// Verificar que la longitud del email ingresado cumpla las condiciones.
        /// </summary>
        private bool IsValidEmailLength()
        {
            return txbxEmail.Text.Length > 0 && txbxEmail.Text.Length <= 100;
        }

        /// <summary>
        /// Configurar la interfaz de usuario cuando el email ingresado cumple
        /// los requisitos.
        /// </summary>
        private void ValidEmail()
        {
            txbxEmail.CssClass = Constants.FormControlValid;
            invalidEmail.Visible = false;
            invalidEmailLength.Visible = false;
            VerifyInformation();
        }

        /// <summary>
        /// Configurar la interfaz de usuario cuando el email ingresado no
        /// cumple los requisitos.
        /// </summary>
        private void InvalidEmail()
        {
            txbxEmail.Text = string.Empty;
            txbxEmail.CssClass = Constants.FormControlInvalid;
            invalidEmail.Visible = true;
            invalidEmailLength.Visible = false;
            txbxEmail.Focus();
        }

        /// <summary>
        /// Verificar que los campos obligatorios estén rellenados para poder habilitar
        /// el botón de registro.
        /// </summary>
        private void VerifyInformation()
        {
            if (!string.IsNullOrEmpty(txbxEmail.Text) && 
                !string.IsNullOrEmpty(txbxFirstPassword.Text) && 
                !string.IsNullOrEmpty(txbxSecondPassword.Text))
            {
                btnRegister.Attributes.Remove("disabled");
            }
            else
            {
                btnRegister.Attributes["disabled"] = "true";
            }
        }


        /* ---------------------------------------------------------------------------- */
        /*                                   REGISTER                                   */
        /* ---------------------------------------------------------------------------- */
        /// <summary>
        /// Cuando el usuario quiere registrarse
        /// </summary>
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string email = txbxEmail.Text.Trim();
            string firstPass = txbxFirstPassword.Text.Trim();
            string secondPass = txbxSecondPassword.Text.Trim();

            if (AreValidFields(email, firstPass, secondPass))
            {
                try
                {
                    UserBLL.CreateUser(email, firstPass);

                    SendWelcomeEmail(email);

                    Session["ALERTMESSAGE"] = "¡Usuario registrado de forma exitosa!";
                    Response.Redirect($"{Constants.LoginPagePath}?alert=success");
                }
                catch (ThreadAbortException) { }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }

        /// <summary>
        /// Verificar que los campos de entradas obligatorios del formulario cumplan
        /// todos los requisitos pre-registro.
        /// </summary>
        /// <param name="email">Correo electrónico introducido en <see cref="txbxEmail"/></param>
        /// <param name="firstPass">Contraseña ingresada en <see cref="txbxFirstPassword"/></param>
        /// <param name="secondPass">Contraseña ingresada en <see cref="txbxSecondPassword"/></param>
        /// <returns></returns>
        private bool AreValidFields(string email, string firstPass, string secondPass)
        {
            // Si algún campo está vacío
            if (string.IsNullOrEmpty(email) || 
                string.IsNullOrEmpty(firstPass) ||
                string.IsNullOrEmpty(secondPass))
            {
                ShowRegisterError("No pueden haber campos vacíos.");
                return false;
            }
            // Si el email ya existe en la base de datos
            else if (UserBLL.EmailExistsInDB(email))
            {
                ShowRegisterError("El correo electrónico introducido ya existe en la base de datos.");
                return false;
            }
            // Si las contraseñas no coinciden
            else if (firstPass != secondPass)
            {
                ShowRegisterError("Las contraseñas deben coincidir.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Mostrar una alerta de error cuando se intenta el usuario intenta registrarse.
        /// </summary>
        /// <param name="errorMessage">Mensaje de error personalizado</param>
        private void ShowRegisterError(string errorMessage)
        {
            lbRegisterError.Text = errorMessage;
            registerErrorAlert.Visible = true;
        }

        /// <summary>
        /// Enviar un correo electrónico de bienvenida al usuario post-registro.
        /// </summary>
        /// <param name="email">Correo electrónico del nuevo usuario</param>
        private void SendWelcomeEmail(string email)
        {
            try
            {
                EmailService es = new EmailService();
                es.CreateMail("no-replay@almacenero.com", email, "¡Bienvenido!", 
                              Auxiliary.CreateRegisterHTMLBody());
                es.SendMail();
            }
            catch (SmtpException)
            {
                try
                {
                    Session["ALERTMESSAGE"] = "Usuario registrado, pero ocurrió un error " +
                                              "al enviar el correo de bienvenida.";
                    Response.Redirect($"{Constants.LoginPagePath}?alert=success");
                }
                catch (ThreadAbortException) { }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
    }
}