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
