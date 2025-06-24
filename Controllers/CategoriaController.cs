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
    public class CategoriaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Categoria
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Categorias.Include(c => c.CategoriaPai);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Categoria/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var categoria = await _context.Categorias
                .Include(c => c.CategoriaPai)
                .FirstOrDefaultAsync(m => m.CategoriaId == id);
            if (categoria == null) return NotFound();

            return View(categoria);
        }

        // GET: Categoria/Create
        public IActionResult Create()
        {
            ViewData["CategoriaPaiId"] = GetCategoriaPaiSelectList();
            return View();
        }

        // POST: Categoria/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoriaId,Nome,CategoriaPaiId")] Categoria categoria,
                                               IFormFile? imageFile, IFormFile? bannerFile)
        {
            if (ModelState.IsValid)
            {
                // Processar imagem principal
                if (imageFile != null && imageFile.Length > 0)
                {
                    categoria.Image = await ConvertToByteArray(imageFile);
                }

                // Processar banner
                if (bannerFile != null && bannerFile.Length > 0)
                {
                    categoria.Banner = await ConvertToByteArray(bannerFile);
                }

                _context.Add(categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoriaPaiId"] = GetCategoriaPaiSelectList(categoria.CategoriaPaiId);
            return View(categoria);
        }

        // GET: Categoria/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null) return NotFound();

            ViewData["CategoriaPaiId"] = GetCategoriaPaiSelectList(categoria.CategoriaPaiId);
            return View(categoria);
        }

        // POST: Categoria/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoriaId,Nome,CategoriaPaiId")] Categoria categoria,
                                             IFormFile? imageFile, IFormFile? bannerFile)
        {
            if (id != categoria.CategoriaId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Buscar a categoria existente para preservar imagens se não foram alteradas
                    var categoriaExistente = await _context.Categorias.AsNoTracking()
                        .FirstOrDefaultAsync(c => c.CategoriaId == id);

                    if (categoriaExistente != null)
                    {
                        // Preservar imagem existente se nova não foi enviada
                        categoria.Image = categoriaExistente.Image;
                        categoria.Banner = categoriaExistente.Banner;
                    }

                    // Processar nova imagem se foi enviada
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        categoria.Image = await ConvertToByteArray(imageFile);
                    }

                    // Processar novo banner se foi enviado
                    if (bannerFile != null && bannerFile.Length > 0)
                    {
                        categoria.Banner = await ConvertToByteArray(bannerFile);
                    }

                    _context.Update(categoria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriaExists(categoria.CategoriaId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoriaPaiId"] = GetCategoriaPaiSelectList(categoria.CategoriaPaiId);
            return View(categoria);
        }

        // GET: Categoria/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var categoria = await _context.Categorias
                .Include(c => c.CategoriaPai)
                .FirstOrDefaultAsync(m => m.CategoriaId == id);
            if (categoria == null) return NotFound();

            return View(categoria);
        }

        // POST: Categoria/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria != null)
            {
                _context.Categorias.Remove(categoria);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Método para exibir imagem
        public async Task<IActionResult> GetImage(int id, string type = "image")
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null) return NotFound();

            byte[]? imageData = type.ToLower() == "banner" ? categoria.Banner : categoria.Image;

            if (imageData == null || imageData.Length == 0)
                return NotFound();

            return File(imageData, "image/jpeg"); // ou detectar o tipo automaticamente
        }

        private bool CategoriaExists(int id)
        {
            return _context.Categorias.Any(e => e.CategoriaId == id);
        }

        // Método auxiliar para montar a SelectList
        private List<SelectListItem> GetCategoriaPaiSelectList(int? selectedId = null)
        {
            var categorias = _context.Categorias
                .OrderBy(c => c.Nome)
                .ToList();

            var lista = new List<SelectListItem>
            {
                new SelectListItem { Text = "Nenhuma (categoria raiz)", Value = "" }
            };

            lista.AddRange(categorias.Select(c => new SelectListItem
            {
                Text = c.Nome,
                Value = c.CategoriaId.ToString(),
                Selected = selectedId.HasValue && c.CategoriaId == selectedId.Value
            }));

            return lista;
        }

        // Método auxiliar para converter IFormFile em byte array
        private async Task<byte[]> ConvertToByteArray(IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}