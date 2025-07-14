// JS extraído de Views/Home/telaItensPedidos.cshtml (do bloco <script> do @section Scripts)
let carrinhoPedido = [];
let totalPedido = 0;
let cupomAplicado = null;

document.addEventListener('DOMContentLoaded', function() {
    carregarItensPedido();
});

function carregarItensPedido() {
    const pedidoSalvo = localStorage.getItem('pedido-finalizar');
    const loadingElement = document.getElementById('loading-itens');
    const carrinhoVazioElement = document.getElementById('carrinho-vazio');
    const listaItensElement = document.getElementById('lista-itens-pedido');
    if (!pedidoSalvo) {
        loadingElement.style.display = 'none';
        carrinhoVazioElement.style.display = 'block';
        return;
    }
    try {
        carrinhoPedido = JSON.parse(pedidoSalvo);
        if (carrinhoPedido.length === 0) {
            loadingElement.style.display = 'none';
            carrinhoVazioElement.style.display = 'block';
            return;
        }
        loadingElement.style.display = 'none';
        renderizarItensPedido();
        calcularResumo();
    } catch (error) {
        console.error('Erro ao carregar pedido:', error);
        loadingElement.style.display = 'none';
        carrinhoVazioElement.style.display = 'block';
    }
}

function calcularValorIndividualItem(item) {
    let valorBase = parseFloat(item.valor) || 0;
    let valorAdicionais = 0;
    if (item.personalizacoes && Array.isArray(item.personalizacoes) && item.personalizacoes.length > 0) {
        item.personalizacoes.forEach(personalizacao => {
            const quantidadeBase = parseFloat(personalizacao.quantidadeBase) || 0;
            const quantidadeAtual = parseFloat(personalizacao.quantidade) || 0;
            const valorPersonalizacao = parseFloat(
                personalizacao.valor !== undefined ? personalizacao.valor :
                personalizacao.valorExtra !== undefined ? personalizacao.valorExtra :
                personalizacao.value || 
                personalizacao.preco || 
                personalizacao.price || 
                0
            );
            if ((personalizacao.isAdicional === true || personalizacao.tipoModificacao === 'adicional') && quantidadeAtual > quantidadeBase) {
                const quantidadeExtra = quantidadeAtual - quantidadeBase;
                valorAdicionais += valorPersonalizacao * quantidadeExtra;
            }
        });
    }
    return valorBase + valorAdicionais;
}

function calcularValorTotalItem(item) {
    const valorIndividual = calcularValorIndividualItem(item);
    const quantidade = parseInt(item.quantidade) || 1;
    return valorIndividual * quantidade;
}

function renderizarItensPedido() {
    const listaItensElement = document.getElementById('lista-itens-pedido');
    listaItensElement.innerHTML = '';
    carrinhoPedido.forEach((item, index) => {
        const itemElement = criarElementoItem(item, index);
        // Corrige o onclick para sempre usar o índice atual
        const btnRemover = itemElement.querySelector('.btn-remover-item');
        if (btnRemover) {
            btnRemover.onclick = function() { removerItem(index); };
        }
        listaItensElement.appendChild(itemElement);
    });
}

function criarElementoItem(item, index) {
    const divContainer = document.createElement('div');
    divContainer.className = 'VisualizaItensPedidos';
    const valorBase = parseFloat(item.valor) || 0;
    const valorIndividual = calcularValorIndividualItem(item);
    const valorTotal = calcularValorTotalItem(item);
    const valorAdicionais = valorIndividual - valorBase;
    let ingredientesHtml = '';
    if (item.personalizacoes && Array.isArray(item.personalizacoes) && item.personalizacoes.length > 0) {
        item.personalizacoes.forEach(personalizacao => {
            const quantidadeBase = parseFloat(personalizacao.quantidadeBase) || 0;
            const quantidadeAtual = parseFloat(personalizacao.quantidade) || 0;
            const valorPersonalizacao = parseFloat(personalizacao.valor) || 0;
            if ((personalizacao.isAdicional === true || personalizacao.tipoModificacao === 'adicional') && quantidadeAtual > quantidadeBase) {
                const quantidadeExtra = quantidadeAtual - quantidadeBase;
                ingredientesHtml += `<li>${personalizacao.nome} <span>+R$ ${(valorPersonalizacao * quantidadeExtra).toFixed(2).replace('.', ',')} (${quantidadeExtra}x)</span></li>`;
            }
        });
    }
    const imagemSrc = item.imagem && item.imagem !== '' 
        ? `data:image/jpeg;base64,${item.imagem}`
        : '/image/produtos/default.png';
    let detalhamentoPreco = `<div class="preco-detalhado">
        <span class="precoIndividual">Valor unitário: <b>R$ ${valorIndividual.toFixed(2).replace('.', ',')}</b></span>`;
    if (valorAdicionais > 0) {
        detalhamentoPreco += `<span class="precoAdicionais"> | Adicionais: <b>R$ ${valorAdicionais.toFixed(2).replace('.', ',')}</b></span>`;
    }
    detalhamentoPreco += `<span class="precoPedido"> | Total: <b>R$ ${valorTotal.toFixed(2).replace('.', ',')}</b></span>
    </div>`;
    divContainer.innerHTML = `
        <div class="card-pedido">
            <div class="card-conteudo">
                <div class="info-lanche">
                    <img src="${imagemSrc}" alt="${item.nome}" />
                    <h2 class="nomePedido">${item.nome}</h2>
                    ${detalhamentoPreco}
                    <p class="descricaoPedido">${item.descricao || ''}</p>
                    <div class="quantidade-item">
                        <span class="quantidade-texto">Quantidade: ${item.quantidade}</span>
                        <button class="btn-remover-item" type="button" onclick="removerItem(${index})" title="Remover item do pedido">
                            <div class="btn-icon-container">
                                <i class="fas fa-trash-alt"></i>
                            </div>
                            <span class="btn-text">Remover Item</span>
                            <div class="btn-ripple"></div>
                        </button>
                    </div>
                </div>
                <div class="ingredientesPedido">
                    <ul class="ingredientesPedidoLista">
                        ${ingredientesHtml}
                    </ul>
                </div>
            </div>
        </div>
    `;
    return divContainer;
}

function calcularResumo() {
    const totalListaElement = document.getElementById('total-lista');
    const categorias = {};
    totalPedido = 0;
    carrinhoPedido.forEach(item => {
        const categoria = 'Lanches';
        if (!categorias[categoria]) {
            categorias[categoria] = { quantidade: 0, valor: 0 };
        }
        const valorTotalItem = calcularValorTotalItem(item);
        const quantidadeItem = parseInt(item.quantidade) || 1;
        categorias[categoria].quantidade += quantidadeItem;
        categorias[categoria].valor += valorTotalItem;
        totalPedido += valorTotalItem;
    });
    totalListaElement.innerHTML = '';
    Object.keys(categorias).forEach(categoria => {
        const cat = categorias[categoria];
        const divItem = document.createElement('div');
        divItem.className = 'total-item';
        divItem.innerHTML = `
            <span>${categoria} ${cat.quantidade}x</span>
            <span>R$ ${cat.valor.toFixed(2).replace('.', ',')}</span>
        `;
        totalListaElement.appendChild(divItem);
    });
    let valorFinal = totalPedido;
    if (cupomAplicado) {
        const desconto = totalPedido * (cupomAplicado.desconto / 100);
        valorFinal = totalPedido - desconto;
        const divDesconto = document.createElement('div');
        divDesconto.className = 'total-item desconto';
        divDesconto.innerHTML = `
            <span>Desconto (${cupomAplicado.codigo})</span>
            <span>-R$ ${desconto.toFixed(2).replace('.', ',')}</span>
        `;
        totalListaElement.appendChild(divDesconto);
    }
    const divTotal = document.createElement('div');
    divTotal.className = 'total-item total';
    divTotal.innerHTML = `
        <span>Total:</span>
        <span>R$ ${valorFinal.toFixed(2).replace('.', ',')}</span>
    `;
    totalListaElement.appendChild(divTotal);
}

function removerItem(index) {
    const item = carrinhoPedido[index];
    const valorTotalItem = calcularValorTotalItem(item);
    const confirmar = confirm(`Deseja remover "${item.nome}" do pedido?\n\nQuantidade: ${item.quantidade}x\nValor Total: R$ ${valorTotalItem.toFixed(2).replace('.', ',')}`);
    if (confirmar) {
        // Remove do array
        carrinhoPedido.splice(index, 1);
        localStorage.setItem('pedido-finalizar', JSON.stringify(carrinhoPedido));
        // Atualiza a lista e o resumo
        renderizarItensPedido();
        calcularResumo();
        // Se ficou vazio, mostra tela de vazio
        if (carrinhoPedido.length === 0) {
            document.getElementById('carrinho-vazio').style.display = 'block';
            document.getElementById('resumo-pedido').style.display = 'none';
        }
        mostrarNotificacao('Item removido com sucesso!', 'success');
    }
}

function mostrarNotificacao(mensagem, tipo = 'info') {
    const notificacao = document.createElement('div');
    notificacao.className = `notificacao ${tipo}`;
    notificacao.textContent = mensagem;
    document.body.appendChild(notificacao);
    setTimeout(() => {
        notificacao.remove();
    }, 3000);
}

function cancelarPedido() {
    if (confirm('Tem certeza que deseja cancelar o pedido? Todos os itens serão removidos.')) {
        localStorage.removeItem('pedido-finalizar');
        window.location.href = '/Pedidos/TelaPedidos';
    }
}

function finalizarPedido() {
    if (carrinhoPedido.length === 0) {
        alert('Carrinho vazio!');
        return;
    }
    try {
        let valorFinal = totalPedido;
        if (cupomAplicado) {
            const desconto = totalPedido * (cupomAplicado.desconto / 100);
            valorFinal = totalPedido - desconto;
        }
        const dadosPedido = {
            itens: carrinhoPedido.map(item => {
                const valorBase = parseFloat(item.valor) || 0;
                const valorIndividual = calcularValorIndividualItem(item);
                const valorTotal = calcularValorTotalItem(item);
                const valorAdicionais = valorIndividual - valorBase;
                return {
                    produtoId: item.id,
                    nome: item.nome,
                    quantidade: parseInt(item.quantidade) || 1,
                    valorBase: valorBase,
                    valorAdicionais: valorAdicionais,
                    valorIndividual: valorIndividual,
                    valorTotal: valorTotal,
                    personalizacoes: item.personalizacoes || [],
                    imagem: item.imagem || null,
                    descricao: item.descricao || ''
                };
            }),
            subtotal: totalPedido,
            total: valorFinal,
            cupom: cupomAplicado ? {
                codigo: cupomAplicado.codigo,
                desconto: cupomAplicado.desconto,
                valorDesconto: totalPedido * (cupomAplicado.desconto / 100)
            } : null,
            dataHora: new Date().toISOString(),
            quantidadeItens: carrinhoPedido.reduce((total, item) => total + (parseInt(item.quantidade) || 1), 0)
        };
        localStorage.setItem('dados-pedido-pagamento', JSON.stringify(dadosPedido));
        window.location.href = '/Pagamento/Index';
    } catch (error) {
        console.error('Erro ao finalizar pedido:', error);
        alert('Erro ao processar pedido: ' + error.message);
    }
}

async function aplicarCupom(codigoCupom) {
    try {
        const response = await fetch('/Pedidos/ValidarCupom', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ codigo: codigoCupom })
        });
        const result = await response.json();
        if (result.success) {
            cupomAplicado = { 
                codigo: result.codigo, 
                desconto: result.desconto 
            };
            calcularResumo();
            alert(`Cupom ${result.codigo} aplicado com sucesso! Desconto de ${result.desconto}%`);
            return true;
        } else {
            alert(result.message || 'Cupom inválido ou expirado.');
            return false;
        }
    } catch (error) {
        console.error('Erro ao validar cupom:', error);
        alert('Erro ao validar cupom. Tente novamente.');
        return false;
    }
}
if (typeof window.aplicarCupomCallback === 'undefined') {
    window.aplicarCupomCallback = aplicarCupom;
}
