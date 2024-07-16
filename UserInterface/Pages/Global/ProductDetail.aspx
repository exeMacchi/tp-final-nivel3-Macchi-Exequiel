<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.Master" AutoEventWireup="true" CodeBehind="ProductDetail.aspx.cs" Inherits="UserInterface.Pages.Global.ProductDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <title>El Almacenero - Detalle de producto</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <section class="row mx-auto align-items-center">


        <div class="col-6">
            <div class="input-group my-3">
                <span class="input-group-text text-white bg-black col-3 justify-content-center fs-5">
                    Código
                </span>
                <asp:TextBox ID="productCode" runat="server" ReadOnly="true"
                             CssClass="form-control bg-dark text-white"></asp:TextBox>
            </div>

            <div class="input-group my-3">
                <span class="input-group-text text-white bg-black col-3 justify-content-center fs-5">
                    Nombre
                </span>
                <asp:TextBox ID="productName" runat="server" ReadOnly="true" 
                             CssClass="form-control bg-dark text-white"></asp:TextBox>
            </div>

            <div class="input-group my-3">
                <span class="input-group-text text-white bg-black col-3 justify-content-center fs-5">
                    Descripción
                </span>
                <asp:TextBox ID="productDescription" runat="server" TextMode="MultiLine" 
                             CssClass="form-control bg-dark text-white" 
                             style="resize: none;" ReadOnly="true" Rows="8"></asp:TextBox>
            </div>

            <div class="input-group my-3">
                <span class="input-group-text text-white bg-black col-3 justify-content-center fs-5">
                    Marca
                </span>
                <asp:TextBox ID="productBrand" runat="server" ReadOnly="true"
                             CssClass="form-control bg-dark text-white"></asp:TextBox>
            </div>

            <div class="input-group my-3">
                <span class="input-group-text text-white bg-black col-3 justify-content-center fs-5">
                    Categoría
                </span>
                <asp:TextBox ID="productCategory" runat="server" ReadOnly="true"
                             CssClass="form-control bg-dark text-white"></asp:TextBox>
            </div>
            <div class="input-group my-3">
                <span class="input-group-text text-white bg-black col-3 justify-content-center fs-5">
                    Precio
                </span>
                <asp:TextBox ID="productPrice" runat="server" ReadOnly="true"
                             CssClass="form-control bg-dark text-white"></asp:TextBox>
            </div>
        </div>

        <div class="col-6 d-flex justify-content-center align-content-center">
            <figure class="text-white" style="height: 300px">
                <asp:Image ID="productImage" runat="server"/>
            </figure>
        </div>
    </section>
</asp:Content>
