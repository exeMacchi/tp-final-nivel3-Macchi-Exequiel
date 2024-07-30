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
    public partial class ForgottenPass : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    // Recuperar cuenta
                    if (Request.QueryString["id"] == null)
                    {
                        txbxEmail.CssClass = Constants.FormControlNormal;
                        btnSend.Enabled = false;
                    }
                    // Restaurar contraseña
                    else
                    {
                        // Middleware de seguridad: el usuario solo puede acceder a esta sección
                        // de la página si tiene el hash que se envía en el correo electrónico.
                        if (Request.QueryString["hash"] != Session["FORGOTTENHASH"].ToString())
                        {
                            Session["ALERTMESSAGE"] = "No tiene las credenciales necesarias " +
                                                      "para acceder a la página solicitada.";
                            Response.Redirect($"{Constants.LoginPagePath}?alert=error", false);
                            return;
                        }

                        txbxFirstPassword.CssClass = Constants.FormControlNormal;
                        txbxSecondPassword.CssClass = Constants.FormControlNormal;
                        // Se utiliza el atributo 'disabled' en lugar de la propiedad 'Enabled'
                        // porque la habilitación del botón también se gestiona desde el cliente.
                        btnSubmit.Attributes["disabled"] = "true";
                    }
                }
                catch (Exception ex)
                {
                    // TODO: manejar error
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Cuando se inserta un correo electrónico.
        /// </summary>
        protected void txbxEmail_TextChanged(object sender, EventArgs e)
        {
            if (txbxEmail.Text.Length > 0 && txbxEmail.Text.Length <= 100)
            {
                if (!UserBBL.EmailExistsInDB(txbxEmail.Text.Trim()))
                {
                    txbxEmail.Text = string.Empty;
                    txbxEmail.CssClass = Constants.FormControlInvalid;
                    invalidEmail.Visible = true;
                    invalidEmailLength.Visible = false;
                    txbxEmail.Focus();
                    btnSend.Enabled = false;
                }
                else
                {
                    txbxEmail.CssClass = Constants.FormControlValid;
                    invalidEmail.Visible = false;
                    invalidEmailLength.Visible = false;
                    btnSend.Enabled = true;
                }
            }
            else
            {
                txbxEmail.Text = string.Empty;
                txbxEmail.CssClass = Constants.FormControlInvalid;
                invalidEmailLength.Visible = true;
                invalidEmail.Visible = false;
                txbxEmail.Focus();
                btnSend.Enabled = false;
            }
        }

        /// <summary>
        /// Cuando se quiere enviar el correo electrónico para recuperar una cuenta.
        /// </summary>
        protected void btnSend_Click(object sender, EventArgs e)
        {
            string email = txbxEmail.Text.Trim();
            try
            {
                int id = UserBBL.GetUserIDByEmail(email);
                if (id != 0)
                {
                    Session["FORGOTTENHASH"] = $"{id}{DateTime.Now.Ticks}";
                    EmailService es = new EmailService();
                    es.CreateMail("no-replay@almacenero.com", email, "Restaurar contraseña", Auxiliary.CreateForgottenPassHTMLBody(id, Session["FORGOTTENHASH"].ToString()));
                    es.SendMail();
                    forgottenSuccessHeader.Text = "¡Correo electrónico enviado!";
                    forgottenSuccessText.Text = "Le hemos enviado un correo electrónico con " +
                                                "las instrucciones a seguir para cambiar su " +
                                                "contraseña. Verifique su bandeja de entrada " +
                                                "principal o en la bandeja spam.";
                    forgottenSuccess.Visible = true;
                    forgottenError.Visible = false;
                    btnSend.Enabled = false;
                }
                else
                {
                    forgottenErrorHeader.Text = "Error";
                    forgottenErrorText.Text = "El correo electrónico proporcionado no se " +
                                              "encuentra en la base de datos. Por favor, " +
                                              "verifique que el correo electrónico ingresado " +
                                              "sea correcto.";
                    forgottenError.Visible = true;
                }
            }
            catch (Exception ex)
            {
                // TODO: manejar error
                throw ex;
            }
        }

        /// <summary>
        /// Cuando se quiere restaurar la contraseña.
        /// </summary>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string firstPass = txbxFirstPassword.Text.Trim();
                string secondPass = txbxSecondPassword.Text.Trim();
                int id = int.Parse(Request.QueryString["id"]);

                if (firstPass != secondPass)
                {
                    forgottenErrorHeader.Text = "Error";
                    forgottenErrorText.Text = "Las contraseñas introducidas no coinciden. " +
                                              "Por favor, inténtelo de nuevo.";
                    forgottenError.Visible = true;
                    return;
                }

                UserBBL.UpdateUserPassword(id, firstPass);
                Session["ALERTMESSAGE"] = "¡La contraseña se ha actualizado de forma exitosa!";
                Response.Redirect($"{Constants.LoginPagePath}?alert=success", false);
            }
            catch (Exception ex)
            {
                // TODO: manejar error
                throw ex;
            }
        }
    }
}