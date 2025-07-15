using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TotemPWA.Data;
using TotemPWA.Models;

namespace TotemPWA.Controllers
{
    public class CupomController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CupomController> _logger;

        public CupomController(ApplicationDbContext context, ILogger<CupomController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Cupom
        public async Task<IActionResult> Index()
        {
            try
            {
                var cupons = await _context.Cupons
                    .Include(c => c.Administrador)
                    .ToListAsync();
                return View(cupons);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar lista de cupons");
                TempData["ErrorMessage"] = "Erro ao carregar cupons: " + ex.Message;
                return View(new List<Cupom>());
            }
        }

        // GET: Cupom/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                await LoadAdministradores();
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar página de criação");
                TempData["ErrorMessage"] = "Erro ao carregar página: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Cupom/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cupom cupom)
        {
            try
            {
                _logger.LogInformation("Tentando criar cupom: {Codigo}", cupom.Codigo);

                // Validações manuais adicionais
                if (string.IsNullOrWhiteSpace(cupom.Codigo))
                {
                    ModelState.AddModelError("Codigo", "O código do cupom é obrigatório.");
                }

                if (cupom.Desconto <= 0 || cupom.Desconto > 100)
                {
                    ModelState.AddModelError("Desconto", "O desconto deve estar entre 0.01% e 100%.");
                }

                if (cupom.Validade <= DateTime.Now.Date)
                {
                    ModelState.AddModelError("Validade", "A data de validade deve ser futura.");
                }

                if (cupom.AdministradorId <= 0)
                {
                    ModelState.AddModelError("AdministradorId", "Selecione um administrador.");
                }

                // Verificar se o administrador existe
                var administradorExiste = await _context.Administradores
                    .AnyAsync(a => a.AdministradorId == cupom.AdministradorId);
                
                if (!administradorExiste)
                {
                    ModelState.AddModelError("AdministradorId", "Administrador selecionado não existe.");
                }

                // Verificar se o código já existe
                var codigoExiste = await _context.Cupons
                    .AnyAsync(c => c.Codigo.ToUpper() == cupom.Codigo.ToUpper());
                
                if (codigoExiste)
                {
                    ModelState.AddModelError("Codigo", "Já existe um cupom com este código.");
                }

                // Se há erros de validação, retornar a view
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Validação falhou para cupom: {Codigo}", cupom.Codigo);
                    await LoadAdministradores(cupom.AdministradorId);
                    return View(cupom);
                }

                // Normalizar o código para maiúsculas
                cupom.Codigo = cupom.Codigo.ToUpper().Trim();

                // Adicionar ao contexto
                _context.Cupons.Add(cupom);
                
                // Salvar alterações
                var resultado = await _context.SaveChangesAsync();
                
                _logger.LogInformation("Cupom criado com sucesso: {Codigo}, ID: {CupomId}", 
                    cupom.Codigo, cupom.CupomId);

                TempData["SuccessMessage"] = "Cupom criado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Erro de banco de dados ao criar cupom: {Codigo}", cupom.Codigo);
                
                var innerException = dbEx.InnerException?.Message ?? dbEx.Message;
                ModelState.AddModelError("", $"Erro ao salvar no banco de dados: {innerException}");
                
                await LoadAdministradores(cupom.AdministradorId);
                return View(cupom);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro geral ao criar cupom: {Codigo}", cupom.Codigo);
                ModelState.AddModelError("", $"Erro inesperado: {ex.Message}");
                
                await LoadAdministradores(cupom.AdministradorId);
                return View(cupom);
            }
        }

        // GET: Cupom/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var cupom = await _context.Cupons.FindAsync(id);
                if (cupom == null)
                {
                    return NotFound();
                }

                await LoadAdministradores(cupom.AdministradorId);
                return View(cupom);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar cupom para edição: {Id}", id);
                TempData["ErrorMessage"] = "Erro ao carregar cupom: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Cupom/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cupom cupom)
        {
            if (id != cupom.CupomId)
            {
                return NotFound();
            }

            try
            {
                // Validações similares ao Create
                if (string.IsNullOrWhiteSpace(cupom.Codigo))
                {
                    ModelState.AddModelError("Codigo", "O código do cupom é obrigatório.");
                }

                if (cupom.Desconto <= 0 || cupom.Desconto > 100)
                {
                    ModelState.AddModelError("Desconto", "O desconto deve estar entre 0.01% e 100%.");
                }

                if (cupom.AdministradorId <= 0)
                {
                    ModelState.AddModelError("AdministradorId", "Selecione um administrador.");
                }

                // Verificar se o código já existe (excluindo o atual)
                var codigoExiste = await _context.Cupons
                    .AnyAsync(c => c.Codigo.ToUpper() == cupom.Codigo.ToUpper() && c.CupomId != cupom.CupomId);
                
                if (codigoExiste)
                {
                    ModelState.AddModelError("Codigo", "Já existe um cupom com este código.");
                }

                if (!ModelState.IsValid)
                {
                    await LoadAdministradores(cupom.AdministradorId);
                    return View(cupom);
                }

                cupom.Codigo = cupom.Codigo.ToUpper().Trim();

                _context.Update(cupom);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Cupom atualizado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CupomExists(cupom.CupomId))
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
                _logger.LogError(ex, "Erro ao atualizar cupom: {Id}", id);
                ModelState.AddModelError("", "Erro ao atualizar cupom: " + ex.Message);
                await LoadAdministradores(cupom.AdministradorId);
                return View(cupom);
            }
        }

        // GET: Cupom/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var cupom = await _context.Cupons
                    .Include(c => c.Administrador)
                    .FirstOrDefaultAsync(m => m.CupomId == id);

                if (cupom == null)
                {
                    return NotFound();
                }

                return View(cupom);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar detalhes do cupom: {Id}", id);
                TempData["ErrorMessage"] = "Erro ao carregar detalhes: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Cupom/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var cupom = await _context.Cupons
                    .Include(c => c.Administrador)
                    .FirstOrDefaultAsync(m => m.CupomId == id);

                if (cupom == null)
                {
                    return NotFound();
                }

                return View(cupom);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar cupom para exclusão: {Id}", id);
                TempData["ErrorMessage"] = "Erro ao carregar cupom: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Cupom/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var cupom = await _context.Cupons.FindAsync(id);
                if (cupom != null)
                {
                    _context.Cupons.Remove(cupom);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cupom excluído com sucesso!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Cupom não encontrado.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir cupom: {Id}", id);
                TempData["ErrorMessage"] = "Erro ao excluir cupom: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CupomExists(int id)
        {
            return await _context.Cupons.AnyAsync(e => e.CupomId == id);
        }

        private async Task LoadAdministradores(int? selectedId = null)
        {
            try
            {
                var administradores = await _context.Administradores
                    .OrderBy(a => a.CPF)
                    .ToListAsync();

                ViewBag.AdministradorId = new SelectList(administradores, "AdministradorId", "CPF", selectedId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar administradores");
                ViewBag.AdministradorId = new SelectList(new List<Administrador>(), "AdministradorId", "CPF");
            }
        }

        // Método para testar conexão com banco
        [HttpGet]
        public async Task<IActionResult> TestDatabase()
        {
            try
            {
                var count = await _context.Cupons.CountAsync();
                return Json(new { success = true, message = $"Conexão OK. {count} cupons encontrados." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}