<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.Master" AutoEventWireup="true" CodeBehind="Favorites.aspx.cs" Inherits="UserInterface.Pages.User.Favorites" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <title>El Almacenero - Favoritos</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <div class="row align-items-center my-4">
        <div class="col-6">
            <h1 class="text-white">Tus productos favoritos</h1>
        </div>
    </div>

    <!-- Filtros -->
    <asp:ScriptManager ID="smFavoriteFilter" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="upFavoriteFilter" runat="server">
        <ContentTemplate>
            </div>
            <div class="row border-bottom pb-4">
                <!-- Buscador -->
                <div class="col input-group">
                    <asp:TextBox ID="txbxFilter" runat="server"
                                 Placeholder ="Buscar producto favorito por nombre..."
                                 CssClass="form-control bg-dark text-white"></asp:TextBox>
                    <div class="input-group-text bg-warning p-0 mx-1">
                        <asp:Button ID="btnFind" runat="server" CssClass="d-none" 
                                    OnClick="btnFind_Click"/>
                        <asp:Label ID="lbFind" runat="server" AssociatedControlID="btnFind" 
                                   CssClass="btn btn-warning">
                            <i class="bi bi-search fs-5"></i>
                        </asp:Label>
                    </div>
                    <asp:LinkButton ID="btnResetFilter" runat="server" 
                                    CssClass="btn btn-warning border disabled"
                                    OnClick="btnResetFilter_Click">
                        <i class="bi bi-arrow-counterclockwise fs-5"></i>
                    </asp:LinkButton>
                </div>

                <!-- Filtros -->
                <div class="col-6 d-flex justify-content-between gap-3">
                    <asp:DropDownList ID="ddlFirstCriteria" runat="server" 
                                      CssClass="form-select bg-dark" AutoPostBack="true"
                                      OnSelectedIndexChanged="ddlFirstCriteria_SelectedIndexChanged"></asp:DropDownList>
                    <asp:DropDownList ID="ddlSecondCriteria" runat="server" 
                                      CssClass="form-select bg-dark"></asp:DropDownList>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnFind" />
            <asp:PostBackTrigger ControlID="btnResetFilter" />
        </Triggers>
    </asp:UpdatePanel>

    <!-- MAIN -->
    <div class="container col d-flex flex-column">
        <!-- Alertas -->
        <div class="row justify-content-center">
            <div>
                <!-- Alerta lista de favoritos vacía -->
                <asp:Panel ID="alertEmptyFavoriteList" runat="server" Visible="true"
                           CssClass="alert border-warning my-4 p-4 text-warning d-flex flex-column align-items-center gap-3">
                    <h2 class="alert-heading text-center">
                        ¡Vaya! Parece que aún no has marcado ningún producto 
                        como favorito.
                    </h2>
                    <p class="lead fs-5">
                        Puedes explorar nuestra sección de productos, seleccionar 
                        aquellos que te interesen y marcarlos como favoritos para 
                        tenerlos siempre a mano.
                    </p>
                    <a href="<%: Domain.Constants.ProductsPagePath %>" 
                       class="btn btn-outline-warning p-3 text-uppercase w-50">
                        Ir a productos →
                    </a>
                </asp:Panel>

                <!-- Alerta de producto favorito no encontrado -->
                <asp:Panel ID="alertFavoriteNotFound" runat="server" Visible="true"
                           CssClass="alert border-warning my-4 text-warning text-center">
                    <h2 class="alert-heading text-center">Producto favorito no encontrado</h2>
                    <p class="lead fs-5">
                        No se ha encontrado ningún producto favorito con el 
                        criterio de búsqueda seleccionado.
                    </p>
                </asp:Panel>
            </div>
        </div>

        <!-- Productos favoritos -->
        <section class="col d-flex flex-wrap my-4">
            <% if (((List<Domain.Product>)Session["FAVORITEPRODUCTS"]).Count > 0) { %>
                <asp:Repeater ID="FavoriteProductCards" runat="server">
                    <ItemTemplate>
                        <div class="col-3 p-4">
                            <article class="card bg-dark overflow-hidden border rounded-5 border-warning border-2"
                                     style="max-height: 420px">
                                <figure class="card-img-top bg-gradient text-white">
                                    <img src="<%# Eval("Image") %>"
                                         alt="Imagen del producto <%# Eval("Name") %>">
                                </figure>
                                <div class="card-body d-flex flex-column gap-1 justify-content-between">
                                    <div class="d-flex justify-content-center">
                                        <i class="bi bi-star-fill text-warning"></i>
                                    </div>
                                    <h5 class="card-title text-center text-white fw-bold">
                                        <%# Eval("Name") %>
                                    </h5>
                                    <div class="d-flex justify-content-center gap-2">
                                        <span class="badge rounded-pill text-white border bg-gradient"><%# Eval("Brand.Description") %></span>
                                        <span class="badge rounded-pill text-white border bg-gradient"><%# Eval("Category.Description") %></span>
                                    </div>
                                    <p class="card-text text-white lead fs-6 mt-2 text-center">
                                        <%# Eval("Description") %>
                                    </p>
                                    <p class="card-text text-warning text-center fs-2">
                                        $<%# ((decimal)Eval("Price")).ToString("N2") %>
                                    </p>
                                    <a href="<%: Domain.Constants.ProductDetailPagePath %>?id=<%# Eval("ID") %>&p=favorites"
                                       class="btn btn-outline-light">Ver detalle →</a>
                                </div>
                            </article>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            <% } %>
        </section>

        <!-- Paginación -->
        <div class="row justify-content-center my-4">
            <div class="col-6 d-flex justify-content-center gap-5">
                <% // Elemento fundamental para la paginación al proporcionar la página actual %>
                <asp:HiddenField ID="hfCurrentPage" runat="server" Value="1"/>

                <asp:LinkButton ID="btnPrev" runat="server"
                                CssClass="btn btn-outline-warning"
                                OnClick="btnPrev_Click">
                    <i class="bi bi-chevron-left"></i>
                </asp:LinkButton>

                <asp:LinkButton ID="btnNext" runat="server" 
                                CssClass="btn btn-outline-warning"
                                OnClick="btnNext_Click">
                    <i class="bi bi-chevron-right"></i>
                </asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>
