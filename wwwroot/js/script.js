document.addEventListener("DOMContentLoaded", () => {
    let cartCount = 0;
    let cartTotal = 0;

    const cartCountEl = document.getElementById("cart-count");
    const cartTotalEl = document.getElementById("cart-total");

    document.querySelectorAll(".add-to-cart").forEach(button => {
        button.addEventListener("click", () => {
            cartCount++;
            cartTotal += 25; // Valor fixo para simulação

            cartCountEl.textContent = cartCount;
            cartTotalEl.textContent = cartTotal.toFixed(2);
        });
    });
});

function openModal(modalId) {
    const modalContainer = document.getElementById(modalId);
    modalContainer.classList.add('mostrar');
}

document.querySelectorAll('.modal-container').forEach(modal => {
    modal.addEventListener('click', (e) => {
        if (e.target.classList.contains('modal-container') || e.target.classList.contains('fechar')) {
            modal.classList.remove('mostrar');
        }
    });
});
