﻿/**
 * Intercambiar el modo de vista de un input type="password".
 * @param {String} inputID Cadena que representa el 'id' del elemento HTML TextBox/input.
 * @param {String} buttonID Cadena que repersenta el 'id' del elemento HTML button.
 */
function togglePassword(inputID, buttonID) {
    const passInput = document.querySelector(`#${inputID}`);
    const button = document.querySelector(`#${buttonID}`);
    const icon = document.querySelector(`#${buttonID} > i`);

    if (passInput.type === 'password') {
        passInput.type = 'text';
        icon.classList.replace("bi-eye-slash", "bi-eye");
        icon.classList.replace("text-warning", "text-black");
        button.classList.replace("bg-black", "bg-warning");
    } else {
        passInput.type = 'password';
        icon.classList.replace("bi-eye", "bi-eye-slash");
        icon.classList.replace("text-black", "text-warning");
        button.classList.replace("bg-warning", "bg-black");
    }
}