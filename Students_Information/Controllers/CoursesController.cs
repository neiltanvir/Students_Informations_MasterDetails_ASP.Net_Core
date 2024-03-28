using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Students_Information.Models;

namespace Students_Information.Controllers
{
    public class CoursesController : Controller
    {
        private readonly StudentDbContext db;
        public CoursesController(StudentDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create(int id)
        {
            ViewBag.StudentId = id;
            return View();
        }
        [HttpPost]
        public IActionResult Create(Course model)
        {
            if (ModelState.IsValid)
            {
                db.Database.ExecuteSqlInterpolated($"EXEC InsertCourse {model.CourseName},{model.CourseFee}, {model.AdmissionDate}, {model.StudentId}");
                return RedirectToAction("Index", "Students");

            }
            ViewBag.Students = db.Students.ToList();
            return View(model);
        }
        public IActionResult Edit(int id)
        {
            var data = db.Courses.FirstOrDefault(x => x.CourseId == id);
            if (data == null) { return NotFound(); }
            ViewBag.Students = db.Students.ToList();
            return View(data);
        }
        [HttpPost]
        public IActionResult Edit(Course model)
        {
            if (ModelState.IsValid)
            {
                db.Database.ExecuteSqlInterpolated($"EXEC UpdateCourse {model.CourseId},{model.CourseName}, {model.CourseFee}, {model.AdmissionDate}, {model.StudentId}");
                return RedirectToAction("Index", "Students");

            }
            ViewBag.Students = db.Students.ToList();
            return View(model);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            db.Database.ExecuteSqlInterpolated($"EXEC DeleteCourse {id}");
            return Json(new { success = true, id });
        }
    }
}
