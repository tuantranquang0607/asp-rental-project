using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalsProject.Data;
using RentalsProject.Models;
using System.Diagnostics;

namespace RentalsProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index(string? search, string? reset, int pageNumber = 1, int? pageSize = null, string previousSort = "", string sortBy = "")
        {
            var query = _context.RentalListings.AsQueryable();

            if (!string.IsNullOrEmpty(reset))
            {
                search = null;
            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s => s.ListingSummary.Contains(search) ||
                                         s.ListingDetails.Contains(search) ||
                                         s.City.Contains(search) ||
                                         s.Province.Contains(search) ||
                                         s.NumberOfBedrooms.ToString().Contains(search));
            }

            var newSortOrder = Sorting.GetNewSortOrder(previousSort, sortBy);

            query = query.Sort(newSortOrder);

            var viewModel = await query.ToSortedPaginatedListAsync(pageNumber, pageSize ?? _configuration.getDefaultPageSize());

            viewModel.PreviousSort = newSortOrder;

            viewModel.SearchCriteria = search;

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentalListing = await _context.RentalListings.FirstOrDefaultAsync(m => m.Id == id);

            if (rentalListing == null)
            {
                return NotFound();
            }

            return View(rentalListing);
        }
    }
}
