using BusinessLogic;
using Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UserInterface.Pages.Admin
{
    public partial class CreateEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    // Cargas iniciales
                    ddlBrand.DataSource = BrandBBL.GetBrands();
                    ddlBrand.DataTextField = "Description";
                    ddlBrand.DataValueField = "ID";
                    ddlBrand.DataBind();

                    ddlCategory.DataSource = CategoryBBL.GetCategories();
                    ddlCategory.DataTextField = "Description";
                    ddlCategory.DataValueField = "ID";
                    ddlCategory.DataBind();

                    imgProduct.ImageUrl = Constants.PlaceholderImagePath;

                    // Si se está modificando un producto...
                    if (Request.QueryString["id"] != null)
                    {
                        // Carga de datos
                        Product productMOD = ProductBBL.GetProduct(int.Parse(Request.QueryString["id"]));
                        txbxCode.Text = productMOD.Code;
                        // Objeto en sesión que sirve para la verificación de cambio de código (deben ser únicos).
                        Session["PRODUCTCODE"] = productMOD.Code; 
                        txbxName.Text = productMOD.Name;
                        txbxDescription.Text = productMOD.Description;
                        txbxPrice.Text = productMOD.Price.ToString("0.##", CultureInfo.CurrentCulture);
                        ddlBrand.SelectedValue = productMOD.Brand.ID.ToString();
                        ddlCategory.SelectedValue = productMOD.Category.ID.ToString();
                        if (productMOD.Image.StartsWith("https"))
                        {
                            txbxImage.Text = productMOD.Image;
                        }
                        else if (productMOD.Image.StartsWith(Constants.LocalImagePath))
                        {
                            // Objeto en sesión que sirve por un posible reemplazo de imagen local.
                            Session["PRODUCTIMAGE"] = productMOD.Image; 
                        }
                        imgProduct.ImageUrl = productMOD.Image;

                        // Estilos de los DropDownList
                        ddlBrand.CssClass = Constants.FormSelectOptionSelected;
                        ddlCategory.CssClass = Constants.FormSelectOptionSelected;

                        // Botón
                        btnSubmit.Text = "MODIFICAR";
                    }

                    // Si se está creando un nuevo producto...
                    else
                    {
                        // Botón
                        btnSubmit.Text = "AGREGAR";
                        btnSubmit.Enabled = false;

                        // Placeholder marca
                        ListItem brandPlaceholder = new ListItem("Seleccione una marca...", "0");
                        brandPlaceholder.Attributes["disabled"] = "true";
                        brandPlaceholder.Attributes["selected"] = "true";
                        brandPlaceholder.Attributes["class"] = "form-option--placeholder";
                        ddlBrand.Items.Insert(0, brandPlaceholder);
                        ddlBrand.CssClass = Constants.FormSelectPlaceholder; // Es necesario para los estilos del placeholder inicial.

                        // Placeholder categoría
                        ListItem categoryPlaceholder = new ListItem("Seleccione una categoría...", "0");
                        categoryPlaceholder.Attributes["disabled"] = "true";
                        categoryPlaceholder.Attributes["selected"] = "true";
                        categoryPlaceholder.Attributes["class"] = "form-option--placeholder";
                        ddlCategory.Items.Insert(0, categoryPlaceholder);
                        ddlCategory.CssClass = Constants.FormSelectPlaceholder; // Es necesario para los estilos del placeholder inicial.
                    }
                }
                catch (Exception ex)
                {
                    // TODO: manejar errores
                    Session.Add("ERROR", ex);
                    throw ex;
                }
            }
        }

        protected void VerifyInformation(object sender, EventArgs e)
        {
            if (txbxCode.Text != "" && txbxName.Text != "" && 
                txbxDescription.Text != "" && txbxPrice.Text != "" &&
                ddlBrand.Text != "0" && ddlCategory.Text != "0")
            {
                btnSubmit.Enabled = true;
            }
            else
            {
                btnSubmit.Enabled = false;
            }
        }

        // Cuando se ingresa un código.
        protected void txbxCode_TextChanged(object sender, EventArgs e)
        {
            if (txbxCode.Text.Length <= 50 && txbxCode.Text.Length > 0)
            {
                // Verificación de código único cuando se está creando un producto
                if (Request.QueryString["id"] == null)
                {
                    if (ProductBBL.CodeExistsInDB(txbxCode.Text.Trim()))
                    {
                        txbxCode.Text = string.Empty;
                        txbxCode.CssClass = Constants.FormControlInvalid;
                        invalidCodeName.CssClass = "invalid-feedback";
                        invalidCodeName.Visible = true;
                        invalidCodeLength.Visible = false; 
                        txbxCode.Focus();
                    }
                    else
                    {
                        txbxCode.CssClass = Constants.FormControlValid;
                        invalidCodeLength.Visible = false; 
                        invalidCodeName.Visible = false;
                    }
                } 
                // Verificación de código único cuando se está modificando un producto.
                else
                {
                    if (txbxCode.Text.Trim() != Session["PRODUCTCODE"].ToString() &&
                        ProductBBL.CodeExistsInDB(txbxCode.Text.Trim()))
                    {
                        txbxCode.Text = string.Empty;
                        txbxCode.CssClass = Constants.FormControlInvalid;
                        invalidCodeName.CssClass = "invalid-feedback";
                        invalidCodeName.Visible = true;
                        invalidCodeLength.Visible = false; 
                        txbxCode.Focus();
                    }
                    else
                    {
                        txbxCode.CssClass = Constants.FormControlValid;
                        invalidCodeLength.Visible = false; 
                        invalidCodeName.Visible = false;
                    }
                }
            }
            else
            {
                txbxCode.Text = string.Empty;
                txbxCode.CssClass = Constants.FormControlInvalid;
                invalidCodeLength.CssClass = "invalid-feedback";
                invalidCodeLength.Visible = true;
                invalidCodeName.Visible = false;
                txbxCode.Focus();
            }
            VerifyInformation(sender, e);
        }

        // Cuando se ingresa un nombre.
        protected void txbxName_TextChanged(object sender, EventArgs e)
        {
            if (txbxName.Text.Length <= 50 && txbxName.Text.Length > 0)
            {
                txbxName.CssClass = Constants.FormControlValid;
            }
            else
            {
                txbxName.Text = string.Empty;
                txbxName.CssClass = Constants.FormControlInvalid;
                txbxName.Focus();
            }
            VerifyInformation(sender, e);
        }

        // Cuando se ingresa una descripción
        protected void txbxDescription_TextChanged(object sender, EventArgs e)
        {
            if (txbxDescription.Text.Length <= 50 && txbxDescription.Text.Length > 0)
            {
                txbxDescription.CssClass = Constants.FormControlValid;
            }
            else
            {
                txbxDescription.Text = string.Empty;
                txbxDescription.CssClass = Constants.FormControlInvalid;
                txbxDescription.Focus();
            }
            VerifyInformation(sender, e);
        }

        // Cuando se ingresa un precio.
        protected void txbxPrice_TextChanged(object sender, EventArgs e)
        {
            // Verificar que se haya ingresado un número correcto.
            if (!decimal.TryParse(txbxPrice.Text, out decimal price))
            {
                txbxPrice.Text = string.Empty;
                txbxPrice.CssClass = Constants.FormControlInvalid;
                txbxPrice.Focus();
            }
            else
            {
                txbxPrice.CssClass = Constants.FormControlValid;
            }
            VerifyInformation(sender, e);
        }

        // Cuando se selecciona una marca en el DropDownList.
        protected void ddlBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Si se está creando un producto
            if (Request.QueryString["id"] == null)
            {
                // Se elimina el Placeholder luego de que se seleccione una marca.
                ddlBrand.Items.Remove(ddlBrand.Items.FindByValue("0"));
            }
            ddlBrand.CssClass = Constants.FormSelectOptionSelected + " is-valid"; // Se cambia los estilos para las opciones seleccionadas.
            VerifyInformation(sender, e);
        }

        // Cuando se selecciona una categoría en el DropDownList.
        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Si se está creando un producto
            if (Request.QueryString["id"] == null)
            {
                // Se elimina el Placeholder luego de que se seleccione una categoría.
                ddlCategory.Items.Remove(ddlCategory.Items.FindByValue("0"));
            }
            ddlCategory.CssClass = Constants.FormSelectOptionSelected + " is-valid"; // Se cambia los estilos para las opciones seleccionadas.
            VerifyInformation(sender, e);
        }

        // Cuando se ingresa una URL para la imagen.
        protected void txbxImage_TextChanged(object sender, EventArgs e)
        {
            if (txbxImage.Text != "" && txbxImage.Text.Length <= 1000)
            {
                txbxImage.CssClass = Constants.FormControlValid;
                imgProduct.ImageUrl = txbxImage.Text;
            }
            else
            {
                txbxImage.CssClass = Constants.FormControlNormal;
                imgProduct.ImageUrl = Constants.PlaceholderImagePath;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Product myProd = new Product();
                myProd.Code = txbxCode.Text.Trim().ToUpper();
                myProd.Name = txbxName.Text.Trim();
                myProd.Description = txbxDescription.Text.Trim();
                myProd.Price = decimal.Parse(txbxPrice.Text.Trim());
                myProd.Brand = new Brand(int.Parse(ddlBrand.SelectedValue), ddlBrand.SelectedItem.Text);
                myProd.Category = new Category(int.Parse(ddlCategory.SelectedValue), ddlCategory.SelectedItem.Text);
                // Si se cargó una imagen por sistema de archivos local
                if (fuImage.HasFile)
                {
                    string fileName = $"{DateTime.Now.Ticks}-{fuImage.PostedFile.FileName}";
                    string path = Path.Combine(Server.MapPath($"~{Constants.LocalImagePath}"), fileName);

                    // Si se está modificando una imagen local, y existe una imagen local en sesión,
                    // se borra la imagen anterior para guardar la nueva referencia.
                    if (Request.QueryString["id"] != null && Session["PRODUCTIMAGE"] != null)
                    {
                        string localImagePath = Server.MapPath(Session["PRODUCTIMAGE"].ToString());
                        if (File.Exists(localImagePath))
                        {
                            File.Delete(localImagePath);
                        }
                    }

                    fuImage.SaveAs(path);
                    myProd.Image = $"{Constants.LocalImagePath}{fileName}";
                }
                // Si se cargó una imagen por URL
                else if (!string.IsNullOrEmpty(txbxImage.Text))
                {
                    // Si se está modificando una imagen local, y existe una imagen local en sesión,
                    // se borra la imagen anterior para guardar la nueva referencia.
                    if (Request.QueryString["id"] != null && Session["PRODUCTIMAGE"] != null)
                    {
                        string localImagePath = Server.MapPath(Session["PRODUCTIMAGE"].ToString());
                        if (File.Exists(localImagePath))
                        {
                            File.Delete(localImagePath);
                        }
                    }

                    myProd.Image = txbxImage.Text;
                }
                // Si no se cargó una imagen al crear un producto. Si se modifica un producto,
                // y no se cargó nada, se sobreentiende que no se está modificando la imagen
                // preexistente
                else if (Request.QueryString["id"] == null)
                {
                    myProd.Image = Constants.PlaceholderImagePath;
                }

                if (Request.QueryString["id"] == null)
                {
                    ProductBBL.CreateProduct(myProd);
                    Session["ALERTMESSAGE"] = "Nuevo producto agregado en la base de datos de forma exitosa.";
                }
                else
                {
                    myProd.ID = int.Parse(Request.QueryString["id"]);
                    ProductBBL.UpdateProduct(myProd);
                    Session["ALERTMESSAGE"] = "El producto fue modificado en la base de datos de forma exitosa.";
                }

                Response.Redirect($"{Constants.AdminPagePath}?alert=success", false);
            }
            catch (Exception ex)
            {
                // TODO: manejar error
                throw ex;
            }
        }
    }
}