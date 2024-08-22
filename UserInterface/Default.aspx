<%@ Page Title="" Language="C#" MasterPageFile="~/MainTemplate.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="UserInterface.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <title>El Almacenero - Inicio</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <!-- HERO -->
    <section class="my-5 py-4 bg-dark text-center text-white border rounded-5">
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
                con filtros por nombre, marca, categoría y precio para facilitar la 
                navegación. 
                Además, contamos con un sistema de registro que permite a los usuarios 
                realizar un seguimiento de sus productos favoritos.
            </p>
            <% if (Session["USER"] == null) { %>
                <div class="d-grid gap-2 d-sm-flex justify-content-sm-center">
                    <a href="<%: Domain.Constants.RegisterPagePath %>" 
                       class="btn btn-outline-light w-100 btn-lg px-4 gap-3">
                        ¡Regístrate!
                    </a>
                </div>
            <% } %>
        </div>
    </section>

    <!-- Products -->
    <section class="row bg-dark my-5 border rounded-5 p-4">
        <div class="col-6 text-white d-flex flex-column gap-2">
            <div class="border-bottom py-3">
                <h2 class="fw-bold text-center">
                    ¿Cómo se presentan los productos?
                </h2>
            </div>
            <p class="lead mb-4">
                En nuestra plataforma, los productos se presentan en un formato de tarjeta 
                visualmente atractivo y fácil de navegar. 
            </p>
            <p class="lead mb-4">
                Cada tarjeta contiene una imagen del producto, el título con el nombre 
                del producto, burbujas que indican la categoría y la marca, una pequeña 
                descripción, el precio y un botón de "Ver detalle" para visualizar
                características adicionales.
            </p>
            <p class="lead mb-4">
                Asimismo, iniciando sesión podrá marcar un producto como favorito,
                el cual se destacará visualmente con un borde redondeado dorado junto 
                a una estrella debajo de la imagen del artículo, facilitando su 
                identificación y acceso rápido.
            </p>
            <a href="<%: Domain.Constants.ProductsPagePath %>" 
               class="btn btn-outline-light mt-auto py-2 fs-5">
                Ver catálogo de productos →
            </a>
        </div>

        <div class="col-6">
            <div id="productCarousel" class="carousel slide h-100">
                <div class="carousel-inner h-100">
                    <!-- Producto normal -->
                    <div class="carousel-item h-100 active">
                        <div class="d-flex flex-column justify-content-center h-100">
                            <article class="card w-50 mx-auto bg-dark overflow-hidden border rounded-5">
                                <figure class="card-img-top bg-gradient text-white"
                                        style="max-height: 180px">
                                    <img src=<%: Domain.Constants.PlaceholderImagePath %>
                                         alt="Marcador de posición par imágenes">
                                </figure>
                                <div class="card-body d-flex flex-column gap-1 justify-content-between">
                                    <h5 class="card-title text-center text-white fw-bold">
                                        Nombre del producto
                                    </h5>
                                    <div class="d-flex justify-content-center gap-2">
                                        <span class="badge rounded-pill text-white border bg-gradient">Marca</span>
                                        <span class="badge rounded-pill text-white border bg-gradient">Categoría</span>
                                    </div>
                                    <p class="card-text text-white lead text-center m-0">
                                        Descripción del producto.
                                    </p>
                                    <p class="card-text text-warning text-center fs-2">
                                        $0,00
                                    </p>
                                    <a href="#" class="btn btn-outline-light">Ver detalle →</a>
                                </div>
                            </article>
                        </div>
                    </div>

                    <!-- Producto favorito -->
                    <div class="carousel-item h-100">
                        <div class="d-flex flex-column justify-content-center h-100">
                            <article class="card w-50 mx-auto bg-dark overflow-hidden border rounded-5 border-warning border-2">
                                <figure class="card-img-top bg-gradient text-white"
                                        style="max-height: 180px">
                                    <img src=<%: Domain.Constants.PlaceholderImagePath %>
                                         alt="Marcador de posición par imágenes">
                                </figure>
                                <div class="card-body d-flex flex-column gap-1 justify-content-between">
                                    <div class="d-flex justify-content-center">
                                        <i class="bi bi-star-fill text-warning"></i>
                                    </div>
                                    <h5 class="card-title text-center text-white fw-bold">
                                        Producto favorito
                                    </h5>
                                    <div class="d-flex justify-content-center gap-2">
                                        <span class="badge rounded-pill text-white border bg-gradient">Marca</span>
                                        <span class="badge rounded-pill text-white border bg-gradient">Categoría</span>
                                    </div>
                                    <p class="card-text text-white lead text-center m-0">
                                        Descripción del producto.
                                    </p>
                                    <p class="card-text text-warning text-center fs-2">
                                        $0,00
                                    </p>
                                    <a href="#" class="btn btn-outline-light">Ver detalle →</a>
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
        </div>
    </section>
</asp:Content>
