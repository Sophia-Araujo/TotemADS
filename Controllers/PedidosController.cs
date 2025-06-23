using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TotemPWA.Data;
using TotemPWA.Models;
using TotemPWA.ViewModels;

namespace TotemPWA.Controllers
{
    [Route("Pedidos")]
    public class PedidosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PedidosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Pedidos/TelaPedidos
        [HttpGet("TelaPedidos")]
        public async Task<IActionResult> TelaPedidos()
        {
            try
            {
                // Busca categorias principais (sem categoria pai) com suas subcategorias
                var categorias = await _context.Categorias
                    .Where(c => c.CategoriaPaiId == null) // Apenas categorias principais
                    .Include(c => c.Subcategorias)
                    .OrderBy(c => c.Nome)
                    .ToListAsync();

                // Busca todos os produtos com suas categorias
                var produtos = await _context.Produto
                    .Include(p => p.Categoria)
                    .OrderBy(p => p.Descricao)
                    .ToListAsync();

                // Cria um ViewModel para passar os dados para a view
                var viewModel = new TelaPedidosViewModel
                {
                    Categorias = categorias,
                    Produtos = produtos
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Log do erro (você pode usar ILogger aqui)
                // _logger.LogError(ex, "Erro ao carregar dados da tela de pedidos");
                
                // Retorna view com dados vazios em caso de erro
                var viewModelVazio = new TelaPedidosViewModel
                {
                    Categorias = new List<Categoria>(),
                    Produtos = new List<Produto>()
                };

                return View(viewModelVazio);
            }
        }

        // API endpoint para buscar produtos por categoria (AJAX)
        [HttpGet("GetProdutosPorCategoria")]
        public async Task<JsonResult> GetProdutosPorCategoria(int categoriaId)
        {
            try
            {
                var produtos = await _context.Produto
                    .Where(p => p.CategoriaId == categoriaId)
                    .Select(p => new
                    {
                        id = p.ProdutoId,
                        descricao = p.Descricao,
                        valor = p.Valor,
                        isCombo = p.IsCombo,
                        // Converte byte[] para base64 string se houver imagem
                        imagem = p.Imagem != null ? Convert.ToBase64String(p.Imagem) : null
                    })
                    .ToListAsync();

                return Json(produtos);
            }
            catch (Exception)
            {
                return Json(new List<object>());
            }
        }

        // API endpoint para buscar detalhes de um produto específico
        [HttpGet("GetProdutoDetalhes")]
        public async Task<JsonResult> GetProdutoDetalhes(int produtoId)
        {
            try
            {
                var produto = await _context.Produto
                    .Include(p => p.Categoria)
                    .Where(p => p.ProdutoId == produtoId)
                    .Select(p => new
                    {
                        id = p.ProdutoId,
                        descricao = p.Descricao,
                        valor = p.Valor,
                        isCombo = p.IsCombo,
                        categoria = p.Categoria.Nome,
                        // Converte byte[] para base64 string se houver imagem
                        imagem = p.Imagem != null ? Convert.ToBase64String(p.Imagem) : null
                    })
                    .FirstOrDefaultAsync();

                if (produto == null)
                {
                    return Json(new { erro = "Produto não encontrado" });
                }

                return Json(produto);
            }
            catch (Exception)
            {
                return Json(new { erro = "Erro ao buscar produto" });
            }
        }
    }
}