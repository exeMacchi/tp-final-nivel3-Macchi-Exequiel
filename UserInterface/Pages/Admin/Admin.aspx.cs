using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using Domain;

namespace UserInterface.Pages.Admin
{
    public partial class Admin : System.Web.UI.Page
    {
        /* ---------------------------------------------------------------------------- */
        /*                                      GENERAL                                 */
        /* ---------------------------------------------------------------------------- */
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    // En la página de administrador, siempre que se cargue por primera vez,
                    // se recargan los productos por el caso de que esta página provenga
                    // de una operación de creación, modificación o eliminación.
                    Session["PRODUCTS"] = ProductBLL.GetProducts();
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
                        BindProductsToGridView((List<Product>)Session["PRODUCTS"]);
                    }
                    else
                    {
                        ShowNoProductsDBAlert();
                    }

                    InitializeAdminStyles();
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }

        /// <summary>
        /// Configurar la interfaz de usuario predeterminada para la página de administrador.
        /// </summary>
        private void InitializeAdminStyles()
        {
            // Estilos Filtros avanzados.
            ddlFirstCriteria.CssClass = Constants.FormSelectOptionSelected;
            ddlSecondCriteria.CssClass = Constants.FormSelectOptionSelected;
        }

        /// <summary>
        /// Vincular los productos con el <see cref="gvProducts"/>
        /// </summary>
        /// <param name="products">Lista de productos</param>
        private void BindProductsToGridView(List<Product> products)
        {
            gvProducts.DataSource = products;
            gvProducts.DataBind();
            alertProductNotFound.Visible = false;
            alertEmptyGV.Visible = false;
        }

        /// <summary>
        /// Mostrar alerta de que no hay actualmente productos en la base de datos.
        /// </summary>
        private void ShowNoProductsDBAlert()
        {
            alertEmptyGV.Visible = true;
            alertProductNotFound.Visible = false; // Por las dudas.
        }

        /// <summary>
        /// Manejar la excepción guardándola en sesión para después rederigirla a la
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

        /// <summary>
        /// Evento que ocurre cuando se cambia de página (paginación activada).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvProducts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvProducts.PageIndex = e.NewPageIndex;
                gvProducts.DataSource = (List<Product>)Session["PRODUCTS"];
                gvProducts.DataBind();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }


        /* ---------------------------------------------------------------------------- */
        /*                                    MODIFICAR                                 */
        /* ---------------------------------------------------------------------------- */
        /// <summary>
        /// Evento que se lanza cuando se quiere modificar un producto.
        /// </summary>
        protected void gvProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string id = gvProducts.SelectedDataKey.Value.ToString();
                Response.Redirect($"{Constants.CreateEditPagePath}?id={id}");
            }
            catch (ThreadAbortException) { }
        }


        /* ---------------------------------------------------------------------------- */
        /*                                     ELIMINAR                                 */
        /* ---------------------------------------------------------------------------- */
        /// <summary>
        /// Lanzar el modal de advertencia sobre la eliminación física del registro seleccionado.
        /// </summary>
        protected void gvProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string id = gvProducts.DataKeys[e.RowIndex].Value.ToString();
            string nombre = gvProducts.Rows[e.RowIndex].Cells[1].Text;

            lbDeleteProduct.Text = nombre;
            btnDeleteConfirm.CommandArgument = id;

            string script = @"<script type='text/javascript'>
                                document.addEventListener('DOMContentLoaded', function() {
                                    document.getElementById('btnDeleteProduct').click();
                                });
                              </script>";
            ClientScript.RegisterStartupScript(this.GetType(), "ShowModalScript", script);
        }

        /// <summary>
        /// Eliminar de forma física un registro en la base de datos.
        /// </summary>
        protected void btnDeleteConfirm_Click(object sender, EventArgs e)
        {
            int id = int.Parse(((Button)sender).CommandArgument);

            try
            {
                DeleteExistingLocalImageIfNeeded(id);

                ProductBLL.DeleteProduct(id);
                Session["ALERTMESSAGE"] = "El producto fue eliminado de la base de datos de forma exitosa.";
                Response.Redirect($"{Constants.AdminPagePath}?alert=success");
            }
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Eliminar una posible imagen local vinculada al producto que el usuario
        /// quiere eliminar.
        /// </summary>
        private void DeleteExistingLocalImageIfNeeded(int id)
        {
            try
            {
                string productImage = ProductBLL.GetProductImage(id);
                if (productImage.StartsWith(Constants.LocalImagePath))
                {
                    string localImagePath = Server.MapPath(productImage);
                    if (File.Exists(localImagePath))
                    {
                        File.Delete(localImagePath);
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }


        /* ---------------------------------------------------------------------------- */
        /*                                     FILTROS                                  */
        /* ---------------------------------------------------------------------------- */
        /// <summary>
        /// Filtrar la lista de productos en sesión según un criterio (dependiendo
        /// qué tipo de filtro esté activado) y actualizar el <see cref="gvProducts"/> 
        /// con los resultados obtenidos.
        /// </summary>
        protected void btnFind_Click(object sender, EventArgs e)
        {
            try
            {
                List<Product> filteredProducts = GetFilteredProducts();
                BindProductsToGridView(filteredProducts);

                // Si no se encuentra resultados.
                if (filteredProducts.Count == 0)
                {
                    ShowProductNotFoundAlert();
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Devolver una lista de productos filtradas según el filtro activado
        /// (filtro avanzado = base de datos || filtro básico = lista en sesión)
        /// </summary>
        /// <returns>Lista de productos filtrados</returns>
        private List<Product> GetFilteredProducts()
        {
            try
            {
                string filterText = txbxFilter.Text;

                // Filtro avanzado (base de datos)
                if (advancedPanel.Visible)
                {
                    string condition = GetAdvancedSearchCondition(filterText);
                    return ProductBLL.SearchProducts(condition);
                }
                // Filtro básico (lista)
                else
                {
                    return ((List<Product>)Session["PRODUCTS"]).FindAll(p => p.Name.ToUpper().Contains(filterText.ToUpper()));
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return null; // Esto es solo por el compilador
            }
        }

        /// <summary>
        /// Devolver la condición de filtrado requerida en la consulta a la base de datos
        /// del filtro avanzado
        /// </summary>
        /// <param name="filterText">Texto introducido en <see cref="txbxFilter"/></param>
        /// <returns>Condición de filtrado completo</returns>
        private string GetAdvancedSearchCondition(string filterText)
        {
            string firstCriteria = ddlFirstCriteria.SelectedValue;
            string secondCriteria = ddlSecondCriteria.SelectedValue;
            return Auxiliary.CreateCondition(firstCriteria, secondCriteria, filterText);
        }

        /// <summary>
        /// Mostrar alerta de que no se encontraron productos según el filtro utilizado.
        /// </summary>
        private void ShowProductNotFoundAlert()
        {
            alertProductNotFound.Visible = true;
            alertEmptyGV.Visible = false; // Por las dudas.
        }

        /// <summary>
        /// Activar o desactivar el filtro avanzado de búsqueda.
        /// </summary>
        protected void btnAdvanced_Click(object sender, EventArgs e)
        {
            if (advancedPanel.Visible)
            {
                ConfigureBasicFilter();
            }
            else
            {
                ConfigureAdvancedFilter();
            }

            ResetFilter();
        }

        /// <summary>
        /// Configurar el filtro al sistema básico.
        /// </summary>
        private void ConfigureBasicFilter()
        {
            txbxFilter.Attributes["placeholder"] = "Buscar producto por nombre...";
            advancedPanel.Visible = false;
        }

        /// <summary>
        /// Configurar el filtro al sistema avanzado.
        /// </summary>
        private void ConfigureAdvancedFilter()
        {
            // Primer criterio de búsqueda.
            ddlFirstCriteria.Items.Clear();
            ddlFirstCriteria.Items.Add(new ListItem("Nombre", "Name"));
            ddlFirstCriteria.Items.Add(new ListItem("Código", "Code"));
            ddlFirstCriteria.Items.Add(new ListItem("Marca", "Brand"));
            ddlFirstCriteria.Items.Add(new ListItem("Categoría", "Category"));
            ddlFirstCriteria.Items.Add(new ListItem("Precio", "Price"));

            // Segundo criterio de búsqueda por defecto ("Nombre").
            ddlSecondCriteria.Items.Clear();
            ddlSecondCriteria.Items.Add(new ListItem("Comienza con...", "Starts"));
            ddlSecondCriteria.Items.Add(new ListItem("Contiene...", "Contains"));
            ddlSecondCriteria.Items.Add(new ListItem("Termina con...", "Ends"));

            txbxFilter.Attributes["placeholder"] = "Buscar producto por nombre...";
            advancedPanel.Visible = true;
        }

        /// <summary>
        /// Reiniciar los productos filtrados.
        /// </summary>
        private void ResetFilter()
        {
            try
            {
                // Reinicio
                txbxFilter.Text = string.Empty;
                BindProductsToGridView((List<Product>)Session["PRODUCTS"]);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Cargar el <see cref="ddlSecondCriteria"/> según la selección del primer
        /// criterio en <see cref="ddlFirstCriteria"/>.
        /// </summary>
        protected void ddlFirstCriteria_SelectedIndexChanged(object sender, EventArgs e)
        {
            txbxFilter.Text = string.Empty;
            // Criterio por valor
            if (ddlFirstCriteria.SelectedValue == "Price")
            {
                ddlSecondCriteria.Items.Clear();
                ddlSecondCriteria.Items.Add(new ListItem("Mayor a...", "Greater"));
                ddlSecondCriteria.Items.Add(new ListItem("Igual a...", "Equal"));
                ddlSecondCriteria.Items.Add(new ListItem("Menor a...", "Smaller"));
                txbxFilter.Attributes["placeholder"] = "Buscar producto por precio...";
            }
            // Criterio por texto
            else
            {
                ddlSecondCriteria.Items.Clear();
                ddlSecondCriteria.Items.Add(new ListItem("Comienza con...", "Starts"));
                ddlSecondCriteria.Items.Add(new ListItem("Contiene...", "Contains"));
                ddlSecondCriteria.Items.Add(new ListItem("Termina con...", "Ends"));
                txbxFilter.Attributes["placeholder"] = $"Buscar producto por {ddlFirstCriteria.SelectedItem.Text.ToLower()}...";
            }
        }
    }
}