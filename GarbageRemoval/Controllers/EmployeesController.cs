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
    public class EmployeesController : Controller
    {
        private readonly ApplicationContext _context;

        public EmployeesController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index(string surname, string email, string address, DateTime dateFrom, DateTime dateTo, 
            string brigadeName, int salaryFrom, int salaryTo, EmployeeSortState sort = EmployeeSortState.EmailAsc)
        {
            IQueryable<Employee> applicationContext = _context.Employees.Include(e => e.Brigade);

            if (surname is not null)
            {
                applicationContext = applicationContext.Where(x => x.Surname.Contains(surname));
            }

            if (email is not null)
            {
                applicationContext = applicationContext.Where(x => x.Email.Contains(email));
            }

            if (address is not null)
            {
                applicationContext = applicationContext.Where(x => x.HomeAddress.Contains(address));
            }

            applicationContext = applicationContext.Where(x => x.EmploymentDate >= dateFrom);

            if (dateTo.Year != 1)
            {
                applicationContext = applicationContext.Where(x => x.EmploymentDate <= dateTo);
            }

            if (brigadeName is not null)
            {
                applicationContext = applicationContext.Where(x => x.Brigade.BrigadeName.Contains(brigadeName));
            }

            applicationContext = applicationContext.Where(x => x.Salary >= salaryFrom);

            if (salaryTo != 0)
            {
                applicationContext = applicationContext.Where(x => x.Salary <= salaryTo);
            }

            switch (sort)
            {
                case EmployeeSortState.SurnameAsc:
                    applicationContext = applicationContext.OrderBy(x => x.Surname);
                    break;
                case EmployeeSortState.SurnameDesc:
                    applicationContext = applicationContext.OrderByDescending(x => x.Surname);
                    break;
                case EmployeeSortState.EmailAsc:
                    applicationContext = applicationContext.OrderBy(x => x.Email);
                    break;
                case EmployeeSortState.EmailDesc:
                    applicationContext = applicationContext.OrderByDescending(x => x.Email);
                    break;
                case EmployeeSortState.SalaryAsc:
                    applicationContext = applicationContext.OrderBy(x => x.Salary);
                    break;
                case EmployeeSortState.SalaryDesc:
                    applicationContext = applicationContext.OrderByDescending(x => x.Salary);
                    break;
                case EmployeeSortState.EmploymentDateAsc:
                    applicationContext = applicationContext.OrderBy(x => x.EmploymentDate);
                    break;
                case EmployeeSortState.EmploymentDateDesc:
                    applicationContext = applicationContext.OrderByDescending(x => x.EmploymentDate);
                    break;
                case EmployeeSortState.BirthdayAsc:
                    applicationContext = applicationContext.OrderBy(x => x.Birthday);
                    break;
                case EmployeeSortState.BirthdayDesc:
                    applicationContext = applicationContext.OrderByDescending(x => x.Birthday);
                    break;
                default:
                    applicationContext = applicationContext.OrderBy(x => x.Surname);
                    break;
            }

            ViewBag.Sort = (List<SelectListItem>)Enum.GetValues(typeof(EmployeeSortState)).Cast<EmployeeSortState>()
               .Select(x => new SelectListItem
               {
                   Text = x.ToString(),
                   Value = x.ToString(),
                   Selected = (x == sort)
               }).ToList();

            ViewBag.Surname = surname;
            ViewBag.Email = email;
            ViewBag.Address = address;
            ViewBag.DateFrom = dateFrom;
            ViewBag.DateTo = dateTo;
            ViewBag.BrigadeName = brigadeName;
            ViewBag.SalaryFrom = salaryFrom;
            ViewBag.SalaryTo = salaryTo;

            return View(await applicationContext.ToListAsync());
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Brigade)
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewData["BrigadeId"] = new SelectList(_context.Brigades, "BrigadeId", "BrigadeName");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId,Surname,Name,Patronymic,HomeAddress,Email,Telephone,Birthday,EmploymentDate,Salary,BrigadeId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrigadeId"] = new SelectList(_context.Brigades, "BrigadeId", "BrigadeName", employee.BrigadeId);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["BrigadeId"] = new SelectList(_context.Brigades, "BrigadeId", "BrigadeName", employee.BrigadeId);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,Surname,Name,Patronymic,HomeAddress,Email,Telephone,Birthday,EmploymentDate,Salary,BrigadeId")] Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeId))
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
            ViewData["BrigadeId"] = new SelectList(_context.Brigades, "BrigadeId", "BrigadeName", employee.BrigadeId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Brigade)
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}
