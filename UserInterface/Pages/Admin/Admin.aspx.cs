using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using Domain;

namespace UserInterface.Pages.Admin
{
    public partial class Admin : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    // En la página de administrador, siempre que se cargue por primera vez,
                    // se recargan los productos por el caso de que esta página provenga
                    // de una operación de creación, modificación o eliminación.
                    Session["PRODUCTS"] = ProductBBL.GetProducts();
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
                        alertEmptyGV.Visible = false;
                        alertProductNotFound.Visible = false;
                        gvProducts.DataSource = (List<Product>)Session["PRODUCTS"];
                        gvProducts.DataBind();
                    }
                    else
                    {
                        alertProductNotFound.Visible = false; // Por las dudas.
                        alertEmptyGV.Visible = true;
                    }

                    // Estilos Filtros avanzados.
                    ddlFirstCriteria.CssClass = Constants.FormSelectOptionSelected;
                    ddlSecondCriteria.CssClass = Constants.FormSelectOptionSelected;
                }
                catch (Exception ex)
                {
                    Session["Error"] = ex;
                    Response.Redirect(Constants.ErrorPagePath);
                }
            }
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
                Session["Error"] = ex;
                Response.Redirect(Constants.ErrorPagePath);
            }
        }

        /// <summary>
        /// Evento que se lanza cuando se quiere modificar un producto.
        /// </summary>
        protected void gvProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id = gvProducts.SelectedDataKey.Value.ToString();
            Response.Redirect($"{Constants.CreateEditPagePath}?id={id}");
        }

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
                // Verificar si el producto que se quiere eliminar tiene alguna referencia
                // de una imagen local. Si tiene una imagen local, se elimina antes de que
                // se elimine el producto.
                string productImage = ProductBBL.GetProductImage(id);
                if (productImage.StartsWith(Constants.LocalImagePath))
                {
                    string localImagePath = Server.MapPath(productImage);
                    if (File.Exists(localImagePath))
                    {
                        File.Delete(localImagePath);
                    }
                }

                ProductBBL.DeleteProduct(id);
                Session["ALERTMESSAGE"] = "El producto fue eliminado de la base de datos de forma exitosa.";
                Response.Redirect($"{Constants.AdminPagePath}?alert=success", false);
            }
            catch (Exception ex)
            {
                Session["ERROR"] = ex;
                Response.Redirect(Constants.ErrorPagePath);
            }
        }

        /// <summary>
        /// Filtrar la lista de productos en sesión según un criterio (dependiendo
        /// qué tipo de filtro esté activado) y actualizar el <see cref="gvProducts"/> 
        /// con los resultados obtenidos.
        /// </summary>
        protected void btnFind_Click(object sender, EventArgs e)
        {
            string filterText = txbxFilter.Text;
            List<Product> filteredProducts = new List<Product>();

            try
            {
                // Filtro avanzado
                if (advancedPanel.Visible)
                {
                    string firstCriteria = ddlFirstCriteria.SelectedValue;
                    string secondCriteria = ddlSecondCriteria.SelectedValue;
                    string condition = Auxiliary.CreateCondition(firstCriteria, secondCriteria, filterText);

                    filteredProducts = ProductBBL.SearchProducts(condition);
                    gvProducts.DataSource = filteredProducts;
                    gvProducts.DataBind();
                    alertProductNotFound.Visible = false;
                }
                // Filtro básico
                else
                {
                    filteredProducts = ((List<Product>)Session["PRODUCTS"]).FindAll(p => p.Name.ToUpper().Contains(filterText.ToUpper()));
                    gvProducts.DataSource = filteredProducts;
                    gvProducts.DataBind();
                    alertProductNotFound.Visible = false;
                }

                // Si no se encuentra resultados.
                if (filteredProducts.Count == 0)
                {
                    alertProductNotFound.Visible = true;
                    alertEmptyGV.Visible = false; // Por las dudas.
                }
            }
            catch (Exception ex)
            {
                Session["ERROR"] = ex;
                Response.Redirect(Constants.ErrorPagePath);
            }
        }

        /// <summary>
        /// Activar o desactivar el filtro avanzado de búsqueda.
        /// </summary>
        protected void btnAdvanced_Click(object sender, EventArgs e)
        {
            if (advancedPanel.Visible)
            {
                txbxFilter.Attributes["placeholder"] = "Buscar producto por nombre...";
                advancedPanel.Visible = false;
            }
            else
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

            try
            {
                // Reinicio
                txbxFilter.Text = string.Empty;
                gvProducts.DataSource = (List<Product>)Session["PRODUCTS"];
                gvProducts.DataBind();
                alertProductNotFound.Visible = false;
            }
            catch (Exception ex)
            {
                Session["ERROR"] = ex;
                Response.Redirect(Constants.ErrorPagePath);
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