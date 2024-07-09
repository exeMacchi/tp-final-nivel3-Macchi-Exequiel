using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Auxiliary
    {

        /// <summary>
        /// Verificar si la imagen obtenida en la base de datos es válida (verificación parcial).
        /// </summary>
        /// <param name="imageURL">URL de la imagen</param>
        /// <returns>Cadena con la dirección pasada como argumento o la dirección de un placeholder.</returns>
        public static string VerifyImage(string imageURL)
        {
            if (imageURL.StartsWith("https")) {
                return imageURL;
            }
            else if (imageURL.StartsWith("/Assets/Images"))
            {
                return imageURL;
            }
            else
            {
                return "/Assets/Images/Placeholder.jpg";
            }
        }
    }
}
