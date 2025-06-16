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
    public class CupomController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CupomController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cupom
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Cupons.Include(c => c.Administrador);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Cupom/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cupom = await _context.Cupons
                .Include(c => c.Administrador)
                .FirstOrDefaultAsync(m => m.CupomId == id);
            if (cupom == null)
            {
                return NotFound();
            }

            return View(cupom);
        }

        // GET: Cupom/Create
        public IActionResult Create()
        {
            ViewData["AdministradorId"] = new SelectList(_context.Administradores, "AdministradorId", "CPF");
            return View();
        }

        // POST: Cupom/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CupomId,Codigo,Desconto,Validade,ProdutoId,AdministradorId")] Cupom cupom)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cupom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AdministradorId"] = new SelectList(_context.Administradores, "AdministradorId", "CPF", cupom.AdministradorId);
            return View(cupom);
        }

        // GET: Cupom/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cupom = await _context.Cupons.FindAsync(id);
            if (cupom == null)
            {
                return NotFound();
            }
            ViewData["AdministradorId"] = new SelectList(_context.Administradores, "AdministradorId", "CPF", cupom.AdministradorId);
            return View(cupom);
        }

        // POST: Cupom/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CupomId,Codigo,Desconto,Validade,ProdutoId,AdministradorId")] Cupom cupom)
        {
            if (id != cupom.CupomId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cupom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CupomExists(cupom.CupomId))
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
            ViewData["AdministradorId"] = new SelectList(_context.Administradores, "AdministradorId", "CPF", cupom.AdministradorId);
            return View(cupom);
        }

        // GET: Cupom/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cupom = await _context.Cupons
                .Include(c => c.Administrador)
                .FirstOrDefaultAsync(m => m.CupomId == id);
            if (cupom == null)
            {
                return NotFound();
            }

            return View(cupom);
        }

        // POST: Cupom/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cupom = await _context.Cupons.FindAsync(id);
            if (cupom != null)
            {
                _context.Cupons.Remove(cupom);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CupomExists(int id)
        {
            return _context.Cupons.Any(e => e.CupomId == id);
        }
    }
}
