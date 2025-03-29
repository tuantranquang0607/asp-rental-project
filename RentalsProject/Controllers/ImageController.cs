using Microsoft.AspNetCore.Mvc;
using RentalsProject.Data;

namespace RentalsProject.Controllers
{
    public class ImageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ImageController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Display( int? Id )
        {
            if (Id == null)
            {
                return NotFound();
            }

            var fileModel = _context.Files.Find(Id);

            if (fileModel == null)
            {
                return NotFound();
            }

            if (fileModel.Content == null || fileModel.ContentType == null)
            {
                return NotFound();
            }

            return File(fileModel.Content, fileModel.ContentType);
        }
    }
}
