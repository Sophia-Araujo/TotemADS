$(document).ready(function () {
        $('#cpf').mask('000.000.000-00');
    });

    function validarCPF() {
        const cpfNaNota = $('#nota').is(':checked');

        if (!cpfNaNota) {
            return true;
        }

        let cpf = $('#cpf').val().replace(/[^\d]+/g, '');

        if (cpf.length === 0) {
            alert('A opção "CPF na nota" está marcada. Por favor, preencha o campo CPF.');
            return false;
        }

        if (cpf.length !== 11 || /^(\d)\1+$/.test(cpf)) {
            alert('CPF inválido! Verifique o número digitado.');
            return false;
        }

        let soma = 0;
        let resto;
        for (let i = 0; i < 9; i++) {
            soma += parseInt(cpf.charAt(i)) * (10 - i);
        }
        resto = (soma * 10) % 11;
        if (resto === 10 || resto === 11) {
            resto = 0;
        }
        if (resto !== parseInt(cpf.charAt(9))) {
            alert('CPF inválido! Verifique o número digitado.');
            return false;
        }

        soma = 0;
        for (let i = 0; i < 10; i++) {
            soma += parseInt(cpf.charAt(i)) * (11 - i);
        }
        resto = (soma * 10) % 11;
        if (resto === 10 || resto === 11) {
            resto = 0;
        }
        if (resto !== parseInt(cpf.charAt(10))) {
            alert('CPF inválido! Verifique o número digitado.');
            return false;
        }

        return true;
    }