*USER STORIES E CRITÉRIOS DE ACEITAÇÃO* 
# **Requisitos Funcionais (RF)**  

**USER STORY: RF01 - TELA DE INÍCIO**  
**Como** cliente  
**Eu quero** visualizar uma tela inicial intuitiva  
**Para** que eu possa iniciar meu pedido de forma fácil.  
**Acceptance Criteria (RF01)**  
- A tela inicial deve exibir categorias organizadas (Lanches, Bebidas, Sobremesas etc.).  
- O botão 'Iniciar Pedido' deve estar visível e acessível.  
- O tempo de carregamento da tela não deve ultrapassar 4 segundos.  
- Ícones e textos devem ser legíveis para diferentes tipos de usuários.

---  

**USER STORY: RF02 - ESCOLHA DO TIPO DE PEDIDO**  
**Como** cliente  
**Eu quero** escolher se meu pedido será para viagem ou consumo no local  
**Para** que eu receba meu pedido da forma correta.  
**Acceptance Criteria (RF02)**  
- A tela deve exibir duas opções: "Viagem" e "Consumo no Local".  
- O usuário deve poder selecionar facilmente uma das opções.  
- Após a seleção, o sistema deve direcionar o usuário para a próxima etapa sem demora.  

---  

**USER STORY: RF03 - LOGIN E IDENTIFICAÇÃO**  
**Como** cliente  
**Eu quero** me identificar pelo CPF ou continuar sem login  
**Para** que eu possa registrar minha compra e acumular pontos se desejar.  
**Acceptance Criteria (RF03)**  
- O sistema deve oferecer um campo para inserir o CPF.  
- Deve haver a opção de prosseguir sem login.  
- A identificação deve ser rápida e sem travamentos.  

---  

**USER STORY: RF04 - APLICAÇÃO DE CUPONS**  
**Como** cliente  
**Eu quero** inserir um cupom de desconto  
**Para** que eu possa economizar na minha compra.  
**Acceptance Criteria (RF04)**  
- O sistema deve disponibilizar um campo para inserção de cupons.  
- Caso o cupom seja inválido, uma mensagem de erro deve ser exibida.  
- O valor com desconto deve ser atualizado no total do pedido.  

---  

**USER STORY: RF05 - SELEÇÃO DE ITENS**  
**Como** cliente  
**Eu quero** visualizar os produtos disponíveis com suas informações  
**Para** que eu possa escolher o que comprar.  
**Acceptance Criteria (RF05)**  
- Todos os produtos devem ser exibidos com imagem, preço e descrição.  
- O usuário deve conseguir visualizar os detalhes do item antes de adicioná-lo ao carrinho.  

---  

**USER STORY: RF06 - PERSONALIZAÇÃO DE INGREDIENTES**  
**Como** cliente  
**Eu quero** poder adicionar ou remover ingredientes  
**Para** que meu pedido atenda minhas preferências.  
**Acceptance Criteria (RF06)**  
- O sistema deve permitir que o cliente remova ou adicione ingredientes.  
- Os itens personalizáveis devem estar claramente indicados.  

---  

**USER STORY: RF07 - ADICIONAR ITEM AO CARRINHO**  
**Como** cliente  
**Eu quero** adicionar produtos ao carrinho  
**Para** que eu possa comprar múltiplos itens antes de finalizar o pedido.  
**Acceptance Criteria (RF07)**  
- Deve existir um botão visível para adicionar itens ao carrinho.  
- O carrinho deve atualizar automaticamente quando um item for adicionado.  

---  

**USER STORY: RF08 - REVISÃO DO PEDIDO**  
**Como** cliente  
**Eu quero** ver um resumo do meu pedido  
**Para** garantir que tudo está correto antes de finalizar.  
**Acceptance Criteria (RF08)**  
- O sistema deve exibir um resumo claro dos itens escolhidos.  
- O usuário deve conseguir editar os itens antes de concluir a compra.  

---  

**USER STORY: RF09 - REMOÇÃO DE ITENS DO CARRINHO**  
**Como** cliente  
**Eu quero** excluir itens do meu carrinho  
**Para** corrigir minha seleção se necessário.  
**Acceptance Criteria (RF09)**  
- Deve haver um botão para remover itens individualmente.  

---  

**USER STORY: RF10 - CANCELAMENTO DO CARRINHO**  
**Como** cliente  
**Eu quero** cancelar meu pedido  
**Para** que eu possa desistir sem precisar remover item por item.  
**Acceptance Criteria (RF10)**  
- Deve haver um botão para cancelar todo o pedido.  


### **USER STORY: RF11 - EFETIVAÇÃO DO PEDIDO**  
**Como** cliente  
**Eu quero** confirmar meu pedido  
**Para** que ele seja enviado para a cozinha.  
**Acceptance Criteria (RF11)**  
- O sistema deve exibir um botão claro para confirmar a compra.  

---  

**USER STORY: RF12 - EMISSÃO DE NOTA FISCAL**  
**Como** cliente  
**Eu quero** receber minha nota fiscal  
**Para** ter um comprovante de compra.  
**Acceptance Criteria (RF12)**  
- O sistema deve perguntar se o cliente deseja incluir CPF.  

---  

**USER STORY: RF13 - PAGAMENTO INTEGRADO**  
**Como** cliente  
**Eu quero** pagar meu pedido de forma conveniente  
**Para** concluir minha compra sem dificuldades.  
**Acceptance Criteria (RF13)**  
- O sistema deve aceitar pagamentos via Dinheiro, Pix, Carteira Digital e Cartão.  

---  

 **USER STORY: RF14 - IDENTIFICAÇÃO DO PEDIDO**  
**Como** cliente  
**Eu quero** saber como identificar meu pedido  
**Para** pegá-lo quando estiver pronto.  
**Acceptance Criteria (RF14)**  
- O sistema deve exibir um número ou nome para identificação.  

---  

**USER STORY: RF15 - MODO DALTÔNICO**  
**Como** cliente com daltonismo  
**Eu quero** que o sistema tenha um modo daltônico  
**Para** que eu possa visualizar os conteúdos com melhor contraste.  
**Acceptance Criteria (RF15)**  
- O sistema deve permitir ativar/desativar o modo daltônico nas configurações.  
- O modo daltônico deve modificar as cores e contrastes para melhor acessibilidade.  
- A alteração das cores não pode comprometer a usabilidade da interface.  

---  

**USER STORY: RF16 - NAVEGAÇÃO POR CATEGORIAS**  
**Como** cliente  
**Eu quero** navegar pelos produtos organizados por categorias  
**Para** encontrar rapidamente os itens que desejo.  
**Acceptance Criteria (RF16)**  
- O sistema deve exibir categorias como lanches, combos e sobremesas.  
- O usuário deve conseguir alternar entre categorias facilmente.  
- Os produtos exibidos devem corresponder à categoria selecionada.  

---  

**USER STORY: RF17 - FILTROS NO MENU**  
**Como** cliente  
**Eu quero** poder aplicar filtros no menu  
**Para** refinar minha busca e encontrar produtos específicos com mais facilidade.  
**Acceptance Criteria (RF17)**  
- O sistema deve oferecer filtros como "vegetariano", "mais pedidos" e "promoções".  
- O usuário deve conseguir ativar/desativar múltiplos filtros simultaneamente.  
- Os produtos exibidos devem corresponder aos filtros aplicados.  

---  

**USER STORY: RF18 - PROGRESSO DO PEDIDO**  
**Como** cliente  
**Eu quero** visualizar o progresso do meu pedido em tempo real  
**Para** acompanhar cada etapa até a retirada.  
**Acceptance Criteria (RF18)**  
- O sistema deve exibir status como "Pedido Recebido", "Em Preparo" e "Pronto para Retirada".  
- A atualização do status deve ser automática e em tempo real.  
- O cliente deve conseguir visualizar essa informação na tela do sistema.  


---

**USER STORY: RF19 - NOTIFICAÇÃO DE PEDIDO PRONTO**  
**Como** cliente  
**Eu quero** ser notificado quando meu pedido estiver pronto  
**Para** saber o momento exato de retirá-lo.  
**Acceptance Criteria (RF19)**  
- O sistema deve exibir um alerta visual no telão do estabelecimento.  
- O número do pedido deve ser destacado na notificação.  
- A notificação deve permanecer visível por um período adequado para que o cliente a veja.  

---  

**USER STORY: RF20 - CADASTRO DE CATEGORIAS**  
**Como** administrador  
**Eu quero** cadastrar e gerenciar categorias e subcategorias de produtos  
**Para** organizar melhor o cardápio e facilitar a navegação do cliente.  
**Acceptance Criteria (RF20)**  
- O sistema deve permitir a criação, edição e exclusão de categorias e subcategorias.  
- Os produtos devem ser corretamente associados às categorias correspondentes.  
- A interface deve exibir categorias e subcategorias de forma organizada.  

---  

**USER STORY: RF21 - LIMITE DE INGREDIENTES**  
**Como** cliente  
**Eu quero** ter um limite definido de ingredientes personalizáveis  
**Para** garantir que meu pedido siga as regras do restaurante.  
**Acceptance Criteria (RF21)**  
- O sistema deve permitir configurar a quantidade máxima de ingredientes adicionais por item.  
- O usuário deve ser avisado ao tentar exceder o limite permitido.  
- A interface deve exibir claramente a quantidade disponível para personalização.  

---  

**USER STORY: RF22 - GERENCIAMENTO DO CARDÁPIO (ADMIN)**  
**Como** administrador  
**Eu quero** adicionar, editar e remover itens do cardápio  
**Para** manter o menu sempre atualizado.  
**Acceptance Criteria (RF22)**  
- O sistema deve permitir adicionar novos produtos com nome, descrição, preço e imagem.  
- Deve ser possível editar e excluir produtos já cadastrados.  
- As alterações devem ser refletidas imediatamente na interface do cliente.  

---  

**USER STORY: RF23 - RELATÓRIO DE VENDAS**  
**Como** administrador  
**Eu quero** acessar relatórios de vendas e preferências dos clientes  
**Para** entender melhor o desempenho dos produtos e otimizar o cardápio.  
**Acceptance Criteria (RF23)**  
- O sistema deve gerar relatórios com dados de vendas diárias, semanais e mensais.  
- O administrador deve visualizar quais produtos são mais vendidos e quais são menos populares.  
- Os relatórios devem ser exportáveis para formatos como PDF e CSV.  

---  

**USER STORY: RF24 - PROGRAMA DE FIDELIDADE**  
**Como** cliente  
**Eu quero** acumular e resgatar pontos ao fazer pedidos  
**Para** aproveitar benefícios e descontos no restaurante.  
**Acceptance Criteria (RF24)**  
- O sistema deve permitir login para vincular os pontos ao cliente.  
- O cliente deve visualizar o saldo de pontos disponível na interface.  
- O sistema deve permitir o resgate de pontos de forma simples e intuitiva.
- 

# **Requisitos Não Funcionais (RNF)**  

**USER STORY: RNF01 - TEMPO DE RESPOSTA**  
**Como** usuário  
**Eu quero** que o sistema responda rapidamente  
**Para** não perder tempo esperando.  
**Acceptance Criteria (RNF01)**  
- O carregamento de menus deve ocorrer em até 4 segundos.  

---

**USER STORY: RNF02 - INTERFACE INTUITIVA**  
**Como** usuário  
**Eu quero** uma interface simples e fácil de entender  
**Para** que minha experiência seja agradável.  
**Acceptance Criteria (RNF02)**  
- O design deve ser claro e sem poluição visual.  

---

**USER STORY: RNF03 - SEGURANÇA DE PAGAMENTO**  
**Como** usuário  
**Eu quero** que meus dados bancários estejam protegidos  
**Para** que eu possa pagar com segurança.  
**Acceptance Criteria (RNF03)**  
- O sistema deve usar criptografia nos pagamentos.  

---

**USER STORY: RNF04 - CONFORMIDADE COM LGPD**  
**Como** usuário  
**Eu quero** que meus dados sejam protegidos  
**Para** que minha privacidade seja respeitada.  
**Acceptance Criteria (RNF04)**  
- O sistema deve seguir as diretrizes da LGPD.  

---

**USER STORY: RNF05 - DEPLOY CONTÍNUO**  
**Como** desenvolvedor  
**Eu quero** atualizações sem interrupção  
**Para** que o sistema esteja sempre disponível.  
**Acceptance Criteria (RNF05)**  
- As atualizações devem ser implementadas sem downtime.  

---

**USER STORY: RNF06 - PREVENÇÃO DE TRAVAMENTOS**  
**Como** usuário  
**Eu quero** que o sistema funcione sem travamentos  
**Para** que minha experiência seja fluida e sem interrupções.  
**Acceptance Criteria (RNF06)**  
- O sistema deve detectar e prevenir falhas críticas que possam travar a interface.  

---

**USER STORY: RNF07 - RESPONSIVIDADE**  
**Como** usuário  
**Eu quero** acessar o sistema em qualquer dispositivo  
**Para** ter uma experiência consistente e fluida.  
**Acceptance Criteria (RNF07)**  
- O sistema deve ser responsivo e se adaptar a diferentes tamanhos de tela.  

---

**USER STORY: RNF08 - ESCALABILIDADE**  
**Como** administrador  
**Eu quero** que o sistema suporte um grande número de usuários  
**Para** garantir um bom desempenho mesmo com alta demanda.  
**Acceptance Criteria (RNF08)**  
- O sistema deve permitir expansão sem perda de desempenho.  

---

**USER STORY: RNF09 - DISPONIBILIDADE 24/7**  
**Como** usuário  
**Eu quero** acessar o sistema a qualquer momento  
**Para** que eu possa utilizá-lo sempre que precisar.  
**Acceptance Criteria (RNF09)**  
- O sistema deve ter uma disponibilidade mínima de 99,9%.  

---

**USER STORY: RNF10 - PROTEÇÃO CONTRA ATAQUES**  
**Como** administrador  
**Eu quero** que o sistema seja seguro contra ameaças  
**Para** evitar acessos indevidos e vazamento de dados.  
**Acceptance Criteria (RNF10)**  
- O sistema deve implementar medidas contra injeção SQL, XSS e outras ameaças.  

---

**USER STORY: RNF11 - BACKUP E SINCRONIZAÇÃO**  
**Como** administrador  
**Eu quero** que o sistema tenha backups frequentes  
**Para** evitar perda de dados em caso de falha.  
**Acceptance Criteria (RNF11)**  
- O sistema deve realizar backups automáticos e sincronização com o servidor.  

---

**USER STORY: RNF12 - INTERAÇÕES SONORAS E VISUAIS**  
**Como** usuário  
**Eu quero** que o sistema forneça feedback visual e sonoro  
**Para** me ajudar a entender as ações realizadas.  
**Acceptance Criteria (RNF12)**  
- O sistema deve ter sons e animações para feedback, com opção de desativação.  

---

**USER STORY: RNF13 - PROTEÇÃO DE DADOS**  
**Como** usuário  
**Eu quero** que minhas informações sejam armazenadas de forma segura  
**Para** evitar vazamento ou uso indevido.  
**Acceptance Criteria (RNF13)**  
- O sistema deve armazenar dados sensíveis de forma criptografada.  

---

**USER STORY: RNF14 - USABILIDADE (UI/UX)**  
**Como** usuário  
**Eu quero** que o sistema tenha uma experiência de uso intuitiva  
**Para** facilitar a navegação e o uso das funcionalidades.  
**Acceptance Criteria (RNF14)**  
- O design deve seguir boas práticas de UI/UX para acessibilidade.  

---

**USER STORY: RNF15 - CAPACIDADE DE PROCESSAMENTO**  
**Como** administrador  
**Eu quero** que o sistema suporte alto volume de transações  
**Para** evitar lentidão ou falhas em momentos de pico.  
**Acceptance Criteria (RNF15)**  
- O sistema deve processar múltiplas solicitações sem degradação de desempenho.  

---

**USER STORY: RNF16 - MODO OFFLINE**  
**Como** usuário  
**Eu quero** que o sistema funcione parcialmente sem internet  
**Para** evitar interrupções caso eu perca a conexão.  
**Acceptance Criteria (RNF16)**  
- O sistema deve permitir operações mínimas sem conexão, com sincronização posterior.  

---

**USER STORY: RNF17 - CACHE PARA CARREGAMENTO RÁPIDO**  
**Como** usuário  
**Eu quero** que o sistema carregue rapidamente  
**Para** evitar esperas desnecessárias.  
**Acceptance Criteria (RNF17)**  
- O sistema deve utilizar cache para otimizar o tempo de carregamento.  

---

**USER STORY: RNF18 - ANÁLISE DE USO E RELATÓRIOS**  
**Como** administrador  
**Eu quero** acessar métricas de uso do sistema  
**Para** identificar melhorias e otimizar a experiência do usuário.  
**Acceptance Criteria (RNF18)**  
- O sistema deve coletar e exibir estatísticas de uso para análise de desempenho.  

---

**USER STORY: RNF19 - TEMPO DE INATIVIDADE**  
**Como** usuário  
**Eu quero** que o sistema volte à tela inicial automaticamente  
**Para** evitar que pedidos abandonados fiquem na tela.  
**Acceptance Criteria (RNF19)**  
- O sistema deve retornar automaticamente à tela inicial após 1 minuto de inatividade.  
  


