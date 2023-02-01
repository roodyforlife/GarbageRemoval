using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GarbageRemoval.DataBase;
using GarbageRemoval.Models;
using GarbageRemoval.Enums;

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
        public async Task<IActionResult> Index(string brigadeName, string email, DateTime createDateFrom, DateTime createDateTo,
            BrigadeSortState sort = BrigadeSortState.BrigadeNameAsc)
        {
            IQueryable<Brigade> applicationContext = _context.Brigades.Include(b => b.Administration);

            if (brigadeName is not null)
            {
                applicationContext = applicationContext.Where(x => x.BrigadeName.Contains(brigadeName));
            }

            if (email is not null)
            {
                applicationContext = applicationContext.Where(x => x.Administration.Email.Contains(email));
            }

            applicationContext = applicationContext.Where(x => x.CreateDate >= createDateFrom);

            if (createDateTo.Year != 1)
            {
                applicationContext = applicationContext.Where(x => x.CreateDate <= createDateTo);
            }

            switch (sort)
            {
                case BrigadeSortState.BrigadeNameAsc:
                    applicationContext = applicationContext.OrderBy(x => x.BrigadeName);
                    break;
                case BrigadeSortState.BrigadeNameDesc:
                    applicationContext = applicationContext.OrderByDescending(x => x.BrigadeName);
                    break;
                case BrigadeSortState.CreateDateAsc:
                    applicationContext = applicationContext.OrderBy(x => x.CreateDate);
                    break;
                case BrigadeSortState.CreateDateDesc:
                    applicationContext = applicationContext.OrderByDescending(x => x.CreateDate);
                    break;
                case BrigadeSortState.AdministrationAsc:
                    applicationContext = applicationContext.OrderBy(x => x.Administration.Email);
                    break;
                case BrigadeSortState.AdministrationDesc:
                    applicationContext = applicationContext.OrderByDescending(x => x.Administration.Email);
                    break;
                default:
                    applicationContext = applicationContext.OrderBy(x => x.BrigadeName);
                    break;
            }

            ViewBag.BridageName = brigadeName;
            ViewBag.Email = email;
            ViewBag.CreateDateFrom = createDateFrom;
            ViewBag.CreateDateTo = createDateTo;
            ViewBag.Sort = (List<SelectListItem>)Enum.GetValues(typeof(BrigadeSortState)).Cast<BrigadeSortState>()
               .Select(x => new SelectListItem
               {
                   Text = x.ToString(),
                   Value = x.ToString(),
                   Selected = (x == sort)
               }).ToList();

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
