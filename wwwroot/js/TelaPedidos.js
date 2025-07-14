// JS extraído de Views/Pedidos/TelaPedidos.cshtml
// (Coloquei aqui todo o conteúdo do <script> do @section Scripts)

// Variáveis globais do carrinho
let carrinho = [];
let produtoSelecionado = null;
let quantidadeProduto = 1;
let ingredientesDisponiveis = [];

function adicionarProduto(produtoId, nome, descricao, valor, imagem, isCombo) {
    console.log('Adicionando produto:', {produtoId, nome, isCombo});
    if (parseInt(isCombo) === 1) {
        adicionarComboAoCarrinho(produtoId, nome, descricao, valor, imagem);
    } else {
        abrirModalPersonalizacao(produtoId, nome, descricao, valor, imagem, isCombo);
    }
}

document.addEventListener('DOMContentLoaded', function() {
    carregarCarrinho();
    atualizarResumoCarrinho();
});

function mostrarTodosProdutos() {
    document.querySelectorAll('.category, .cate').forEach(btn => btn.classList.remove('active'));
    event.target.classList.add('active');
    document.querySelectorAll('.produto-item').forEach(item => item.style.display = 'block');
    document.getElementById('titulo-produtos').textContent = 'Todos os Produtos';
    document.getElementById('banner-image').src = '/image/banners/lanchesBanner.png';
}

function filtrarPorCategoria(categoriaId, nomeCategoria, bannerUrl) {
    document.querySelectorAll('.category, .cate').forEach(btn => btn.classList.remove('active'));
    event.target.classList.add('active');
    document.querySelectorAll('.produto-item').forEach(item => {
        item.style.display = item.dataset.categoria == categoriaId ? 'block' : 'none';
    });
    document.getElementById('titulo-produtos').textContent = nomeCategoria;
    if (bannerUrl && bannerUrl !== '') {
        document.getElementById('banner-image').src = bannerUrl;
    }
}

async function abrirModalPersonalizacao(produtoId, nome, descricao, valor, imagem, isCombo) {
    try {
        console.log('Abrindo modal para produto:', produtoId, 'isCombo:', isCombo);
        if (isCombo === 1) {
            adicionarComboAoCarrinho(produtoId, nome, descricao, valor, imagem);
            return;
        }
        const response = await fetch(`/Pedidos/GetIngredientesProduto?produtoId=${produtoId}`);
        if (!response.ok) {
            console.log('Erro ao carregar ingredientes, adicionando produto sem personalização');
            adicionarProdutoSemPersonalizacao(produtoId, nome, descricao, valor, imagem);
            return;
        }
        ingredientesDisponiveis = await response.json();
        console.log('Ingredientes recebidos:', ingredientesDisponiveis);
        if (!ingredientesDisponiveis || ingredientesDisponiveis.length === 0) {
            console.log('Nenhum ingrediente encontrado, adicionando produto sem personalização');
            adicionarProdutoSemPersonalizacao(produtoId, nome, descricao, valor, imagem);
            return;
        }
        produtoSelecionado = {
            id: produtoId,
            nome: nome,
            descricao: descricao,
            valor: valor,
            imagem: imagem
        };
        quantidadeProduto = 1;
        document.getElementById('modal-produto-nome').textContent = nome;
        document.getElementById('modal-produto-descricao').textContent = descricao;
        if (imagem && imagem !== '') {
            document.getElementById('modal-produto-imagem').src = 'data:image/jpeg;base64,' + imagem;
        } else {
            document.getElementById('modal-produto-imagem').src = '/image/produtos/default.png';
        }
        construirInterfaceIngredientes();
        calcularPrecoTotal();
        document.getElementById('modal-personalizacao').style.display = 'flex';
    } catch (error) {
        console.error('Erro ao carregar ingredientes:', error);
        console.log('Erro na requisição, adicionando produto sem personalização');
        adicionarProdutoSemPersonalizacao(produtoId, nome, descricao, valor, imagem);
    }
}

function construirInterfaceIngredientes() {
    const container = document.getElementById('ingredientes-container');
    const ingredientesExistentes = container.querySelectorAll('.ingrediente:not(.quantidade-produto)');
    ingredientesExistentes.forEach(item => item.remove());
    ingredientesDisponiveis.forEach((ingrediente, index) => {
        const divIngrediente = document.createElement('div');
        divIngrediente.className = 'ingrediente';
        divIngrediente.dataset.ingredienteId = ingrediente.igredienteId;
        divIngrediente.dataset.quantidadeBase = ingrediente.quantidadeBase;
        divIngrediente.dataset.valorExtra = ingrediente.valorExtra || 0;
        divIngrediente.dataset.quantidadeMaxima = ingrediente.quantidadeMaxima || 10;
        divIngrediente.dataset.podeRemover = ingrediente.podeRemover || 'true';
        divIngrediente.dataset.podeAdicionar = ingrediente.podeAdicionar || 'true';
        let imagemHtml = '';
        if (ingrediente.imagem && ingrediente.imagem !== '') {
            imagemHtml = `<img src="data:image/jpeg;base64,${ingrediente.imagem}" alt="${ingrediente.nome}" class="ingrediente-img" style="width: 30px; height: 30px; border-radius: 50%; margin-right: 10px;">`;
        }
        divIngrediente.innerHTML = `
            <div style="display: flex; align-items: center; flex: 1;">
                ${imagemHtml}
                <span>${ingrediente.nome}</span>
            </div>
            <span class="quantidade">${ingrediente.quantidadeBase}x</span>
            <button class="menos" onclick="ajustarIngrediente(this, -1)">−</button>
            <button class="mais" onclick="ajustarIngrediente(this, 1)">+</button>
        `;
        container.appendChild(divIngrediente);
    });
}

function fecharModalPersonalizacao() {
    document.getElementById('modal-personalizacao').style.display = 'none';
    produtoSelecionado = null;
    quantidadeProduto = 1;
    ingredientesDisponiveis = [];
}

function ajustarQuantidade(delta) {
    quantidadeProduto = Math.max(1, quantidadeProduto + delta);
    document.querySelector('.quantidade-produto .quantidade').textContent = quantidadeProduto + 'x';
    calcularPrecoTotal();
}

function ajustarIngrediente(button, delta) {
    const ingredienteDiv = button.parentElement;
    const quantidadeSpan = ingredienteDiv.querySelector('.quantidade');
    const quantidadeBase = parseFloat(ingredienteDiv.dataset.quantidadeBase);
    const valorExtra = parseFloat(ingredienteDiv.dataset.valorExtra) || 0;
    const podeRemover = ingredienteDiv.dataset.podeRemover !== 'false';
    const podeAdicionar = ingredienteDiv.dataset.podeAdicionar !== 'false';
    const quantidadeMaxima = parseFloat(ingredienteDiv.dataset.quantidadeMaxima) || 10;
    let quantidadeAtual = parseFloat(quantidadeSpan.textContent.replace('x', ''));
    let novaQuantidade = quantidadeAtual + delta;
    const minimo = podeRemover ? 0 : quantidadeBase;
    const maximo = podeAdicionar ? quantidadeMaxima : quantidadeBase;
    novaQuantidade = Math.max(minimo, Math.min(maximo, novaQuantidade));
    quantidadeSpan.textContent = novaQuantidade + 'x';
    calcularPrecoTotal();
    const btnMenos = ingredienteDiv.querySelector('.menos');
    const btnMais = ingredienteDiv.querySelector('.mais');
    btnMenos.disabled = novaQuantidade <= minimo;
    btnMais.disabled = novaQuantidade >= maximo;
}

function calcularPrecoTotal() {
    let precoTotal = produtoSelecionado.valor;
    const ingredientes = document.querySelectorAll('.ingrediente:not(.quantidade-produto)');
    ingredientes.forEach(ingredienteDiv => {
        const quantidadeAtual = parseFloat(ingredienteDiv.querySelector('.quantidade').textContent.replace('x', ''));
        const quantidadeBase = parseFloat(ingredienteDiv.dataset.quantidadeBase);
        const valorExtra = parseFloat(ingredienteDiv.dataset.valorExtra) || 0;
        if (quantidadeAtual > quantidadeBase) {
            const quantidadeExtra = quantidadeAtual - quantidadeBase;
            precoTotal += quantidadeExtra * valorExtra;
        }
    });
    precoTotal *= quantidadeProduto;
    document.getElementById('preco-modal').textContent = precoTotal.toFixed(2).replace('.', ',');
}

function obterPersonalizacoes() {
    const personalizacoes = [];
    const ingredientes = document.querySelectorAll('.ingrediente:not(.quantidade-produto)');
    ingredientes.forEach(ingrediente => {
        const nome = ingrediente.querySelector('span').textContent;
        const quantidade = parseFloat(ingrediente.querySelector('.quantidade').textContent.replace('x', ''));
        const quantidadeBase = parseFloat(ingrediente.dataset.quantidadeBase);
        const valorExtra = parseFloat(ingrediente.dataset.valorExtra) || 0;
        const ingredienteId = parseInt(ingrediente.dataset.ingredienteId);
        let valorAdicional = 0;
        let valorUnitarioAdicional = 0;
        if (quantidade > quantidadeBase) {
            const quantidadeExtra = quantidade - quantidadeBase;
            valorAdicional = quantidadeExtra * valorExtra;
            valorUnitarioAdicional = valorExtra;
        }
        let tipoModificacao = 'normal';
        let isAdicional = false;
        if (quantidade > quantidadeBase) {
            tipoModificacao = 'adicional';
            isAdicional = true;
        } else if (quantidade < quantidadeBase) {
            tipoModificacao = 'removido';
        }
        personalizacoes.push({
            ingredienteId: ingredienteId,
            nome: nome,
            quantidade: quantidade,
            quantidadeBase: quantidadeBase,
            valorExtra: valorExtra,
            valor: valorExtra,
            valorAdicional: valorAdicional,
            valorUnitarioAdicional: valorUnitarioAdicional,
            modificado: quantidade !== quantidadeBase,
            tipoModificacao: tipoModificacao,
            isAdicional: isAdicional
        });
    });
    return personalizacoes;
}

function calcularValorTotalAdicionais(personalizacoes) {
    return personalizacoes.reduce((total, p) => {
        return total + (p.valorAdicional || 0);
    }, 0);
}

function adicionarAoCarrinho() {
    if (!produtoSelecionado) return;
    const personalizacoes = obterPersonalizacoes();
    const valorTotalAdicionais = calcularValorTotalAdicionais(personalizacoes);
    const valorUnitarioComAdicionais = produtoSelecionado.valor + valorTotalAdicionais;
    const itemCarrinho = {
        id: produtoSelecionado.id,
        nome: produtoSelecionado.nome,
        descricao: produtoSelecionado.descricao,
        valor: produtoSelecionado.valor,
        valorUnitario: valorUnitarioComAdicionais,
        valorAdicionais: valorTotalAdicionais,
        valorUnitarioAdicionais: valorTotalAdicionais,
        quantidade: quantidadeProduto,
        imagem: produtoSelecionado.imagem,
        personalizacoes: personalizacoes
    };
    itemCarrinho.valorTotal = valorUnitarioComAdicionais * quantidadeProduto;
    const itemExistente = carrinho.find(item => 
        item.id === itemCarrinho.id && 
        JSON.stringify(item.personalizacoes) === JSON.stringify(itemCarrinho.personalizacoes)
    );
    if (itemExistente) {
        itemExistente.quantidade += itemCarrinho.quantidade;
        itemExistente.valorTotal = itemExistente.valorUnitario * itemExistente.quantidade;
    } else {
        carrinho.push(itemCarrinho);
    }
    salvarCarrinho();
    atualizarResumoCarrinho();
    fecharModalPersonalizacao();
    mostrarNotificacao('Produto adicionado ao carrinho!');
}

function gerarTextoPersonalizacoes(personalizacoes) {
    const personalizacoesModificadas = personalizacoes.filter(p => p.modificado);
    if (personalizacoesModificadas.length === 0) {
        return '';
    }
    let personalizacoesText = '<ul class="personalizacoes">';
    personalizacoesModificadas.forEach(p => {
        if (p.tipoModificacao === 'adicional') {
            const quantidadeExtra = p.quantidade - p.quantidadeBase;
            const valorExtraTotal = p.valorAdicional;
            const textoValor = valorExtraTotal > 0 ? ` (+R$ ${valorExtraTotal.toFixed(2).replace('.', ',')})` : '';
            personalizacoesText += `<li>Extra ${p.nome} (${quantidadeExtra}x)${textoValor}</li>`;
        } else if (p.tipoModificacao === 'removido') {
            const quantidadeRemovida = p.quantidadeBase - p.quantidade;
            personalizacoesText += `<li>Sem ${p.nome} (${quantidadeRemovida}x removido)</li>`;
        }
    });
    personalizacoesText += '</ul>';
    return personalizacoesText;
}

function atualizarModalCarrinho() {
    const container = document.getElementById('carrinho-detalhes-container');
    const totalModal = document.getElementById('total-modal-carrinho');
    container.innerHTML = '';
    if (carrinho.length === 0) {
        container.innerHTML = '<p>Carrinho vazio</p>';
        totalModal.textContent = '0,00';
        return;
    }
    let totalGeral = 0;
    carrinho.forEach((item, index) => {
        const itemDiv = document.createElement('div');
        itemDiv.className = 'carrinho-item';
        const personalizacoesText = gerarTextoPersonalizacoes(item.personalizacoes);
        const valorBase = item.valor;
        const valorAdicionais = item.valorAdicionais || 0;
        const valorUnitario = item.valorUnitario || item.valor;
        const valorTotal = item.valorTotal;
        let detalhesValor = `
            <div class="item-valores">
                <div class="valor-base">Base: R$ ${valorBase.toFixed(2).replace('.', ',')}</div>
        `;
        if (valorAdicionais > 0) {
            detalhesValor += `<div class="valor-adicionais">Adicionais: R$ ${valorAdicionais.toFixed(2).replace('.', ',')}</div>`;
        }
        detalhesValor += `
                <div class="valor-unitario">Unitário: R$ ${valorUnitario.toFixed(2).replace('.', ',')}</div>
            </div>
        `;
        itemDiv.innerHTML = `
            <div class="item-header">
                <h4>${item.nome}</h4>
                <div class="item-precos">
                    <span class="item-valor-total">R$ ${valorTotal.toFixed(2).replace('.', ',')}</span>
                </div>
            </div>
            <div class="item-details">
                <p>${item.descricao}</p>
                ${detalhesValor}
                ${personalizacoesText}
            </div>
            <div class="item-controls">
                <button class="btn-menos" onclick="atualizarQuantidadeCarrinho(${index}, ${item.quantidade - 1})">−</button>
                <span class="item-quantidade">${item.quantidade}x</span>
                <button class="btn-mais" onclick="atualizarQuantidadeCarrinho(${index}, ${item.quantidade + 1})">+</button>
                <button class="btn-remover" onclick="removerDoCarrinho(${index})">Excluir</button>
            </div>
        `;
        container.appendChild(itemDiv);
        totalGeral += item.valorTotal;
    });
    totalModal.textContent = totalGeral.toFixed(2).replace('.', ',');
}

function calcularValorItemCarrinho(item) {
    if (item.valorUnitario) {
        return item.valorUnitario * item.quantidade;
    }
    let valorUnitario = item.valor;
    item.personalizacoes.forEach(personalizacao => {
        if (personalizacao.quantidade > personalizacao.quantidadeBase) {
            valorUnitario += (personalizacao.quantidade - personalizacao.quantidadeBase) * personalizacao.valorExtra;
        }
    });
    return valorUnitario * item.quantidade;
}

function removerDoCarrinho(index) {
    carrinho.splice(index, 1);
    salvarCarrinho();
    atualizarResumoCarrinho();
    atualizarModalCarrinho();
}

function atualizarQuantidadeCarrinho(index, novaQuantidade) {
    if (novaQuantidade <= 0) {
        removerDoCarrinho(index);
    } else {
        carrinho[index].quantidade = novaQuantidade;
        const valorUnitario = carrinho[index].valorUnitario || carrinho[index].valor;
        carrinho[index].valorTotal = valorUnitario * novaQuantidade;
        salvarCarrinho();
        atualizarResumoCarrinho();
        atualizarModalCarrinho();
    }
}

function atualizarResumoCarrinho() {
    const resumoDiv = document.getElementById('resumo-carrinho');
    const resumoVazio = document.getElementById('resumo-vazio');
    const btnEditar = document.getElementById('btn-editar');
    const btnPagar = document.getElementById('btn-pagar');
    const btnCancelar = document.getElementById('btn-cancelar');
    const totalPagar = document.getElementById('total-pagar');
    if (carrinho.length === 0) {
        resumoVazio.style.display = 'block';
        btnEditar.style.display = 'none';
        btnPagar.style.display = 'none';
        btnCancelar.style.display = 'none';
        const itensExistentes = resumoDiv.querySelectorAll('p:not(#resumo-vazio):not(.total)');
        itensExistentes.forEach(item => item.remove());
        let totalExistente = resumoDiv.querySelector('.total');
        if (!totalExistente) {
            const pTotal = document.createElement('p');
            pTotal.className = 'total';
            pTotal.innerHTML = `Total: <span>R$ 0,00</span>`;
            resumoDiv.appendChild(pTotal);
        } else {
            totalExistente.innerHTML = `Total: <span>R$ 0,00</span>`;
        }
        totalPagar.textContent = '0,00';
        return;
    }
    resumoVazio.style.display = 'none';
    btnEditar.style.display = 'inline-block';
    btnPagar.style.display = 'inline-block';
    btnCancelar.style.display = 'inline-block';
    const itensExistentes = resumoDiv.querySelectorAll('p:not(#resumo-vazio):not(.total)');
    itensExistentes.forEach(item => item.remove());
    let totalGeral = 0;
    let totalItens = 0;
    carrinho.forEach(item => {
        totalGeral += item.valorTotal;
        totalItens += item.quantidade;
    });
    const pResumo = document.createElement('p');
    pResumo.innerHTML = `${totalItens} ${totalItens === 1 ? 'item' : 'itens'} <span>R$ ${totalGeral.toFixed(2).replace('.', ',')}</span>`;
    resumoDiv.appendChild(pResumo);
    let totalExistente = resumoDiv.querySelector('.total');
    if (!totalExistente) {
        const pTotal = document.createElement('p');
        pTotal.className = 'total';
        pTotal.innerHTML = `Total: <span>R$ ${totalGeral.toFixed(2).replace('.', ',')}</span>`;
        resumoDiv.appendChild(pTotal);
    } else {
        totalExistente.innerHTML = `Total: <span>R$ ${totalGeral.toFixed(2).replace('.', ',')}</span>`;
    }
    totalPagar.textContent = totalGeral.toFixed(2).replace('.', ',');
}

function abrirModalCarrinho() {
    atualizarModalCarrinho();
    document.getElementById('modal-carrinho').style.display = 'flex';
}

function fecharModalCarrinho() {
    document.getElementById('modal-carrinho').style.display = 'none';
}

function salvarCarrinho() {
    localStorage.setItem('carrinho-totem', JSON.stringify(carrinho));
}

function carregarCarrinho() {
    const carrinhoSalvo = localStorage.getItem('carrinho-totem');
    if (carrinhoSalvo) {
        carrinho = JSON.parse(carrinhoSalvo);
    }
}

function editarPedido() {
    abrirModalCarrinho();
}

function finalizarPedido() {
    if (carrinho.length === 0) {
        alert('Carrinho vazio!');
        return;
    }
    localStorage.setItem('pedido-finalizar', JSON.stringify(carrinho));
    window.location.href = '/Home/telaItensPedidos';
}

function cancelarPedido() {
    if (confirm('Tem certeza que deseja cancelar o pedido?')) {
        carrinho = [];
        localStorage.removeItem('pedido-finalizar');
        salvarCarrinho();
        atualizarResumoCarrinho();
        fecharModalCarrinho();
    }
}

function mostrarNotificacao(mensagem) {
    const notificacao = document.createElement('div');
    notificacao.style.cssText = `
        position: fixed;
        top: 20px;
        right: 20px;
        background: #4CAF50;
        color: white;
        padding: 15px 20px;
        border-radius: 5px;
        z-index: 9999;
        box-shadow: 0 2px 10px rgba(0,0,0,0.2);
        animation: slideIn 0.3s ease-out;
    `;
    notificacao.textContent = mensagem;
    document.body.appendChild(notificacao);
    setTimeout(() => {
        notificacao.style.animation = 'slideOut 0.3s ease-in';
        setTimeout(() => {
            if (notificacao.parentNode) {
                notificacao.parentNode.removeChild(notificacao);
            }
        }, 300);
    }, 3000);
}

document.getElementById('modal-personalizacao').addEventListener('click', function(e) {
    if (e.target === this) {
        fecharModalPersonalizacao();
    }
});
document.getElementById('modal-carrinho').addEventListener('click', function(e) {
    if (e.target === this) {
        fecharModalCarrinho();
    }
});

function adicionarProdutoSemPersonalizacao(produtoId, nome, descricao, valor, imagem) {
    const itemCarrinho = {
        id: produtoId,
        nome: nome,
        descricao: descricao,
        valor: valor,
        valorUnitario: valor,
        valorAdicionais: 0,
        quantidade: 1,
        imagem: imagem,
        personalizacoes: []
    };
    itemCarrinho.valorTotal = valor;
    const itemExistente = carrinho.find(item => 
        item.id === itemCarrinho.id && 
        item.personalizacoes.length === 0
    );
    if (itemExistente) {
        itemExistente.quantidade += 1;
        itemExistente.valorTotal = itemExistente.valorUnitario * itemExistente.quantidade;
    } else {
        carrinho.push(itemCarrinho);
    }
    salvarCarrinho();
    atualizarResumoCarrinho();
    mostrarNotificacao('Produto adicionado ao carrinho!');
}

function adicionarComboAoCarrinho(produtoId, nome, descricao, valor, imagem) {
    const itemCarrinho = {
        id: produtoId,
        nome: nome,
        descricao: descricao,
        valor: valor,
        valorUnitario: valor,
        valorAdicionais: 0,
        quantidade: 1,
        imagem: imagem,
        personalizacoes: [],
        isCombo: true
    };
    itemCarrinho.valorTotal = valor;
    const itemExistente = carrinho.find(item => 
        item.id === itemCarrinho.id && 
        item.isCombo === true
    );
    if (itemExistente) {
        itemExistente.quantidade += 1;
        itemExistente.valorTotal = itemExistente.valorUnitario * itemExistente.quantidade;
    } else {
        carrinho.push(itemCarrinho);
    }
    salvarCarrinho();
    atualizarResumoCarrinho();
    mostrarNotificacao('Combo adicionado ao carrinho!');
}
