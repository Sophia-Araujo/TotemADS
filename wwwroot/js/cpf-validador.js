$(document).ready(function () {
    // Aplica máscara ao campo CPF
    $('#cpf').mask('000.000.000-00');

    // Intercepta o submit do formulário
    $('#formCpf').on('submit', function (e) {
        e.preventDefault(); // Sempre previne o envio padrão
        
        const valido = validarCPF();
        if (valido) {
            // Se válido, redireciona para telaEscolha.cshtml
            setTimeout(() => {
                // Tente essas opções uma por vez (comente as outras):
                
                window.location.href = '/Home/telaTipoPedido';     // Opção 1
                // window.location.href = '/Home/TelaEscolha';  // Opção 2 - PascalCase
                // window.location.href = '/telaEscolha';       // Opção 3 - Só a action
                // window.location.href = '/TelaEscolha';       // Opção 4 - Action PascalCase
                
                // Se nenhuma funcionar, adicione o console.log abaixo para debug:
                // console.log('Tentando redirecionar para:', '/Home/telaEscolha');
            }, 2000);
        }
    });

    // Valida em tempo real quando o checkbox muda
    $('#nota').on('change', function() {
        if ($(this).is(':checked')) {
            const cpfValue = $('#cpf').val().replace(/[^\d]+/g, '');
            if (cpfValue.length > 0) {
                validarCPF();
            }
        } else {
            // Limpa mensagens quando desmarca o checkbox
            $('#mensagem').fadeOut();
        }
    });

    // Valida quando termina de digitar (opcional)
    $('#cpf').on('blur', function() {
        if ($('#nota').is(':checked')) {
            validarCPF();
        }
    });
});

function mostrarMensagem(texto, tipo = "erro") {
    const $msg = $('#mensagem');
    $msg.removeClass("erro ok").addClass(tipo).text(texto).fadeIn();

    // Remove listeners anteriores para evitar duplicação
    $(document).off('click.mensagem');
    $(document).off('keydown.mensagem');

    // Adiciona listener para clique fora da mensagem
    $(document).on('click.mensagem', function(e) {
        // Se clicou fora da mensagem e ela está visível
        if (!$(e.target).closest('#mensagem').length && $msg.is(':visible')) {
            $msg.fadeOut();
            $(document).off('click.mensagem keydown.mensagem'); // Remove os listeners
        }
    });

    // Adiciona listener para tecla ESC
    $(document).on('keydown.mensagem', function(e) {
        if (e.key === 'Escape' && $msg.is(':visible')) {
            $msg.fadeOut();
            $(document).off('click.mensagem keydown.mensagem'); // Remove os listeners
        }
    });

    // Auto-hide apenas para mensagens de erro após 8 segundos
    if (tipo === "erro") {
        setTimeout(() => {
            if ($msg.is(':visible')) {
                $msg.fadeOut();
                $(document).off('click.mensagem keydown.mensagem'); // Remove os listeners
            }
        }, 8000);
    }
}

function validarCPF() {
    const cpfNaNota = $('#nota').is(':checked');
    const cpfValue = $('#cpf').val().replace(/[^\d]+/g, '');

    // Se não marcou "CPF na nota"
    if (!cpfNaNota) {
        // Se não tem CPF na nota, pode prosseguir independente do campo CPF
        if (cpfValue.length === 0) {
            mostrarMensagem('Prosseguindo sem CPF na nota...', 'ok');
            return true;
        }
        // Se preencheu CPF mas não marcou a opção, valida mesmo assim para dar feedback
        if (cpfValue.length > 0) {
            const cpfValido = validarAlgoritmoCPF(cpfValue);
            if (!cpfValido) {
                mostrarMensagem('CPF inválido! Marque a opção "CPF na nota" se desejar incluir um CPF válido.');
                return false;
            } else {
                mostrarMensagem('CPF válido! Marque a opção "CPF na nota" se desejar incluí-lo.', 'ok');
                return true;
            }
        }
        return true;
    }

    // Se marcou "CPF na nota", o campo se torna obrigatório
    if (cpfValue.length === 0) {
        mostrarMensagem('A opção "CPF na nota" está marcada. Por favor, preencha o campo CPF.');
        return false;
    }

    // Valida o CPF
    const cpfValido = validarAlgoritmoCPF(cpfValue);
    if (!cpfValido) {
        return false;
    }

    mostrarMensagem('CPF válido! Prosseguindo...', 'ok');
    return true;
}

function validarAlgoritmoCPF(cpf) {
    // Remove caracteres não numéricos
    cpf = cpf.replace(/[^\d]+/g, '');

    // Verifica se tem 11 dígitos
    if (cpf.length !== 11) {
        mostrarMensagem('CPF deve ter 11 dígitos! Verifique o número digitado.');
        return false;
    }

    // Verifica se todos os dígitos são iguais
    if (/^(\d)\1+$/.test(cpf)) {
        mostrarMensagem('CPF inválido! Todos os dígitos são iguais.');
        return false;
    }

    // Validação do primeiro dígito verificador
    let soma = 0;
    for (let i = 0; i < 9; i++) {
        soma += parseInt(cpf.charAt(i)) * (10 - i);
    }
    let resto = (soma * 10) % 11;
    if (resto === 10 || resto === 11) resto = 0;
    if (resto !== parseInt(cpf.charAt(9))) {
        mostrarMensagem('CPF inválido! Primeiro dígito verificador incorreto.');
        return false;
    }

    // Validação do segundo dígito verificador
    soma = 0;
    for (let i = 0; i < 10; i++) {
        soma += parseInt(cpf.charAt(i)) * (11 - i);
    }
    resto = (soma * 10) % 11;
    if (resto === 10 || resto === 11) resto = 0;
    if (resto !== parseInt(cpf.charAt(10))) {
        mostrarMensagem('CPF inválido! Segundo dígito verificador incorreto.');
        return false;
    }

    return true;
}