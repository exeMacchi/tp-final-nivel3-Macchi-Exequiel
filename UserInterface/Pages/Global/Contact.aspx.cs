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

namespace UserInterface.Pages.Global
{
    public partial class Contact : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitializeContactForm();
            }
        }

        /// <summary>
        /// Configurar la interfaz de usuario inicial para el formulario de contacto.
        /// </summary>
        private void InitializeContactForm()
        {
            txbxEmail.CssClass = Constants.FormControlNormal;
            txbxSubject.CssClass = Constants.FormControlNormal;
            txbxMessage.CssClass = Constants.FormControlNormal;
            btnSend.Enabled = false;
        }

        /// <summary>
        /// Verificar la información cargada en un campo de entrada de texto.
        /// </summary>
        protected void txbxTextChanged(object sender, EventArgs e)
        {
            TextBox txbxControl = (TextBox)sender;
            string text = txbxControl.Text.Trim();
            if (text.Length > 0)
            {
                txbxControl.CssClass = Constants.FormControlValid;
            }
            else
            {
                txbxControl.CssClass = Constants.FormControlInvalid;
            }
            VerifyInformation();
        }

        /// <summary>
        /// Verificar que los campos de entrada tengan información para activar el
        /// botón de envío.
        /// </summary>
        private void VerifyInformation()
        {
            if (string.IsNullOrEmpty(txbxEmail.Text) || 
                string.IsNullOrEmpty(txbxSubject.Text) || 
                string.IsNullOrEmpty(txbxMessage.Text))
            {
                btnSend.Enabled = false;
            }
            else
            {
                btnSend.Enabled = true;
            }
        }

        /// <summary>
        /// Enviar el formulario de contacto.
        /// </summary>
        protected void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                string email = txbxEmail.Text.Trim();
                string subject = txbxSubject.Text.Trim();
                string message = txbxMessage.Text.Trim();

                EmailService es = new EmailService();
                es.CreateMail(email, "contact@almacenero.com", subject, Auxiliary.CreateContactHTMLBody(email, message));
                es.SendMail();

                contactSuccess.Visible = true;
                btnSend.Enabled = false;
            }
            catch (SmtpException)
            {
                contactError.Visible = true;
            }
            catch (Exception ex)
            {
                try
                {
                    Session["ERROR"] = ex;
                    Response.Redirect(Constants.ErrorPagePath);
                }
                catch (ThreadAbortException) { }
            }
        }
    }
}