import priceKeypressValidation from "./NumberInput.js";
import changeImage from "./FileInput.js";

document.addEventListener("DOMContentLoaded", (event) => {
    priceKeypressValidation();
    changeImage();
});


// Esto asegura que la función de validación y el cambio de imagen se ejecute
// cada vez que se complete una postback parcial del UpdatePanel.
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(priceKeypressValidation);
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(changeImage);
