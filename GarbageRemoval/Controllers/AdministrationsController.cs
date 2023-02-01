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
    public class AdministrationsController : Controller
    {
        private readonly ApplicationContext _context;

        public AdministrationsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Administrations
        public async Task<IActionResult> Index()
        {
            var applicationContext = _context.Administrations.Include(a => a.Company);
            return View(await applicationContext.ToListAsync());
        }

        // GET: Administrations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administration = await _context.Administrations
                .Include(a => a.Company)
                .Include(x => x.Brigades)
                .FirstOrDefaultAsync(m => m.AdministrationId == id);
            if (administration == null)
            {
                return NotFound();
            }

            return View(administration);
        }

        // GET: Administrations/Create
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "Name");
            return View();
        }

        // POST: Administrations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AdministrationId,Surname,Name,Patronymic,HomeAddress,Email,Telephone,CompanyId")] Administration administration)
        {
            if (ModelState.IsValid)
            {
                _context.Add(administration);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "Name", administration.CompanyId);
            return View(administration);
        }

        // GET: Administrations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administration = await _context.Administrations.FindAsync(id);
            if (administration == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "Name", administration.CompanyId);
            return View(administration);
        }

        // POST: Administrations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AdministrationId,Surname,Name,Patronymic,HomeAddress,Email,Telephone,CompanyId")] Administration administration)
        {
            if (id != administration.AdministrationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(administration);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdministrationExists(administration.AdministrationId))
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
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "Name", administration.CompanyId);
            return View(administration);
        }

        // GET: Administrations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administration = await _context.Administrations
                .Include(a => a.Company)
                .FirstOrDefaultAsync(m => m.AdministrationId == id);
            if (administration == null)
            {
                return NotFound();
            }

            return View(administration);
        }

        // POST: Administrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var administration = await _context.Administrations.FindAsync(id);
            _context.Administrations.Remove(administration);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdministrationExists(int id)
        {
            return _context.Administrations.Any(e => e.AdministrationId == id);
        }
    }
}
