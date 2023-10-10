using Exam_Practice01.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Exam_Practice01.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var emp = _context.Employee.FromSqlRaw($"sp_GetAllEmp").AsEnumerable().ToList();    
            return View(emp);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee emp)
        {
            if(ModelState.IsValid)
            {
                string empCreate = $"sp_CreateEmp {emp.EmpName}, {emp.EmpAddress}, {emp.PhoneNumber},{emp.Age}, {emp.Salary}, '{emp.Dob}'";
                _context.Database.ExecuteSqlRaw(empCreate);
                return RedirectToAction("Index");
                
            }
            return View(emp);
        }
        public IActionResult Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var empEdit = _context.Employee.FromSqlRaw($"sp_GetEmpById {id}").AsEnumerable().FirstOrDefault();
            if(empEdit == null)
            {
                return NotFound();
            }
            return View(empEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Employee emp)
        {
            if(ModelState.IsValid)
            {
                string updateEmp = $"sp_UpdateEmp {emp.Id}, {emp.EmpName}, {emp.EmpAddress}, {emp.PhoneNumber}, {emp.Age}, {emp.Salary}, '{emp.Dob}'";
                _context.Database.ExecuteSqlRaw(updateEmp);
                return RedirectToAction("Index");
            }
            return View(emp);
        }

        public IActionResult Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var empDelete = _context.Employee.FromSqlRaw($"sp_GetEmpById {id}").AsEnumerable().FirstOrDefault();
            if(empDelete == null)
            {
                return NotFound();
            }
            return View(empDelete);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Employee emp)
        {
            if (ModelState.IsValid)
            {
                _context.Database.ExecuteSqlRaw($"sp_DeleteEmp {emp.Id}");
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }


        public IActionResult Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var empDetails = _context.Employee.FromSqlRaw($"sp_GetEmpById {id}").AsEnumerable().FirstOrDefault();
            if(empDetails == null)
            {
                return NotFound();
            }
            return View(empDetails);
        }

        public IActionResult EmpStatus() 
        {
            //count
            int empCount = _context.Employee.Count();
            ViewBag.Count = empCount;

            //avarage
            double ageAverage = (double)_context.Employee.Average(a => a.Age);
            ViewBag.Average = ageAverage;

            //Min
            int ageMin = (int)_context.Employee.Min(a => a.Age);
            ViewBag.Minimum = ageMin;

            //max
            int maxAge = (int)_context.Employee.Max(a => a.Age);
            ViewBag.MaxAge = maxAge;
            //sum
            decimal totalSalary = (decimal)_context.Employee.Sum(a => a.Salary);
            ViewBag.SumSalary = totalSalary;
            // Group by department
            var employeesByDepartment = _context.Employee.GroupBy(e => e.Age);
            ViewBag.EmployeesByDepartment = employeesByDepartment;

            return View();
        }

        public IActionResult GroupBySalary()
        {
            var expectedSalary2000Count = _context.Employee.Where(x => x.Salary
           == 2000).Count();
            var expectedSalary5000Count = _context.Employee.Where(x => x.Salary
           == 5000).Count();
            var groupedSalary = (from a in _context.Employee.ToList()
                                    group a by a.Id into g
                                    select new
                                    {
                                        empId = g.Key,
                                        Salary =
                                   g.Select(x => x)
                                    }).ToList();
            ViewBag.Salary2000 = expectedSalary2000Count;
            ViewBag.Salary5000 = expectedSalary5000Count;
            return View();

            
        }
        public IActionResult MinSalary()
        {
            var expectedIlMinSalary = _context.Employee.Where(x=>x.Salary<2000).Count();
            var groupedMinSalary = from a in _context.Employee
                                   select new
                                   {
                                       EmployeeID = a.Id,
                                       EmployeeSalary = a.Salary
                                   } into empSalary
                                   group empSalary by empSalary.EmployeeID into g
                                   select new
                                   {
                                       Salary = g.Key,
                                       MinAge = g.Min(a =>
                                  a.EmployeeSalary)
                                   };
            ViewBag.Salary2000 = expectedIlMinSalary;

            return View();

        }
        public IActionResult  MaxSalary()         {
            var expectedCaMaxSalary = _context.Employee.Where(x => x.Salary > 2000).Count();
            var groupedMaxSalary = from a in _context.Employee
                                   select new
                                   {
                                       EmployeeID = a.Id,
                                       EmployeeSalary = a.Salary
                                   } into empSalary
                                   group empSalary by empSalary.EmployeeID into g
                                   select new
                                   {
                                       Salary = g.Key,
                                       MinAge = g.Min(a =>
                                  a.EmployeeSalary)
                                   };
            ViewBag.Salary = expectedCaMaxSalary;
            return View();

        }
        public IActionResult AverageSalary()
        {
            var   expectedCaAverageSalary = _context.Employee.Where(x => x.Salary > 2000).Count();

            var groupedSalary = from a in _context.Employee
                                select new
                                {
                                    EmployeeID = a.Id,
                                    EmployeeSalary = a.Salary
                                } into empSalary
                                group empSalary by empSalary.EmployeeID into g
                                select new
                                {
                                    Salary = g.Key,
                                    MinAge = g.Min(a =>
                               a.EmployeeSalary)
                                };
            ViewBag.Salary = expectedCaAverageSalary;
            return View();
        }


    }

    
}

