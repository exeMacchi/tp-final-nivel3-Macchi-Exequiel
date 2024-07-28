<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="UserInterface.Pages.Auth.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <title>El Almacenero - Registrarse</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <div class="row justify-content-center">
        <div class="col-6">
            <h1 class="text-white text-center">Crear una cuenta</h1>
        </div>
    </div>

    <!-- Alertas -->
    <div class="row">
    </div>

    <asp:ScriptManager ID="spRegister" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="upRegister" runat="server">
        <ContentTemplate>
            <!-- Email -->
            <div class="row justify-content-center my-4">
                <div class="form-floating col-6">
                    <asp:TextBox ID="txbxEmail" runat="server" Placeholder="usuario@ejemplo.com"
                                 Required="true" MaxLength="100" TextMode="Email"
                                 AutoPostBack="true" OnTextChanged="txbxEmail_TextChanged"></asp:TextBox>
                    <asp:Label ID="lbEmail" runat="server" AssociatedControlID="txbxEmail"
                               Text="Correo electrónico" CssClass="ps-4"></asp:Label>
                    <asp:Panel ID="invalidEmailLength" runat="server" Visible="false"
                               CssClass="invalid-feedback">
                        El correo electrónico debe tener entre 1 a 100 caracteres.
                    </asp:Panel>
                    <asp:Panel ID="invalidEmail" runat="server" Visible="false"
                               CssClass="invalid-feedback">
                        El correo electrónico introducido ya existe en la base de datos.
                    </asp:Panel>
                </div>
            </div>

            <!-- FirstPass -->
            <div class="row justify-content-center my-4">
                <div class="col-6">
                    <div class="input-group">
                        <div class="form-floating">
                            <asp:TextBox ID="txbxFirstPassword" runat="server" Required="true"
                                         Placeholder="•••••••" MaxLength="20" TextMode="Password"
                                         ClientIDMode="Static"></asp:TextBox>
                            <asp:Label ID="lbFirstPassword" runat="server" Text="Contraseña"
                                       AssociatedControlID="txbxFirstPassword"></asp:Label>
                            <div id="invalidFirstPassword" class="invalid-feedback">
                                La contraseña debe tener entre 1 a 20 caracteres.
                            </div>
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
                                         ClientIDMode="Static"></asp:TextBox>
                            <asp:Label ID="lbSecondPassword" runat="server" Text="Confirmar contraseña"
                                       AssociatedControlID="txbxFirstPassword"></asp:Label>
                            <div id="invalidSecondPassword" class="invalid-feedback">
                                La contraseña debe tener entre 1 a 20 caracteres.
                            </div>
                        </div>

                        <button id="btnSecondPassword" type="button" tabindex="-1"
                                class="btn border border-1 p-1 px-2 rounded-1 bg-black"
                                style="width: 15%;"
                                onclick="togglePassword('txbxSecondPassword', 'btnSecondPassword')">
                                <i class="bi bi-eye-slash text-warning fs-4"></i>
                        </button>
                    </div>
                </div>
            </div>

            <!-- Terms & Conditions -->
                <div class="row justify-content-center my-4">
                <div class="col-6">
                    <input type="checkbox" class="form-check-input" required />
                    <span class="ms-1 text-white">
                        Acepto los 
                        <a href="#" class="link-warning link-underline-opacity-0"
                           tabindex="-1">
                            términos y condiciones
                        </a>
                    </span>
                </div>
            </div>

            <!-- Submit -->
            <div class="row justify-content-center my-4">
                <div class="col-6">
                    <asp:Button ID="btnRegister" runat="server" Text="REGISTRARSE"
                                CssClass="btn btn-outline-warning w-100 p-3"
                                OnClick="btnRegister_Click" />
                </div>
            </div>

            <!-- Login -->
            <div class="row justify-content-center my-4">
                <div class="col-6">
                    <p class="d-inline text-white">¿Ya tienes una cuenta?</p>
                    <a href="<%: Domain.Constants.LoginPagePath %>"
                       class="link-warning link-underline-opacity-0"
                       tabindex="-1">
                        Inicie sesión
                    </a>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnRegister" />
        </Triggers>
    </asp:UpdatePanel>

    <script src="/Assets/Scripts/TogglePasswordReveal.js"></script>
</asp:Content>
