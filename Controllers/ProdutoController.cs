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

        // POST: Produto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Produto produto, IFormFile imagemFile)
        {
            Console.WriteLine("=== INÍCIO DO CREATE ===");
            Console.WriteLine($"Nome: {produto.Nome}");
            Console.WriteLine($"Descricao: {produto.Descricao}");
            Console.WriteLine($"Valor: {produto.Valor}");
            Console.WriteLine($"IsCombo: {produto.IsCombo}");
            Console.WriteLine($"AdministradorId: {produto.AdministradorId}");
            Console.WriteLine($"CategoriaId: {produto.CategoriaId}");
            Console.WriteLine($"Imagem File: {(imagemFile != null ? imagemFile.FileName : "null")}");

            // Debug do ModelState ANTES de remover validações
            Console.WriteLine("=== ERROS MODELSTATE INICIAL ===");
            foreach (var error in ModelState.Where(x => x.Value.Errors.Count > 0))
            {
                Console.WriteLine($"{error.Key}: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
            }

            // Remove TODAS as validações problemáticas
            string[] keysToRemove = { "Imagem", "Administrador", "Categoria", "IgredienteProdutos", 
                                    "ItensPedidos", "ItensCombo", "Administrador.CPF", "Categoria.Nome" };
            
            foreach (var key in keysToRemove)
            {
                ModelState.Remove(key);
            }

            // Processa a imagem
            if (imagemFile != null && imagemFile.Length > 0)
            {
                Console.WriteLine($"Processando imagem: {imagemFile.FileName}, Tamanho: {imagemFile.Length}");
                
                var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif" };
                if (!allowedTypes.Contains(imagemFile.ContentType.ToLower()))
                {
                    ModelState.AddModelError("imagemFile", "Arquivo de imagem inválido.");
                    Console.WriteLine($"Tipo de arquivo inválido: {imagemFile.ContentType}");
                }
                else if (imagemFile.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("imagemFile", "Imagem muito grande (máx 5MB).");
                    Console.WriteLine("Imagem muito grande");
                }
                else
                {
                    try
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await imagemFile.CopyToAsync(memoryStream);
                            produto.Imagem = memoryStream.ToArray();
                            Console.WriteLine($"Imagem processada com sucesso: {produto.Imagem.Length} bytes");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao processar imagem: {ex.Message}");
                        ModelState.AddModelError("imagemFile", "Erro ao processar imagem.");
                    }
                }
            }
            else
            {
                produto.Imagem = null;
                Console.WriteLine("Nenhuma imagem fornecida - definindo como null");
            }

            // Debug do ModelState APÓS processamento
            Console.WriteLine("=== ERROS MODELSTATE APÓS PROCESSAMENTO ===");
            foreach (var error in ModelState.Where(x => x.Value.Errors.Count > 0))
            {
                Console.WriteLine($"{error.Key}: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
            }

            Console.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");

            if (ModelState.IsValid)
            {
                try
                {
                    Console.WriteLine("ModelState válido - tentando salvar...");
                    
                    // Cria novo produto limpo
                    var novoProduto = new Produto
                    {
                        Nome = produto.Nome,
                        Descricao = produto.Descricao,
                        Valor = produto.Valor,
                        IsCombo = produto.IsCombo,
                        AdministradorId = produto.AdministradorId,
                        CategoriaId = produto.CategoriaId,
                        Imagem = produto.Imagem
                    };

                    Console.WriteLine("Produto criado - adicionando ao contexto...");
                    _context.Add(novoProduto);
                    
                    Console.WriteLine("Salvando no banco...");
                    await _context.SaveChangesAsync();
                    
                    Console.WriteLine("Produto salvo com sucesso!");
                    TempData["SuccessMessage"] = "Produto criado com sucesso!";
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
            else
            {
                Console.WriteLine("ModelState INVÁLIDO - retornando para view");
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
        public async Task<IActionResult> Edit(int id, Produto produto, IFormFile imagemFile)
        {
            Console.WriteLine("=== INÍCIO DO EDIT ===");
            Console.WriteLine($"ID: {id}");
            Console.WriteLine($"ProdutoId: {produto.ProdutoId}");
            Console.WriteLine($"Nome: {produto.Nome}");
            Console.WriteLine($"Descricao: {produto.Descricao}");
            Console.WriteLine($"Valor: {produto.Valor}");
            Console.WriteLine($"IsCombo: {produto.IsCombo}");
            Console.WriteLine($"AdministradorId: {produto.AdministradorId}");
            Console.WriteLine($"CategoriaId: {produto.CategoriaId}");
            Console.WriteLine($"Nova Imagem: {(imagemFile != null ? imagemFile.FileName : "null")}");

            if (id != produto.ProdutoId)
            {
                Console.WriteLine("IDs não conferem!");
                return NotFound();
            }

            // Debug do ModelState ANTES de limpar
            Console.WriteLine("=== ERROS MODELSTATE INICIAL ===");
            foreach (var error in ModelState.Where(x => x.Value.Errors.Count > 0))
            {
                Console.WriteLine($"{error.Key}: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
            }

            // Remove validações problemáticas
            string[] keysToRemove = { "Imagem", "Administrador", "Categoria", "IgredienteProdutos", 
                                    "ItensPedidos", "ItensCombo", "Administrador.CPF", "Categoria.Nome" };
            
            foreach (var key in keysToRemove)
            {
                ModelState.Remove(key);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Console.WriteLine("ModelState válido - buscando produto original...");
                    
                    // Busca produto original
                    var produtoOriginal = await _context.Produto
                        .AsNoTracking()
                        .FirstOrDefaultAsync(p => p.ProdutoId == id);
                    
                    if (produtoOriginal == null)
                    {
                        Console.WriteLine("Produto original não encontrado!");
                        return NotFound();
                    }

                    Console.WriteLine($"Produto original encontrado. Imagem atual: {(produtoOriginal.Imagem != null ? produtoOriginal.Imagem.Length + " bytes" : "null")}");

                    // Gerencia imagem
                    if (imagemFile != null && imagemFile.Length > 0)
                    {
                        Console.WriteLine("Processando nova imagem...");
                        
                        var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif" };
                        if (!allowedTypes.Contains(imagemFile.ContentType.ToLower()))
                        {
                            ModelState.AddModelError("imagemFile", "Arquivo de imagem inválido.");
                            Console.WriteLine($"Tipo inválido: {imagemFile.ContentType}");
                        }
                        else if (imagemFile.Length > 5 * 1024 * 1024)
                        {
                            ModelState.AddModelError("imagemFile", "Imagem muito grande.");
                            Console.WriteLine("Imagem muito grande");
                        }
                        else
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                await imagemFile.CopyToAsync(memoryStream);
                                produto.Imagem = memoryStream.ToArray();
                                Console.WriteLine($"Nova imagem processada: {produto.Imagem.Length} bytes");
                            }
                        }
                    }
                    else
                    {
                        // Mantém imagem original
                        produto.Imagem = produtoOriginal.Imagem;
                        Console.WriteLine("Mantendo imagem original");
                    }

                    if (ModelState.IsValid)
                    {
                        Console.WriteLine("Atualizando produto...");
                        
                        // Cria produto atualizado limpo
                        var produtoAtualizado = new Produto
                        {
                            ProdutoId = produto.ProdutoId,
                            Nome = produto.Nome,
                            Descricao = produto.Descricao,
                            Valor = produto.Valor,
                            IsCombo = produto.IsCombo,
                            AdministradorId = produto.AdministradorId,
                            CategoriaId = produto.CategoriaId,
                            Imagem = produto.Imagem
                        };

                        _context.Update(produtoAtualizado);
                        await _context.SaveChangesAsync();
                        
                        Console.WriteLine("Produto atualizado com sucesso!");
                        TempData["SuccessMessage"] = "Produto atualizado com sucesso!";
                        return RedirectToAction(nameof(Index));
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
            else
            {
                Console.WriteLine("ModelState INVÁLIDO após limpeza:");
                foreach (var error in ModelState.Where(x => x.Value.Errors.Count > 0))
                {
                    Console.WriteLine($"{error.Key}: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
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
    }
}