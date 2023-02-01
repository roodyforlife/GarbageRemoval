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
    public class OrdersController : Controller
    {
        private readonly ApplicationContext _context;

        public OrdersController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index(string bridageName, string address, DateTime createDateFrom, DateTime createDateTo, 
            int costFrom, int costTo, OrderSortState sort = OrderSortState.BrigadeNameAsc)
        {
            IQueryable<Order> applicationContext = _context.Orders.Include(o => o.Brigade).Include(o => o.House);

            if (bridageName is not null)
            {
                applicationContext = applicationContext.Where(x => x.Brigade.BrigadeName.Contains(bridageName));
            }

            if (address is not null)
            {
                applicationContext = applicationContext.Where(x => x.House.Address.Contains(address));
            }

            if (createDateTo.Year != 1)
            {
                applicationContext = applicationContext.Where(x => x.CreateDate <= createDateTo);
            }

            applicationContext = applicationContext.Where(x => x.CreateDate >= createDateFrom);

            if (costTo != 0)
            {
                applicationContext = applicationContext.Where(x => x.CostPerKilogram <= costTo);
            }

            applicationContext = applicationContext.Where(x => x.CostPerKilogram >= costFrom);

            switch (sort)
            {
                case OrderSortState.BrigadeNameAsc:
                    applicationContext = applicationContext.OrderBy(x => x.Brigade.BrigadeName);
                    break;
                case OrderSortState.BrigadeNameDesc:
                    applicationContext = applicationContext.OrderByDescending(x => x.Brigade.BrigadeName);
                    break;
                case OrderSortState.AddressAsc:
                    applicationContext = applicationContext.OrderBy(x => x.House.Address);
                    break;
                case OrderSortState.AddressDesc:
                    applicationContext = applicationContext.OrderByDescending(x => x.House.Address);
                    break;
                case OrderSortState.IsServedAsc:
                    applicationContext = applicationContext.OrderBy(x => x.IsServed);
                    break;
                case OrderSortState.IsServedDesc:
                    applicationContext = applicationContext.OrderByDescending(x => x.IsServed);
                    break;
                case OrderSortState.CostAsc:
                    applicationContext = applicationContext.OrderBy(x => x.CostPerKilogram);
                    break;
                case OrderSortState.CostDesc:
                    applicationContext = applicationContext.OrderByDescending(x => x.CostPerKilogram);
                    break;
                case OrderSortState.WeightAsc:
                    applicationContext = applicationContext.OrderBy(x => x.GarbadgeWeight);
                    break;
                case OrderSortState.WeightDesc:
                    applicationContext = applicationContext.OrderByDescending(x => x.GarbadgeWeight);
                    break;
                default:
                    applicationContext = applicationContext.OrderBy(x => x.Brigade.BrigadeName);
                    break;
            }

            ViewBag.Sort = (List<SelectListItem>)Enum.GetValues(typeof(OrderSortState)).Cast<OrderSortState>()
               .Select(x => new SelectListItem
               {
                   Text = x.ToString(),
                   Value = x.ToString(),
                   Selected = (x == sort)
               }).ToList();

            ViewBag.BrigadeName = bridageName;
            ViewBag.Address = address;
            ViewBag.CreateDateFrom = createDateFrom;
            ViewBag.CreateDateTo = createDateTo;
            ViewBag.CostFrom = costFrom;
            ViewBag.CostTo = costTo;


            return View(await applicationContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Brigade)
                .Include(o => o.House)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["BrigadeId"] = new SelectList(_context.Brigades, "BrigadeId", "BrigadeName");
            ViewData["HouseId"] = new SelectList(_context.Houses, "HouseId", "Address");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,BrigadeId,HouseId,CreateDate,IsServed,CostPerKilogram,GarbadgeWeight")] Order order)
        {
            order.CreateDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrigadeId"] = new SelectList(_context.Brigades, "BrigadeId", "BrigadeName", order.BrigadeId);
            ViewData["HouseId"] = new SelectList(_context.Houses, "HouseId", "Address", order.HouseId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["BrigadeId"] = new SelectList(_context.Brigades, "BrigadeId", "BrigadeName", order.BrigadeId);
            ViewData["HouseId"] = new SelectList(_context.Houses, "HouseId", "Address", order.HouseId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,BrigadeId,HouseId,CreateDate,IsServed,CostPerKilogram,GarbadgeWeight")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            order.CreateDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
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
            ViewData["BrigadeId"] = new SelectList(_context.Brigades, "BrigadeId", "BrigadeName", order.BrigadeId);
            ViewData["HouseId"] = new SelectList(_context.Houses, "HouseId", "Address", order.HouseId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Brigade)
                .Include(o => o.House)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
