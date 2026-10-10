using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using To_Do_List.DataAccess;

namespace To_Do_List.Controllers
{
    public class ToDoList : Controller
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();
        public IActionResult Index(DateOnly? SearchDate , int Page = 1)
        {
            var toDoLists = _context.toDoLists.OrderBy(t => t.Deadline).AsQueryable();
            
            // Search 
            if(SearchDate != null)
            {
                // to convert DeadLine to DateOnly
                toDoLists = toDoLists.Where(t => DateOnly.FromDateTime(t.Deadline) == SearchDate);
            }

            //Pagination
            int pageSize = 5;

            ViewBag.TotalPages = Math.Ceiling(toDoLists.Count() / (double)pageSize);

            toDoLists = toDoLists.Skip((Page - 1) * pageSize).Take(pageSize);

            ViewBag.CurrentPage = Page;

            return View(toDoLists.AsEnumerable());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Models.ToDoList toDoList , IFormFile formFile)
        {
            if(formFile != null && formFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + "-" + formFile.FileName;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    formFile.CopyTo(stream);
                }

                toDoList.FileUrl = fileName;
            }

            _context.toDoLists.Add(toDoList);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult Update(int id)
        {
            var toDoList = _context.toDoLists.FirstOrDefault(t=>t.Id == id);
            if(toDoList == null)
            {
                return NotFound();
            }
            return View(toDoList);
        }

        [HttpPost]
        public IActionResult Update(Models.ToDoList toDoList , IFormFile formFile)
        {
            var toDoListInDb = _context.toDoLists.AsNoTracking().FirstOrDefault(t => t.Id == toDoList.Id);


            if (formFile != null && formFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + "-" + formFile.FileName;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    formFile.CopyTo(stream);
                }

                toDoList.FileUrl = fileName;

                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", toDoListInDb.FileUrl);
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }

            }
            else
            {
                toDoList.FileUrl = toDoListInDb.FileUrl;
            }

            _context.toDoLists.Update(toDoList);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var toDoList = _context.toDoLists.FirstOrDefault(t => t.Id == id);

            if(toDoList == null)
            {
                return NotFound();
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", toDoList.FileUrl);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _context.toDoLists.Remove(toDoList);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

    }
}
