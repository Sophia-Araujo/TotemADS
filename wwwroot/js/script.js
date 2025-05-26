document.addEventListener("DOMContentLoaded", () => {
  let cartCount = 0;
  let cartTotal = 0;

  const cartCountEl = document.getElementById("cart-count");
  const cartTotalEl = document.getElementById("cart-total");

  document.querySelectorAll(".add-to-cart").forEach((button) => {
    button.addEventListener("click", () => {
      cartCount++;
      cartTotal += 25; // Valor fixo para simulação

      cartCountEl.textContent = cartCount;
      cartTotalEl.textContent = cartTotal.toFixed(2);
    });
  });
});

document.querySelectorAll(".add-to-cart").forEach((button) => {
  button.addEventListener("click", () => {
    document.getElementById("popup").style.display = "flex";
  });
});

document.getElementById("voltar").addEventListener("click", () => {
  document.getElementById("popup").style.display = "none";
});

// Lógica de + e - por ingrediente
document.querySelectorAll(".ingrediente").forEach((item) => {
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

// Lógica de scroll suave e banner dinâmico
const sections = document.querySelectorAll(".menu-section");
const categoryLinks = document.querySelectorAll(".category");
const bannerImage = document.getElementById("banner-image");

const bannerImages = {
  lanches: "/image/banners/lanchesBanner.png",
  combos: "/image/banners/combisBanner.png",
  sobremesas: "/image/banners/sobremesasBanner.png",
  bebidas: "/image/banners/bebidasBanner.png",
  extras: "/image/banners/extrasBanner.png",
  promocoes: "/image/banners/promocoesBanner.png",
};

const scrollOffset = 210;
function updateBanner(category) {
  if (bannerImages[category]) {
    bannerImage.src = bannerImages[category];
  }
}

categoryLinks.forEach((link) => {
  link.addEventListener("click", () => {
    const category = link.getAttribute("data-category");
    const section = document.getElementById(category);
    if (section) {
      section.scrollIntoView({ behavior: "smooth" });
      updateBanner(category);
    }
  });
});

window.addEventListener("scroll", () => {
  let current = "";

  sections.forEach((section) => {
    const sectionTop = section.offsetTop - scrollOffset;
    const sectionHeight = section.offsetHeight;
    if (pageYOffset >= sectionTop - sectionHeight / 3) {
      current = section.getAttribute("id");
    }
  });

  categoryLinks.forEach((link) => {
    link.classList.remove("active");
    if (link.getAttribute("data-category") === current) {
      link.classList.add("active");
    }
  });

  if (current) {
    updateBanner(current);
  }
});
