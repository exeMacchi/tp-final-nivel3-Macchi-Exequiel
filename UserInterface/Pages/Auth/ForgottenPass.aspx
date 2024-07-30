<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.Master" AutoEventWireup="true" CodeBehind="ForgottenPass.aspx.cs" Inherits="UserInterface.Pages.Auth.ForgottenPass" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <title>El Almacenero - Restaurar contraseña</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <!-- Título -->
    <div class="row justify-content-center my-4">
        <div class="col-6">
            <% if (Request.QueryString["id"] == null) { %>
                <h1 class="text-white text-center">Recuperar cuenta</h1>
                <p class="lead text-white text-center my-1">
                    ¿Te olvidaste tu contraseña? ¡No te preocupes! 
                </p>
                <p class="lead text-white my-1">
                    Ingrese su correo electrónico y recibe las instrucciones 
                    para recuperarla.
                </p>
            <% } else { %>
                <h1 class="text-white text-center">Restaurar contraseña</h1>
                <p class="lead text-white text-center my-1">
                    Por favor, ingrese su nueva contraseña y confírmela
                </p>
            <% } %>
        </div>
    </div>

    <!-- Alertas -->
    <div class="row justify-content-center my-1">
        <div class="col-6">
            <!-- Alertas de éxito -->
            <asp:Panel ID="forgottenSuccess" runat="server" Visible="false"
                       CssClass="alert border-success bg-dark text-success text-center">
                <asp:Label ID="forgottenSuccessHeader" runat="server"
                           CssClass="d-block alert-heading fs-2" style="font-weight: 500;"></asp:Label>
                <asp:Label ID="forgottenSuccessText" runat="server" CssClass="text-success"></asp:Label>
            </asp:Panel>

            <!-- Alertas de error -->
            <asp:Panel ID="forgottenError" runat="server" Visible="false"
                       CssClass="alert border-danger bg-dark text-danger text-center">
                <asp:Label ID="forgottenErrorHeader" runat="server"
                           CssClass="d-block alert-heading fs-2" style="font-weight: 500;"></asp:Label>
                <asp:Label ID="forgottenErrorText" runat="server" CssClass="text-dagner"></asp:Label>
            </asp:Panel>
        </div>
    </div>

    <% // Recuperar cuenta %>
    <% if (Request.QueryString["id"] == null) { %>
        <asp:ScriptManager ID="spForgottenPass" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="upForgottenPass" runat="server">
            <ContentTemplate>
                <!-- Email -->
                <div class="row justify-content-center my-4">
                    <div class="form-floating col-6">
                        <asp:TextBox ID="txbxEmail" runat="server" Placeholder="usuario@ejemplo.com"
                                     Required="true" MaxLength="100" TextMode="Email"
                                     ClientIDMode="Static" AutoPostBack="true" 
                                     OnTextChanged="txbxEmail_TextChanged"></asp:TextBox>
                        <asp:Label ID="lbEmail" runat="server" AssociatedControlID="txbxEmail"
                                   Text="Correo electrónico" CssClass="ps-4"></asp:Label>
                        <asp:Panel ID="invalidEmailLength" runat="server" Visible="false"
                                   CssClass="invalid-feedback">
                            El correo electrónico debe tener entre 1 a 100 caracteres.
                        </asp:Panel>
                        <asp:Panel ID="invalidEmail" runat="server" Visible="false"
                                   CssClass="invalid-feedback">
                            El correo electrónico proporcionado no se encuentra en la base de datos.
                            Por favor, verifique que el correo electrónico ingresado sea correcto.
                        </asp:Panel>
                    </div>
                </div>

                <!-- Send -->
                <div class="row justify-content-center my-4">
                    <div class="col-6">
                        <asp:Button ID="btnSend" runat="server" Text="ENVIAR"
                                    CssClass="btn btn-outline-warning w-100 p-3"
                                    ClientIDMode="Static" OnClick="btnSend_Click" />
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSend" />
            </Triggers>
        </asp:UpdatePanel>

    <% // Restaurar contraseña %>
    <% } else { %>
        <!-- FirstPass -->
        <div class="row justify-content-center my-4">
            <div class="col-6">
                <div class="input-group">
                    <div class="form-floating">
                        <asp:TextBox ID="txbxFirstPassword" runat="server" Required="true"
                                     Placeholder="•••••••" MaxLength="20" TextMode="Password"
                                     ClientIDMode="Static" oninput="checkPasswordsMatch()"></asp:TextBox>
                        <asp:Label ID="lbFirstPassword" runat="server" Text="Contraseña"
                                   AssociatedControlID="txbxFirstPassword"></asp:Label>
                    </div>

                    <button id="btnFirstPassword" type="button" tabindex="-1"
                            class="btn border border-1 p-1 px-2 rounded-1 bg-black"
                            style="width: 15%;"
                            onclick="togglePassword('txbxFirstPassword', 'btnFirstPassword')">
                            <i class="bi bi-eye-slash text-warning fs-4"></i>
                    </button>
                </div>
            </div>
        </div>

        <!-- SecondPass -->
        <div class="row justify-content-center my-4">
            <div class="col-6">
                <div class="input-group">
                    <div class="form-floating">
                        <asp:TextBox ID="txbxSecondPassword" runat="server" Required="true"
                                     Placeholder="•••••••" MaxLength="20" TextMode="Password"
                                     ClientIDMode="Static" oninput="checkPasswordsMatch()"></asp:TextBox>
                        <asp:Label ID="lbSecondPassword" runat="server" Text="Confirmar contraseña"
                                   AssociatedControlID="txbxFirstPassword"></asp:Label>
                    </div>

                    <button id="btnSecondPassword" type="button" tabindex="-1"
                            class="btn border border-1 p-1 px-2 rounded-1 bg-black"
                            style="width: 15%;"
                            onclick="togglePassword('txbxSecondPassword', 'btnSecondPassword')">
                            <i class="bi bi-eye-slash text-warning fs-4"></i>
                    </button>
                </div>
                <asp:Label ID="lbPassAlert" runat="server" ClientIDMode="Static" 
                           CssClass="invalid-feedback" style="display:none;">
                    Las contraseñas no coinciden.
                </asp:Label>
            </div>
        </div>
        <!-- Submit -->
        <div class="row justify-content-center my-4">
            <div class="col-6">
                <asp:Button ID="btnSubmit" runat="server" Text="RESTAURAR CONTRASEÑA"
                            CssClass="btn btn-outline-warning w-100 p-3"
                            ClientIDMode="Static" OnClick="btnSubmit_Click" />
            </div>
        </div>

        <script src="/Assets/Scripts/TogglePasswordReveal.js"></script>
        <script src="/Assets/Scripts/CheckPasswordsForgotten.js"></script>
    <% } %>
</asp:Content>
