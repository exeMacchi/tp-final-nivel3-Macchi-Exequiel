<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.Master" AutoEventWireup="true" CodeBehind="ProductDetail.aspx.cs" Inherits="UserInterface.Pages.Global.ProductDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <title>El Almacenero - Detalle de producto</title>
    <link href="/Assets/Styles/FavoriteProduct.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <div class="my-5">
        <!-- Alerta -->
        <asp:Panel ID="pnlFavoriteAlert" runat="server" CssClass="d-flex justify-content-end my-2">
            <div class="alert border-warning rounded-4 fade show d-inline-flex gap-2 align-items-center m-0"
                 role="alert">
                <asp:Label ID="lbFavoriteAlertText" runat="server" Text=""
                           CssClass="text-warning lead"></asp:Label>
                <asp:LinkButton runat="server" data-bs-dismiss="alert" aria-label="Close">
                    <i class="bi bi-x-circle text-warning fs-5"></i>
                </asp:LinkButton>
            </div>
        </asp:Panel>

        <!-- Detalle -->
        <asp:Panel ID="pnlProductDetail" runat="server" 
                   CssClass="row bg-dark rounded-5 border text-white py-4 px-3">
            <div class="col-6 d-flex justify-content-center align-items-center">
                <figure class="text-white" style="height: 300px">
                    <asp:Image ID="productImage" runat="server"/>
                </figure>
            </div>

            <div class="col-6 d-flex flex-column gap-2">
                <div class="d-flex justify-content-between">
                    <!-- Código -->
                    <asp:Label ID="lbCode" runat="server" CssClass="fs-6 opacity-75"></asp:Label>

                    <% if (Session["USER"] != null) { %>
                        <!-- Botón de favorito -->
                        <asp:LinkButton ID="btnFavorite" runat="server"
                                        CssClass="btn border p-2 border rounded-circle lh-1"
                                        OnClick="btnFavorite_Click">
                            <asp:Panel ID="pnlFavoriteIcon" runat="server" CssClass="bi"></asp:Panel>
                        </asp:LinkButton>
                    <% } %>
                </div>

                <!-- Título -->
                <asp:Label ID="lbTitle" runat="server"
                           CssClass="d-block fs-1 fw-bolder my-1"></asp:Label>

                <!-- Etiquetas -->
                <div class="d-flex gap-3 align-items-center">
                    <asp:Label ID="lbBrand" runat="server"
                               CssClass="badge rounded-pill text-white border bg-gradient fs-6"></asp:Label>
                    <asp:Label ID="lbCategory" runat="server" 
                               CssClass="badge rounded-pill text-white border bg-gradient fs-6"></asp:Label>
                </div>

                <!-- Precio -->
                <asp:Label ID="lbPrice" runat="server"
                           CssClass="d-block text-warning fs-2 my-3"></asp:Label>

                <!-- Descripción -->
                <asp:Label ID="lbDescription" runat="server"
                           CssClass="lead fst-italic"></asp:Label>

                <!-- Volver a la lista de productos -->
                <% if (Request.QueryString["p"] == "favorites") { %>
                    <a href="<%: Domain.Constants.FavoritesPagePath %>"
                       class="btn btn-outline-light">← Volver a la lista de productos favoritos</a>
                <% } else { %>
                    <a href="<%: Domain.Constants.ProductsPagePath %>"
                       class="btn btn-outline-light">← Volver a la lista de productos</a>
                <% } %>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
