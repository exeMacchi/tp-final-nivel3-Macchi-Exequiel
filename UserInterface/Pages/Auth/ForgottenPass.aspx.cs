using BusinessLogic;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UserInterface.Pages.Auth
{
    public partial class ForgottenPass : System.Web.UI.Page
    {
        /* ---------------------------------------------------------------------------- */
        /*                                      GENERAL                                 */
        /* ---------------------------------------------------------------------------- */
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    // Recuperar cuenta
                    if (Request.QueryString["id"] == null)
                    {
                        InitializeAccountRecovery();
                    }
                    // Restaurar contraseña
                    else
                    {
                        InitializePasswordReset();
                    }
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }

        /// <summary>
        /// Inicializar la interfaz de usuario para la recuperación de cuenta.
        /// </summary>
        private void InitializeAccountRecovery()
        {
            txbxEmail.CssClass = Constants.FormControlNormal;
            btnSend.Enabled = false;
        }

        /// <summary>
        /// Inicializar la interfaz de usuario para la restauración de la contraseña.
        /// </summary>
        private void InitializePasswordReset()
        {
            try
            {
                // Middleware de seguridad: el usuario solo puede acceder a esta sección
                // de la página si tiene el hash que se envía en el correo electrónico.
                if (Request.QueryString["hash"] != Session["FORGOTTENHASH"].ToString())
                {
                    Session["ALERTMESSAGE"] = "No tiene las credenciales necesarias " +
                                              "para acceder a la página solicitada.";
                    Response.Redirect($"{Constants.LoginPagePath}?alert=error", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            txbxFirstPassword.CssClass = Constants.FormControlNormal;
            txbxSecondPassword.CssClass = Constants.FormControlNormal;
            // Se utiliza el atributo 'disabled' en lugar de la propiedad 'Enabled'
            // porque la habilitación del botón también se gestiona desde el cliente.
            btnSubmit.Attributes["disabled"] = "true";
        }

        /// <summary>
        /// Manejar la excepción guardándola en sesión para después rederigirla a la
        /// página de error.
        /// </summary>
        private void HandleException(Exception ex)
        {
            Session["ERROR"] = ex;
            Response.Redirect(Constants.ErrorPagePath, false);
            Context.ApplicationInstance.CompleteRequest(); // Esto evita un posible ThreadAbortException
        }

        
        /* ---------------------------------------------------------------------------- */
        /*                                ACCOUNT RECOVERY                              */
        /* ---------------------------------------------------------------------------- */

        /// <summary>
        /// Cuando se inserta un correo electrónico.
        /// </summary>
        protected void txbxEmail_TextChanged(object sender, EventArgs e)
        {
            if (IsValidEmailLength())
            {
                try
                {
                    // Se verifica que exista el email en la base de datos para permitir
                    // recuperar la cuenta.
                    if (!UserBLL.EmailExistsInDB(txbxEmail.Text.Trim()))
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
        /// Verificar que la longitud del email ingresado sea el correcto.
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
            btnSend.Enabled = true;
        }

        /// <summary>
        /// Configurar la interfaz de usuario cuando el email ingresado no cumple
        /// los requisitos.
        /// </summary>
        private void InvalidEmail()
        {
            txbxEmail.Text = string.Empty;
            txbxEmail.CssClass = Constants.FormControlInvalid;
            invalidEmail.Visible = true;
            invalidEmailLength.Visible = false;
            txbxEmail.Focus();
            btnSend.Enabled = false;
        }

        /// <summary>
        /// Cuando se quiere enviar el correo electrónico para recuperar una cuenta.
        /// </summary>
        protected void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                string email = txbxEmail.Text.Trim();
                int id = UserBLL.GetUserIDByEmail(email);

                if (id != 0)
                {
                    SendRecoveryMail(email, id);
                    forgottenSuccess.Visible = true;
                    forgottenError.Visible = false;
                    btnSend.Enabled = false;
                }
                else
                {
                    ShowForgottenError("El correo electrónico proporcionado no se " +
                                       "encuentra en la base de datos. Por favor, " +
                                       "verifique que el correo electrónico ingresado " +
                                       "sea correcto.");
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Enviar el mail con las instrucciones de recuperación de cuenta al correo
        /// electrónico del usuario. Se guarda en sesión un hash de seguridad que se
        /// utilizará como medida de seguridad.
        /// </summary>
        /// <param name="email">Correo electrónico del usuario que quiere recuperar la cuenta</param>
        /// <param name="id">ID del usuario</param>
        private void SendRecoveryMail(string email, int id)
        {
            try
            {
                // Hash de seguridad que se guarda en sesión y se envía con el mail
                // de recuperación.
                string hash = $"{id}{DateTime.Now.Ticks}";
                Session["FORGOTTENHASH"] = hash;

                EmailService es = new EmailService();
                es.CreateMail("no-replay@almacenero.com", email, "Restaurar contraseña", 
                              Auxiliary.CreateForgottenPassHTMLBody(id, hash));
                es.SendMail();
            }
            catch (SmtpException)
            {
                ShowForgottenError("No se pudo enviar el correo electrónico con las " +
                                   "instrucciones. Por favor, inténtelo nuevamente " +
                                   "más tarde.");
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Mostrar una alerta de error cuando se quiere recuperar una cuenta.
        /// </summary>
        /// <param name="errorMessage">Mensaje que detalla el problema</param>
        private void ShowForgottenError(string errorMessage)
        {
            forgottenErrorText.Text = errorMessage;
            forgottenError.Visible = true;
            forgottenSuccess.Visible = false;
        }


        /* ---------------------------------------------------------------------------- */
        /*                             PASSWORD RESTAURATION                            */
        /* ---------------------------------------------------------------------------- */
        /// <summary>
        /// Cuando se quiere restaurar la contraseña.
        /// </summary>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string firstPass = txbxFirstPassword.Text.Trim();
                string secondPass = txbxSecondPassword.Text.Trim();

                if (firstPass == secondPass)
                {
                    UpdateUserPassword(int.Parse(Request.QueryString["id"]), firstPass);
                }
                else
                {
                    ShowForgottenError("Las contraseñas introducidas no coinciden. " +
                                       "Por favor inténtelo de nuevo.");
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Actualizar la contraseña del usuario en la base de datos.
        /// </summary>
        /// <param name="id">ID del usuario</param>
        /// <param name="firstPass">Contraseña nueva</param>
        private void UpdateUserPassword(int id, string firstPass)
        {
            try
            {
                UserBLL.UpdateUserPassword(id, firstPass);
                Session["ALERTMESSAGE"] = "¡La contraseña se ha actualizado de forma exitosa!";
                Response.Redirect($"{Constants.LoginPagePath}?alert=success", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
    }
}