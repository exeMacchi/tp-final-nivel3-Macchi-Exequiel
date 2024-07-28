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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (((List<Product>)Session["PRODUCTS"]).Count > 0)
                    {
                        ProductCards.DataSource = (List<Product>)Session["PRODUCTS"];
                        ProductCards.DataBind();

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
                        // Se bloquea la búsqueda de productos.
                        alertEmptyDB.Visible = true;
                        alertProductNotFound.Visible = false;
                        txbxFilter.Enabled = false;
                        btnFind.Enabled = false;
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de errores
                    throw ex;
                }
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!(Session["PRODUCTS"] != null))
            {
                Session["PRODUCTS"] = ProductBBL.GetProducts();
            }

            if (!(Session["CATEGORIES"] != null))
            {
                Session["CATEGORIES"] = CategoryBBL.GetCategories();
            }

            if (!(Session["BRANDS"] != null))
            {
                Session["BRANDS"] = BrandBBL.GetBrands();
            }
        }

        /// <summary>
        /// Buscar productos que coincidan con el nombre del producto introducido.
        /// </summary>
        protected void btnFindByName_Click(object sender, EventArgs e)
        {
            string filterText = txbxFilter.Text;
            List<Product> filteredProducts = ((List<Product>)Session["PRODUCTS"]).FindAll(p => p.Name.ToUpper().Contains(filterText.ToUpper()));
            ProductCards.DataSource = filteredProducts;
            ProductCards.DataBind();

            // Si no se encuentra resultados.
            if (filteredProducts.Count == 0)
            {
                alertProductNotFound.Visible = true;
                alertEmptyDB.Visible = false; // Por las dudas.
            }
        }

        /// <summary>
        /// Buscar productos que coincidan con la categoría seleccionada por el usuario.
        /// </summary>
        protected void btnFindByCategory_Click(object sender, EventArgs e)
        {
            string category = ((LinkButton) sender).CommandArgument;

            List<Product> filteredProducts = ((List<Product>)Session["PRODUCTS"]).FindAll(p => p.Category.Description == category);
            ProductCards.DataSource = filteredProducts;
            ProductCards.DataBind();

            // Si no se encuentra resultados.
            if (filteredProducts.Count == 0)
            {
                alertProductNotFound.Visible = true;
                alertEmptyDB.Visible = false; // Por las dudas.
            }

            // Estilos para el filtro
            ResetFilterStyles();
            ActiveResetButton(CategoriesFilter, "btnResetCategory", category);
            ((LinkButton)sender).CssClass = Constants.FilterLinkSelected;
        }

        /// <summary>
        /// Buscar productos que coincidan con la marca seleccionada por el usuario.
        /// </summary>
        protected void btnFindByBrand_Click(object sender, EventArgs e)
        {
            string brand = ((LinkButton) sender).CommandArgument;
            List<Product> filteredProducts = ((List<Product>)Session["PRODUCTS"]).FindAll(p => p.Brand.Description == brand);
            ProductCards.DataSource = filteredProducts;
            ProductCards.DataBind();

            // Si no se encuentra resultados.
            if (filteredProducts.Count == 0)
            {
                alertProductNotFound.Visible = true;
                alertEmptyDB.Visible = false; // Por las dudas.
            }

            // Estilos para el filtro
            ResetFilterStyles();
            ActiveResetButton(BrandsFilter, "btnResetBrand", brand);
            ((LinkButton)sender).CssClass = Constants.FilterLinkSelected;
        }

        /// <summary>
        /// Buscar productos que coincidan con el rango de precio seleccionado por el usuario.
        /// </summary>
        protected void btnFindByPrice_Click(object sender, EventArgs e)
        {
            string price = ((LinkButton) sender).CommandArgument;
            List<Product> filteredProducts = (List<Product>)Session["PRODUCTS"];

            if (price == "LOW")
            {
                filteredProducts = filteredProducts.FindAll(p => p.Price < 50_000);
            }
            else if (price == "MEDIUM")
            {
                filteredProducts = filteredProducts.FindAll(p => p.Price >= 50_000 && p.Price < 100_000);
            }
            else if (price == "HIGH")
            {
                filteredProducts = filteredProducts.FindAll(p => p.Price >= 100_000 && p.Price < 500_000);
            }
            else
            {
                filteredProducts = filteredProducts.FindAll(p => p.Price >= 500_000);
            }

            ProductCards.DataSource = filteredProducts;
            ProductCards.DataBind();

            // Si no se encuentra resultados.
            if (filteredProducts.Count == 0)
            {
                alertProductNotFound.Visible = true;
                alertEmptyDB.Visible = false; // Por las dudas.
            }

            // Estilos para el filtro
            ResetFilterStyles();
            ActiveResetButton(PriceFilter, "btnResetPrice", price);
            ((LinkButton)sender).CssClass = Constants.FilterLinkSelected;
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
                // TODO: manejar error
                throw ex;
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
                // TODO: manejar error
                throw ex;
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
                // TODO: manejar error
                throw ex;
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
            ResetFilterStyles();
            ProductCards.DataSource = (List<Product>)Session["PRODUCTS"];
            ProductCards.DataBind();
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
    }
}