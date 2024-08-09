<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="UserInterface.Pages.User.Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <title>El Almacenero - Perfil</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <div class="row justify-content-center my-4">
        <div class="col-6">
            <h1 class="text-white text-center">PERFIL DE USUARIO</h1>
        </div>
    </div>

    <!-- Alertas -->
    <% if (Request.QueryString["alert"] != null) { %>
        <% if (Request.QueryString["alert"] == "success") { %>
            <asp:Panel ID="profileSuccessAlert" runat="server"
                       CssClass="alert border-success bg-dark my-4 text-success text-center">
                <h2 class="alert-heading fs-2">¡Éxito!</h2>
                <p class="lead fs-5 mb-0"><%:(string)Session["ALERTMESSAGE"] %></p>
            </asp:Panel>
        <% } else if (Request.QueryString["alert"] == "error") { %>
            <asp:Panel ID="profileErrorAlert" runat="server" 
                       CssClass="alert border-danger my-4 bg-dark text-danger">
                <h2 class="alert-heading fs-2">Error</h2>
                <p class="lead fs-5 mb-0"><%:(string)Session["ALERTMESSAGE"] %></p>
            </asp:Panel>
        <% } %>
    <% } %>

    <div class="row">
        <div class="col-6">
            <!-- Email -->
            <div class="row px-3">
                <asp:Label ID="lbEmail" runat="server" AssociatedControlID="txbxEmail" 
                           CssClass="form-label fw-medium text-white p-0">
                    Correo electrónico
                </asp:Label>
                <asp:TextBox ID="txbxEmail" runat="server"
                             CssClass="form-control bg-dark text-white opacity-75"
                             ReadOnly="true"></asp:TextBox>
            </div>

            <!-- Nombre -->
            <div class="row my-4 px-3">
                <asp:Label ID="lbName" runat="server" AssociatedControlID="txbxName"
                           CssClass="form-label fw-medium text-white p-0">
                    Nombre
                </asp:Label>
                <div class="input-group p-0">
                    <asp:TextBox ID="txbxName" runat="server"
                                 CssClass="form-control bg-dark text-white"
                                 MaxLength="50"
                                 Placeholder="Introduzca su nombre..."
                                 ReadOnly="true"></asp:TextBox>

                    <asp:LinkButton ID="btnName" runat="server"
                                    CssClass="btn border border-1 p-1 px-2 rounded-1 bg-black"
                                    style="width: 15%;"
                                    OnClick="btnName_Click">
                        <asp:Panel ID="pnName" runat="server" 
                                   CssClass="bi bi-pencil-square text-white fs-5"></asp:Panel>
                    </asp:LinkButton>
                    <div id="invalidName" class="invalid-feedback">
                        El nombre debe tener entre 1 a 50 caracteres.
                    </div>
                </div>
            </div>

            <!-- Apellido -->
            <div class="row my-4 px-3">
                <asp:Label ID="lbSurname" runat="server" AssociatedControlID="txbxSurname"
                           CssClass="form-label fw-medium text-white p-0">
                    Apellido
                </asp:Label>
                <div class="input-group p-0">
                    <asp:TextBox ID="txbxSurname" runat="server"
                                 CssClass="form-control bg-dark text-white"
                                 MaxLength="50"
                                 Placeholder="Introduzca su apellido..."
                                 ReadOnly="true"></asp:TextBox>

                    <asp:LinkButton ID="btnSurname" runat="server"
                                    CssClass="btn border border-1 p-1 px-2 rounded-1 bg-black"
                                    style="width: 15%;"
                                    OnClick="btnSurname_Click">
                        <asp:Panel ID="pnSurname" runat="server" 
                                   CssClass="bi bi-pencil-square text-white fs-5"></asp:Panel>
                    </asp:LinkButton>
                    <div id="invalidSurname" class="invalid-feedback">
                        El apellido debe tener entre 1 a 50 caracteres.
                    </div>
                </div>
            </div>
        </div>

        <!-- Avatar -->
        <div class="col-6">
            <div class="row justify-content-center my-4">
                <figure class="rounded-circle overflow-hidden align-content-center" 
                        style="max-width:220px; margin:0">
                    <asp:Image ID="imgAvatar" runat="server"
                               CssClass="img-fluid rounded-circle"/>
                </figure>
            </div>
        </div>
    </div>
</asp:Content>
