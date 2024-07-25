using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public static class Constants
    {
        /* --- IMÁGENES --- */
        public const string PlaceholderImagePath = "/Assets/Images/Placeholder.jpg";
        public const string LocalImagePath = "/Assets/Images/Uploads/";

        /* --- RUTAS --- */ 
        // Raíz
        public const string DefaultPagePath = "/Default.aspx";

        // Global
        public const string ProductsPagePath = "/Pages/Global/Products.aspx";
        public const string ProductDetailPagePath = "/Pages/Global/ProductDetail.aspx";
        public const string ContactPagePath = "/Pages/Global/Contact.aspx";

        // Admin
        public const string AdminPagePath = "/Pages/Admin/Admin.aspx";
        public const string CreateEditPagePath = "/Pages/Admin/CreateEdit.aspx";

        // Auth
        public const string LoginPagePath = "/Pages/Auth/Login.aspx";
        public const string RegisterPagePath = "/Pages/Auth/Register.aspx";
        public const string ForgottenPassPath = "/Pages/Auth/ForgottenPass.aspx";

        // User
        public const string FavoritesPagePath = "/Pages/User/Favorites.aspx";

        /* --- CLASES --- */ 
        public const string FormControlNormal = "form-control bg-dark text-white";
        public const string FormControlValid = "form-control bg-dark text-white is-valid";
        public const string FormControlInvalid = "form-control bg-dark text-white is-invalid";
        public const string FormSelectPlaceholder = "form-select bg-dark form-select--placeholder";
        public const string FormSelectOptionSelected = "form-select bg-dark form-select--option-selected";
    }
}
