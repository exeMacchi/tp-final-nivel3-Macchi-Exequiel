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

        /// <summary>
        /// Crear la condición de búsqueda avanzada que se pondrá en la sección WHERE de la
        /// query que se realizará a la base de datos.
        /// </summary>
        /// <param name="firstCriteria">Primer criterio de búsqueda (Nombre, Código, Marca, Categoría o Precio).</param>
        /// <param name="secondCriteria">Segundo criterio de búsqueda (Comienza con, Termina con...).</param>
        /// <param name="filterText">Texto de filtro de búsqueda que se inserta en el TextBox.</param>
        /// <returns>Condición completa para insertar como parámetro después de la cláusula WHERE.</returns>
        public static string CreateCondition(string firstCriteria, string secondCriteria, string filterText)
        {
            string condition = string.Empty;
            string criteria = CreateCriteria(firstCriteria, secondCriteria, filterText);

            if (firstCriteria == "Name")
                condition = $"A.Nombre {criteria} ";

            else if (firstCriteria == "Code")
                condition = $"A.Codigo {criteria}";

            else if (firstCriteria == "Brand")
                condition = $"M.Descripcion {criteria} ";

            else if (firstCriteria == "Category")
                condition = $"C.Descripcion {criteria} ";

            else if (firstCriteria == "Price")
                condition = $"A.Precio {criteria} ";

            return condition;
        }

        /// <summary>
        /// Devolver el criterio de búsqueda para la condición WHERE de la consulta que se
        /// realizará a la base de datos.
        /// </summary>
        /// <param name="firstCriteria">Primer criterio de búsqueda (Nombre, Código, Marca, Categoría o Precio).</param>
        /// <param name="secondCriteria">Segundo criterio de búsqueda (Comienza con, Termina con...).</param>
        /// <param name="text">Texto de filtro de búsqueda que se inserta en el TextBox.</param>
        /// <returns>Criterio completo para la condición WHERE</returns>
        private static string CreateCriteria(string firstCriteria, string secondCriteria, string text)
        {
            string criteria = string.Empty;
            if (firstCriteria == "Name" ||
                firstCriteria == "Code" ||
                firstCriteria == "Brand" ||
                firstCriteria == "Category")
            {
                if (secondCriteria == "Starts")
                    criteria = $"LIKE '{text}%' ";
                else if (secondCriteria == "Contains")
                    criteria = $"LIKE '%{text}%' ";
                else
                    criteria = $"LIKE '%{text}' ";
            }
            else
            {
                if (decimal.TryParse(text, out decimal price))
                {
                    if (secondCriteria == "Greater")
                        criteria = $"> {price} ";
                    else if (secondCriteria == "Equal")
                        criteria = $"= {price} ";
                    else
                        criteria = $"< {price} ";
                }
                else
                {
                    criteria = $"> 0 ";
                }
            }

            return criteria;
        }

        /// <summary>
        /// Crear el HTML Body del correo electrónico de bienvenida post-registro.
        /// </summary>
        /// <returns>HTML Body formateado.</returns>
        public static string CreateRegisterHTMLBody()
        {
            string body =
                "<body style=\"font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0;\">" +
                    "<div style=\"max-width: 600px; margin: 20px auto; background-color: #fff; padding: 20px; border-radius: 8px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);\">" +
                        "<h1 style=\"color: #333;\">¡Bienvenido a El Almacenero!</h1>" +
                        "<p style=\"color: #666; line-height: 1.6;\">¡Gracias por registrarte en El Almacenero! Estamos encantados de tenerte como parte de nuestra comunidad.</p>" +
                        "<p style=\"color: #666; line-height: 1.6;\">Nuestra plataforma está diseñada para ofrecerte la mejor experiencia de usuario. Puedes explorar nuestro catálogo de productos de manera sencilla y atractiva, accediendo a detalles específicos de cada uno.</p>" +
                        "<p style=\"color: #666; line-height: 1.6;\">Utiliza nuestros filtros por nombre, marca y categoría para encontrar fácilmente lo que buscas. Además, con tu cuenta podrás realizar un seguimiento de tus productos favoritos y disfrutar de una navegación personalizada.</p>" +
                        "<p style=\"color: #666; line-height: 1.6;\">Haz clic en el botón de abajo para comenzar a explorar nuestro catálogo y descubrir todo lo que tenemos para ofrecer.</p>" +
                        "<a href=\"https://localhost:44323/Default.aspx\" style=\"display: inline-block; padding: 10px 20px; background-color: #007bff; color: #fff; text-decoration: none; border-radius: 5px; transition: background-color 0.3s ease;\">¡Explora ahora!</a>" +
                        "<p style=\"color: #666; line-height: 1.6;\">¡Esperamos que disfrutes de tu experiencia en El Almacenero!</p>" +
                        "<p style=\"color: #666; line-height: 1.6;\">Equipo de El Almacenero</p>" +
                    "</div>" +
                "</body>";

            return body;
        }
    }

}
