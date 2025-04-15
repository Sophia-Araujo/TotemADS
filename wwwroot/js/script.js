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

document.querySelectorAll(".add-to-cart").forEach(button => {
    button.addEventListener("click", () => {
        document.getElementById("popup").style.display = "flex";
    });
});

document.getElementById("voltar").addEventListener("click", () => {
    document.getElementById("popup").style.display = "none";
});

// Lógica de + e - por ingrediente
document.querySelectorAll(".ingrediente").forEach(item => {
    const qtdEl = item.querySelector(".quantidade");
    let quantidade = parseInt(qtdEl.textContent);

    item.querySelector(".mais").addEventListener("click", () => {
        quantidade++;
        qtdEl.textContent = quantidade + "x";
    });

    item.querySelector(".menos").addEventListener("click", () => {
        if (quantidade > 0) {
            quantidade--;
            qtdEl.textContent = quantidade + "x";
        }
    });
});