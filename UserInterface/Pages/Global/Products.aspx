<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.Master" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="UserInterface.Pages.Global.Products" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <title>El Almacenero - Catálogo de productos</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

    <!-- TEST START -->
    <div class="row">

    <aside class="col-3 border-end">
        <h1 class="text-white text-center my-4">Catálogo</h1>
        <!-- Texto de filtro -->
        <div class="col input-group">
            <asp:TextBox ID="txbxFilter" runat="server"
                         Placeholder ="Buscar producto por nombre..."
                         CssClass="form-control bg-dark text-white"></asp:TextBox>
            <div class="input-group-text bg-warning p-0">
                <asp:Button ID="btnFind" runat="server" CssClass="d-none" 
                            OnClick="btnFindByName_Click"/>
                <asp:Label ID="lbFind" runat="server" AssociatedControlID="btnFind" 
                           CssClass="btn btn-warning">
                    <i class="bi bi-search fs-5"></i>
                </asp:Label>
            </div>
        </div>

        <!-- Filtros -->
        <ul class="list-unstyled ps-0">
            <!-- Filtros de categorías -->
            <li class="my-4">
                <h3 class="text-white">Categorías</h3>
                <ul class="btn-toggle-nav list-unstyled fw-normal pb-1 small">
                    <asp:Repeater ID="CategoriesFilter" runat="server" OnItemDataBound="CategoriesFilter_ItemDataBound">
                        <ItemTemplate>
                            <li class="my-2">
                                <asp:LinkButton ID="btnFindCategory" runat="server"
                                                Text='<%# Eval("Key") + " (" + Eval("Value") + ")" %>'
                                                CssClass="d-inline-flex rounded text-decoration-none filter-link ms-4"
                                                CommandArgument='<%# Eval("Key") %>' OnClick="btnFindByCategory_Click"></asp:LinkButton>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </li>

            <!-- Filtros de marcas -->
            <li class="my-4">
                <h3 class="text-white">Marcas</h3>
                <ul class="btn-toggle-nav list-unstyled fw-normal pb-1 small">
                    <asp:Repeater ID="BrandsFilter" runat="server" OnItemDataBound="BrandsFilter_ItemDataBound">
                        <ItemTemplate>
                            <li class="my-2">
                                <asp:LinkButton ID="btnFindBrand" runat="server"
                                                Text='<%# Eval("Key") + " (" + Eval("Value") + ")" %>'
                                                CssClass="d-inline-flex rounded text-decoration-none filter-link ms-4"
                                                CommandArgument='<%# Eval("Key") %>' OnClick="btnFindByBrand_Click"></asp:LinkButton>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </li>

            <!-- Filtros de precios -->
            <li class="my-4">
                <h3 class="text-white">Precios</h3>
                <ul class="btn-toggle-nav list-unstyled fw-normal pb-1 small">
                    <li class="my-2">
                    <asp:Repeater ID="PriceFilter" runat="server" OnItemDataBound="PriceFilter_ItemDataBound">
                        <ItemTemplate>
                            <li class="my-2">
                                <asp:LinkButton ID="btnFindPrice" runat="server"
                                                CssClass="d-inline-flex rounded text-decoration-none filter-link ms-4"
                                                CommandArgument='<%# Eval("Key") %>' OnClick="btnFindByPrice_Click"></asp:LinkButton>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </li>
        </ul>
    </aside>

    <section class="col d-flex flex-wrap my-4">
        <!-- Alerta de base de datos vacía -->
        <asp:Panel ID="alertEmptyDB" runat="server" Visible="false"
                   CssClass="alert border-warning my-4 p-4 text-warning d-flex flex-column align-items-center">
            <h2 class="alert-heading">
                ¡Vaya! Parece que no hay productos disponibles en este momento.
            </h2>
            <p class="lead fs-5">
                Estamos trabajando para solucionar este inconveniente y que los
                productos vuelvan a estar a disposición en la brevedad.
            </p>
            <p class="lead fs-5">
                Si el problema persiste, no dudes en ponerte en contacto con nuestro equipo 
                de atención al cliente.
            </p>
            <a href="<%: Domain.Constants.ContactPagePath %>" 
               class="btn btn-outline-warning p-3 text-uppercase w-50">
                Formulario de contacto →
            </a>
        </asp:Panel>

        <!-- Alerta de producto no encontrado -->
        <asp:Panel ID="alertProductNotFound" runat="server" Visible="false"
                   CssClass="alert border-warning my-4 text-warning text-center">
            <h2 class="alert-heading">Producto no encontrado</h2>
            <p class="lead fs-5">
                No se ha encontrado ningún producto con el criterio de 
                búsqueda seleccionado.
            </p>
        </asp:Panel>

        <% if (((List<Domain.Product>)Session["PRODUCTS"]).Count > 0) { %>
            <asp:Repeater ID="ProductCards" runat="server">
                <ItemTemplate>
                    <div class="col-4 p-4">
                        <article class="card bg-dark overflow-hidden border rounded-5" style="max-height: 420px">
                            <figure class="card-img-top bg-gradient text-white">
                                <img src="<%# Eval("Image") %>"
                                     alt="Imagen del producto <%# Eval("Name") %>">
                            </figure>
                            <div class="card-body d-flex flex-column gap-1 justify-content-between">
                                <h5 class="card-title text-center text-white fw-bold">
                                    <%# Eval("Name") %>
                                </h5>
                                <div class="d-flex justify-content-center gap-2">
                                    <span class="badge rounded-pill text-white border bg-gradient"><%# Eval("Brand.Description") %></span>
                                    <span class="badge rounded-pill text-white border bg-gradient"><%# Eval("Category.Description") %></span>
                                </div>
                                <p class="card-text text-white lead fs-6 mt-2">
                                    <%# Eval("Description") %>
                                </p>
                                <p class="card-text text-warning text-center fs-2">
                                    $<%# ((decimal)Eval("Price")).ToString("N2") %>
                                </p>
                                <a href="<%: Domain.Constants.ProductDetailPagePath %>?id=<%# Eval("ID") %>"
                                   class="btn btn-outline-light">Ver detalle →</a>
                            </div>
                        </article>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        <% } %>
    </section>

    <!-- TEST END -->
    </div>

</asp:Content>
