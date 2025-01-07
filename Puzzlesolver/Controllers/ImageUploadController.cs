using Microsoft.AspNetCore.Mvc;

namespace Puzzlesolver.Controllers
{
    public class ImageUploadController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile? file)
        {
            if (file != null && file.Length > 0)
            {
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images");
                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }

                var filePath = Path.Combine(uploads, file.FileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                ViewBag.git  = "Upload successful!";

                ImageRecognition imageRecognition = new ImageRecognition();

                imageRecognition.ReadFile(filePath);

                return View("Index");
            }

            ViewBag.Message = "Upload failed!";
            return View("Index");
        }
    }
}