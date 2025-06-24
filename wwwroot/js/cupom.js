// Funcionalidade do Modal
document.addEventListener('DOMContentLoaded', function() {
    const btnCupom = document.getElementById('btnCupom');
    const modalOverlay = document.getElementById('modalOverlay');
    const closeModal = document.getElementById('closeModal');
    const input = document.getElementById('input');

    // Abrir modal
    btnCupom.addEventListener('click', function() {
        modalOverlay.classList.add('active');
        document.body.style.overflow = 'hidden'; // Previne scroll do body
        createKeyboard(); // Inicializa o teclado
    });

    // Fechar modal
    closeModal.addEventListener('click', function() {
        modalOverlay.classList.remove('active');
        document.body.style.overflow = 'auto'; // Restaura scroll do body
        input.value = ''; // Limpa o input
        isShiftActive = false; // Reset shift
    });

    // Fechar modal clicando no overlay
    modalOverlay.addEventListener('click', function(e) {
        if (e.target === modalOverlay) {
            modalOverlay.classList.remove('active');
            document.body.style.overflow = 'auto';
            input.value = '';
            isShiftActive = false;
        }
    });

    // Teclado Virtual - Layout das teclas
    const keyboardLayouts = {
        normal: [
            ['1', '2', '3', '4', '5', '6', '7', '8', '9', '0'],
            ['Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'I', 'O', 'P'],
            ['A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L'],
            ['Z', 'X', 'C', 'V', 'B', 'N', 'M', '⌫'],
            ['ESPAÇO']
        ]
    };

    let isShiftActive = false;

    function createKeyboard() {
        const rows = [
            document.getElementById('row1'),
            document.getElementById('row2'),
            document.getElementById('row3'),
            document.getElementById('row4'),
            document.getElementById('row5')
        ];

        const layout = isShiftActive ? keyboardLayouts.shift : keyboardLayouts.normal;

        rows.forEach((row, rowIndex) => {
            row.innerHTML = '';
            layout[rowIndex].forEach(key => {
                const button = document.createElement('button');
                button.textContent = key;
                button.className = 'key';

                if (key === 'SHIFT') {
                    button.className += ' shift';
                    if (isShiftActive) button.className += ' active';
                } else if (key === '⌫') {
                    button.className += ' delete';
                } else if (key === 'ESPAÇO') {
                    button.className += ' space';
                    button.textContent = 'ESPAÇO';
                }

                button.addEventListener('click', () => handleKeyPress(key));
                row.appendChild(button);
            });
        });
    }

    function handleKeyPress(key) {
        const currentValue = input.value;

        if (key === 'SHIFT') {
            isShiftActive = !isShiftActive;
            createKeyboard();
        } else if (key === '⌫') {
            input.value = currentValue.slice(0, -1);
        } else if (key === 'ESPAÇO') {
            input.value = currentValue + ' ';
        } else {
            input.value = currentValue + key;
            if (isShiftActive && key !== 'SHIFT') {
                isShiftActive = false;
                createKeyboard();
            }
        }
    }

    // Funcionalidade do botão Confirmar
    document.querySelector('.confirmar').addEventListener('click', function() {
        const cupomValue = input.value.trim();
        if (cupomValue) {
            // Aqui você pode fazer uma requisição AJAX para validar o cupom
            // Por enquanto, apenas mostra um alerta
            alert(`Cupom "${cupomValue}" aplicado!`);
            modalOverlay.classList.remove('active');
            document.body.style.overflow = 'auto';
            input.value = '';
            isShiftActive = false;
            
            // Aqui você pode atualizar os valores do pedido se o cupom for válido
            // aplicarCupom(cupomValue);
        } else {
            alert('Por favor, digite um cupom válido.');
        }
    });

    // Função para aplicar o cupom (opcional - para integração futura)
    function aplicarCupom(cupom) {
        // Aqui você faria uma requisição para seu backend
        // fetch('/Home/AplicarCupom', {
        //     method: 'POST',
        //     headers: {
        //         'Content-Type': 'application/json',
        //     },
        //     body: JSON.stringify({ cupom: cupom })
        // })
        // .then(response => response.json())
        // .then(data => {
        //     if (data.sucesso) {
        //         // Atualizar os valores na tela
        //         atualizarValoresPedido(data.novoTotal, data.desconto);
        //     } else {
        //         alert('Cupom inválido ou expirado.');
        //     }
        // });
    }

    // Função para atualizar os valores do pedido (opcional)
    function atualizarValoresPedido(novoTotal, desconto) {
        // Atualizar os elementos da página com os novos valores
        // document.querySelector('.total span:last-child').textContent = `R$: ${novoTotal.toFixed(2)}`;
        // Se houver desconto, adicionar uma linha mostrando o desconto
    }
});