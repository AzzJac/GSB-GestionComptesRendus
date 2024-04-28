using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GSB_GCR.Models;

namespace GSB_GCR.Controllers
{
    public class RapportVisitesController : Controller
    {
        private readonly GSBCRContext _context;

        public RapportVisitesController(GSBCRContext context)
        {
            _context = context;
        }

        // GET: RapportVisites
        public async Task<IActionResult> Index()
        {
            var gSBCRContext = _context.RapportVisites.Include(r => r.PraNumNavigation).Include(r => r.VisMatriculeNavigation);
            return View(await gSBCRContext.ToListAsync());
        }

        // GET: RapportVisites/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.RapportVisites == null)
            {
                return NotFound();
            }

            var rapportVisite = await _context.RapportVisites
                .Include(r => r.PraNumNavigation)
                .Include(r => r.VisMatriculeNavigation)
                .FirstOrDefaultAsync(m => m.VisMatricule == id);
            if (rapportVisite == null)
            {
                return NotFound();
            }

            return View(rapportVisite);
        }

        // GET: RapportVisites/Create
        public IActionResult Create()
        {
            ViewData["PraNum"] = new SelectList(_context.Praticiens, "PraNum", "PraNom");
            ViewData["VisMatricule"] = new SelectList(_context.Visiteurs, "VisMatricule", "VisNom");
            return View();
        }

        // POST: RapportVisites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VisMatricule,RapNum,PraNum,RapDate,RapBilan,RapMotif")] RapportVisite rapportVisite)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rapportVisite);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PraNum"] = new SelectList(_context.Praticiens, "PraNum", "PraNum", rapportVisite.PraNum);
            ViewData["VisMatricule"] = new SelectList(_context.Visiteurs, "VisMatricule", "VisMatricule", rapportVisite.VisMatricule);
            return View(rapportVisite);
        }

        // GET: RapportVisites/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.RapportVisites == null)
            {
                return NotFound();
            }

            var rapportVisite = await _context.RapportVisites.FindAsync(id);
            if (rapportVisite == null)
            {
                return NotFound();
            }
            ViewData["PraNum"] = new SelectList(_context.Praticiens, "PraNum", "PraNum", rapportVisite.PraNum);
            ViewData["VisMatricule"] = new SelectList(_context.Visiteurs, "VisMatricule", "VisMatricule", rapportVisite.VisMatricule);
            return View(rapportVisite);
        }

        // POST: RapportVisites/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("VisMatricule,RapNum,PraNum,RapDate,RapBilan,RapMotif")] RapportVisite rapportVisite)
        {
            if (id != rapportVisite.VisMatricule)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rapportVisite);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RapportVisiteExists(rapportVisite.VisMatricule))
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
            ViewData["PraNum"] = new SelectList(_context.Praticiens, "PraNum", "PraNum", rapportVisite.PraNum);
            ViewData["VisMatricule"] = new SelectList(_context.Visiteurs, "VisMatricule", "VisMatricule", rapportVisite.VisMatricule);
            return View(rapportVisite);
        }

        // GET: RapportVisites/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.RapportVisites == null)
            {
                return NotFound();
            }

            var rapportVisite = await _context.RapportVisites
                .Include(r => r.PraNumNavigation)
                .Include(r => r.VisMatriculeNavigation)
                .FirstOrDefaultAsync(m => m.VisMatricule == id);
            if (rapportVisite == null)
            {
                return NotFound();
            }

            return View(rapportVisite);
        }

        // POST: RapportVisites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.RapportVisites == null)
            {
                return Problem("Entity set 'GSBCRContext.RapportVisites'  is null.");
            }
            var rapportVisite = await _context.RapportVisites.FindAsync(id);
            if (rapportVisite != null)
            {
                _context.RapportVisites.Remove(rapportVisite);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RapportVisiteExists(string id)
        {
          return (_context.RapportVisites?.Any(e => e.VisMatricule == id)).GetValueOrDefault();
        }
    }
}
