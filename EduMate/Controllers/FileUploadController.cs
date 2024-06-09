using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;
using EduMate.Data;
using EduMate.Models;

namespace EduMate.Controllers
{
    public class FileUploadController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _context;

        public FileUploadController(IWebHostEnvironment webHostEnvironment, ApplicationDbContext context)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return Content("File not selected");
            }

            var path = Path.Combine(
                        _webHostEnvironment.WebRootPath, "uploads",
                        file.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var material = new Material
            {
                Title = Path.GetFileNameWithoutExtension(file.FileName),
                FilePath = path,
                UploadedBy = User.Identity.Name,
                UploadedAt = DateTime.Now
            };
            _context.Materials.Add(material);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}