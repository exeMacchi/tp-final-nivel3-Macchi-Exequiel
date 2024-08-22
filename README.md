# Trabajo Práctico Final C# Nivel 3
## Descripción
Aplicación web diseñada para la visualización y gestión de un catálogo de productos, destacando un diseño moderno y minimalista.

El proyecto integra múltiples funcionalidades avanzadas como manejo de usuarios, gestión de productos, y un sistema de favoritos, todo dentro de una arquitectura de software estructurada.

---
## Características Destacadas
- **CRUD Completo**: gestión integral de productos (lectura, creación, modificación y eliminación).
- **Manejo de Sesión de Usuario**: autenticación, perfiles, y manejo de favoritos.
- **Gestión de Favoritos**: los usuarios pueden marcar productos como favoritos y gestionarlos desde su perfil.
- **Manejo de Imágenes**: soporte para imágenes locales y mediante URL.
- **Filtros Avanzados y Básicos**: filtros básicos y avanzados para la visualización de productos específicos.
- **Lógica de Negocio**: implementación robusta de reglas de negocio.
- **Lógica de Base de Datos**: interacciones seguras y eficientes con la base de datos.
- **Envío de Correos Electrónicos**: funcionalidad implementada con MailTrap para pruebas.

---

## Tecnologías Utilizadas
- **Lenguaje de Programación**: C#
- **Framework**: ASP.NET Web Forms
- **Frontend**: HTML, CSS, JavaScript, Bootstrap
- **Base de Datos**: SQL Server
- **Acceso a Datos**: ADO.NET SqlClient

---

## Arquitectura de Software
La aplicación está organizada bajo una arquitectura de cuatro capas:

- **UI** (*Interfaz de Usuario*): manejo de la presentación visual y la interacción del usuario.
- **BLL** (*Lógica de Negocio*): implementación de las reglas de negocio y procesos.
- **DAL** (*Acceso a Datos*): manejo de las operaciones con la base de datos.
- **DOMAIN** (*Dominio*): manejo de las plantillas de objetos, métodos auxiliares y constantes.

---

## Páginas y Funcionalidades
1. **Default** (*Home*)
    - Presentación general de la aplicación, incluyendo la paleta de colores y el diseño de la interfaz.
    - Muestra de productos en formato de tarjetas.

2. **Products** (*General*)
    - Visualización del catálogo completo de productos.
    - Filtros por nombre, categoría, marca y precio.
    - Paginación de productos.
    - Visualización de productos favoritos para usuarios autenticados.

3. **ProductDetail** (*General*)
    - Detalle individual de cada producto.
    - Funcionalidad para añadir o remover productos de favoritos si el usuario está autenticado.

4. **Contact** (*General*)
    - Formulario de contacto con lógica para el envío de correos electrónicos.

5. **Error** (*General*)
    - Página dedicada al manejo de excepciones y errores.

6. **UserFavorites** (*User*)
    - Disponible solo para usuarios autenticados.
    - Muestra y permite gestionar los productos marcados como favoritos.
    - Incluye paginación y filtros (nombre, categoría, marca, precio).

7. **Profile** (*User*)
    - Visualización y edición del perfil del usuario, incluyendo nombre, apellido y foto de avatar.

8. **Admin** (*Admin*)
    - Acceso exclusivo para administradores.
    - Gestión completa de productos mediante CRUD.
    - Filtros básicos y avanzados para la búsqueda de productos.

9. **CreateEdit** (*Admin*)
    - Permite crear o editar productos, incluyendo detalles como código, nombre, descripción, precio, marca, categoría e imagen.
    - Manejo de imagenes externas o locales.

10. **Login** (*Auth*)
    - Formulario de autenticación con validaciones en frontend y backend.

11. **Register** (*Auth*)
    - Formulario de registro de usuarios con validaciones en frontend y backend.

12. **ForgottenPass** (*Auth*)
    - Recuperación de cuentas mediante el envío de un correo electrónico a usuarios registrados.
