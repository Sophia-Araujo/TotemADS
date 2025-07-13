using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TotemPWA.Data;
using TotemPWA.Models;
using Microsoft.AspNetCore.Authorization;

namespace TotemPWA.Controllers
{
    [Authorize]
    public class ItensComboController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ItensComboController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ItensCombo?produtoId=1
        public async Task<IActionResult> Index(int? produtoId)
        {
            if (produtoId == null)
            {
                return NotFound();
            }

            // Busca o produto que é um combo
            var produtoCombo = await _context.Produto
                .Include(p => p.Categoria)
                .Include(p => p.Administrador)
                .FirstOrDefaultAsync(p => p.ProdutoId == produtoId && p.IsCombo == 1);
                     
            if (produtoCombo == null)
            {
                return NotFound("Produto não encontrado ou não é um combo.");
            }

            // Busca todos os produtos que NÃO são combos para serem itens do combo
            var todosProdutos = await _context.Produto
                .Where(p => p.IsCombo == 0) // Apenas produtos que não são combos
                .OrderBy(p => p.Nome)
                .ToListAsync();

            // Busca itens já associados ao combo usando ComboId
            var itensAssociados = await _context.ItensCombos
                .Include(ic => ic.Produto)
                .Where(ic => ic.ComboId == produtoId)
                .ToListAsync();

            ViewBag.ProdutoCombo = produtoCombo;
            ViewBag.TodosProdutos = todosProdutos;
            ViewBag.ItensAssociados = itensAssociados;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SalvarItens(int produtoId, Dictionary<int, int> produtos)
        {
            try
            {
                // Verifica se o produto é realmente um combo
                var produtoCombo = await _context.Produto
                    .FirstOrDefaultAsync(p => p.ProdutoId == produtoId && p.IsCombo == 1);
                
                if (produtoCombo == null)
                {
                    return Json(new { success = false, message = "❌ Produto não encontrado ou não é um combo." });
                }

                // Busca todos os itens existentes deste combo
                var itensExistentes = await _context.ItensCombos
                    .Where(ic => ic.ComboId == produtoId)
                    .ToListAsync();

                // Para cada produto enviado no formulário
                foreach (var produto in produtos)
                {
                    int produtoIdAtual = produto.Key;
                    int quantidadeNova = produto.Value;

                    // Verifica se o produto existe e não é um combo
                    var produtoExiste = await _context.Produto
                        .AnyAsync(p => p.ProdutoId == produtoIdAtual && p.IsCombo == 0);
                    
                    if (!produtoExiste)
                    {
                        return Json(new { success = false, message = $"❌ Produto com ID {produtoIdAtual} não encontrado ou é um combo." });
                    }

                    // Busca se já existe um item para este produto no combo
                    var itemExistente = itensExistentes
                        .FirstOrDefault(ie => ie.ProdutoId == produtoIdAtual);

                    if (quantidadeNova > 0)
                    {
                        if (itemExistente != null)
                        {
                            // Atualiza a quantidade do item existente
                            itemExistente.Quantidade = quantidadeNova;
                            _context.ItensCombos.Update(itemExistente);
                        }
                        else
                        {
                            // Cria um novo item
                            var novoItem = new ItensCombo
                            {
                                ComboId = produtoId,           // ID do produto-combo
                                ProdutoId = produtoIdAtual,    // ID do produto que é item do combo
                                Quantidade = quantidadeNova,
                                CupomId = null                 // Deixa null para itens de combo
                            };
                            _context.ItensCombos.Add(novoItem);
                        }
                    }
                    else
                    {
                        // Quantidade = 0, remove o item se existir
                        if (itemExistente != null)
                        {
                            _context.ItensCombos.Remove(itemExistente);
                        }
                    }
                }

                // Remove itens que não estão mais no formulário (limpeza)
                var produtoIdsEnviados = produtos.Keys.ToList();
                var itensParaRemover = itensExistentes
                    .Where(ie => !produtoIdsEnviados.Contains(ie.ProdutoId))
                    .ToList();

                if (itensParaRemover.Any())
                {
                    _context.ItensCombos.RemoveRange(itensParaRemover);
                }

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "✅ Itens do combo salvos com sucesso!" });
            }
            catch (Exception ex)
            {
                // Log do erro se necessário
                return Json(new { success = false, message = "❌ Erro ao salvar itens do combo: " + ex.Message });
            }
        }

        // GET: ItensCombo/ExibirImagemProduto/5
        public async Task<IActionResult> ExibirImagemProduto(int id)
        {
            var produto = await _context.Produto.FindAsync(id);
            if (produto?.Imagem != null && produto.Imagem.Length > 0)
            {
                return File(produto.Imagem, "image/jpeg");
            }
                     
            return NotFound();
        }
    }
}