using BusinessLogic;
using Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UserInterface.Pages.User
{
    public partial class Profile : System.Web.UI.Page
    {
        /* ---------------------------------------------------------------------------- */
        /*                                      GENERAL                                 */
        /* ---------------------------------------------------------------------------- */
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitializeProfileFields();
            }
        }

        /// <summary>
        /// Inicializar los campos con la información del usuario.
        /// </summary>
        private void InitializeProfileFields()
        {
            try
            {
                Domain.User user = (Domain.User)Session["USER"];

                txbxEmail.Text = user.Email;
                txbxName.Text = !string.IsNullOrEmpty(user.Firstname) ? 
                                user.Firstname : 
                                string.Empty;

                txbxSurname.Text = !string.IsNullOrEmpty(user.Lastname) ? 
                                   user.Lastname : 
                                   string.Empty;

                imgAvatar.ImageUrl = !string.IsNullOrEmpty(user.Avatar) ? 
                                     user.Avatar : 
                                     Constants.AvatarPlaceholderPath;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
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
        /*                                      NOMBRE                                  */
        /* ---------------------------------------------------------------------------- */
        /// <summary>
        /// Actualizar el nombre de usuario en la base de datos si cumple las condiciones
        /// necesarias.
        /// </summary>
        protected void btnName_Click(object sender, EventArgs e)
        {
            // Se quiere modificar el nombre
            if (txbxName.ReadOnly)
            {
                EnableNameEditing();
            }
            // Se confirma la modificación
            else
            {
                if (IsInvalidName())
                {
                    txbxName.CssClass = Constants.FormControlInvalid;
                    txbxName.Focus();
                    return;
                }

                UpdateUserName();
            }
        }

        /// <summary>
        /// Habilitar el campo de entrada de nombre para editarlo.
        /// </summary>
        private void EnableNameEditing()
        {
            txbxName.ReadOnly = false;
            txbxName.Focus();
            pnName.CssClass = Constants.ConfirmButtonIcon;
        }

        /// <summary>
        /// Verificar si el nombre introducido en el campo de entrada es un nombre
        /// inválido.
        /// </summary>
        /// <returns>Valor booleano que indica si el nombre es inválido</returns>
        private bool IsInvalidName()
        {
            return string.IsNullOrEmpty(txbxName.Text) || txbxName.Text.Length > 50;
        }

        /// <summary>
        /// Actualizar el nombre del usuario en la base de datos.
        /// </summary>
        private void UpdateUserName()
        {
            try
            {
                ((Domain.User)Session["USER"]).Firstname = txbxName.Text.Trim();
                UserBLL.UpdateUser((Domain.User)Session["USER"]);
                txbxName.CssClass = Constants.FormControlNormal;
                pnName.CssClass = Constants.EditButtonIcon;
                txbxName.ReadOnly = true;

                Session["ALERTMESSAGE"] = "Nombre de usuario actualizado correctamente.";
                Response.Redirect($"{Constants.ProfilePagePath}?alert=success", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /* ---------------------------------------------------------------------------- */
        /*                                    APELLIDO                                  */
        /* ---------------------------------------------------------------------------- */
        /// <summary>
        /// Actualizar el apellido de usuario en la base de datos si cumple las
        /// condiciones necesarias
        /// </summary>
        protected void btnSurname_Click(object sender, EventArgs e)
        {
            // Se quiere modificar el apellido
            if (txbxSurname.ReadOnly)
            {
                EnableSurnameEditing();
            }
            // Se confirma la modificación
            else
            {
                if (IsInvalidSurname())
                {
                    txbxSurname.CssClass = Constants.FormControlInvalid;
                    txbxSurname.Focus();
                    return;
                }

                UpdateUserSurname();
            }
        }

        /// <summary>
        /// Habilitar el campo de entrada de apellido para editarlo.
        /// </summary>
        private void EnableSurnameEditing()
        {
            txbxSurname.ReadOnly = false;
            txbxSurname.Focus();
            pnSurname.CssClass = Constants.ConfirmButtonIcon;
        }

        /// <summary>
        /// Verificar si el apellido introducido en el campo de entrada es un
        /// apellido inválido.
        /// </summary>
        /// <returns>Valor booleano que indica si el apellido es inválido</returns>
        private bool IsInvalidSurname()
        {
            return string.IsNullOrEmpty(txbxSurname.Text) || txbxSurname.Text.Length > 50;
        }

        /// <summary>
        /// Actualizar el apellido del usuario en la base de datos.
        /// </summary>
        private void UpdateUserSurname()
        {
            try
            {
                ((Domain.User)Session["USER"]).Lastname = txbxSurname.Text.Trim();
                UserBLL.UpdateUser((Domain.User)Session["USER"]);
                txbxSurname.CssClass = Constants.FormControlNormal;
                pnSurname.CssClass = Constants.EditButtonIcon;
                txbxSurname.ReadOnly = true;

                Session["ALERTMESSAGE"] = "Apellido de usuario actualizado correctamente.";
                Response.Redirect($"{Constants.ProfilePagePath}?alert=success", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /* ---------------------------------------------------------------------------- */
        /*                                     AVATAR                                   */
        /* ---------------------------------------------------------------------------- */
        /// <summary>
        /// Actualizar el avatar de perfil del usuario en la base de datos cuando se
        /// suba una imagen local.
        /// </summary>
        protected void btnAvatarSubmit_Click(object sender, EventArgs e)
        {
            // Si se subió una imagen
            if (fuAvatar.HasFile)
            {
                string fileName = $"{((Domain.User)Session["USER"]).ID}-{fuAvatar.PostedFile.FileName}";
                string path = Path.Combine(Server.MapPath($"~{Constants.LocalImagePath}"), fileName);

                // Se verifica si el usuario tiene un avatar previo, si existe, se elimina.
                DeletePreviousAvatarIfNeeded();

                try
                {
                    SaveNewAvatar(path, fileName);
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }

        /// <summary>
        /// Eliminar un avatar de usuario previo si se quiere actualizar con una nueva
        /// </summary>
        private void DeletePreviousAvatarIfNeeded()
        {
            try
            {
                if (((Domain.User)Session["USER"]).Avatar.StartsWith(Constants.LocalImagePath))
                {
                    string localAvatarPath = Server.MapPath(((Domain.User)Session["USER"]).Avatar);
                    if (File.Exists(localAvatarPath))
                    {
                        File.Delete(localAvatarPath);
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Guardar el nuevo avatar del usuario en la base de datos y actualizar
        /// el usuario en sesión.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        private void SaveNewAvatar(string path, string fileName)
        {
            try
            {
                fuAvatar.SaveAs(path);
                ((Domain.User)Session["USER"]).Avatar = $"{Constants.LocalImagePath}{fileName}";
                UserBLL.UpdateUser((Domain.User)Session["USER"]);

                Session["ALERTMESSAGE"] = "Avatar de usuario actualizado correctamente.";
                Response.Redirect($"{Constants.ProfilePagePath}?alert=success", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
    }
}