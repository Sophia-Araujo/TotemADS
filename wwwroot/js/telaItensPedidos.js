let carrinhoPedido = [];
let totalPedido = 0;
let cupomAplicado = null;

document.addEventListener("DOMContentLoaded", function () {
  carregarItensPedido();
});

function carregarItensPedido() {
  const pedidoSalvo = localStorage.getItem("pedido-finalizar");
  const loadingElement = document.getElementById("loading-itens");
  const carrinhoVazioElement = document.getElementById("carrinho-vazio");
  const listaItensElement = document.getElementById("lista-itens-pedido");

  // Inicialmente, mostra o loading e esconde o carrinho vazio e a lista
  loadingElement.style.display = "block";
  carrinhoVazioElement.style.display = "none";
  listaItensElement.style.display = "none"; // Esconde a lista até carregar

  if (!pedidoSalvo) {
    loadingElement.style.display = "none";
    carrinhoVazioElement.style.display = "block";
    listaItensElement.style.display = "none";
    return;
  }
  try {
    carrinhoPedido = JSON.parse(pedidoSalvo);
    if (carrinhoPedido.length === 0) {
      loadingElement.style.display = "none";
      carrinhoVazioElement.style.display = "block";
      listaItensElement.style.display = "none";
      return;
    }

    loadingElement.style.display = "none";
    carrinhoVazioElement.style.display = "none";
    listaItensElement.style.display = "block"; // Mostra a lista após carregar
    renderizarItensPedido();
    calcularResumo();
  } catch (error) {
    console.error("Erro ao carregar pedido:", error);
    loadingElement.style.display = "none";
    carrinhoVazioElement.style.display = "block";
    listaItensElement.style.display = "none";
  }
}

function calcularValorIndividualItem(item) {
  let valorBase = parseFloat(item.valor) || 0;
  let valorAdicionais = 0;
  if (
    item.personalizacoes &&
    Array.isArray(item.personalizacoes) &&
    item.personalizacoes.length > 0
  ) {
    item.personalizacoes.forEach((personalizacao) => {
      const quantidadeBase = parseFloat(personalizacao.quantidadeBase) || 0;
      const quantidadeAtual = parseFloat(personalizacao.quantidade) || 0;
      const valorPersonalizacao = parseFloat(
        personalizacao.valor !== undefined
          ? personalizacao.valor
          : personalizacao.valorExtra !== undefined
          ? personalizacao.valorExtra
          : personalizacao.value ||
            personalizacao.preco ||
            personalizacao.price ||
            0
      );
      if (
        (personalizacao.isAdicional === true ||
          personalizacao.tipoModificacao === "adicional") &&
        quantidadeAtual > quantidadeBase
      ) {
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
  const listaItensElement = document.getElementById("lista-itens-pedido");
  listaItensElement.innerHTML = ""; // Limpa a lista antes de renderizar
  carrinhoPedido.forEach((item, index) => {
    const itemElement = criarElementoItem(item, index);
    listaItensElement.appendChild(itemElement);
  });
}

function criarElementoItem(item, index) {
  const divContainer = document.createElement("div");
  divContainer.className = "VisualizaItensPedidos";

  const valorBase = parseFloat(item.valor) || 0;
  const valorIndividual = calcularValorIndividualItem(item);
  const valorTotal = calcularValorTotalItem(item); // Total do item (valor_individual * quantidade)
  const valorAdicionais = valorIndividual - valorBase;

  let ingredientesHtml = "";
  if (
    item.personalizacoes &&
    Array.isArray(item.personalizacoes) &&
    item.personalizacoes.length > 0
  ) {
    item.personalizacoes.forEach((personalizacao) => {
      const quantidadeBase = parseFloat(personalizacao.quantidadeBase) || 0;
      const quantidadeAtual = parseFloat(personalizacao.quantidade) || 0;
      const valorPersonalizacao = parseFloat(personalizacao.valor) || 0;
      const nomePersonalizacao = personalizacao.nome || "Item";

      if (
        personalizacao.isAdicional === true ||
        personalizacao.tipoModificacao === "adicional"
      ) {
        if (quantidadeAtual > quantidadeBase) {
          const quantidadeExtra = quantidadeAtual - quantidadeBase;
          ingredientesHtml += `<li>${nomePersonalizacao} <span>+R$ ${(
            valorPersonalizacao * quantidadeExtra
          )
            .toFixed(2)
            .replace(".", ",")} (${quantidadeExtra}x)</span></li>`;
        } else {
          // Se for adicional mas não gerou custo extra, ainda pode ser listado
          ingredientesHtml += `<li>${nomePersonalizacao}</li>`;
        }
      } else if (personalizacao.tipoModificacao === "remocao") {
        ingredientesHtml += `<li>Sem ${nomePersonalizacao}</li>`;
      } else if (personalizacao.tipoModificacao === "padrao") {
        ingredientesHtml += `<li>Com ${nomePersonalizacao}</li>`;
      } else if (
        !personalizacao.isAdicional &&
        !personalizacao.isRemocao &&
        !personalizacao.tipoModificacao
      ) {
        ingredientesHtml += `<li>${nomePersonalizacao}</li>`;
      }
    });
  }

  const imagemSrc =
    item.imagem && item.imagem !== ""
      ? `data:image/jpeg;base64,${item.imagem}`
      : "/image/produtos/default.png";

  // Construção do HTML para o card do item do pedido
  divContainer.innerHTML = `
        <div class="card-pedido">
            <div class="info-produto-esquerda">
                <img src="${imagemSrc}" alt="${item.nome}" />
                <h3 class="nomePedido">${item.nome}</h3>
                <p class="descricaoPedido">${item.descricao || ""}</p>
                <ul class="ingredientesPedidoLista">
                    ${ingredientesHtml}
                </ul>
            </div>

            <div class="info-preco-direita">
                <div class="precos-principais">
                    <span class="preco-unitario">R$ ${valorIndividual
                      .toFixed(2)
                      .replace(".", ",")} /unid.</span>
                    <span class="preco-total-item">R$ ${valorTotal
                      .toFixed(2)
                      .replace(".", ",")}</span>
                </div>
                ${
                  valorAdicionais > 0
                    ? `<p class="precoAdicionais">Adicionais: <b>R$ ${valorAdicionais
                        .toFixed(2)
                        .replace(".", ",")}</b></p>`
                    : ""
                }
                
                <div class="quantidade-e-remover">
                    <span class="quantidade-item-carrinho">Quantidade: ${
                      item.quantidade
                    }x</span> <button class="btn-remover-item" type="button" onclick="removerItem(${index})" title="Remover item do pedido">
                        <i class="fas fa-trash-alt"></i> Excluir
                    </button>
                </div>
            </div>
        </div>
    `;
  return divContainer;
}

function calcularResumo() {
  const totalListaElement = document.getElementById("total-lista");
  const categorias = {};
  totalPedido = 0;

  // Garante que o total-lista esteja visível se houver itens
  const resumoPedidoElement = document.getElementById("resumo-pedido");
  if (carrinhoPedido.length > 0) {
    resumoPedidoElement.style.display = "block";
  } else {
    resumoPedidoElement.style.display = "none";
  }

  carrinhoPedido.forEach((item) => {
    const categoria = "Lanches"; // Considerando que todos são 'Lanches' aqui
    if (!categorias[categoria]) {
      categorias[categoria] = { quantidade: 0, valor: 0 };
    }
    const valorTotalItem = calcularValorTotalItem(item);
    const quantidadeItem = parseInt(item.quantidade) || 1;
    categorias[categoria].quantidade += quantidadeItem;
    categorias[categoria].valor += valorTotalItem;
    totalPedido += valorTotalItem;
  });

  totalListaElement.innerHTML = "";
  Object.keys(categorias).forEach((categoria) => {
    const cat = categorias[categoria];
    const divItem = document.createElement("div");
    divItem.className = "total-item";
    divItem.innerHTML = `
            <span>${categoria} ${cat.quantidade}x</span>
            <span>R$ ${cat.valor.toFixed(2).replace(".", ",")}</span>
        `;
    totalListaElement.appendChild(divItem);
  });

  let valorFinal = totalPedido;
  if (cupomAplicado) {
    const desconto = totalPedido * (cupomAplicado.desconto / 100);
    valorFinal = totalPedido - desconto;
    const divDesconto = document.createElement("div");
    divDesconto.className = "total-item desconto";
    divDesconto.innerHTML = `
            <span>Desconto (${cupomAplicado.codigo})</span>
            <span>-R$ ${desconto.toFixed(2).replace(".", ",")}</span>
        `;
    totalListaElement.appendChild(divDesconto);
  }

  const divTotal = document.createElement("div");
  divTotal.className = "total-item total";
  divTotal.innerHTML = `
        <span>Total:</span>
        <span>R$ ${valorFinal.toFixed(2).replace(".", ",")}</span>
    `;
  totalListaElement.appendChild(divTotal);
}

function removerItem(index) {
  const item = carrinhoPedido[index];
  const valorTotalItem = calcularValorTotalItem(item);
  const confirmar = confirm(
    `Deseja remover "${item.nome}" do pedido?\n\nQuantidade: ${
      item.quantidade
    }x\nValor Total: R$ ${valorTotalItem.toFixed(2).replace(".", ",")}`
  );
  if (confirmar) {
    carrinhoPedido.splice(index, 1);
    localStorage.setItem("pedido-finalizar", JSON.stringify(carrinhoPedido));
    renderizarItensPedido();
    calcularResumo();
    if (carrinhoPedido.length === 0) {
      document.getElementById("carrinho-vazio").style.display = "block";
      document.getElementById("resumo-pedido").style.display = "none";
    }
    mostrarNotificacao("Item removido com sucesso!", "success");
  }
}

function mostrarNotificacao(mensagem, tipo = "info") {
  const notificacao = document.createElement("div");
  notificacao.className = `notificacao ${tipo}`;
  notificacao.textContent = mensagem;
  document.body.appendChild(notificacao);
  setTimeout(() => {
    notificacao.remove();
  }, 3000);
}

function cancelarPedido() {
  if (
    confirm(
      "Tem certeza que deseja cancelar o pedido? Todos os itens serão removidos."
    )
  ) {
    localStorage.removeItem("pedido-finalizar");
    window.location.href = "/Pedidos/TelaPedidos";
  }
}

function finalizarPedido() {
  if (carrinhoPedido.length === 0) {
    alert("Carrinho vazio!");
    return;
  }
  try {
    let valorFinal = totalPedido;
    if (cupomAplicado) {
      const desconto = totalPedido * (cupomAplicado.desconto / 100);
      valorFinal = totalPedido - desconto;
    }
    const dadosPedido = {
      itens: carrinhoPedido.map((item) => {
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
          descricao: item.descricao || "",
        };
      }),
      subtotal: totalPedido,
      total: valorFinal,
      cupom: cupomAplicado
        ? {
            codigo: cupomAplicado.codigo,
            desconto: cupomAplicado.desconto,
            valorDesconto: totalPedido * (cupomAplicado.desconto / 100),
          }
        : null,
      dataHora: new Date().toISOString(),
      quantidadeItens: carrinhoPedido.reduce(
        (total, item) => total + (parseInt(item.quantidade) || 1),
        0
      ),
    };
    localStorage.setItem("dados-pedido-pagamento", JSON.stringify(dadosPedido));
    window.location.href = "/Pagamento/Index";
  } catch (error) {
    console.error("Erro ao finalizar pedido:", error);
    alert("Erro ao processar pedido: " + error.message);
  }
}

async function aplicarCupom(codigoCupom) {
  try {
    const response = await fetch("/Pedidos/ValidarCupom", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ codigo: codigoCupom }),
    });
    const result = await response.json();
    if (result.success) {
      cupomAplicado = {
        codigo: result.codigo,
        desconto: result.desconto,
      };
      calcularResumo();
      alert(
        `Cupom ${result.codigo} aplicado com sucesso! Desconto de ${result.desconto}%`
      );
      return true;
    } else {
      alert(result.message || "Cupom inválido ou expirado.");
      return false;
    }
  } catch (error) {
    console.error("Erro ao validar cupom:", error);
    alert("Erro ao validar cupom. Tente novamente.");
    return false;
  }
}
if (typeof window.aplicarCupomCallback === "undefined") {
  window.aplicarCupomCallback = aplicarCupom;
}
