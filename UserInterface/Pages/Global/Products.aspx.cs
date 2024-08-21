using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Domain;
using BusinessLogic;
using System.Threading;

namespace UserInterface.Pages.Global
{
    public partial class Products : System.Web.UI.Page
    {
        /* ---------------------------------------------------------------------------- */
        /*                                      GENERAL                                 */
        /* ---------------------------------------------------------------------------- */
        // Número de elementos por página
        private const int PageSize = 6;

        // Página actual de la paginación
        private int CurrentPage
        {
            get { return int.Parse(hfCurrentPage.Value); }
            set { hfCurrentPage.Value = value.ToString(); }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (Session["PRODUCTS"] == null)
                    {
                        Session["PRODUCTS"] = ProductBLL.GetProducts();
                    }

                    if (Session["USER"] != null)
                    {
                        // Se actualiza siempre la lista de productos favoritos cuando se recarga
                        // la página porque el usuario puede que haya agregado o eliminado algún
                        // producto de su lista de favoritos.
                        Session["FAVORITEPRODUCTS"] = ProductBLL.GetFavoriteProducts(((Domain.User)Session["USER"]).ID);
                    }

                    if (Session["CATEGORIES"] == null)
                    {
                        Session["CATEGORIES"] = CategoryBLL.GetCategories();
                    }

                    if (Session["BRANDS"] == null)
                    {
                        Session["BRANDS"] = BrandBLL.GetBrands();
                    }
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (((List<Product>)Session["PRODUCTS"]).Count > 0)
                    {
                        SetProductPagination((List<Product>)Session["PRODUCTS"]);

                        FillCategoryFilters();
                        FillBrandFilters();
                        FillPriceFilters();

                        ProductsAvailable();
                    }
                    else
                    {
                        NoProductsAvailable();
                    }
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }

        /// <summary>
        /// Si hay una sesión de usuario, verificar si se deben activar estilos para las
        /// cards de productos según sean favoritos o no.
        /// </summary>
        protected void ProductCards_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (Session["USER"] != null)
                {
                    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                    {
                        int productID = (int)DataBinder.Eval(e.Item.DataItem, "ID");
                        bool isFavorite = ((List<Product>)Session["FAVORITEPRODUCTS"]).Any(favoriteP => favoriteP.ID == productID);

                        if (isFavorite)
                        {
                            Panel pnlFavoriteProduct = (Panel)e.Item.FindControl("pnlFavoriteProduct");
                            Panel pnlProductCard = (Panel)e.Item.FindControl("pnlProductCard");
                            pnlFavoriteProduct.Visible = true;
                            pnlProductCard.CssClass += " border-warning border-3";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Configurar la interfaz de usuario cuando hay productos disponibles.
        /// </summary>
        private void ProductsAvailable()
        {
            txbxNameFilter.Enabled = true;
            btnFind.Enabled = true;
            alertEmptyDB.Visible = false;
            alertProductNotFound.Visible = false;
        }

        /// <summary>
        /// Configurar la interfaz de usuario cuando no hay productos disponibles.
        /// </summary>
        private void NoProductsAvailable()
        {
            alertEmptyDB.Visible = true;
            alertProductNotFound.Visible = false;

            // Se bloquea la búsqueda de productos.
            txbxNameFilter.Enabled = false;
            btnFind.Enabled = false;
            btnPrev.Enabled = false;
            btnNext.Enabled = false;
        }

        /// <summary>
        /// Configurar la paginación según una lista de productos pasada como argumento.
        /// </summary>
        /// <param name="products">Lista de productos</param>
        private void SetProductPagination(List<Product> products)
        {
            Session["PAGEITEMS"] = products;
            CurrentPage = 1;
            BindRepeater();
        }

        /// <summary>
        /// Manejar la excepción guardándola en sesión para después redirigirla a la
        /// página de error.
        /// </summary>
        private void HandleException(Exception ex)
        {
            try
            {
                Session["ERROR"] = ex;
                Response.Redirect(Constants.ErrorPagePath);
            }
            catch (ThreadAbortException) { }
        }


        /* ---------------------------------------------------------------------------- */
        /*                                      FILTROS                                 */
        /* ---------------------------------------------------------------------------- */

        /* --- Nombre de producto --- */
        /// <summary>
        /// Buscar productos que coincidan con el nombre del producto introducido.
        /// </summary>
        protected void btnFindByName_Click(object sender, EventArgs e)
        {
            try
            {
                string filterText = txbxNameFilter.Text;
                SetProductPagination(((List<Product>)Session["PRODUCTS"]).FindAll(p => p.Name.ToUpper().Contains(filterText.ToUpper())));
                ResetFilterStyles();
                ActiveResetFilterButton();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }


        /* --- Categoría de producto --- */
        /// <summary>
        /// Buscar productos que coincidan con la categoría seleccionada por el usuario.
        /// </summary>
        protected void btnFindByCategory_Click(object sender, EventArgs e)
        {
            try
            {
                string category = ((LinkButton) sender).CommandArgument;
                SetProductPagination(((List<Product>)Session["PRODUCTS"]).FindAll(p => p.Category.Description == category));

                // Estilos para el filtro
                ResetFilterStyles();
                ActiveResetFilterButton(CategoriesFilter, "btnResetCategory", category);
                ((LinkButton)sender).CssClass = Constants.FilterLinkSelected;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Rellenar el filtro de categorías con todas las categorías de la base de datos
        /// como también el conteo de cuántos productos coinciden con cada una.
        /// </summary>
        private void FillCategoryFilters()
        {
            Dictionary<string, int> categoryCount = new Dictionary<string, int>();

            try
            {
                foreach (Category category in (List<Category>)Session["CATEGORIES"])
                {
                    categoryCount.Add(category.Description, 0);
                }

                foreach (Product product in (List<Product>)Session["PRODUCTS"])
                {
                    if (categoryCount.ContainsKey(product.Category.Description))
                    {
                        categoryCount[product.Category.Description]++;
                    }
                }

                CategoriesFilter.DataSource = categoryCount;
                CategoriesFilter.DataBind();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Verificar si se debe desactivar un filtro de categoría si su conteo de productos
        /// que coincidan equivale a 0.
        /// </summary>
        protected void CategoriesFilter_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                KeyValuePair<string, int> filter = (KeyValuePair<string, int>)e.Item.DataItem;
                LinkButton btnFilter = (LinkButton)e.Item.FindControl("btnFindCategory");

                // Si el contador del filtro es 0, se deshabilita.
                if (filter.Value == 0)
                {
                    btnFilter.Enabled = false;
                }
            }
        }


        /* --- Marca de producto --- */
        /// <summary>
        /// Buscar productos que coincidan con la marca seleccionada por el usuario.
        /// </summary>
        protected void btnFindByBrand_Click(object sender, EventArgs e)
        {
            try
            {
                string brand = ((LinkButton) sender).CommandArgument;
                SetProductPagination(((List<Product>)Session["PRODUCTS"]).FindAll(p => p.Brand.Description == brand));

                // Estilos para el filtro
                ResetFilterStyles();
                ActiveResetFilterButton(BrandsFilter, "btnResetBrand", brand);
                ((LinkButton)sender).CssClass = Constants.FilterLinkSelected;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Rellenar el filtro de marcas con todas las marcas de la base de datos
        /// como también el conteo de cuántos productos coinciden con cada una.
        /// </summary>
        private void FillBrandFilters()
        {
            Dictionary<string, int> brandCount = new Dictionary<string, int>();

            try
            {
                foreach (Brand brand in (List<Brand>)Session["BRANDS"])
                {
                    brandCount.Add(brand.Description, 0);
                }

                foreach (Product product in (List<Product>)Session["PRODUCTS"])
                {
                    if (brandCount.ContainsKey(product.Brand.Description))
                    {
                        brandCount[product.Brand.Description]++;
                    }
                }

                BrandsFilter.DataSource = brandCount;
                BrandsFilter.DataBind();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Verificar si se debe desactivar un filtro de marca si su conteo de productos
        /// que coincidan equivale a 0.
        /// </summary>
        protected void BrandsFilter_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                KeyValuePair<string, int> filter = (KeyValuePair<string, int>)e.Item.DataItem;
                LinkButton btnFilter = (LinkButton)e.Item.FindControl("btnFindBrand");

                if (filter.Value == 0)
                {
                    btnFilter.Enabled = false;
                }
            }
        }


        /* --- Precio de producto --- */
        /// <summary>
        /// Buscar productos que coincidan con un rango de precio seleccionado 
        /// por el usuario entre las opciones disponibles.
        /// </summary>
        protected void btnFindByPrice_Click(object sender, EventArgs e)
        {
            try
            {
                string price = ((LinkButton) sender).CommandArgument;

                List<Product> filteredProducts;
                if (price == "LOW")
                {
                    filteredProducts = ((List<Product>)Session["PRODUCTS"]).FindAll(p => p.Price < 50_000);
                }
                else if (price == "MEDIUM")
                {
                    filteredProducts = ((List<Product>)Session["PRODUCTS"]).FindAll(p => p.Price >= 50_000 && p.Price < 100_000);
                }
                else if (price == "HIGH")
                {
                    filteredProducts = ((List<Product>)Session["PRODUCTS"]).FindAll(p => p.Price >= 100_000 && p.Price < 500_000);
                }
                else
                {
                    filteredProducts = ((List<Product>)Session["PRODUCTS"]).FindAll(p => p.Price >= 500_000);
                }
                SetProductPagination(filteredProducts);

                // Estilos para el filtro
                ResetFilterStyles();
                ActiveResetFilterButton(PriceFilter, "btnResetPrice", price);
                ((LinkButton)sender).CssClass = Constants.FilterLinkSelected;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Rellenar el filtro de precios con un rango manualmente determinado, así mismo
        /// también se realiza un conteo de cuántos productos coinciden con cada rango.
        /// </summary>
        private void FillPriceFilters()
        {
            Dictionary<string, int> priceCount = new Dictionary<string, int>
            {
                { "LOW", 0 },
                { "MEDIUM", 0 },
                { "HIGH", 0 },
                { "PREMIUM", 0 }
            };

            try
            {
                foreach (Product product in (List<Product>)Session["PRODUCTS"])
                {
                    if (product.Price < 50_000)
                    {
                        priceCount["LOW"]++;
                    }
                    else if (product.Price >= 50_000 && product.Price < 100_000)
                    {
                        priceCount["MEDIUM"]++;
                    }
                    else if (product.Price >= 100_000 && product.Price < 500_000)
                    {
                        priceCount["HIGH"]++;
                    }
                    else
                    {
                        priceCount["PREMIUM"]++;
                    }
                }

                PriceFilter.DataSource = priceCount;
                PriceFilter.DataBind();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Verificar si se debe desactivar un filtro de precio si su conteo de productos
        /// que coincidan equivale a 0.
        /// </summary>
        protected void PriceFilter_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                KeyValuePair<string, int> filter = (KeyValuePair<string, int>)e.Item.DataItem;
                LinkButton btnFilter = (LinkButton)e.Item.FindControl("btnFindPrice");

                if (filter.Value == 0)
                {
                    btnFilter.Enabled = false;
                }

                if (filter.Key == "LOW")
                {
                    btnFilter.Text = $"Hasta $50.000 ({filter.Value})";
                } 
                else if (filter.Key == "MEDIUM")
                {
                    btnFilter.Text = $"$50.000 a $100.000 ({filter.Value})";
                }
                else if (filter.Key == "HIGH")
                {
                    btnFilter.Text = $"$100.000 a $500.000 ({filter.Value})";
                }
                else
                {
                    btnFilter.Text = $"Superior a $500.000 ({filter.Value})";
                }
            }
        }

        /// <summary>
        /// Buscar productos que coincidan con un rango de precio personalizado 
        /// por el usuario.
        /// </summary>
        protected void btnCustomPrice_Click(object sender, EventArgs e)
        {
            try
            {
                // Verificar precio mínimo
                if (!decimal.TryParse(txbxMinPriceFilter.Text, out decimal minPrice) || minPrice < 0)
                {
                    InvalidMinPrice();
                    return;
                }

                // Verificar precio máximo
                if (!decimal.TryParse(txbxMaxPriceFilter.Text, out decimal maxPrice))
                {
                    InvalidMaxPrice();
                    return;
                }

                // Verificar que precio máximo sea mayor a precio mínimo
                if (maxPrice < minPrice)
                {
                    InvalidMinPrice();
                    InvalidMaxPrice();
                    return;
                }

                SetProductPagination(((List<Product>)Session["PRODUCTS"]).FindAll(p => p.Price >= minPrice && p.Price <= maxPrice));
                ResetFilterStyles();
                ActiveResetFilterButton();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Configurar interfaz de usuario cuando el precio mínimo ingresado no
        /// cumple con los requisitos necesarios.
        /// </summary>
        private void InvalidMinPrice()
        {
            txbxMinPriceFilter.CssClass = Constants.FormControlInvalid;
            pnlMinPrice.Visible = true;
        }

        /// <summary>
        /// Configurar interfaz de usuario cuando el precio máximo ingresado no
        /// cumple con los requisitos necesarios.
        /// </summary>
        private void InvalidMaxPrice()
        {
            txbxMaxPriceFilter.CssClass = Constants.FormControlInvalid;
            pnlMaxPrice.Visible = true;
        }


        /* --- Reinicio de filtros --- */
        /// <summary>
        /// Activar el botón de reset del buscador (general).
        /// </summary>
        private void ActiveResetFilterButton()
        {
            btnResetFilters.CssClass = Constants.ResetFilterButtonEnabled;
        }

        /// <summary>
        /// Activar el botón de reset del filtro seleccionado (particular) y 
        /// también del buscador (general).
        /// </summary>
        /// <param name="repeater">Filtro (categorías, marcas, precio)</param>
        /// <param name="btnResetID">ID del botón de reinicio</param>
        /// <param name="filter">Filtro seleccionado</param>
        private void ActiveResetFilterButton(Repeater repeater, string btnResetID, string filter)
        {
            foreach (RepeaterItem item in repeater.Items)
            {
                LinkButton button = (LinkButton)item.FindControl(btnResetID);
                if (button != null && button.CommandArgument == filter)
                {
                    button.Visible = true;
                }
                else
                {
                    button.Visible = false;
                }
            }

            btnResetFilters.CssClass = Constants.ResetFilterButtonEnabled;
        }

        /// <summary>
        /// Reiniciar los estilos de todos los filtros y ocultar todos los botones de reinicio.
        /// </summary>
        private void ResetFilterStyles()
        {
            // Categoría
            foreach (RepeaterItem item in CategoriesFilter.Items)
            {
                LinkButton filter = (LinkButton)item.FindControl("btnFindCategory");
                LinkButton resetButton = (LinkButton)item.FindControl("btnResetCategory");
                if (filter != null && resetButton != null)
                {
                    filter.CssClass = Constants.FilterLinkNormal;
                    resetButton.Visible = false;
                }
            }

            // Marca
            foreach (RepeaterItem item in BrandsFilter.Items)
            {
                LinkButton filter = (LinkButton)item.FindControl("btnFindBrand");
                LinkButton resetButton = (LinkButton)item.FindControl("btnResetBrand");
                if (filter != null && resetButton != null)
                {
                    filter.CssClass = Constants.FilterLinkNormal;
                    resetButton.Visible = false;
                }
            }

            // Precio
            foreach (RepeaterItem item in PriceFilter.Items)
            {
                LinkButton filter = (LinkButton)item.FindControl("btnFindPrice");
                LinkButton resetButton = (LinkButton)item.FindControl("btnResetPrice");
                if (filter != null && resetButton != null)
                {
                    filter.CssClass = Constants.FilterLinkNormal;
                    resetButton.Visible = false;
                }
            }
            txbxMinPriceFilter.CssClass = Constants.FormControlNormal;
            pnlMinPrice.Visible = false;
            txbxMaxPriceFilter.CssClass = Constants.FormControlNormal;
            pnlMaxPrice.Visible = false;
        }

        /// <summary>
        /// Reiniciar todos los filtros de búsqueda (reiniciar estilos, ocultar todos
        /// los botones de reinicio y volver a bindear todos los productos).
        /// </summary>
        protected void btnResetFilter_Click(object sender, EventArgs e)
        {
            try
            {
                ResetFilterStyles();
                txbxNameFilter.Text = string.Empty;
                txbxMinPriceFilter.Text = string.Empty;
                txbxMaxPriceFilter.Text = string.Empty;
                btnResetFilters.CssClass = Constants.ResetFilterButtonDisabled;
                SetProductPagination((List<Product>)Session["PRODUCTS"]);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }


        /* ---------------------------------------------------------------------------- */
        /*                                   PAGINACIÓN                                 */
        /* ---------------------------------------------------------------------------- */
        /// <summary>
        /// Vincular los productos al repetidor según la paginación actual.
        /// </summary>
        private void BindRepeater()
        {
            try
            {
                int skip = (CurrentPage - 1) * PageSize;
                List<Product> pagedItems = ((List<Product>)Session["PAGEITEMS"]).Skip(skip).Take(PageSize).ToList();
                ProductCards.DataSource = pagedItems;
                ProductCards.DataBind();

                // Verificación de la exitencia de productos
                if (pagedItems.Count == 0)
                    alertProductNotFound.Visible = true;
                else
                    alertProductNotFound.Visible = false;

                // Habilitar/Deshabilitar botones de paginación
                btnPrev.Enabled = CurrentPage > 1;
                btnNext.Enabled = (skip + PageSize) < ((List<Product>)Session["PAGEITEMS"]).Count;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Retroceder una página de la paginación de productos vinculados en el repetidor.
        /// </summary>
        protected void btnPrev_Click(object sender, EventArgs e)
        {
            CurrentPage--;
            BindRepeater();
        }

        /// <summary>
        /// Avanzar una página de la paginación de productos vinculados en el repetidor.
        /// </summary>
        protected void btnNext_Click(object sender, EventArgs e)
        {
            CurrentPage++;
            BindRepeater();
        }
    }
}