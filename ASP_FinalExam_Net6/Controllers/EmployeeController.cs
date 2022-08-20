using ASP_FinalExam_Net6.Data;
using ASP_FinalExam_Net6.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP_FinalExam_Net6.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var employees = _context.Employee.Include(x => x.Dept).ToList();
            return View(employees);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult AddEditEmployee(int? id)
        {
            var departments = _context.Department.ToList();
            ViewBag.Department = departments;
            var employee = _context.Employee.Find(id);
            ViewBag.IsEdit = id == null ? false : true;
            if (id == null)
            {
                ViewBag.PageName = "Add Employee";
                return View();
            }
            if (employee == null)
            {
                return NotFound();
            }

            ViewBag.PageName = "Update Employee";
            ViewBag.DepartmentId = employee.DepartmentId;

            return View(employee);
            //return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveData(Employee employeeData)
        {
            bool isAlreadyExist = false;
            Employee employee = _context.Employee.Find(employeeData.Id);
            Department department = _context.Department.Find(employeeData.DepartmentId);

            employeeData.Dept = _context.Department.Find(employeeData.DepartmentId);

            if (employee != null)
            {
                isAlreadyExist = true;
            }
            else
            {
                employee = new Employee();
            }

            try
            {
                try
                {
                    employee.Name = employeeData.Name;
                    employee.IsManager = employeeData.IsManager;
                    employee.DepartmentId = employeeData.DepartmentId;

                    if (isAlreadyExist)
                    {
                        _context.Update(employee);
                    }
                    else
                    {
                        _context.Add(employee);
                        department.EmployeeCount += 1;
                        _context.Update(department);
                    }

                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex.InnerException;
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult DeleteEmployee(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var employee = _context.Employee.Include(d => d.Dept).FirstOrDefault(m => m.Id == id);

            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                var employee = _context.Employee.Find(id);
                _context.Employee.Remove(employee);

                Department department = _context.Department.Find(employee.DepartmentId);

                department.EmployeeCount += 1;
                _context.Update(department);

                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult EmployeeDetail(int id)
        {
            var employee = _context.Employee.Include(x => x.Dept).FirstOrDefault(x => x.Id == id);

            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }
    }
}
