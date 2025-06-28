using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TotemPWA.Models;
using TotemPWA.Data;

namespace TotemPWA.Controllers
{
    public class IngredienteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IngredienteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Ingrediente
        public async Task<IActionResult> Index()
        {
            try
            {
                var ingredientes = await _context.Igredientes.ToListAsync();
                return View(ingredientes);
            }
            catch (Exception ex)
            {
                // Para debug - você pode remover depois
                ViewBag.Error = ex.Message;
                return View(new List<Igrediente>());
            }
        }

        // GET: Ingrediente/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingrediente = await _context.Igredientes
                .FirstOrDefaultAsync(m => m.IgredienteId == id);
            if (ingrediente == null)
            {
                return NotFound();
            }

            return View(ingrediente);
        }

        // GET: Ingrediente/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ingrediente/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Igrediente ingrediente, IFormFile imagemFile)
        {
            // Remove validação automática do campo Imagem
            ModelState.Remove("Imagem");
            
            try
            {
                // Validação manual da imagem
                if (imagemFile == null || imagemFile.Length == 0)
                {
                    ModelState.AddModelError("imagemFile", "Por favor, selecione uma imagem");
                    return View(ingrediente);
                }

                // Validar tipo de arquivo
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(imagemFile.FileName).ToLowerInvariant();
                
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("imagemFile", "Apenas arquivos de imagem são permitidos (.jpg, .jpeg, .png, .gif)");
                    return View(ingrediente);
                }

                // Converter imagem para byte array
                using (var memoryStream = new MemoryStream())
                {
                    await imagemFile.CopyToAsync(memoryStream);
                    ingrediente.Imagem = memoryStream.ToArray();
                }

                if (ModelState.IsValid)
                {
                    _context.Add(ingrediente);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao salvar: " + ex.Message);
            }

            return View(ingrediente);
        }

        // GET: Ingrediente/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingrediente = await _context.Igredientes.FindAsync(id);
            if (ingrediente == null)
            {
                return NotFound();
            }
            return View(ingrediente);
        }

        // POST: Ingrediente/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Igrediente ingrediente, IFormFile? imagemFile)
        {
            if (id != ingrediente.IgredienteId)
            {
                return NotFound();
            }

            // Remove validação automática do campo Imagem
            ModelState.Remove("Imagem");

            if (ModelState.IsValid)
            {
                try
                {
                    // Buscar o ingrediente existente para manter a imagem atual se não for fornecida nova
                    var ingredienteExistente = await _context.Igredientes.AsNoTracking().FirstOrDefaultAsync(i => i.IgredienteId == id);
                    
                    if (imagemFile != null && imagemFile.Length > 0)
                    {
                        // Nova imagem fornecida
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                        var extension = Path.GetExtension(imagemFile.FileName).ToLowerInvariant();
                        
                        if (!allowedExtensions.Contains(extension))
                        {
                            ModelState.AddModelError("imagemFile", "Apenas arquivos de imagem são permitidos (.jpg, .jpeg, .png, .gif)");
                            return View(ingrediente);
                        }

                        using (var memoryStream = new MemoryStream())
                        {
                            await imagemFile.CopyToAsync(memoryStream);
                            ingrediente.Imagem = memoryStream.ToArray();
                        }
                    }
                    else
                    {
                        // Manter imagem existente
                        ingrediente.Imagem = ingredienteExistente?.Imagem;
                    }

                    _context.Update(ingrediente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IngredienteExists(ingrediente.IgredienteId))
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
            return View(ingrediente);
        }

        // GET: Ingrediente/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingrediente = await _context.Igredientes
                .FirstOrDefaultAsync(m => m.IgredienteId == id);
            if (ingrediente == null)
            {
                return NotFound();
            }

            return View(ingrediente);
        }

        // POST: Ingrediente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ingrediente = await _context.Igredientes.FindAsync(id);
            if (ingrediente != null)
            {
                _context.Igredientes.Remove(ingrediente);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Action para servir imagens
        public async Task<IActionResult> GetImagem(int id)
        {
            var ingrediente = await _context.Igredientes.FindAsync(id);
            if (ingrediente?.Imagem != null)
            {
                return File(ingrediente.Imagem, "image/jpeg");
            }
            return NotFound();
        }

        private bool IngredienteExists(int id)
        {
            return _context.Igredientes.Any(e => e.IgredienteId == id);
        }
    }
}