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
    }
}
