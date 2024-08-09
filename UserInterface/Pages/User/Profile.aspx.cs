using BusinessLogic;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UserInterface.Pages.User
{
    public partial class Profile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
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
                catch(Exception ex) 
                {
                    // TODO: manejar error
                    throw ex;
                }
            }
        }

        protected void btnName_Click(object sender, EventArgs e)
        {
            // Se quiere modificar el nombre
            if (txbxName.ReadOnly)
            {
                txbxName.ReadOnly = false;
                txbxName.Focus();
                pnName.CssClass = Constants.ConfirmButtonIcon;
            }
            // Se confirma la modificación
            else
            {
                if (string.IsNullOrEmpty(txbxName.Text) || txbxName.Text.Length > 50)
                {
                    txbxName.CssClass = Constants.FormControlInvalid;
                    txbxName.Focus();
                    return;
                }

                try
                {
                    ((Domain.User)Session["USER"]).Firstname = txbxName.Text.Trim();
                    UserBBL.UpdateUser((Domain.User)Session["USER"]);
                    txbxName.CssClass = Constants.FormControlNormal;
                    pnName.CssClass = Constants.EditButtonIcon;
                    txbxName.ReadOnly = true;

                    Session["ALERTMESSAGE"] = "Nombre de usuario actualizado correctamente.";
                    Response.Redirect($"{Constants.ProfilePagePath}?alert=success", false);
                }
                catch (Exception ex)
                {
                    // TODO: manejar error
                    throw ex;
                }
            }
        }

        protected void btnSurname_Click(object sender, EventArgs e)
        {
            // Se quiere modificar el apellido
            if (txbxSurname.ReadOnly)
            {
                txbxSurname.ReadOnly = false;
                txbxSurname.Focus();
                pnSurname.CssClass = Constants.ConfirmButtonIcon;
            }
            // Se confirma la modificación
            else
            {
                if (string.IsNullOrEmpty(txbxSurname.Text) || txbxSurname.Text.Length > 50)
                {
                    txbxSurname.CssClass = Constants.FormControlInvalid;
                    txbxSurname.Focus();
                    return;
                }

                try
                {
                    ((Domain.User)Session["USER"]).Lastname = txbxSurname.Text.Trim();
                    UserBBL.UpdateUser((Domain.User)Session["USER"]);
                    txbxSurname.CssClass = Constants.FormControlNormal;
                    pnSurname.CssClass = Constants.EditButtonIcon;
                    txbxSurname.ReadOnly = true;

                    Session["ALERTMESSAGE"] = "Apellido de usuario actualizado correctamente.";
                    Response.Redirect($"{Constants.ProfilePagePath}?alert=success", false);
                }
                catch (Exception ex)
                {
                    // TODO: manejar error
                    throw ex;
                }
            }
        }
    }
}