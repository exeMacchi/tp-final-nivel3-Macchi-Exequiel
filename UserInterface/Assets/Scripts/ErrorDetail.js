window.addEventListener("DOMContentLoaded", () => {
    const detailContainer = document.querySelector("#pnlErrorDetail");
    const detailHeaderContainer = document.querySelector("#detailHeaderContainer");
    const detailTextContainer = document.querySelector("#detailTextContainer");
    const chevronDefault = document.querySelector("#chevronDefault");
    const chevronActive = document.querySelector("#chevronActive");

    detailContainer.addEventListener("click", () => {
        if (chevronActive.classList.contains("d-none")) {
            chevronDefault.click();
        }
        else {
            chevronActive.click();
        }
    })

    chevronDefault.addEventListener("click", (event) => {
        event.stopPropagation();
        chevronDefault.classList.add("d-none");
        chevronActive.classList.remove("d-none");
        detailTextContainer.classList.remove("d-none");
        detailHeaderContainer.classList.add("border-warning");
    });

    chevronActive.addEventListener("click", (event) => {
        event.stopPropagation();
        chevronDefault.classList.remove("d-none");
        chevronActive.classList.add("d-none");
        detailTextContainer.classList.add("d-none");
        detailHeaderContainer.classList.remove("border-warning");
    })
});