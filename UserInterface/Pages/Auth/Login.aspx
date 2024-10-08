﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="UserInterface.Pages.Auth.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <title>El Almacenero - Ingresar</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <div class="row">
        <h1 class="text-white text-center my-4">Ingrese a su cuenta</h1>
    </div>

    <!-- Alertas -->
    <!-- Middlewares -->
    <div class="row justify-content-center my-1">
        <div class="col-6">
            <% if (Request.QueryString["alert"] != null) { %>
                <% if (Request.QueryString["alert"] == "error") { %>
                    <asp:Panel ID="loginErrorAlert" runat="server" 
                               CssClass="alert border-danger bg-dark text-danger text-center">
                        <h2 class="alert-heading fs-2 text-center">Error</h2>
                        <p class="lead fs-5 text-center my-0"><%:Session["ALERTMESSAGE"].ToString()%></p>
                    </asp:Panel>
                <% } else if (Request.QueryString["alert"] == "success") { %>
                    <asp:Panel ID="loginSuccessAlert" runat="server"
                               CssClass="alert border-success bg-dark my-4 text-success">
                        <h2 class="alert-heading fs-2 text-center">¡Éxito!</h2>
                        <p class="lead fs-5 text-center my-0"><%:Session["ALERTMESSAGE"].ToString()%></p>
                    </asp:Panel>
                <% } %>
            <% } %>
        </div>
    </div>

    <!-- Datos incorrectos -->
    <div class="row justify-content-center my-1">
        <div class="col-6">
            <asp:Panel ID="alertWrongUserCredentials" runat="server" Visible="false"
                       CssClass="alert border-danger bg-dark text-danger text-center">
                <h2 class="alert-heading fs-2">Error de inicio de sesión</h2>
                <p class="lead fs-5">
                    Datos de usuario incorrectos. Por favor, verifique tus 
                    credenciales y vuelva a intentarlo.
                </p>
            </asp:Panel>
        </div>
    </div>

    <!-- Email -->
    <div class="row justify-content-center my-4">
        <div class="form-floating col-6">
            <asp:TextBox ID="txbxEmail" runat="server" Placeholder="usuario@ejemplo.com"
                         Required="true" MaxLength="100" TextMode="Email"
                         ClientIDMode="Static"></asp:TextBox>
            <asp:Label ID="lbEmail" runat="server" CssClass="ps-4"
                       AssociatedControlID="txbxEmail" Text="Correo electrónico"></asp:Label>
            <div id="invalidEmail" class="invalid-feedback">
                El correo electrónico debe tener entre 1 a 100 caracteres.
            </div>
        </div>
    </div>

    <!-- Password -->
    <div class="row justify-content-center my-4">
        <div class="col-6">
            <div class="input-group">
                <div class="form-floating">
                    <asp:TextBox ID="txbxPassword" runat="server" Required="true"
                                 Placeholder="•••••••" MaxLength="20" TextMode="Password"
                                 ClientIDMode="Static"></asp:TextBox>
                    <asp:Label ID="lbFirstPassword" runat="server" Text="Contraseña"
                               AssociatedControlID="txbxPassword"></asp:Label>
                </div>

                <button id="btnPassword" type="button" tabindex="-1"
                        class="btn border border-1 p-1 px-2 rounded-1 bg-black"
                        style="width: 15%;"
                        onclick="togglePassword('txbxPassword', 'btnPassword')">
                        <i class="bi bi-eye-slash text-warning fs-4"></i>
                </button>
            </div>
            <asp:Label ID="lbPassAlert" runat="server" ClientIDMode="Static" 
                       CssClass="invalid-feedback" style="display:none;">
                La contraseña debe tener entre 1 a 20 caracteres.
            </asp:Label>
        </div>
    </div>

    <!-- Forgotten Pass & Register -->
    <div class="row justify-content-center my-4">
        <div class="col-6 d-flex justify-content-between">
            <a href="<%: Domain.Constants.ForgottenPassPath %>"
               class="link-warning link-underline-opacity-0" 
               tabindex="-1">
                ¿Olvidaste tu contraseña?
            </a>

            <a href="<%: Domain.Constants.RegisterPagePath %>" 
               class="link-warning link-underline-opacity-0" 
               tabindex="-1">
                ¿No tienes una cuenta? ¡Regístrate! 
            </a>
        </div>
    </div>

    <!-- Log-in -->
    <div class="row justify-content-center my-4">
        <div class="col-6">
            <asp:Button ID="btnLogin" runat="server" Text="INGRESAR"
                        CssClass="btn btn-outline-warning w-100 p-3"
                        ClientIDMode="Static" OnClick="btnLogin_Click" />
        </div>
    </div>

    <script src="/Assets/Scripts/TogglePasswordReveal.js"></script>
    <script src="/Assets/Scripts/CheckInputsLogin.js"></script>
</asp:Content>
