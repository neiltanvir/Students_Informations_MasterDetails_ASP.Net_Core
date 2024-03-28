using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Students_Information.Models;
using Students_Information.ViewModels;
using Students_Information.ViewModels.Input;
using System.Drawing;
using X.PagedList;

namespace Students_Information.Controllers
{
    public class StudentsController : Controller
    {
        private readonly StudentDbContext db;
        private readonly IWebHostEnvironment env;
        public StudentsController(StudentDbContext db, IWebHostEnvironment env)
        {
            this.db = db;
            this.env = env;
        }
        public async Task<IActionResult> Index(int pg = 1)
        {
            return View(await db.Students.OrderBy(x => x.StudentId).Include(x => x.Courses).ToPagedListAsync(pg, 5));
        }
        public async Task<IActionResult> Aggregates()
        {
            var data = await db.Courses.Include(x => x.Student)
                .ToListAsync();
            return View(data);
        }
        public IActionResult Grouping()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Grouping(string groupby)
        {

            if (groupby == "studentname")
            {
                var data = db.Courses.Include(x => x.Student)
               .ToList()
               .GroupBy(x => x.Student?.StudentName)
               .Select(g => new GroupedData { Key = g.Key, Data = g })
               .ToList();

                return View("GroupingResult", data);
            }
            if (groupby == "year month")
            {
                var data = db.Courses.Include(x => x.Student)
                    .OrderByDescending(x => x.AdmissionDate)
               .ToList()
               .GroupBy(x => $"{x.AdmissionDate:MMM, yyyy}")
               .Select(g => new GroupedData { Key = g.Key, Data = g })
               .ToList();

                return View("GroupingResult", data);
            }
            if (groupby == "count")
            {
                var data = db.Courses.Include(x => x.Student)
                    .OrderByDescending(x => x.AdmissionDate)
               .ToList()
                .GroupBy(x => x.Student?.StudentName)
               .Select(g => new GroupedDataPrimitive<int> { Key = g.Key, Data = g.Count() })
               .ToList();

                return View("GroupingResultPrimitive", data);
            }

            return RedirectToAction("Grouping");
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(StudentInputModel model)
        {
            if (ModelState.IsValid)
            {
                await db.Database.ExecuteSqlInterpolatedAsync($"EXEC InsertStudent {model.StudentName}, {model.Age}, {(int)model.Gender}, {model.Picture}, {(model.IsRegular ? 1 : 0)}");
                var id = await db.Students.MaxAsync(x => x.StudentId);
                foreach (var s in model.Courses)
                {
                    await db.Database.ExecuteSqlInterpolatedAsync($"EXEC InsertCourse {s.CourseName}, {s.CourseFee}, {s.AdmissionDate}, {id}");
                }
                return Json(new { success = true });

            }
            return Json(new { success = true });
        }
        public IActionResult GetCoursesForm()
        {
            return PartialView("_CoursesForm");
        }
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            string ext = Path.GetExtension(file.FileName);
            string fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
            string savePath = Path.Combine(env.WebRootPath, "Pictures", fileName);
            FileStream fs = new FileStream(savePath, FileMode.Create);
            await file.CopyToAsync(fs);
            fs.Close();
            return Json(new { name = fileName });
        }
        public async Task<IActionResult> Edit(int id)
        {
            var data = await db.Students.FirstOrDefaultAsync(x => x.StudentId == id);
            if (data == null) return NotFound();
            return View(new StudentEditModel
            {
                StudentId = data.StudentId,
                StudentName = data.StudentName,
                Age = data.Age,
                Gender = data.Gender ?? Gender.boy,
                IsRegular = data.IsRegular ?? false

            });
        }
        [HttpPost]
        public async Task<IActionResult> Edit(StudentEditModel model)
        {
            if (ModelState.IsValid)
            {
                var student = await db.Students.FirstOrDefaultAsync(x => x.StudentId == model.StudentId);
                if (student == null) return NotFound();
                student.StudentId = model.StudentId;
                student.StudentName = model.StudentName;
                student.Age = model.Age;
                student.Gender = model.Gender;
                student.IsRegular = model.IsRegular;

                if (model.Picture != null)
                {
                    string ext = Path.GetExtension(model.Picture.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                    string savePath = Path.Combine(env.WebRootPath, "Pictures", fileName);
                    FileStream fs = new FileStream(savePath, FileMode.Create);
                    await model.Picture.CopyToAsync(fs);
                    student.Picture = fileName;
                    fs.Close();
                }
                db.Database.ExecuteSqlInterpolated($"EXEC UpdateStudent {student.StudentId}, {student.StudentName}, {student.Age}, {(int)student.Gender}, {student.Picture}, {(model.IsRegular ? 1 : 0)}");
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            db.Database.ExecuteSqlInterpolated($"EXEC DeleteStudent {id}");
            return Json(new { success = true, id });
        }
    }
}
