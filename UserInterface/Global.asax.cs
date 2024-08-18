using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace UserInterface
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Manejador de excepciones genérico
        /// </summary>
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();

            if (ex is HttpUnhandledException)
            {
                Session["ERROR"] = ex;
                Server.Transfer(Constants.ErrorPagePath);
            }
        }
    }
}