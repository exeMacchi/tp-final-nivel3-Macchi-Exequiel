﻿/** Verificar si la contraseña y la confirmación de contraseñas coinciden. */
function checkPasswordsMatch() {
    const firstPass = document.querySelector("#txbxFirstPassword");
    const secondPass = document.querySelector("#txbxSecondPassword");
    const errorAlert = document.querySelector("#lbPassAlert");
    const validForm = "form-control bg-dark text-white is-valid";
    const invalidForm = "form-control bg-dark text-white is-invalid";

    if (firstPass.value !== "" && secondPass.value !== "") {
        if (firstPass.value !== secondPass.value) {
            firstPass.className = invalidForm;
            secondPass.className = invalidForm;
            errorAlert.style.display = "block";
            document.querySelector("#btnSubmit").setAttribute("disabled", "true");
        }
        else {
            firstPass.className = validForm;
            secondPass.className = validForm;
            errorAlert.style.display = "none";
            verifyInformation();
        }
    }
    else {
        firstPass.className = invalidForm;
        secondPass.className = invalidForm;
        errorAlert.style.display = "block";
        document.querySelector("#btnSubmit").setAttribute("disabled", "true");
    }
}

/** Verificar desde el cliente si se debe habilitar el botón de envío. */
function verifyInformation() {
    const firstPass = document.querySelector("#txbxFirstPassword").value;
    const secondPass = document.querySelector("#txbxSecondPassword").value;
    const btnSubmit = document.querySelector("#btnSubmit");

    if (firstPass !== "" && secondPass != "") {
        btnSubmit.removeAttribute("disabled");
    }
    else {
        btnSubmit.setAttribute("disabled", "true");
    }
}
