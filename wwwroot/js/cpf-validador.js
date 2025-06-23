$(document).ready(function () {
    $('#cpf').mask('000.000.000-00');
    $('#formCpf').on('submit', function (e) {
        e.preventDefault(); 
        const valido = validarCPF();
        if (valido) {
            setTimeout(() => {
                window.location.href = '/Home/telaTipoPedido';   
            }, 2000);
        }
    });
    $('#nota').on('change', function() {
        if ($(this).is(':checked')) {
            const cpfValue = $('#cpf').val().replace(/[^\d]+/g, '');
            if (cpfValue.length > 0) {
                validarCPF();
            }
        } else {
            $('#mensagem').fadeOut();
        }
    });
    $('#cpf').on('blur', function() {
        if ($('#nota').is(':checked')) {
            validarCPF();
        }
    });
});

function mostrarMensagem(texto, tipo = "erro") {
    const $msg = $('#mensagem');
    $msg.removeClass("erro ok").addClass(tipo).text(texto).fadeIn();
    $(document).off('click.mensagem');
    $(document).off('keydown.mensagem');
    $(document).on('click.mensagem', function(e) {
        if (!$(e.target).closest('#mensagem').length && $msg.is(':visible')) {
            $msg.fadeOut();
            $(document).off('click.mensagem keydown.mensagem');
        }
    });
    $(document).on('keydown.mensagem', function(e) {
        if (e.key === 'Escape' && $msg.is(':visible')) {
            $msg.fadeOut();
            $(document).off('click.mensagem keydown.mensagem'); 
        }
    });
    if (tipo === "erro") {
        setTimeout(() => {
            if ($msg.is(':visible')) {
                $msg.fadeOut();
                $(document).off('click.mensagem keydown.mensagem');
            }
        }, 8000);
    }
}
function validarCPF() {
    const cpfNaNota = $('#nota').is(':checked');
    const cpfValue = $('#cpf').val().replace(/[^\d]+/g, '');
    if (!cpfNaNota) {
        if (cpfValue.length === 0) {
            mostrarMensagem('Prosseguindo sem CPF na nota...', 'ok');
            return true;
        }
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
    if (cpfValue.length === 0) {
        mostrarMensagem('A opção "CPF na nota" está marcada. Por favor, preencha o campo CPF.');
        return false;
    }
    const cpfValido = validarAlgoritmoCPF(cpfValue);
    if (!cpfValido) {
        return false;
    }
    mostrarMensagem('CPF válido! Prosseguindo...', 'ok');
    return true;
}
function validarAlgoritmoCPF(cpf) {
    cpf = cpf.replace(/[^\d]+/g, '');
    if (cpf.length !== 11) {
        mostrarMensagem('CPF deve ter 11 dígitos! Verifique o número digitado.');
        return false;
    }
    if (/^(\d)\1+$/.test(cpf)) {
        mostrarMensagem('CPF inválido! Todos os dígitos são iguais.');
        return false;
    }
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