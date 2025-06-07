const options = document.querySelectorAll('.option');
const confirmBtn = document.getElementById('confirm-btn');
const errorMsg = document.getElementById('error-msg');
let selectedOption = null;

options.forEach(option => {
  option.addEventListener('click', () => {
    // Remove seleção anterior
    options.forEach(o => o.classList.remove('selected'));

    // Marca a nova seleção
    option.classList.add('selected');
    selectedOption = option.id;

    // Esconde a mensagem de erro se já estiver visível
    errorMsg.style.display = 'none';
  });
});

confirmBtn.addEventListener('click', () => {
  if (!selectedOption) {
    errorMsg.style.display = 'block';
  } else {
    alert("Você escolheu: " + selectedOption);
    // Exemplo: window.location.href = "proxima-pagina.html?opcao=" + selectedOption;
  }
});
