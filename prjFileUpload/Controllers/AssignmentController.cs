using Microsoft.AspNetCore.Mvc;
using prjFileUpload.Models;

namespace prjFileUpload.Controllers
{
    public class AssignmentController : Controller
    {
        public readonly IWebHostEnvironment _environment; //pulls envoirmnent variables to detrmine where the variableas are stored within the computer
        public AssignmentController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        private static List<Assignment> assignments = new List<Assignment>();

        public IActionResult Index()
        {
            return View(assignments);
        }
        [HttpPost]
        public IActionResult Upload(IFormFile file, string uploaderName) //passes through the file and who passed the file through
        {
            if(file != null && file.Length > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(_environment.WebRootPath, "uploads", fileName); //gets the file path
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);//copies the file into the upload folder within the root folder
                }
                assignments.Add(new Assignment
                {
                    Id = assignments.Count + 1,
                    FileName = fileName,
                    UploaderName = uploaderName,
                    UploadDate = DateTime.Now
                });
            }
            return RedirectToAction("Index");
        }
        public IActionResult OpenFile(string fileName)
        {
            var path = Path.Combine(_environment.WebRootPath, "uploads", fileName); //gope to the file path
            var fileBytes = System.IO.File.ReadAllBytes(path); //reads the file path and then
            return File(fileBytes, "application/octect-stream", fileName); //returns the file
        }
    }
}
