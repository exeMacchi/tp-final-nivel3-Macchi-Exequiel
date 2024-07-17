<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.Master" AutoEventWireup="true" CodeBehind="CreateEdit.aspx.cs" Inherits="UserInterface.Pages.Admin.CreateEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <% if (Request.QueryString["id"] != null) { %>
            <title>El Almacenero - Modificar producto</title>
    <% } else { %>
            <title>El Almacenero - Agregar producto</title>
    <% } %> 
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <asp:ScriptManager ID="scriptManagerCreateEdit" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="updatePanelCreateEdit" runat="server">
        <ContentTemplate>
            <!-- Alerta de error -->
            <asp:Panel ID="errorAlert" runat="server" CssClass="alert alert-danger my-4"
                       Visible="false">
                <h2 class="alert-heading fs-2">¡Ups!</h2>
                <asp:Label ID="errorText" runat="server" Text=""></asp:Label>
            </asp:Panel>

            <!-- Formulario -->
            <div class="row my-4">
                <h1 class="text-center text-white">Plantilla de producto</h1>
            </div>
            <div class="row my-4">
                <div class="col-6 d-flex flex-column gap-3 justify-content-between">
                    <!-- Código -->
                    <div>
                        <asp:Label ID="lbCode" runat="server" Text="Código"
                                   CssClass="form-label fw-medium text-white"
                                   AssociatedControlID="txbxCode"></asp:Label>
                        <span class="text-danger">*</span>
                        <asp:TextBox ID="txbxCode" runat="server"
                                     CssClass="form-control bg-dark text-white"
                                     Required="true" Placeholder="Ingrese el código..."
                                     AutoPostBack="true" OnTextChanged="txbxCode_TextChanged"
                                     MaxLength="50"></asp:TextBox>
                        <asp:Panel ID="invalidCodeLength" runat="server" Visible="false">
                            El código del producto debe tener entre 1 a 50 caracteres.
                        </asp:Panel>
                        <asp:Panel ID="invalidCodeName" runat="server" Visible="false">
                            El código introducido ya existe en la base de datos. Introduzca otro.
                        </asp:Panel>
                    </div>

                    <!-- Nombre -->
                    <div>
                        <asp:Label ID="lbName" runat="server" Text="Nombre"
                                   CssClass="form-label fw-medium text-white"
                                   AssociatedControlID="txbxName"></asp:Label>
                        <span class="text-danger">*</span>
                        <asp:TextBox ID="txbxName" runat="server" Required="true"
                                     CssClass="form-control bg-dark text-white"
                                     Placeholder="Ingrese el nombre..."
                                     AutoPostBack="true" OnTextChanged="txbxName_TextChanged"
                                     MaxLength="50"></asp:TextBox>
                        <div id="invalidName" class="invalid-feedback">
                            El nombre del producto debe tener entre 1 a 50 caracteres.
                        </div>
                    </div>

                    <!-- Descripción -->
                    <div>
                        <asp:Label ID="lbDescription" runat="server" Text="Descripción"
                                   CssClass="form-label fw-medium text-white"
                                   AssociatedControlID="txbxDescription"></asp:Label>
                        <span class="text-danger">*</span>
                        <asp:TextBox ID="txbxDescription" runat="server" TextMode="MultiLine"
                                     Rows="8" CssClass="form-control bg-dark text-white" 
                                     style="resize: none;" Required="true"
                                     Placeholder="Ingrese una descripción del producto..."
                                     OnTextChanged="txbxDescription_TextChanged" AutoPostBack="true"
                                     MaxLength="50"></asp:TextBox>
                        <div id="invalidDescription" class="invalid-feedback">
                            La descripción del producto debe tener entre 1 a 50 caracteres.
                        </div>
                    </div>

                    <!-- Precio -->
                    <div>
                        <asp:Label ID="lbPrice" runat="server" Text="Precio"
                                   CssClass="form-label fw-medium text-white"
                                   AssociatedControlID="txbxPrice"></asp:Label>
                        <span class="text-danger">*</span>
                        <div class="input-group">
                            <span class="input-group-text text-white bg-black col-2 justify-content-center">
                                $
                            </span>
                            <asp:TextBox ID="txbxPrice" runat="server" ClientIDMode="Static"
                                         CssClass="form-control bg-dark text-white"
                                         Required="true" AutoPostBack="true"
                                         OnTextChanged="txbxPrice_TextChanged"
                                         Placeholder="Ingrese el precio..." Min="0"></asp:TextBox>
                            <div id="invalidPrice" class="invalid-feedback">
                                Introducir un número válido.
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-6 d-flex flex-column gap-3">
                    <div class="d-flex justify-content-between gap-3">
                        <!-- Marca -->
                        <div class="w-50">
                            <asp:Label ID="lbBrand" runat="server" Text="Marca"
                                       CssClass="form-label fw-medium text-white" 
                                       AssociatedControlID="ddlBrand"></asp:Label>
                            <span class="text-danger">*</span>
                            <asp:DropDownList ID="ddlBrand" runat="server" ClientIDMode="Static"
                                              CssClass="form-select bg-dark" 
                                              OnSelectedIndexChanged="ddlBrand_SelectedIndexChanged"
                                              AutoPostBack="true" Required="true"></asp:DropDownList>
                        </div>

                        <!-- Categoría -->
                        <div class="w-50">
                            <asp:Label ID="lbCategory" runat="server" Text="Categoría"
                                       CssClass="form-label fw-medium text-white"
                                       AssociatedControlID="ddlCategory"></asp:Label>
                            <span class="text-danger">*</span>
                            <asp:DropDownList ID="ddlCategory" runat="server" ClientIDMode="Static"
                                              CssClass="form-select bg-dark"
                                              OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"
                                              AutoPostBack="true" Required="true"></asp:DropDownList>
                        </div>
                    </div>

                    <!-- URL/File Imagen -->
                    <div class="d-flex flex-column justify-content-between">
                        <asp:Label ID="lbImage" runat="server" Text="Imagen"
                                   CssClass="form-label fw-medium text-white"
                                   AssociatedControlID="txbxImage"></asp:Label>
                        <!-- URL -->
                        <asp:TextBox ID="txbxImage" runat="server" ClientIDMode="Static"
                                     CssClass="form-control bg-dark text-white"
                                     Placeholder="Inserte una URL de imagen..."
                                     AutoPostBack="true"
                                     OnTextChanged="txbxImage_TextChanged"></asp:TextBox>
                        <!-- File -->
                        <asp:FileUpload ID="fuImage" runat="server" ClientIDMode="Static" 
                                        CssClass="form-control bg-dark input-file mt-2"/>
                    </div>

                    <div>
                        <!-- Imagen -->
                        <div class="d-flex justify-content-center mt-4">
                            <figure style="max-width:220px; margin:0">
                                <asp:Image ID="imgProduct" runat="server"
                                           ClientIDMode="Static" CssClass="img-fluid"/>
                            </figure>
                        </div>
                    </div>
                </div>

                <!-- Botones -->
                <div class="row mt-5">
                    <div class="col-6"></div>
                    <div class="col-6 d-flex justify-content-between gap-3">
                        <asp:Button ID="btnSubmit" runat="server"
                                    CssClass="btn btn-outline-warning w-50"
                                    OnClick="btnSubmit_Click" />
                        <a href="<%: Domain.Constants.AdminPagePath %>" class="btn btn-outline-danger w-50">
                            CANCELAR
                        </a>
                    </div>
                </div>

        </ContentTemplate>
        <%-- Estos triggers son necesarios para poder enviar el archivo seleccionado
             por el usuario al backend. --%>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSubmit" />
        </Triggers>
    </asp:UpdatePanel>

    <script src="/Assets/Scripts/CreateEdit.js" type="module"></script>
</asp:Content>
