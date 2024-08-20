using BusinessLogic;
using Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UserInterface.Pages.Admin
{
    public partial class CreateEdit : System.Web.UI.Page
    {
        /* ---------------------------------------------------------------------------- */
        /*                                      GENERAL                                 */
        /* ---------------------------------------------------------------------------- */
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    // Cargas iniciales
                    InitialPageLoad();

                    // Si se está creando un nuevo producto...
                    if (IsCreatePageMode())
                    {
                        LoadPageForCreate();
                    }
                    // Si se está modificando un producto...
                    else
                    {
                        LoadPageForEdit();
                    }
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }

        /// <summary>
        /// Cargar algunos controles por defecto
        /// </summary>
        private void InitialPageLoad()
        {
            try
            {
                ddlBrand.DataSource = BrandBLL.GetBrands();
                ddlBrand.DataTextField = "Description";
                ddlBrand.DataValueField = "ID";
                ddlBrand.DataBind();

                ddlCategory.DataSource = CategoryBLL.GetCategories();
                ddlCategory.DataTextField = "Description";
                ddlCategory.DataValueField = "ID";
                ddlCategory.DataBind();

                imgProduct.ImageUrl = Constants.PlaceholderImagePath;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Cargar los campos de los controles según la información del producto que se
        /// quiere modificar.
        /// </summary>
        private void LoadPageForEdit()
        {
            try
            {
                Product productMOD = ProductBLL.GetProduct(int.Parse(Request.QueryString["id"]));

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
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Preparar la página para la creación de un nuevo producto.
        /// </summary>
        private void LoadPageForCreate()
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

        /// <summary>
        /// Verificar que los campos obligatorios estén rellenados para poder habilitar
        /// el botón de envío.
        /// </summary>
        protected void VerifyInformation()
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

        /// <summary>
        /// Verificar que la página actual sea la creación de un nuevo producto.
        /// </summary>
        /// <returns>Valor booleano que indica si se está creando un nuevo producto o no</returns>
        private bool IsCreatePageMode()
        {
            return Request.QueryString["id"] == null;
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


        /* ---------------------------------------------------------------------------- */
        /*                                      CÓDIGO                                  */
        /* ---------------------------------------------------------------------------- */
        /// <summary>
        /// Cuando se ingresa un código de producto.
        /// </summary>
        protected void txbxCode_TextChanged(object sender, EventArgs e)
        {
            if (txbxCode.Text.Length <= 50 && txbxCode.Text.Length > 0)
            {
                // Verificación de código único cuando se está creando un producto
                if (IsCreatePageMode())
                {
                    ValidateProductCodeForCreate();
                } 
                // Verificación de código único cuando se está modificando un producto.
                else
                {
                    ValidateProductCodeForEdit();
                }
            }
            else
            {
                InvalidProductCodeLength();
            }
            VerifyInformation();
        }

        /// <summary>
        /// Validar el código introducido cuando se quiere crear un nuevo producto
        /// verificando que no exista en la base de datos.
        /// </summary>
        private void ValidateProductCodeForCreate()
        {
            try
            {
                if (ProductBLL.CodeExistsInDB(txbxCode.Text.Trim()))
                {
                    InvalidProductCodeAlreadyExists();
                }
                else
                {
                    ValidProductCode();
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Validar el código introducido cuando se está modificando un producto
        /// verificando que este no exista en la base de datos y sea diferente al
        /// código original.
        /// </summary>
        private void ValidateProductCodeForEdit()
        {
            try
            {
                if (txbxCode.Text.Trim() != Session["PRODUCTCODE"].ToString() &&
                    ProductBLL.CodeExistsInDB(txbxCode.Text.Trim()))
                {
                    InvalidProductCodeAlreadyExists();
                }
                else
                {
                    ValidProductCode();
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// El código introducido es inválido al ya existir en la base de datos.
        /// </summary>
        private void InvalidProductCodeAlreadyExists()
        {
            txbxCode.Text = string.Empty;
            txbxCode.CssClass = Constants.FormControlInvalid;
            invalidCodeName.Visible = true;
            invalidCodeLength.Visible = false; 
            txbxCode.Focus();
        }

        /// <summary>
        /// El código introducido es inválido debido a su longitud.
        /// </summary>
        private void InvalidProductCodeLength()
        {
            txbxCode.Text = string.Empty;
            txbxCode.CssClass = Constants.FormControlInvalid;
            invalidCodeLength.Visible = true;
            invalidCodeName.Visible = false;
            txbxCode.Focus();
        }

        /// <summary>
        /// El código introducido es válido.
        /// </summary>
        private void ValidProductCode()
        {
            txbxCode.CssClass = Constants.FormControlValid;
            invalidCodeLength.Visible = false; 
            invalidCodeName.Visible = false;
        }


        /* ---------------------------------------------------------------------------- */
        /*                                      NOMBRE                                  */
        /* ---------------------------------------------------------------------------- */
        /// <summary>
        /// Cuando se ingresa un nombre de producto
        /// </summary>
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
            VerifyInformation();
        }


        /* ---------------------------------------------------------------------------- */
        /*                                   DESCRIPCIÓN                                */
        /* ---------------------------------------------------------------------------- */
        /// <summary>
        /// Cuando se ingresa una descripción de producto.
        /// </summary>
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
            VerifyInformation();
        }


        /* ---------------------------------------------------------------------------- */
        /*                                     PRECIO                                   */
        /* ---------------------------------------------------------------------------- */
        /// <summary>
        /// Cuando se ingresa un precio de producto.
        /// </summary>
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
            VerifyInformation();
        }


        /* ---------------------------------------------------------------------------- */
        /*                                      MARCA                                   */
        /* ---------------------------------------------------------------------------- */
        /// <summary>
        /// Cuando se selecciona una marca del producto en el DropDownList.
        /// </summary>
        protected void ddlBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Si se está creando un producto
            if (IsCreatePageMode())
            {
                // Se elimina el Placeholder luego de que se seleccione una marca.
                ddlBrand.Items.Remove(ddlBrand.Items.FindByValue("0"));
            }
            ddlBrand.CssClass = Constants.FormSelectOptionSelected + " is-valid"; // Se cambia los estilos para las opciones seleccionadas.
            VerifyInformation();
        }


        /* ---------------------------------------------------------------------------- */
        /*                                   CATEGORIA                                  */
        /* ---------------------------------------------------------------------------- */
        /// <summary>
        /// Cuando se selecciona una categoría del producto en el DropDownList.
        /// </summary>
        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Si se está creando un producto
            if (IsCreatePageMode())
            {
                // Se elimina el Placeholder luego de que se seleccione una categoría.
                ddlCategory.Items.Remove(ddlCategory.Items.FindByValue("0"));
            }
            ddlCategory.CssClass = Constants.FormSelectOptionSelected + " is-valid"; // Se cambia los estilos para las opciones seleccionadas.
            VerifyInformation();
        }


        /* ---------------------------------------------------------------------------- */
        /*                                   URL IMAGEN                                 */
        /* ---------------------------------------------------------------------------- */
        /// <summary>
        /// Cuando se ingresa una URL para la imagen del producto.
        /// </summary>
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


        /* ---------------------------------------------------------------------------- */
        /*                                     SUBMIT                                   */
        /* ---------------------------------------------------------------------------- */
        /// <summary>
        /// Crear/Modificar un producto en la base de datos.
        /// </summary>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Product myProd = ReturnCompleteProduct();

                if (IsCreatePageMode())
                {
                    ProductBLL.CreateProduct(myProd);
                    Session["ALERTMESSAGE"] = "Nuevo producto agregado en la base de datos de forma exitosa.";
                }
                else
                {
                    ProductBLL.UpdateProduct(myProd);
                    Session["ALERTMESSAGE"] = "El producto fue modificado en la base de datos de forma exitosa.";
                }

                Response.Redirect($"{Constants.AdminPagePath}?alert=success");
            }
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Devolver un <see cref="Product"/> con la información cargada según los campos 
        /// de entrada de formulario.
        /// </summary>
        /// <returns>Producto rellenado</returns>
        private Product ReturnCompleteProduct()
        {
            try
            {
                Product myProd = new Product
                {
                    ID = Request.QueryString["id"] != null ? int.Parse(Request.QueryString["id"]) : 0,
                    Code = txbxCode.Text.Trim().ToUpper(),
                    Name = txbxName.Text.Trim(),
                    Description = txbxDescription.Text.Trim(),
                    Price = decimal.Parse(txbxPrice.Text.Trim()),
                    Brand = new Brand(int.Parse(ddlBrand.SelectedValue), ddlBrand.SelectedItem.Text),
                    Category = new Category(int.Parse(ddlCategory.SelectedValue), ddlCategory.SelectedItem.Text)
                };
                FillProductImage(myProd);
                return myProd;
            }
            catch (FormatException ex)
            {
                HandleException(ex);
                return null; // Esto es solo por el compilador
            }
        }

        /// <summary>
        /// Rellenar el campo <see cref="Product.Image"/> del producto según ciertas
        /// condiciones (imagen local, URL o placeholder).
        /// </summary>
        /// <param name="myProd">Referencia del producto a rellenar</param>
        private void FillProductImage(Product myProd)
        {
            // Si se cargó una imagen por sistema de archivos local
            if (fuImage.HasFile)
            {
                ProcessLocalImageUpload(myProd);
            }
            // Si se cargó una imagen por URL
            else if (!string.IsNullOrEmpty(txbxImage.Text))
            {
                ProcessUrlImageUpload(myProd);
            }
            // • Si no se cargó una imagen al crear un producto, se guarda el placeholder.
            // • Si se modifica un producto, y no se cargó nada, se sobreentiende que
            // no se está modificando la imagen preexistente; exceptuando el caso de que
            // la imagen precargada sea el placeholder.
            else if (IsCreatePageMode() ||
                    (Request.QueryString["id"] != null && imgProduct.ImageUrl == Constants.PlaceholderImagePath))
            {
                myProd.Image = Constants.PlaceholderImagePath;
            }
        }
        
        /// <summary>
        /// Manejar la carga de una imagen local desde el sistema de archivos, incluyendo 
        /// la eliminación de cualquier imagen local existente del producto si 
        /// fuese necesario.
        /// </summary>
        /// <param name="myProd">Referencia del producto a rellenar</param>
        private void ProcessLocalImageUpload(Product myProd)
        {
            try
            {
                string fileName = $"{DateTime.Now.Ticks}-{fuImage.PostedFile.FileName}";
                string path = Path.Combine(Server.MapPath($"~{Constants.LocalImagePath}"), fileName);

                DeleteExistingLocalImageIfNeeded();

                fuImage.SaveAs(path);
                myProd.Image = $"{Constants.LocalImagePath}{fileName}";
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Manejar la carga de una imagen desde una URL, también incluyendo la eliminación
        /// de cualquier imagen local existente si fuese necesario.
        /// </summary>
        /// <param name="myProd">Referencia del producto a rellenar</param>
        private void ProcessUrlImageUpload(Product myProd)
        {
            DeleteExistingLocalImageIfNeeded();

            myProd.Image = txbxImage.Text;
        }

        /// <summary>
        /// Eliminar una posible imagen local existente en caso de que el usuario esté 
        /// actualizando un producto cargando una nueva (ya sea por URL o por Sistema
        /// de archivos).
        /// </summary>
        private void DeleteExistingLocalImageIfNeeded()
        {
            try
            {
                // Session["PRODUCTIMAGE"] se carga al principio cuando el producto presenta
                // una imagen local existente.
                if (Request.QueryString["id"] != null && Session["PRODUCTIMAGE"] != null)
                {
                    string localImagePath = Server.MapPath(Session["PRODUCTIMAGE"].ToString());
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
    }
}