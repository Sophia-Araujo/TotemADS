using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TotemPWA.Data;
using TotemPWA.Models;
using Microsoft.AspNetCore.Authorization;

namespace TotemPWA.Controllers
{
    [Authorize]
    public class ProdutoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProdutoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Produto
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Produto.Include(p => p.Administrador).Include(p => p.Categoria);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Produto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produto
                .Include(p => p.Administrador)
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.ProdutoId == id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        // GET: Produto/Create
        public IActionResult Create()
        {
            ViewData["AdministradorId"] = new SelectList(_context.Administradores, "AdministradorId", "CPF");
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nome");
            return View();
        }

       // Substitua o método Create (POST) no ProdutoController.cs pela versão abaixo:

// POST: Produto/Create
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(Produto produto, IFormFile? imagemFile)
{
    Console.WriteLine("=== INÍCIO DO CREATE ===");
    Console.WriteLine($"Nome: {produto.Nome}");
    Console.WriteLine($"Descricao: {produto.Descricao}");
    Console.WriteLine($"Valor: {produto.Valor}");
    Console.WriteLine($"IsCombo: {produto.IsCombo}");
    Console.WriteLine($"AdministradorId: {produto.AdministradorId}");
    Console.WriteLine($"CategoriaId: {produto.CategoriaId}");
    Console.WriteLine($"Imagem File: {(imagemFile != null ? imagemFile.FileName : "null")}");

    // Remove validações das propriedades de navegação
    RemoveNavigationPropertiesValidation();

    // Processa a imagem se fornecida
    var imagemProcessada = await ProcessarImagem(imagemFile);
    if (imagemProcessada.HasError)
    {
        ModelState.AddModelError("imagemFile", imagemProcessada.ErrorMessage!);
    }
    else
    {
        produto.Imagem = imagemProcessada.ImagemBytes;
    }

    Console.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");

    if (ModelState.IsValid)
    {
        try
        {
            Console.WriteLine("ModelState válido - tentando salvar...");
            
            _context.Add(produto);
            await _context.SaveChangesAsync();
            
            Console.WriteLine("Produto salvo com sucesso!");
            TempData["SuccessMessage"] = "Produto criado com sucesso!";
            
            // Redireciona para ingredientes se NÃO for combo
            if (produto.IsCombo == 0)
            {
                TempData["InfoMessage"] = "Agora você pode configurar os ingredientes do produto.";
                return RedirectToAction("Index", "IgredienteProduto", new { produtoId = produto.ProdutoId });
            }
            
            // Se for combo, vai direto para a lista de produtos
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERRO AO SALVAR: {ex.Message}");
            if (ex.InnerException != null)
                Console.WriteLine($"INNER EXCEPTION: {ex.InnerException.Message}");
            
            ModelState.AddModelError("", $"Erro ao salvar: {ex.Message}");
        }
    }
    
    // Recarrega dropdowns
    ViewData["AdministradorId"] = new SelectList(_context.Administradores, "AdministradorId", "CPF", produto.AdministradorId);
    ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nome", produto.CategoriaId);
    
    Console.WriteLine("=== FIM DO CREATE ===");
    return View(produto);
}
        // GET: Produto/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produto.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }
            
            ViewData["AdministradorId"] = new SelectList(_context.Administradores, "AdministradorId", "CPF", produto.AdministradorId);
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nome", produto.CategoriaId);
            return View(produto);
        }

        // POST: Produto/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Produto produto, IFormFile? imagemFile)
        {
            Console.WriteLine("=== INÍCIO DO EDIT ===");
            Console.WriteLine($"ID: {id}");
            Console.WriteLine($"ProdutoId: {produto.ProdutoId}");
            Console.WriteLine($"Nome: {produto.Nome}");
            Console.WriteLine($"Nova Imagem: {(imagemFile != null ? imagemFile.FileName : "null")}");

            if (id != produto.ProdutoId)
            {
                Console.WriteLine("IDs não conferem!");
                return NotFound();
            }

            // Remove validações das propriedades de navegação
            RemoveNavigationPropertiesValidation();

            if (ModelState.IsValid)
            {
                try
                {
                    Console.WriteLine("ModelState válido - buscando produto original...");
                    
                    // Busca o produto original para preservar a imagem se necessário
                    var produtoOriginal = await _context.Produto
                        .AsNoTracking()
                        .FirstOrDefaultAsync(p => p.ProdutoId == id);
                    
                    if (produtoOriginal == null)
                    {
                        Console.WriteLine("Produto original não encontrado!");
                        return NotFound();
                    }

                    // Processa a nova imagem ou mantém a original
                    if (imagemFile != null && imagemFile.Length > 0)
                    {
                        var imagemProcessada = await ProcessarImagem(imagemFile);
                        if (imagemProcessada.HasError)
                        {
                            ModelState.AddModelError("imagemFile", imagemProcessada.ErrorMessage!);
                            ViewData["AdministradorId"] = new SelectList(_context.Administradores, "AdministradorId", "CPF", produto.AdministradorId);
                            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nome", produto.CategoriaId);
                            return View(produto);
                        }
                        produto.Imagem = imagemProcessada.ImagemBytes;
                        Console.WriteLine($"Nova imagem processada: {produto.Imagem?.Length ?? 0} bytes");
                    }
                    else
                    {
                        // Mantém a imagem original se nenhuma nova imagem foi fornecida
                        produto.Imagem = produtoOriginal.Imagem;
                        Console.WriteLine("Mantendo imagem original");
                    }

                    Console.WriteLine("Atualizando produto...");
                    
                    _context.Update(produto);
                    await _context.SaveChangesAsync();
                    
                    Console.WriteLine("Produto atualizado com sucesso!");
                    TempData["SuccessMessage"] = "Produto atualizado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdutoExists(produto.ProdutoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERRO AO ATUALIZAR: {ex.Message}");
                    if (ex.InnerException != null)
                        Console.WriteLine($"INNER EXCEPTION: {ex.InnerException.Message}");
                    
                    ModelState.AddModelError("", $"Erro ao atualizar: {ex.Message}");
                }
            }
            
            ViewData["AdministradorId"] = new SelectList(_context.Administradores, "AdministradorId", "CPF", produto.AdministradorId);
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nome", produto.CategoriaId);
            
            Console.WriteLine("=== FIM DO EDIT ===");
            return View(produto);
        }

        // GET: Produto/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produto
                .Include(p => p.Administrador)
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.ProdutoId == id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        // POST: Produto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produto = await _context.Produto.FindAsync(id);
            if (produto != null)
            {
                _context.Produto.Remove(produto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProdutoExists(int id)
        {
            return _context.Produto.Any(e => e.ProdutoId == id);
        }

        // Action para exibir imagem
        public async Task<IActionResult> ExibirImagem(int id)
        {
            var produto = await _context.Produto.FindAsync(id);
            if (produto?.Imagem != null && produto.Imagem.Length > 0)
            {
                return File(produto.Imagem, "image/jpeg");
            }
            
            return NotFound();
        }

        #region Métodos Auxiliares

        /// <summary>
        /// Remove validações das propriedades de navegação que não devem ser validadas
        /// </summary>
        private void RemoveNavigationPropertiesValidation()
        {
            string[] keysToRemove = { 
                "Imagem", 
                "Administrador", 
                "Categoria", 
                "IgredienteProdutos", 
                "ItensPedidos", 
                "ItensCombo"
            };
            
            foreach (var key in keysToRemove)
            {
                ModelState.Remove(key);
            }
        }

        /// <summary>
        /// Processa o arquivo de imagem carregado
        /// </summary>
        /// <param name="imagemFile">Arquivo de imagem</param>
        /// <returns>Resultado do processamento da imagem</returns>
        private async Task<ImagemProcessadaResult> ProcessarImagem(IFormFile? imagemFile)
        {
            if (imagemFile == null || imagemFile.Length == 0)
            {
                return new ImagemProcessadaResult { ImagemBytes = null };
            }

            Console.WriteLine($"Processando imagem: {imagemFile.FileName}, Tamanho: {imagemFile.Length}");
            
            var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif" };
            
            if (!allowedTypes.Contains(imagemFile.ContentType.ToLower()))
            {
                Console.WriteLine($"Tipo de arquivo inválido: {imagemFile.ContentType}");
                return new ImagemProcessadaResult 
                { 
                    HasError = true, 
                    ErrorMessage = "Formato de imagem inválido. Use JPEG, PNG ou GIF." 
                };
            }
            
            if (imagemFile.Length > 5 * 1024 * 1024) // 5MB
            {
                Console.WriteLine("Imagem muito grande");
                return new ImagemProcessadaResult 
                { 
                    HasError = true, 
                    ErrorMessage = "Imagem muito grande. Máximo permitido: 5MB." 
                };
            }

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await imagemFile.CopyToAsync(memoryStream);
                    var imagemBytes = memoryStream.ToArray();
                    Console.WriteLine($"Imagem processada com sucesso: {imagemBytes.Length} bytes");
                    
                    return new ImagemProcessadaResult { ImagemBytes = imagemBytes };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar imagem: {ex.Message}");
                return new ImagemProcessadaResult 
                { 
                    HasError = true, 
                    ErrorMessage = "Erro ao processar a imagem." 
                };
            }
        }

        #endregion
    }

    /// <summary>
    /// Classe para retornar o resultado do processamento de imagem
    /// </summary>
    public class ImagemProcessadaResult
    {
        public byte[]? ImagemBytes { get; set; }
        public bool HasError { get; set; } = false;
        public string? ErrorMessage { get; set; }
    }
}