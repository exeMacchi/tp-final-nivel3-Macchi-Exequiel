using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Domain;
using BusinessLogic;

namespace UserInterface.Pages.Global
{
    public partial class Products : System.Web.UI.Page
    {
        // Número de elementos por página
        private const int PageSize = 6;

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
                        Session["PRODUCTS"] = ProductBBL.GetProducts();
                    }

                    if (Session["USER"] != null)
                    {
                        // Se actualiza siempre la lista de productos favoritos cuando se recarga
                        // la página porque el usuario puede que haya agregado o eliminado algún
                        // producto de su lista de favoritos.
                        Session["FAVORITEPRODUCTS"] = ProductBBL.GetFavoriteProducts(((Domain.User)Session["USER"]).ID);
                    }

                    if (Session["CATEGORIES"] == null)
                    {
                        Session["CATEGORIES"] = CategoryBBL.GetCategories();
                    }

                    if (Session["BRANDS"] == null)
                    {
                        Session["BRANDS"] = BrandBBL.GetBrands();
                    }
                }
                catch (Exception ex)
                {
                    Session["ERROR"] = ex;
                    Response.Redirect(Constants.ErrorPagePath);
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
                        Session["PAGEITEMS"] = (List<Product>)Session["PRODUCTS"];
                        CurrentPage = 1;
                        BindRepeater();

                        fillCategoryFilter();
                        fillBrandFilter();
                        fillPriceFilter();

                        txbxFilter.Enabled = true;
                        btnFind.Enabled = true;
                        alertEmptyDB.Visible = false;
                        alertProductNotFound.Visible = false;
                    }
                    else
                    {
                        alertEmptyDB.Visible = true;
                        alertProductNotFound.Visible = false;
                        // Se bloquea la búsqueda de productos.
                        txbxFilter.Enabled = false;
                        btnFind.Enabled = false;
                        btnPrev.Enabled = false;
                        btnNext.Enabled = false;
                    }
                }
                catch (Exception ex)
                {
                    Session["ERROR"] = ex;
                    Response.Redirect(Constants.ErrorPagePath);
                }
            }
        }

        /// <summary>
        /// Buscar productos que coincidan con el nombre del producto introducido.
        /// </summary>
        protected void btnFindByName_Click(object sender, EventArgs e)
        {
            try
            {
                string filterText = txbxFilter.Text;
                Session["PAGEITEMS"] = ((List<Product>)Session["PRODUCTS"]).FindAll(p => p.Name.ToUpper().Contains(filterText.ToUpper()));
                CurrentPage = 1;
                BindRepeater();
                ResetFilterStyles();
            }
            catch (Exception ex)
            {
                Session["ERROR"] = ex;
                Response.Redirect(Constants.ErrorPagePath);
            }
        }

        /// <summary>
        /// Buscar productos que coincidan con la categoría seleccionada por el usuario.
        /// </summary>
        protected void btnFindByCategory_Click(object sender, EventArgs e)
        {
            try
            {
                string category = ((LinkButton) sender).CommandArgument;
                Session["PAGEITEMS"] = ((List<Product>)Session["PRODUCTS"]).FindAll(p => p.Category.Description == category);
                CurrentPage = 1;
                BindRepeater();

                // Estilos para el filtro
                ResetFilterStyles();
                ActiveResetButton(CategoriesFilter, "btnResetCategory", category);
                ((LinkButton)sender).CssClass = Constants.FilterLinkSelected;
            }
            catch (Exception ex)
            {
                Session["ERROR"] = ex;
                Response.Redirect(Constants.ErrorPagePath);
            }
        }

        /// <summary>
        /// Buscar productos que coincidan con la marca seleccionada por el usuario.
        /// </summary>
        protected void btnFindByBrand_Click(object sender, EventArgs e)
        {
            try
            {
                string brand = ((LinkButton) sender).CommandArgument;
                Session["PAGEITEMS"] = ((List<Product>)Session["PRODUCTS"]).FindAll(p => p.Brand.Description == brand);
                CurrentPage = 1;
                BindRepeater();

                // Estilos para el filtro
                ResetFilterStyles();
                ActiveResetButton(BrandsFilter, "btnResetBrand", brand);
                ((LinkButton)sender).CssClass = Constants.FilterLinkSelected;
            }
            catch (Exception ex)
            {
                Session["ERROR"] = ex;
                Response.Redirect(Constants.ErrorPagePath);
            }
        }

        /// <summary>
        /// Buscar productos que coincidan con el rango de precio seleccionado por el usuario.
        /// </summary>
        protected void btnFindByPrice_Click(object sender, EventArgs e)
        {
            try
            {
                string price = ((LinkButton) sender).CommandArgument;

                if (price == "LOW")
                {
                    Session["PAGEITEMS"] = ((List<Product>)Session["PRODUCTS"]).FindAll(p => p.Price < 50_000);
                }
                else if (price == "MEDIUM")
                {
                    Session["PAGEITEMS"] = ((List<Product>)Session["PRODUCTS"]).FindAll(p => p.Price >= 50_000 && p.Price < 100_000);
                }
                else if (price == "HIGH")
                {
                    Session["PAGEITEMS"] = ((List<Product>)Session["PRODUCTS"]).FindAll(p => p.Price >= 100_000 && p.Price < 500_000);
                }
                else
                {
                    Session["PAGEITEMS"] = ((List<Product>)Session["PRODUCTS"]).FindAll(p => p.Price >= 500_000);
                }
                CurrentPage = 1;
                BindRepeater();

                // Estilos para el filtro
                ResetFilterStyles();
                ActiveResetButton(PriceFilter, "btnResetPrice", price);
                ((LinkButton)sender).CssClass = Constants.FilterLinkSelected;
            }
            catch (Exception ex)
            {
                Session["ERROR"] = ex;
                Response.Redirect(Constants.ErrorPagePath);
            }
        }

        /// <summary>
        /// Rellenar el filtro de categorías con todas las categorías de la base de datos
        /// como también el conteo de cuántos productos coinciden con cada una.
        /// </summary>
        private void fillCategoryFilter()
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
                Session["ERROR"] = ex;
                Response.Redirect(Constants.ErrorPagePath);
            }
        }

        /// <summary>
        /// Rellenar el filtro de marcas con todas las marcas de la base de datos
        /// como también el conteo de cuántos productos coinciden con cada una.
        /// </summary>
        private void fillBrandFilter()
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
                Session["ERROR"] = ex;
                Response.Redirect(Constants.ErrorPagePath);
            }
        }

        /// <summary>
        /// Rellenar el filtro de precios con un rango manualmente determinado, así mismo
        /// también se realiza un conteo de cuántos productos coinciden con cada rango.
        /// </summary>
        private void fillPriceFilter()
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
                Session["ERROR"] = ex;
                Response.Redirect(Constants.ErrorPagePath);
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
                Session["ERROR"] = ex;
                Response.Redirect(Constants.ErrorPagePath);
            }
        }

        /// <summary>
        /// Activar el botón de reset del filtro seleccionado.
        /// </summary>
        /// <param name="repeater">Filtro (categorías, marcas, precio)</param>
        /// <param name="btnResetID">ID del botón de reinicio</param>
        /// <param name="filter">Filtro seleccionado</param>
        private void ActiveResetButton(Repeater repeater, string btnResetID, string filter)
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
        }

        /// <summary>
        /// Reiniciar todos los filtros de búsqueda (reinciar estilos, ocultar todos
        /// los botones de reinicio y volver a bindear todos los productos).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ResetAllFilters(object sender, EventArgs e)
        {
            try
            {
                ResetFilterStyles();
                Session["PAGEITEMS"] = (List<Product>)Session["PRODUCTS"];
                CurrentPage = 1;
                BindRepeater();
            }
            catch (Exception ex)
            {
                Session["ERROR"] = ex;
                Response.Redirect(Constants.ErrorPagePath);
            }
        }

        /// <summary>
        /// Reiniciar los estilos de todos los filtros y ocultar todos los botones de reinicio.
        /// </summary>
        private void ResetFilterStyles()
        {
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
        }

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
                Session["ERROR"] = ex;
                Response.Redirect(Constants.ErrorPagePath);
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