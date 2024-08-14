using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public static class Constants
    {
        /* --- IMÁGENES --- */
        public const string PlaceholderImagePath = "/Assets/Images/Placeholder.jpg";
        public const string AvatarPlaceholderPath = "/Assets/Images/AvatarPlaceholder.jpg";
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
        public const string ProfilePagePath = "/Pages/User/Profile.aspx";

        /* --- CLASES --- */ 
        public const string FormControlNormal = "form-control bg-dark text-white";
        public const string FormControlValid = "form-control bg-dark text-white is-valid";
        public const string FormControlInvalid = "form-control bg-dark text-white is-invalid";
        public const string FormSelectPlaceholder = "form-select bg-dark form-select--placeholder";
        public const string FormSelectOptionSelected = "form-select bg-dark form-select--option-selected";
        public const string FilterLinkNormal = "filter-link text-decoration-none ms-4";
        public const string FilterLinkSelected = "filter-link text-decoration-none ms-4 text-warning";
        public const string EditButtonIcon = "bi bi-pencil-square text-warning fs-5";
        public const string ConfirmButtonIcon = "bi bi-check-circle text-warning fs-5";
        public const string FavoriteProductButton = "btn border p-2 border rounded-circle lh-1 btn-fav border-warning border-2";
        public const string FavoriteProductIcon = "bi bi-star-fill";
        public const string FavoriteProductDetail = "row bg-dark rounded-5 border text-white py-4 px-3 border-warning border-2";
        public const string UnfavoriteProductButton = "btn border p-2 border rounded-circle lh-1 btn-unfav";
        public const string UnfavoriteProductIcon = "bi bi-star";
        public const string UnfavoriteProductDetail = "row bg-dark rounded-5 border text-white py-4 px-3";
    }
}
