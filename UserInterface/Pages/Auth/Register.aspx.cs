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

                    // Se utiliza el atributo 'disabled' en lugar de la propiedad 'Enabled'
                    // porque la habilitación del botón también se gestiona desde el cliente.
                    btnRegister.Attributes["disabled"] = "true";
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
            string email = txbxEmail.Text.Trim();
            string firstPass = txbxFirstPassword.Text.Trim();
            string secondPass = txbxSecondPassword.Text.Trim();

            if (string.IsNullOrEmpty(email) || 
                string.IsNullOrEmpty(firstPass) ||
                string.IsNullOrEmpty(secondPass))
            {
                lbRegisterError.Text = "No pueden haber campos vacíos.";
                registerErrorAlert.Visible = true;
                return;
            }
            else if (UserBBL.EmailExistsInDB(email))
            {
                lbRegisterError.Text = "El correo electrónico introducido ya existe en la base de datos.";
                registerErrorAlert.Visible = true;
                return;
            }
            else if (firstPass != secondPass)
            {
                lbRegisterError.Text = "Las contraseñas deben coincidir.";
                registerErrorAlert.Visible = true;
                return;
            }

            try
            {
                UserBBL.CreateUser(email, firstPass);

                try
                {
                    EmailService es = new EmailService();
                    es.CreateMail("no-replay@almacenero.com", email, "¡Bienvenido!", Auxiliary.CreateRegisterHTMLBody());
                    es.SendMail();

                    Session["ALERTMESSAGE"] = "¡Usuario registrado de forma exitosa!";
                    Response.Redirect($"{Constants.LoginPagePath}?alert=success", false);
                }
                catch (SmtpException)
                {
                    Session["ALERTMESSAGE"] = "Usuario registrado, pero ocurrió un error al " + 
                                              "enviar el correo de bienvenida.";
                    Response.Redirect($"{Constants.LoginPagePath}?alert=success", false);
                }
            }
            catch (Exception ex)
            {
                // TODO: manejar error
                throw ex;
            }
        }

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
    }
}