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
    public class IgredienteProdutoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IgredienteProdutoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: IgredienteProduto?produtoId=1
        public async Task<IActionResult> Index(int? produtoId)
        {
            if (produtoId == null)
            {
                return NotFound();
            }

            var produto = await _context.Produto.FindAsync(produtoId);
            if (produto == null)
            {
                return NotFound();
            }

            // Verifica se é combo - se for, não permite gerenciar ingredientes
            if (produto.IsCombo == 1)
            {
                TempData["ErrorMessage"] = "Produtos do tipo combo não possuem ingredientes individuais.";
                return RedirectToAction("Index", "Produto");
            }

            // Busca todos os ingredientes disponíveis
            var todosIngredientes = await _context.Igredientes
                .OrderBy(i => i.Nome)
                .ToListAsync();

            // Busca ingredientes já associados ao produto
            var ingredientesAssociados = await _context.IgredienteProdutos
                .Where(ip => ip.ProdutoId == produtoId)
                .Include(ip => ip.Igrediente)
                .ToListAsync();

            var viewModel = new IgredienteProdutoViewModel
            {
                Produto = produto,
                TodosIngredientes = todosIngredientes,
                IngredientesAssociados = ingredientesAssociados
            };

            return View(viewModel);
        }

        

        [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> SalvarIngredientes(int produtoId, Dictionary<int, float> ingredientes)
{
    try
    {
        // Remove todos os ingredientes existentes deste produto
        var ingredientesExistentes = _context.IgredienteProdutos
            .Where(ip => ip.ProdutoId == produtoId);
        
        _context.IgredienteProdutos.RemoveRange(ingredientesExistentes);

        // Adiciona os novos ingredientes (apenas os que têm quantidade > 0)
        foreach (var item in ingredientes.Where(i => i.Value > 0))
        {
            var ingredienteProduto = new IgredienteProduto
            {
                ProdutoId = produtoId,
                IgredienteId = item.Key,
                Quantidade = item.Value
            };

            _context.IgredienteProdutos.Add(ingredienteProduto);
        }

        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "✅ Ingredientes salvos com sucesso!" });
    }
    catch (Exception ex)
    {
        // Log do erro aqui se necessário
        return Json(new { success = false, message = "❌ Erro ao salvar ingredientes: " + ex.Message });
    }
}

        // GET: IgredienteProduto/ExibirImagemIngrediente/5
        public async Task<IActionResult> ExibirImagemIngrediente(int id)
        {
            var ingrediente = await _context.Igredientes.FindAsync(id);
            if (ingrediente?.Imagem != null && ingrediente.Imagem.Length > 0)
            {
                return File(ingrediente.Imagem, "image/jpeg");
            }
            
            return NotFound();
        }
    }

    // ViewModel para a view
    public class IgredienteProdutoViewModel
    {
        public Produto Produto { get; set; }
        public List<Igrediente> TodosIngredientes { get; set; }
        public List<IgredienteProduto> IngredientesAssociados { get; set; }

        public float GetQuantidadeIngrediente(int ingredienteId)
        {
            var associado = IngredientesAssociados?.FirstOrDefault(x => x.IgredienteId == ingredienteId);
            return associado?.Quantidade ?? 0;
        }
    }
}