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
    public class AreasController : Controller
    {
        private readonly ApplicationContext _context;

        public AreasController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Areas
        public async Task<IActionResult> Index(string areaName, string brigadeName)
        {
            IQueryable<Area> applicationContext = _context.Areas.Include(a => a.Brigade);

            if (areaName is not null)
            {
                applicationContext = applicationContext.Where(x => x.AreaName.Contains(areaName));
            }

            if (brigadeName is not null)
            {
                applicationContext = applicationContext.Where(x => x.Brigade.BrigadeName.Contains(brigadeName));
            }

            ViewBag.AreaName = areaName;
            ViewBag.BrigadeName = brigadeName;

            return View(await applicationContext.ToListAsync());
        }

        // GET: Areas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var area = await _context.Areas
                .Include(a => a.Brigade)
                .FirstOrDefaultAsync(m => m.AreaId == id);
            if (area == null)
            {
                return NotFound();
            }

            return View(area);
        }

        // GET: Areas/Create
        public IActionResult Create()
        {
            ViewData["BrigadeId"] = new SelectList(_context.Brigades, "BrigadeId", "BrigadeName");
            return View();
        }

        // POST: Areas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AreaId,AreaName,BrigadeId")] Area area)
        {
            if (ModelState.IsValid)
            {
                _context.Add(area);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrigadeId"] = new SelectList(_context.Brigades, "BrigadeId", "BrigadeName", area.BrigadeId);
            return View(area);
        }

        // GET: Areas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var area = await _context.Areas.FindAsync(id);
            if (area == null)
            {
                return NotFound();
            }
            ViewData["BrigadeId"] = new SelectList(_context.Brigades, "BrigadeId", "BrigadeName", area.BrigadeId);
            return View(area);
        }

        // POST: Areas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AreaId,AreaName,BrigadeId")] Area area)
        {
            if (id != area.AreaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(area);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AreaExists(area.AreaId))
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
            ViewData["BrigadeId"] = new SelectList(_context.Brigades, "BrigadeId", "BrigadeName", area.BrigadeId);
            return View(area);
        }

        // GET: Areas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var area = await _context.Areas
                .Include(a => a.Brigade)
                .FirstOrDefaultAsync(m => m.AreaId == id);
            if (area == null)
            {
                return NotFound();
            }

            return View(area);
        }

        // POST: Areas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var area = await _context.Areas.FindAsync(id);
            _context.Areas.Remove(area);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AreaExists(int id)
        {
            return _context.Areas.Any(e => e.AreaId == id);
        }
    }
}
