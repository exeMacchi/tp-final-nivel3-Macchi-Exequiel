using System;
using System.Collections.Generic;
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
            // En la página de administrador, siempre que se cargue, se recargan
            // los productos por el caso de que esta página provenga de una operación
            // de creación, modificación o eliminación.
            Session["PRODUCTS"] = ProductBBL.GetProducts();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (((List<Product>)Session["PRODUCTS"]).Count > 0)
                    {
                        gvProducts.DataSource = (List<Product>)Session["PRODUCTS"];
                        gvProducts.DataBind();
                    }
                    else
                    {
                        // TODO: ALERTAS DE VACIO
                    }
                }
                catch (Exception ex)
                {
                    // TODO: Manejar errores
                    Session.Add("ERROR", ex);
                    throw ex;
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
            gvProducts.PageIndex = e.NewPageIndex;
            gvProducts.DataSource = (List<Product>)Session["PRODUCTS"];
            gvProducts.DataBind();
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
                ProductBBL.DeleteProduct(id);
                Session["ALERTMESSAGE"] = "El producto fue eliminado de la base de datos de forma exitosa.";
                Response.Redirect($"{Constants.AdminPagePath}?alert=success", false);
            }
            catch (Exception ex)
            {
                // TODO: manejar error
                throw ex;
            }
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            // Filtro básico
            string filterText = txbxFilter.Text;

            List<Product> productFiltered = ((List<Product>)Session["PRODUCTS"]).FindAll(p => p.Name.ToUpper().Contains(filterText.ToUpper()));

            gvProducts.DataSource = productFiltered;
            gvProducts.DataBind();

            if (productFiltered.Count == 0)
            {
                // TODO: activar alerta de no resultados encontrados.
            }

        }

        protected void btnAdvanced_Click(object sender, EventArgs e)
        {

        }

        protected void ddlFirstCriteria_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}