window.addEventListener("DOMContentLoaded", () => {
    const emailInput = document.querySelector("#txbxEmail");
    const passInput = document.querySelector("#txbxPassword");
    const passAlert = document.querySelector("#lbPassAlert");

    emailInput.addEventListener("input", () => {
        if (emailInput.value === "") {
            emailInput.classList.add("is-invalid");
        }
        else {
            emailInput.classList.remove("is-invalid");
        }
        verifyInformation();
    });

    passInput.addEventListener("input", () => {
        if (passInput.value === "") {
            passInput.classList.add("is-invalid");
            passAlert.style.display = "block";
        }
        else {
            passInput.classList.remove("is-invalid");
            passAlert.style.display = "none";
        }
        verifyInformation();
    });
})

/** Verificar desde el cliente si se debe habilitar el botón de registro. */
function verifyInformation() {
    const email = document.querySelector("#txbxEmail").value;
    const pass = document.querySelector("#txbxPassword").value;
    const btnLogin = document.querySelector("#btnLogin");

    if (email !== "" && pass !== "") {
        btnLogin.removeAttribute("disabled");
    }
    else {
        btnLogin.setAttribute("disabled", "true");
    }
}
