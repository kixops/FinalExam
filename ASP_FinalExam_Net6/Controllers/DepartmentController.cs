using ASP_FinalExam_Net6.Data;
using ASP_FinalExam_Net6.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP_FinalExam_Net6.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var departments = _context.Department.ToList();
            return View(departments);
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

        public ActionResult AddEditDepartment(int? id)
        {
            var department = _context.Department.Find(id);
            ViewBag.IsEdit = id == null ? false : true;
            if (id == null)
            {
                ViewBag.PageName = "Add Department";
                return View();
            }
            if (department == null)
            {
                return NotFound();
            }

            ViewBag.PageName = "Update Department";
            return View(department);
            //return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveData([Bind("Id,Name")] Department departmentData)
        {
            bool isAlreadyExist = false;
            Department department = _context.Department.Find(departmentData.Id);

            if (department != null)
            {
                isAlreadyExist = true;
            }
            else
            {
                department = new Department();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        department.Name = departmentData.Name;

                        if (isAlreadyExist)
                        {
                            _context.Update(department);
                        }
                        else
                        {
                            _context.Add(department);
                        }
                        _context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        throw ex.InnerException;
                    }
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View(departmentData);
                }
            }
            catch
            {
                return View();
            }
        }

        public ActionResult DeleteDepartment(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var department = _context.Department.FirstOrDefault(m => m.Id == id);

            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                var department = _context.Department.Find(id);
                _context.Department.Remove(department);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult DepartmentDetail(int id)
        {
            var department = _context.Department.Find(id);

            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

    }
}
