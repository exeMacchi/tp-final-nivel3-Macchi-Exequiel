<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="UserInterface.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <title>El Almacenero - Inicio</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <!-- HERO -->
    <section class="my-5 py-4 bg-dark text-center text-white border rounded-5 border-opacity-50">
        <div>
            <i class="bi bi-box" style="font-size: 80px"></i>
        </div>
        <h1 class="display-5 fw-bold">El Almacenero</h1>
        <div class="col-8 mx-auto">
            <p class="lead mb-4">
                Nuestra plataforma web está diseñada para ofrecer una visualización sencilla
                y atractiva de nuestro catálogo de productos. Navega a través de una variedad
                de productos y accede a detalles específicos de cada uno.
            </p>
            <p class="lead mb-4">
                Nuestro portal está enfocado en brindar la mejor experiencia de usuario, 
                con filtros por nombre, marca y categoría para facilitar la navegación. 
                Además, contamos con un sistema de registro que permite a los usuarios 
                realizar un seguimiento de sus productos favoritos.
            </p>
            <div class="d-grid gap-2 d-sm-flex justify-content-sm-center">
                <button type="button" class="btn btn-outline-light w-100 btn-lg px-4 gap-3">¡Regístrate!</button>
            </div>
        </div>
    </section>

    <!-- Products -->
    <section class="row bg-dark my-5 border rounded-5 p-4">
        <div class="col-6 text-white d-flex flex-column">
            <h2 class="fw-bold border-bottom fs-3 pb-2">¿Cómo se presentan los productos?</h2>
            <p class="lead mb-4">
                En nuestra plataforma, los productos se presentan en un formato de tarjeta 
                visualmente atractivo y fácil de navegar. 
            </p>
            <p class="lead mb-4">
                Cada tarjeta contiene una imagen del producto, el título con el nombre 
                del producto, burbujas que indican la categoría y la marca, una pequeña 
                descripción, el precio y un botón de "Ver más" para obtener detalles 
                adicionales.
            </p>
            <a href="/Pages/Global/Products.aspx" class="btn btn-outline-light">
                Ver catálogo de productos
            </a>
        </div>

        <div class="col-6">
            <div id="productCarousel" class="carousel slide h-100">
                <div class="carousel-inner h-100">
                    <div class="carousel-item h-100 active">
                        <article class="card w-50 h-100 mx-auto">
                            <img src="..." class="card-img-top" alt="...">
                            <div class="card-body">
                                <h5 class="card-title">B</h5>
                                <p class="card-text">Some quick example text to build on the card title and make up the bulk of the card's content.</p>
                                <a href="#" class="btn btn-outline-light">Go somewhere</a>
                            </div>
                        </article>
                    </div>

                    <div class="carousel-item h-100">
                        <article class="card w-50 h-100 mx-auto">
                            <img src="..." class="card-img-top" alt="...">
                            <div class="card-body">
                                <h5 class="card-title">A</h5>
                                <p class="card-text">Some quick example text to build on the card title and make up the bulk of the card's content.</p>
                                <a href="#" class="btn btn-outline-light">Go somewhere</a>
                            </div>
                        </article>
                    </div>
                </div>
                <button class="carousel-control-prev" type="button" data-bs-target="#productCarousel" data-bs-slide="prev">
                    <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                    <span class="visually-hidden">Previous</span>
                </button>
                <button class="carousel-control-next" type="button" data-bs-target="#productCarousel" data-bs-slide="next">
                    <span class="carousel-control-next-icon" aria-hidden="true"></span>
                    <span class="visually-hidden">Next</span>
                </button>
            </div>
        </div>
    </section>
</asp:Content>
