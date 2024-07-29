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
            document.querySelector("#btnRegister").setAttribute("disabled", "true");
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
        document.querySelector("#btnRegister").setAttribute("disabled", "true");
    }
}

/** Verificar desde el cliente si se debe habilitar el botón de registro. */
function verifyInformation() {
    const email = document.querySelector("#txbxEmail").value;
    const firstPass = document.querySelector("#txbxFirstPassword").value;
    const secondPass = document.querySelector("#txbxSecondPassword").value;
    const termsAndConditions = document.querySelector("#chbxTermsConditions").checked;
    const btnRegister = document.querySelector("#btnRegister");

    if (email !== "" && firstPass !== "" && secondPass != "" && termsAndConditions) {
        btnRegister.removeAttribute("disabled");
    }
    else {
        btnRegister.setAttribute("disabled", "true");
    }
}