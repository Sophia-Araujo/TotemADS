*Como está funcionando??* 
# **Tela de Pedidos (telaPedidos.cshtml)**  

**MENU LATERAL (SIDEBAR)**  

***Objetivo:***  
Permitir que o usuário navegue rapidamente entre as principais categorias do Totem: Lanches, Combos, Sobremesas, Bebidas, Extras e Promoções. Cada botão do menu lateral também altera dinamicamente o banner principal da página, reforçando visualmente a escolha do usuário.  

***Funções:***  
Cada botão contém o atributo `data-category` que corresponde ao `id` de uma `section` no conteúdo da página. Ao clicar em um botão, a página faz o scroll suave para a respectiva seção. O banner principal (`#banner-image`) é atualizado conforme a categoria selecionada. A classe `.active` é adicionada ao botão selecionado, destacando visualmente a categoria ativa.

***Classes e Ids:***  
- `.category`: classe usada nos botões do menu lateral.
- `.active`: classe que estiliza o botão atualmente selecionado.
- `data-category`: atributo utilizado para associar o botão com a seção correspondente.
- `#banner-image`: id da imagem do banner que muda conforme a categoria.
---
**MENU DE FILTRO (SIDEBAR)**  

***Objetivo:***  
Permitir que o usuário filtre os itens dentro da categoria atual por tipos de produtos como: Carne, Especiais, Frango, Vegetarianos, Veganos.

***Funções:***  
Cada botão possui a classe `.cate`. Esses botões estão previstos para implementar uma lógica de filtro dentro das categorias, embora na versão atual ainda não estejam associados a uma função JavaScript.

***Classes e Ids:***  
- `.cate`: classe dos botões de filtro lateral.
---

**SEÇÕES DE MENU**  

***Objetivo:***  
Exibir os itens disponíveis para cada categoria do cardápio com imagem, nome, preço, descrição e botão de adicionar ao carrinho.

***Funções:***  
Cada categoria está dentro de uma `div` com a classe `.menu-section` e um `id` que corresponde ao `data-category` dos botões de menu. Dentro de cada seção, há múltiplos `.card` representando os itens do cardápio. O botão `.add-to-cart` permite adicionar o item ao carrinho.

***Classes e Ids:***  
- `.menu-section`: representa cada seção do cardápio.
- `id="lanches"`, `id="combos"`, etc.: identificadores das categorias.
- `.card`: representa um item do cardápio.
- `.add-to-cart`: botão para adicionar item ao carrinho.
---

**POPUP DE INGREDIENTES**  

***Objetivo:***  
Permitir que o usuário visualize e personalize os ingredientes do lanche selecionado antes de adicioná-lo ao carrinho.

***Funções:***  
O popup é exibido como um `div` com `id="popup"` e inicialmente `display: none`. Dentro do popup, cada ingrediente possui botões `+.mais` e `-.menos` para ajustar a quantidade. Botões de `Voltar` e `Adicionar` controlam o fechamento e a ação de adicionar ao pedido.

***Classes e Ids:***  
- `#popup`: container principal do popup.
- `.popup-overlay`: estilização para sobreposição.
- `.popup-content`: conteúdo do popup.
- `.ingrediente`: cada item com controle de quantidade.
- `.menos`, `.mais`: botões de ajuste de quantidade.
---

**RESUMO DA COMPRA**  

***Objetivo:***  
Apresentar um resumo das escolhas do usuário e permitir ações como editar pedido, pagar ou cancelar.

***Funções:***  
O resumo mostra os itens adicionados com quantidade e preço. Botões de `Editar pedido`, `Pagar` e `Cancelar` estão disponíveis.
Os botões de `Visualizar`, `Pagar` e `Cancelar` direcionam cada um para suas respectivas rotas.

***Classes e Ids:***  
- `.resumo-compra`: rodapé com o resumo.
- `.btn-editar`: botão para editar.
- `.btn-pagar`: botão para pagar.
- `.btn-cancelar`: botão para cancelar.
---

**BANNER DINÂMICO**  

***Objetivo:***  
Alterar a imagem principal de destaque conforme a categoria navegada, reforçando a experiência visual.

***Funções:***  
O JavaScript mapeia as imagens correspondentes para cada categoria. A função `updateBanner(category)` atualiza o `src` do `#banner-image`. A troca ocorre tanto ao clicar no menu quanto ao rolar a página (evento `scroll`).

---

**ROTEAMENTO E SCROLL**  

***Objetivo:***  
Navegação intuitiva e fluida entre as seções do cardápio.

***Funções:***  
Evento `click` nos botões do menu lateral faz `scrollIntoView` suave. O `scroll` detecta qual seção está visível e ajusta a classe `.active` no menu, além de atualizar o banner. Offset de `150px` é usado para compensar a altura do header/banner.

---
