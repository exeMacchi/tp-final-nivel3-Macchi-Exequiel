using BusinessLogic;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UserInterface.Pages.User
{
    public partial class Favorites : System.Web.UI.Page
    {
        // Número de elementos por página
        private const int PageSize = 8;

        private int CurrentPage
        {
            get { return int.Parse(hfCurrentPage.Value); }
            set { hfCurrentPage.Value = value.ToString(); }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Se actualiza siempre la lista de productos favoritos cuando se recarga
                // la página porque el usuario puede que haya agregado o eliminado algún
                // producto de su lista de favoritos.
                Session["FAVORITEPRODUCTS"] = ProductBBL.GetFavoriteProducts(((Domain.User)Session["USER"]).ID);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (((List<Product>)Session["FAVORITEPRODUCTS"]).Count > 0)
                    {
                        Session["PAGEITEMS"] = (List<Product>)Session["FAVORITEPRODUCTS"];
                        CurrentPage = 1;
                        BindRepeater();
                        FillCriterias();

                        txbxFilter.Enabled = true;
                        btnFind.Enabled = true;
                        alertEmptyFavoriteList.Visible = false;
                        alertFavoriteNotFound.Visible = false;
                    }
                    else
                    {
                        // Alerta de lista de favoritos vacía
                        alertEmptyFavoriteList.Visible = true;
                        alertFavoriteNotFound.Visible = false;

                        // Filtro de búsqueda y paginación bloqueados
                        txbxFilter.Enabled = false;
                        ddlFirstCriteria.Enabled = false;
                        ddlSecondCriteria.Enabled = false;
                        btnFind.Enabled = false;
                        btnPrev.Enabled = false;
                        btnNext.Enabled = false;
                    }
                }
                catch (Exception ex)
                {
                    // TODO: manejar error
                    throw ex;
                }
            }
        }

        /* --- Filtros --- */

        /// <summary>
        /// Filtrar la lista de productos favoritos desde la base de datos
        /// según un criterio (dependiendo qué tipo de filtro esté activado) y 
        /// actualizar el <see cref="FavoriteProductCards"/> con los resultados 
        /// obtenidos.
        /// </summary>
        protected void btnFind_Click(object sender, EventArgs e)
        {
            string filterText = txbxFilter.Text;
            string firstCriteria = ddlFirstCriteria.SelectedValue;
            string secondCriteria = ddlSecondCriteria.SelectedValue;
            string condition = Auxiliary.CreateCondition(firstCriteria, secondCriteria, filterText);

            try
            {
                Session["PAGEITEMS"] = ProductBBL.SearchFavoriteProducts(((Domain.User)Session["USER"]).ID, condition);
                CurrentPage = 1;
                BindRepeater();

                btnResetFilter.CssClass = Constants.ResetFilterButtonEnabled;
            }
            catch (Exception ex)
            {
                // TODO: manejar error
                throw ex;
            }
        }

        /// <summary>
        /// Cargar el <see cref="ddlSecondCriteria"/> según la selección del primer
        /// criterio en <see cref="ddlFirstCriteria"/>.
        /// </summary>
        protected void ddlFirstCriteria_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Criterio por valor
            if (ddlFirstCriteria.SelectedValue == "Price")
            {
                ddlSecondCriteria.Items.Clear();
                ddlSecondCriteria.Items.Add(new ListItem("Mayor a...", "Greater"));
                ddlSecondCriteria.Items.Add(new ListItem("Igual a...", "Equal"));
                ddlSecondCriteria.Items.Add(new ListItem("Menor a...", "Smaller"));
                txbxFilter.Attributes["placeholder"] = "Buscar producto favorito por precio...";
            }
            // Criterio por texto
            else
            {
                ddlSecondCriteria.Items.Clear();
                ddlSecondCriteria.Items.Add(new ListItem("Comienza con...", "Starts"));
                ddlSecondCriteria.Items.Add(new ListItem("Contiene...", "Contains"));
                ddlSecondCriteria.Items.Add(new ListItem("Termina con...", "Ends"));
                txbxFilter.Attributes["placeholder"] = $"Buscar producto favorito por {ddlFirstCriteria.SelectedItem.Text.ToLower()}...";
            }

            txbxFilter.Text = string.Empty;
        }

        /// <summary>
        /// Rellenar los filtros avanzados <see cref="ddlFirstCriteria"/> y <see cref="ddlSecondCriteria"/> 
        /// y vaciar el buscador.
        /// </summary>
        private void FillCriterias()
        {
            // Primer criterio de búsqueda.
            ddlFirstCriteria.CssClass = Constants.FormSelectOptionSelected;
            ddlFirstCriteria.Items.Clear();
            ddlFirstCriteria.Items.Add(new ListItem("Nombre", "Name"));
            ddlFirstCriteria.Items.Add(new ListItem("Código", "Code"));
            ddlFirstCriteria.Items.Add(new ListItem("Marca", "Brand"));
            ddlFirstCriteria.Items.Add(new ListItem("Categoría", "Category"));
            ddlFirstCriteria.Items.Add(new ListItem("Precio", "Price"));

            // Segundo criterio de búsqueda por defecto ("Nombre").
            ddlSecondCriteria.CssClass = Constants.FormSelectOptionSelected;
            ddlSecondCriteria.Items.Clear();
            ddlSecondCriteria.Items.Add(new ListItem("Comienza con...", "Starts"));
            ddlSecondCriteria.Items.Add(new ListItem("Contiene...", "Contains"));
            ddlSecondCriteria.Items.Add(new ListItem("Termina con...", "Ends"));

            txbxFilter.Attributes["placeholder"] = "Buscar producto favorito por nombre...";
            txbxFilter.Text = string.Empty;
        }

        /// <summary>
        /// Reiniciar los filtros de búsqueda en su configuración inicial.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnResetFilter_Click(object sender, EventArgs e)
        {
            FillCriterias();
            Session["PAGEITEMS"] = (List<Product>)Session["FAVORITEPRODUCTS"];
            CurrentPage = 1;
            BindRepeater();
            btnResetFilter.CssClass = Constants.ResetFilterButtonDisabled;
        }

        /* --- Paginación --- */

        /// <summary>
        /// Vincular los productos favoritos al repetidor según la paginación actual.
        /// </summary>
        private void BindRepeater()
        {
            int skip = (CurrentPage - 1) * PageSize;
            List<Product> pagedItems = ((List<Product>)Session["PAGEITEMS"]).Skip(skip).Take(PageSize).ToList();
            FavoriteProductCards.DataSource = pagedItems;
            FavoriteProductCards.DataBind();

            // Verificación de la existencia de favoritos
            if (pagedItems.Count == 0)
                alertFavoriteNotFound.Visible = true;
            else
                alertFavoriteNotFound.Visible = false;

            // Habilitar/Deshabilitar botones de paginación.
            btnPrev.Enabled = CurrentPage > 1;
            btnNext.Enabled = (skip + PageSize) < ((List<Product>)Session["PAGEITEMS"]).Count;
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