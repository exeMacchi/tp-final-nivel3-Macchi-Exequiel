<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="UserInterface.Pages.Global.Contact" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <title>El Almacenero - Contacto</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <div class="row justify-content-center my-4">
        <div class="col-6">
            <h1 class="text-white text-center">Formulario de contacto</h1>
            <p class="lead text-white my-2 fs-5">
                Si tienes alguna pregunta, sugerencia o necesitas asistencia, 
                no dudes en comunicarte con nosotros.
            </p>
            <p class="lead text-white my-2 fs-5">
                Rellene el siguiente formulario y nos pondremos en contacto 
                contigo lo antes posible.
            </p>
        </div>
    </div>

    <!-- Alertas -->
    <div class="row justify-content-center my-1">
        <div class="col-6">
            <!-- Alerta de éxito -->
            <asp:Panel ID="contactSuccess" runat="server" Visible="false"
                       CssClass="alert border-success bg-dark text-success text-center">
                <h2 class="alert-heading fs-2">¡Gracias por comunicarse con nosotros!</h2>
                <p class="text-success my-0">
                    ¡El correo electrónico ha sido enviado de forma exitosa! 
                    Dentro de poco le responderemos.
                </p>
            </asp:Panel>

            <!-- Alerta de error -->
            <asp:Panel ID="contactError" runat="server" Visible="false"
                       CssClass="alert border-danger bg-dark text-danger">
                <h2 class="alert-heading fs-2 text-center">Error</h2>
                <p class="text-danger my-2">
                    Lamentablemente, ocurrió un problema al intentar enviar tu mensaje. 
                    Por favor, verifica tu conexión a internet e inténtalo nuevamente. 
                </p>
                <p class="text-danger my-2">
                    Si el problema persiste, contáctanos directamente a través de nuestro 
                    correo electrónico: <strong>soporte@elalmacenero.com</strong>. 
                </p>
                <p class="text-danger my-2">
                    Disculpa las molestias y gracias por tu paciencia.
                </p>
            </asp:Panel>
        </div>
    </div>

    <asp:ScriptManager ID="spContact" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="upContact" runat="server">
        <ContentTemplate>
            <!-- Email -->
            <div class="row justify-content-center my-4">
                <div class="col-6">
                    <asp:Label ID="lbEmail" runat="server" Text="Correo electrónico"
                               CssClass="form-label fw-medium text-white"
                               AssociatedControlID="txbxEmail"></asp:Label>
                    <span class="text-danger">*</span>
                    <asp:TextBox ID="txbxEmail" runat="server" Required="true"
                                 CssClass="form-control bg-dark text-white"
                                 Placeholder="usuario@ejemplo.com"
                                 MaxLength="100" TextMode="Email" AutoPostBack="true"
                                 OnTextChanged="txbxTextChanged"></asp:TextBox>
                    <div id="invalidEmail" class="invalid-feedback">
                        El correo electrónico debe tener entre 1 a 100 caracteres.
                    </div>
                </div>
            </div>

            <!-- Asunto -->
            <div class="row justify-content-center my-4">
                <div class="col-6">
                    <asp:Label ID="lbSubject" runat="server" Text="Asunto"
                               CssClass="form-label fw-medium text-white"
                               AssociatedControlID="txbxSubject"></asp:Label>
                    <span class="text-danger">*</span>
                    <asp:TextBox ID="txbxSubject" runat="server" Required="true"
                                 CssClass="form-control bg-dark text-white"
                                 Placeholder="Ingrese el asunto..."
                                 MaxLength="100" AutoPostBack="true" 
                                 OnTextChanged="txbxTextChanged"></asp:TextBox>
                    <div id="invalidSubject" class="invalid-feedback">
                        El campo no puede estar vacío.
                    </div>
                </div>
            </div>

            <!-- Mensaje -->
            <div class="row justify-content-center my-4">
                <div class="col-6">
                    <asp:Label ID="lbMessage" runat="server" Text="Mensaje"
                               CssClass="form-label fw-medium text-white"
                               AssociatedControlID="txbxMessage"></asp:Label>
                    <span class="text-danger">*</span>
                    <asp:TextBox ID="txbxMessage" runat="server" TextMode="MultiLine"
                                 CssClass="form-control bg-dark text-white"
                                 Rows="8" style="resize: none;" Required="true"
                                 Placeholder="Ingrese el mensaje..." MaxLength="200"
                                 AutoPostBack="true" OnTextChanged="txbxTextChanged"></asp:TextBox>
                    <div id="invalidMessage" class="invalid-feedback">
                        El campo no puede estar vacío.
                    </div>
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

</asp:Content>
