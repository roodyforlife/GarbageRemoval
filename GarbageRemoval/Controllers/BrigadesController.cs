using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GarbageRemoval.DataBase;
using GarbageRemoval.Models;

namespace GarbageRemoval.Controllers
{
    public class BrigadesController : Controller
    {
        private readonly ApplicationContext _context;

        public BrigadesController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Brigades
        public async Task<IActionResult> Index()
        {
            var applicationContext = _context.Brigades.Include(b => b.Administration);
            return View(await applicationContext.ToListAsync());
        }

        // GET: Brigades/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brigade = await _context.Brigades
                .Include(b => b.Administration)
                .FirstOrDefaultAsync(m => m.BrigadeId == id);
            if (brigade == null)
            {
                return NotFound();
            }

            return View(brigade);
        }

        // GET: Brigades/Create
        public IActionResult Create()
        {
            ViewData["AdministrationId"] = new SelectList(_context.Administrations, "AdministrationId", "Email");
            return View();
        }

        // POST: Brigades/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BrigadeId,BrigadeName,CreateDate,UniformColor,AdministrationId")] Brigade brigade)
        {
            if (await _context.Brigades.FirstOrDefaultAsync(x => x.BrigadeName == brigade.BrigadeName
                && brigade.AdministrationId == brigade.AdministrationId) is not null)
            {
                ModelState.AddModelError("BrigadeName", "Name already taken");
            }

            brigade.CreateDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.Add(brigade);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AdministrationId"] = new SelectList(_context.Administrations, "AdministrationId", "Email", brigade.AdministrationId);
            return View(brigade);
        }

        // GET: Brigades/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brigade = await _context.Brigades.FindAsync(id);
            if (brigade == null)
            {
                return NotFound();
            }
            ViewData["AdministrationId"] = new SelectList(_context.Administrations, "AdministrationId", "Email", brigade.AdministrationId);
            return View(brigade);
        }

        // POST: Brigades/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BrigadeId,BrigadeName,CreateDate,UniformColor,AdministrationId")] Brigade brigade)
        {
            if (id != brigade.BrigadeId)
            {
                return NotFound();
            }

            brigade.CreateDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(brigade);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrigadeExists(brigade.BrigadeId))
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
            ViewData["AdministrationId"] = new SelectList(_context.Administrations, "AdministrationId", "Email", brigade.AdministrationId);
            return View(brigade);
        }

        // GET: Brigades/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brigade = await _context.Brigades
                .Include(b => b.Administration)
                .FirstOrDefaultAsync(m => m.BrigadeId == id);
            if (brigade == null)
            {
                return NotFound();
            }

            return View(brigade);
        }

        // POST: Brigades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var brigade = await _context.Brigades.FindAsync(id);
            _context.Brigades.Remove(brigade);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BrigadeExists(int id)
        {
            return _context.Brigades.Any(e => e.BrigadeId == id);
        }
    }
}
