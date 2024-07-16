
/**
 * Función que sirve para permitir solo escribir números, comas o puntos en el input de precio.
 */
export default function priceKeypressValidation() {
    const numberInput = document.querySelector("#txbxPrice");

    if (numberInput) {
        numberInput.addEventListener("keydown", (e) => {
            const allowedKeys = ['Backspace', 'Delete', 'ArrowLeft', 'ArrowRight', 'Tab'];
            const isNumber = e.key >= '0' && e.key <= '9';
            const isDecimalSeparator = e.key === '.' || e.key === ',';
            const isAllowedKey = allowedKeys.includes(e.key);

            if (!isNumber && !isDecimalSeparator && !isAllowedKey) {
                e.preventDefault();
            }
        });

        numberInput.addEventListener("input", () => {
            numberInput.value = numberInput.value.replace(/[^0-9.,]/g, '');
        });
    }
};
