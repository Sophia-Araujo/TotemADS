using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TotemPWA.Data;
using TotemPWA.Models;

namespace TotemPWA.Controllers
{
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
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaId");
            return View();
        }

        // POST: Produto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProdutoId,Descricao,Valor,IsCombo,AdministradorId,CategoriaId")] Produto produto, IFormFile imagemFile)
        {
            // Remove erro de validação da Imagem se existir, pois trataremos manualmente
            ModelState.Remove("Imagem");
            
            // Validação manual da imagem
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

            if (ModelState.IsValid)
            {
                // Converte o arquivo para byte array
                using (var memoryStream = new MemoryStream())
                {
                    await imagemFile.CopyToAsync(memoryStream);
                    produto.Imagem = memoryStream.ToArray();
                }

                _context.Add(produto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["AdministradorId"] = new SelectList(_context.Administradores, "AdministradorId", "CPF", produto.AdministradorId);
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaId", produto.CategoriaId);
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
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaId", produto.CategoriaId);
            return View(produto);
        }

        // POST: Produto/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProdutoId,Descricao,Valor,IsCombo,AdministradorId,CategoriaId")] Produto produto, IFormFile imagemFile)
        {
            if (id != produto.ProdutoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Busca o produto existente para manter a imagem atual se não foi enviada nova
                    var produtoExistente = await _context.Produto.AsNoTracking().FirstOrDefaultAsync(p => p.ProdutoId == id);
                    
                    if (imagemFile != null && imagemFile.Length > 0)
                    {
                        // Verifica se é um arquivo de imagem válido
                        var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif" };
                        if (!allowedTypes.Contains(imagemFile.ContentType.ToLower()))
                        {
                            ModelState.AddModelError("imagemFile", "Por favor, selecione um arquivo de imagem válido (JPEG, PNG, GIF).");
                            ViewData["AdministradorId"] = new SelectList(_context.Administradores, "AdministradorId", "CPF", produto.AdministradorId);
                            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaId", produto.CategoriaId);
                            return View(produto);
                        }

                        // Converte o arquivo para byte array
                        using (var memoryStream = new MemoryStream())
                        {
                            await imagemFile.CopyToAsync(memoryStream);
                            produto.Imagem = memoryStream.ToArray();
                        }
                    }
                    else
                    {
                        // Mantém a imagem existente se nenhuma nova foi enviada
                        produto.Imagem = produtoExistente?.Imagem;
                    }

                    _context.Update(produto);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["AdministradorId"] = new SelectList(_context.Administradores, "AdministradorId", "CPF", produto.AdministradorId);
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaId", produto.CategoriaId);
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
            if (produto?.Imagem != null)
            {
                return File(produto.Imagem, "image/jpeg");
            }
            return NotFound();
        }
    }
}