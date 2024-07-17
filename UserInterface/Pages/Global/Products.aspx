<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.Master" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="UserInterface.Pages.Global.Products" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <title>El Almacenero - Catálogo de productos</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <aside class="border-bottom">
        <h1 class="text-white text-center">Catálogo de productos</h1>
        <h2 class="text-white">Filtros</h2>
    </aside>

    <section class="d-flex flex-wrap my-4">
        <% if (((List<Domain.Product>)Session["PRODUCTS"]).Count > 0) { %>
            <asp:Repeater ID="ProductCards" runat="server">
                <ItemTemplate>
                    <div class="col-4 p-4">
                        <article class="card bg-dark overflow-hidden border rounded-5 h-100">
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
                                <p class="card-text text-white lead">
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
        <% } else { %>
        <% // TODO: alerta de que no hay productos en la base de datos. %>
        <% } %>
    </section>
</asp:Content>
