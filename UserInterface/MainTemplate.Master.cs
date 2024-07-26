using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UserInterface.Pages.Admin;
using UserInterface.Pages.Auth;
using UserInterface.Pages.Global;

namespace UserInterface
{
    public partial class MainTemplate : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        // Esto ocurre antes del Page_Load() de la página solicitada.
        protected void Page_Init(object sender, EventArgs e)
        {
            // Si no es ninguna de las páginas globales o predeterminadas...
            if (!(Page is Default || Page is Products || Page is ProductDetail || 
                Page is Contact || Page is Pages.Auth.Login || Page is Register ||
                Page is ForgottenPass || Page is ErrorPage))
            {
                // y NO hay sesión activa, obliga a loguear (USER MIDDLEWARE)
                if (Session["USER"] == null)
                {
                    Session["ALERTMESSAGE"] = "No tiene las credenciales de usuario necesarias " +
                                              "para acceder a la página solicitada. Por favor, " +
                                              "inicie sesión con un usuario válido.";
                    Response.Redirect($"{Constants.LoginPagePath}?alert=error");
                }
                // y SÍ hay una sesión activa, pero NO es un administrador, se obliga a loguear (ADMIN MIDDLEWARE)
                else if ((Page is Admin || Page is CreateEdit) && !((User)Session["USER"]).IsAdmin)
                {
                    Session["ALERTMESSAGE"] = "No tiene las credenciales de administrador " +
                                              "necesarias para acceder a la página solicitada. " +
                                              "Por favor, inicie sesión con un usuario válido";
                    Response.Redirect($"{Constants.LoginPagePath}?alert=error");

                }
            }
        }

        /// <summary>
        /// El usuario cierra su sesión.
        /// </summary>
        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect(Constants.DefaultPagePath);
        }
    }
}