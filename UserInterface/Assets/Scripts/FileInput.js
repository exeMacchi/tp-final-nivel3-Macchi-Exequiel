/**
* Cargar la imagen de visualización con la imagen cargada en el input file.
*/
export default function changeImage() {
    const inputURL = document.querySelector("#txbxImage");
    const inputFile = document.querySelector("#fuImage");
    const img = document.querySelector("#imgProduct");

    // Si se carga una imagen desde el sistema de archivos.
    inputFile.addEventListener("change", () => {
        const file = inputFile.files[0];
        if (file) {
            // Verificar que se seleccionó un archivo.
            const reader = new FileReader();

            // Asignar la imagen cargada al atributo src de la etiqueta img.
            reader.onload = (e) => {
                img.src = e.target.result;
            };

            // Leer el contenido del archivo como una URL de datos.
            reader.readAsDataURL(file);

            // Agregar estilo de verificación válida.
            inputFile.classList.add("is-valid");

            // Limpiar el contenido del input URL y sus posibles estilos de validación.
            inputURL.value = "";
            if (inputURL.classList.contains("is-valid")) {
                inputURL.classList.remove("is-valid");
            }
        }
    });

    // Si se carga una imagen como una URL.
    inputURL.addEventListener("keydown", () => {
        // Se limpia el archivo cargado si lo hubiera.
        if (inputFile.value.trim() !== "") {
            inputFile.value = "";
        }
    })
}