<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.Master" AutoEventWireup="true" CodeBehind="ErrorPage.aspx.cs" Inherits="UserInterface.Pages.Global.ErrorPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <title>El Almacenero - Error</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <div class="row bg-dark my-5 border rounded-5 border-danger border-3 d-flex flex-column p-4 gap-5">
        <h1 class="text-white text-center">¡Ups! Parece que hubo un error</h1>

        <asp:Label ID="lbErrorText" runat="server"
                   CssClass="text-center text-white fs-5"></asp:Label>

        <asp:Panel ID="pnlErrorDetail" runat="server" Visible="false" 
                   ClientIDMode="Static" style="cursor: pointer;">
            <div id="detailHeaderContainer"
                 class="d-flex justify-content-between border-bottom border-2 py-2">
                <span class="text-white fs-5">Detalle del error</span>
                <i id="chevronDefault" class="bi bi-chevron-down text-white fs-5"></i>
                <i id="chevronActive" class="bi bi-chevron-up text-warning fs-5 d-none"></i>
            </div>
            <ul id="detailTextContainer" 
                class="border-start border-bottom border-end border-2 border-warning my-4 d-none">
                <li class="text-white py-3">
                    <asp:Label ID="lbDetailErrorText" runat="server"></asp:Label>
                </li>
            </ul>
        </asp:Panel>

        <div class="d-flex gap-3">
            <a href="<%: Domain.Constants.DefaultPagePath %>" 
               class="btn btn-outline-light w-50 btn-lg px-4">
                ← Volver al inicio
            </a>

            <a href="<%: Domain.Constants.ContactPagePath %>" 
               class="btn btn-outline-light w-50 btn-lg px-4">
                Contactar soporte técnico →
            </a>
        </div>
    </div>

    <script src="/Assets/Scripts/ErrorDetail.js"></script>
</asp:Content>
