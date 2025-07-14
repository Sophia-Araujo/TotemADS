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

        // Método para servir imagens das categorias
        [HttpGet("GetCategoriaImage/{id}/{type}")]
        public async Task<IActionResult> GetCategoriaImage(int id, string type = "image")
        {
            try
            {
                var categoria = await _context.Categorias.FindAsync(id);
                if (categoria == null) return NotFound();

                byte[]? imageData = type.ToLower() == "banner" ? categoria.Banner : categoria.Image;

                if (imageData == null || imageData.Length == 0)
                    return NotFound();

                // Detecta o tipo de imagem baseado no header do arquivo
                string contentType = DetectImageContentType(imageData);

                return File(imageData, contentType);
            }
            catch (Exception)
            {
                return NotFound();
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

        // Método auxiliar para detectar o tipo de conteúdo da imagem
        private string DetectImageContentType(byte[] imageData)
        {
            if (imageData.Length < 4) return "image/jpeg";

            // Verifica assinatura do arquivo
            if (imageData[0] == 0xFF && imageData[1] == 0xD8) return "image/jpeg";
            if (imageData[0] == 0x89 && imageData[1] == 0x50 && imageData[2] == 0x4E && imageData[3] == 0x47) return "image/png";
            if (imageData[0] == 0x47 && imageData[1] == 0x49 && imageData[2] == 0x46) return "image/gif";
            if (imageData[0] == 0x42 && imageData[1] == 0x4D) return "image/bmp";
            if (imageData[0] == 0x52 && imageData[1] == 0x49 && imageData[2] == 0x46 && imageData[3] == 0x46) return "image/webp";

            // Default para JPEG
            return "image/jpeg";
        }
       [HttpGet("GetIngredientesProduto")]
public async Task<JsonResult> GetIngredientesProduto(int produtoId)
{
    try
    {
        // Primeiro verificar se o produto existe e não é combo
        var produto = await _context.Produto.FindAsync(produtoId);
        if (produto == null || produto.IsCombo == 1)
        {
            return Json(new List<object>());
        }

        var ingredientes = await _context.IgredienteProdutos
            .Where(ip => ip.ProdutoId == produtoId)
            .Include(ip => ip.Igrediente)
            .Select(ip => new
            {
                IgredienteId = ip.IgredienteId,
                Nome = ip.Igrediente.Nome,
                QuantidadeBase = ip.Quantidade,
                ValorExtra = ip.Igrediente.Valor, // Valor para ingredientes extras
                Imagem = ip.Igrediente.Imagem != null ? Convert.ToBase64String(ip.Igrediente.Imagem) : null
            })
            .ToListAsync();

        return Json(ingredientes);
    }
    catch (Exception)
    {
        return Json(new List<object>());
    }
}



    }
    
    
}