
window.addEventListener("DOMContentLoaded", () => {
    const btnAvatar = document.querySelector("#btnAvatar");
    const btnAvatarSubmit = document.querySelector("#btnAvatarSubmit");
    const avatarIcon = document.querySelector("#avatarIcon");
    const fuAvatar = document.querySelector("#fuAvatar");
    const imgAvatar = document.querySelector("#imgAvatar");

    btnAvatar.addEventListener("click", (event) => {
        event.preventDefault();
        // Se va subir una nueva imagen
        if (avatarIcon.classList.contains("bi-pencil-square")) {
            fuAvatar.click();
        }
        // Se va a confirmar la nueva imagen
        else {
            btnAvatarSubmit.click();
            avatarIcon.classList.replace("bi-check-circle", "bi-pencil-square");
        }
    });

    fuAvatar.addEventListener("change", () => {
        const file = fuAvatar.files[0];
        if (file) {
            // Verificar que se seleccionó un archivo.
            const reader = new FileReader();

            // Asignar la imagen cargada al atributo src de la etiqueta img.
            reader.onload = (e) => {
                imgAvatar.src = e.target.result;
            };

            // Leer el contenido del archivo como una URL de datos.
            reader.readAsDataURL(file);

            // Se modifica el ícono del botón que permite la posterior
            // confirmación de la imagen.
            avatarIcon.classList.replace("bi-pencil-square", "bi-check-circle");
        }
    });
});
