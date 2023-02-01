using GarbageRemoval.DataBase;
using GarbageRemoval.Models;
using GarbageRemoval.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GarbageRemoval.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Request(string request)
        {
            // string connectionString = $"Server=(localdb)\\mssqllocaldb;Database=GarbadgeRemoval;Trusted_Connection=True;";
            string connectionString = $"Server=DESKTOP-KIV92L3;Database=GarbadgeRemoval;Trusted_Connection=True;Encrypt=False;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(request, connection);
                    var result = new RequestViewModel();
                    var reader = command.ExecuteReader();
                    result.Displays = new string[reader.FieldCount];
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        result.Displays[i] = reader.GetName(i);
                    }

                    while (reader.Read())
                    {
                        string[] value = new string[reader.FieldCount];
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            value[i] = reader.GetValue(i).ToString();
                        }

                        result.Result.Add(value);
                    }

                    return View(result);
                }
                catch (Exception)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
        }

        // Automation
        // Підняти зарплату кожній бригаді, яка має більше 10 виконаних замовлень за останній місяць
        public async Task<IActionResult> Automation()
        {
            IQueryable<Employee> employees = _context.Employees.Include(x => x.Brigade).ThenInclude(x => x.Orders);
            employees = employees.Where(x => (x.Brigade.Orders.Where(c => c.CreateDate <= DateTime.Now && c.IsServed
                && c.CreateDate >= DateTime.Now.AddMonths(-1)).Count() >= 10));

            foreach (Employee employee in employees)
            {
                employee.Salary = ((employee.Salary * 105) / 100);
            }

            await _context.SaveChangesAsync();
            return Redirect("../Employees/Index");
        }


        public IActionResult Report(string request)
        {
            string connectionString = $"Server=DESKTOP-KIV92L3;Database=GarbadgeRemoval;Trusted_Connection=True;Encrypt=False;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(request, connection);
                    var result = new RequestViewModel();
                    var reader = command.ExecuteReader();
                    result.Displays = new string[reader.FieldCount];
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        result.Displays[i] = reader.GetName(i);
                    }

                    while (reader.Read())
                    {
                        string[] value = new string[reader.FieldCount];
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            value[i] = reader.GetValue(i).ToString();
                        }

                        result.Result.Add(value);
                    }

                    return View(result);
                }
                catch (Exception)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
