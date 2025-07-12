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
using System.Security.Claims;

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
        public async Task<IActionResult> Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nome");
            
            // Obter dados do administrador logado através das Claims
            var adminIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var cpfClaim = User.FindFirst("CPF")?.Value;

            ViewBag.AdministradorId = int.TryParse(adminIdClaim, out var adminId) ? adminId : 0;
            ViewBag.AdministradorCPF = cpfClaim;

            return View(new Produto
            {
                AdministradorId = ViewBag.AdministradorId
            });
        }

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

            // Se o AdministradorId não foi preenchido, obter das Claims
            if (produto.AdministradorId == 0)
            {
                var adminIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(adminIdClaim, out var adminId))
                {
                    produto.AdministradorId = adminId;
                }
            }

            // Remove validações das propriedades de navegação
            RemoveNavigationPropertiesValidation();

            // Validação manual da imagem (usando lógica do segundo controller que funciona)
            if (imagemFile == null || imagemFile.Length == 0)
            {
                ModelState.AddModelError("imagemFile", "Por favor, selecione uma imagem.");
            }
            else
            {
                // Verifica se é um arquivo de imagem válido
                var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif" };
                if (!allowedTypes.Contains(imagemFile.ContentType.ToLower()))
                {
                    ModelState.AddModelError("imagemFile", "Por favor, selecione um arquivo de imagem válido (JPEG, PNG, GIF).");
                }
                else if (imagemFile.Length > 5 * 1024 * 1024) // 5MB
                {
                    ModelState.AddModelError("imagemFile", "A imagem deve ter no máximo 5MB.");
                }
            }

            Console.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");

            if (ModelState.IsValid)
            {
                try
                {
                    Console.WriteLine("ModelState válido - processando imagem...");
                    
                    // Converte o arquivo para byte array (usando método do segundo controller)
                    using (var memoryStream = new MemoryStream())
                    {
                        await imagemFile.CopyToAsync(memoryStream);
                        produto.Imagem = memoryStream.ToArray();
                    }
                    
                    Console.WriteLine("Imagem processada - tentando salvar...");
                    
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
            
            // Recarrega dados para a view
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nome", produto.CategoriaId);
            
            var adminIdClaimReload = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var cpfClaim = User.FindFirst("CPF")?.Value;
            ViewBag.AdministradorId = int.TryParse(adminIdClaimReload, out var adminIdReload) ? adminIdReload : 0;
            ViewBag.AdministradorCPF = cpfClaim;
            
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
            
            // Obter dados do administrador logado através das Claims
            var adminIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var cpfClaim = User.FindFirst("CPF")?.Value;

            ViewBag.AdministradorId = int.TryParse(adminIdClaim, out var adminId) ? adminId : 0;
            ViewBag.AdministradorCPF = cpfClaim;
            
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

            // Se o AdministradorId não foi preenchido, obter das Claims
            if (produto.AdministradorId == 0)
            {
                var adminIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(adminIdClaim, out var adminId))
                {
                    produto.AdministradorId = adminId;
                }
            }

            // Remove validações das propriedades de navegação
            RemoveNavigationPropertiesValidation();

            if (ModelState.IsValid)
            {
                try
                {
                    Console.WriteLine("ModelState válido - buscando produto original...");
                    
                    // Busca o produto existente para manter a imagem atual se não foi enviada nova
                    var produtoExistente = await _context.Produto.AsNoTracking().FirstOrDefaultAsync(p => p.ProdutoId == id);
                    
                    if (produtoExistente == null)
                    {
                        Console.WriteLine("Produto original não encontrado!");
                        return NotFound();
                    }

                    // Processa a nova imagem ou mantém a original (usando lógica do segundo controller)
                    if (imagemFile != null && imagemFile.Length > 0)
                    {
                        // Verifica se é um arquivo de imagem válido
                        var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif" };
                        if (!allowedTypes.Contains(imagemFile.ContentType.ToLower()))
                        {
                            ModelState.AddModelError("imagemFile", "Por favor, selecione um arquivo de imagem válido (JPEG, PNG, GIF).");
                            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nome", produto.CategoriaId);
                            
                            var adminIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                            var cpfClaim = User.FindFirst("CPF")?.Value;
                            ViewBag.AdministradorId = int.TryParse(adminIdClaim, out var adminIdReload) ? adminIdReload : 0;
                            ViewBag.AdministradorCPF = cpfClaim;
                            
                            return View(produto);
                        }

                        // Converte o arquivo para byte array
                        using (var memoryStream = new MemoryStream())
                        {
                            await imagemFile.CopyToAsync(memoryStream);
                            produto.Imagem = memoryStream.ToArray();
                        }
                        Console.WriteLine($"Nova imagem processada: {produto.Imagem?.Length ?? 0} bytes");
                    }
                    else
                    {
                        // Mantém a imagem existente se nenhuma nova foi enviada
                        produto.Imagem = produtoExistente.Imagem;
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
            
            // Recarrega dados para a view
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nome", produto.CategoriaId);
            
            var adminIdClaimFinal = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var cpfClaimFinal = User.FindFirst("CPF")?.Value;
            ViewBag.AdministradorId = int.TryParse(adminIdClaimFinal, out var adminIdFinal) ? adminIdFinal : 0;
            ViewBag.AdministradorCPF = cpfClaimFinal;
            
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
            TempData["SuccessMessage"] = "Produto excluído com sucesso!";
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

        #endregion
    }
}