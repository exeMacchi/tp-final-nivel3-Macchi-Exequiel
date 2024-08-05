<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.Master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="UserInterface.Pages.Admin.Admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <title>El Almacenero - Admin</title>
    <link href="/Assets/Styles/Pagination.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <% if (Request.QueryString["alert"] != null) { %>
        <% if (Request.QueryString["alert"] == "success") { %>
            <asp:Panel ID="adminSuccessAlert" runat="server"
                       CssClass="alert border-success bg-dark my-4 text-success text-center">
                <h2 class="alert-heading fs-2">¡Éxito!</h2>
                <p class="lead fs-5 mb-0"><%:(string)Session["ALERTMESSAGE"] %></p>
            </asp:Panel>
        <% } else if (Request.QueryString["alert"] == "error") { %>
            <asp:Panel ID="adminErrorAlert" runat="server" 
                       CssClass="alert border-danger my-4 bg-dark text-danger">
                <h2 class="alert-heading fs-2">Error</h2>
                <p class="lead fs-5 mb-0"><%:(string)Session["ALERTMESSAGE"] %></p>
            </asp:Panel>
        <% } %>
    <% } %>
    <div class="row my-4 align-items-center">
        <div class="col-6">
            <h1 class="text-white">Panel de administrador</h1>
        </div>
    </div>

    <div class="row">
        <!-- Filtro básico -->
        <div class="col-6 d-flex gap-3">
            <asp:TextBox ID="txbxFilter" runat="server" CssClass="form-control bg-dark text-white"
                         PlaceHolder="Buscar producto por nombre..."></asp:TextBox>

            <!-- Buscar -->
            <asp:Button ID="btnFind" runat="server" CssClass="d-none" OnClick="btnFind_Click"/>
            <asp:Label ID="lbFind" runat="server"  AssociatedControlID="btnFind" 
                       CssClass="btn btn-warning">
                <i class="bi bi-search fs-5"></i>
            </asp:Label>

            <!-- Avanzado -->
            <asp:Button ID="btnAdvanced" runat="server"  CssClass="d-none" OnClick="btnAdvanced_Click"/>
            <asp:Label ID="lbAdvanced" runat="server" AssociatedControlID="btnAdvanced"
                       CssClass="btn btn-warning">
                <i class="bi bi-filter fs-5"></i>
            </asp:Label>
        </div>

        <!-- Crear nuevo producto -->
        <div class="col-6 d-flex justify-content-end gap-3">
            <a href="<%: Domain.Constants.CreateEditPagePath %>" class="btn btn-outline-warning fs-5 w-50">
                Agregar un nuevo producto
            </a>
        </div>
    </div>

    <!-- Filtro avanzado -->
    <asp:Panel ID="advancedPanel" runat="server" CssClass="row my-3" Visible="false">
        <div class="col-6 d-flex justify-content-between gap-3">
            <asp:DropDownList ID="ddlFirstCriteria" runat="server" 
                              CssClass="form-select bg-dark" AutoPostBack="true"
                              OnSelectedIndexChanged="ddlFirstCriteria_SelectedIndexChanged"></asp:DropDownList>
            <asp:DropDownList ID="ddlSecondCriteria" runat="server" 
                              CssClass="form-select bg-dark"></asp:DropDownList>
        </div>
    </asp:Panel>

    <!-- Alertas -->
    <div class="row">
        <div class="col">
            <!-- GridView vacío -->
            <asp:Panel ID="alertEmptyGV" runat="server" Visible="false"
                       CssClass="alert border-warning my-4 text-warning">
                <h2 class="alert-heading">
                    ¡Ups! Parece que no hay ningún producto en la base de datos
                </h2>
                <p class="lead fs-5">
                    Añada algún producto poder ver su información en un formato tabular.
                </p>
            </asp:Panel>

            <!-- Producto no encontrado -->
            <asp:Panel ID="alertProductNotFound" runat="server" Visible="false"
                       CssClass="alert border-warning my-4 text-warning">
                <h2 class="alert-heading">Producto no encontrado</h2>
                <p class="lead fs-5">
                    No se ha encontrado ningún producto con el criterio de 
                    búsqueda seleccionado.
                </p>
            </asp:Panel>
        </div>
    </div>

    <!-- Grilla -->
    <div class="row">
        <asp:GridView ID="gvProducts" runat="server"
                      CssClass="col table table-striped my-4" 
                      AllowPaging="true" PageSize="5" OnPageIndexChanging="gvProducts_PageIndexChanging"
                      HeaderStyle-CssClass="table-dark text-center border-bottom lead fs-5" 
                      RowStyle-CssClass="table-dark text-white align-middle border-bottom"
                      AutoGenerateColumns="false"
                      DataKeyNames="ID"
                      OnSelectedIndexChanged="gvProducts_SelectedIndexChanged"
                      OnRowDeleting="gvProducts_RowDeleting">
            <Columns>
                <asp:BoundField HeaderText="Código" 
                                DataField="Code"
                                ItemStyle-CssClass="text-center" />
                
                <asp:BoundField HeaderText="Nombre" 
                                DataField="Name" 
                                ItemStyle-CssClass="text-center" />

                <asp:BoundField HeaderText="Marca" 
                                DataField="Brand.Description" 
                                ItemStyle-CssClass="text-center" />

                <asp:BoundField HeaderText="Categoría"
                                DataField="Category.Description" 
                                ItemStyle-CssClass="text-center" />

                <asp:BoundField HeaderText="Precio"
                                DataField="Price"
                                DataFormatString="${0:N2}"
                                ItemStyle-CssClass="text-center"/>

                <asp:CommandField HeaderText="Editar" SelectText="" ShowSelectButton="true"
                                  ItemStyle-CssClass="text-center"
                                  ControlStyle-CssClass="bi bi-pencil-square text-warning fs-4 w-50"/>

                <asp:CommandField HeaderText="Eliminar" DeleteText="" ShowDeleteButton="true"
                                  ItemStyle-CssClass="text-center"
                                  ControlStyle-CssClass="bi bi-trash-fill text-warning fs-4 w-50"/>
            </Columns>
            <PagerSettings Mode="NumericFirstLast" />
            <PagerStyle HorizontalAlign="Center" />
        </asp:GridView>

        <!-- Modal Eliminar -->
        <!-- Primer trigger -->
        <a id="btnDeleteProduct" style="display:none" data-bs-toggle="modal" href="#firstAdvertise" role="button">ELIMINAR</a>
        <!-- Primer aviso -->
        <div class="modal fade" id="firstAdvertise" aria-hidden="true" aria-labelledby="firstAdvertiseLabel" tabindex="-1">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content bg-dark border-warning">
                    <div class="modal-header bg-warning border-warning">
                        <h5 class="modal-title" id="exampleModalToggleLabel">
                            ¡ADVERTENCIA!
                        </h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body text-warning">
                        Si sigue con el procedimiento, borrará el producto 
                        <asp:Label ID="lbDeleteProduct" runat="server" CssClass="fw-bold text-white"></asp:Label>
                        de forma permanente de la base de datos. 
                        ¿Realmente desea continuar con la eliminación?
                    </div>
                    <div class="modal-footer" style="border-top: none;">
                        <button type="button"
                                class="btn btn-danger text-white" 
                                data-bs-dismiss="modal">Cancelar</button>
                        <!-- Segundo trigger -->
                        <a class="btn btn-outline-warning"
                           data-bs-target="#secondAdvertise"
                           data-bs-toggle="modal"
                           data-bs-dismiss="modal">Continuar</a>
                    </div>
                </div>
            </div>
        </div>
        <!-- Segundo aviso -->
        <div class="modal fade" id="secondAdvertise" aria-hidden="true" aria-labelledby="secondAdvertiseLabel" tabindex="-1">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content bg-dark border-danger">
                    <div class="modal-header bg-danger border-danger">
                        <h5 class="modal-title text-white fs-3" id="secondAdvertiseLabel">¡ÚLTIMO AVISO!</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body text-white">
                        Este es el último aviso de advertencia. 
                        ¿Está seguro que quiere eliminar el producto seleccionado? 
                        <strong class="text-danger text-uppercase">
                            No se podrá recuperar el registro una vez eliminado
                        </strong>.
                    </div>
                    <div class="modal-footer" style="border-top: none;">
                        <button type="button" class="btn btn-danger" data-bs-dismiss="modal">Cancelar</button>
                        <asp:Button ID="btnDeleteConfirm" runat="server" Text="Confirmar eliminanación"
                                    CssClass="btn btn-outline-danger" OnClick="btnDeleteConfirm_Click"/>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
