using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentalsProject.Data;
using RentalsProject.Data.Migrations;
using RentalsProject.Models;

namespace RentalsProject.Controllers
{
    public class RentalListingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUserProfile> _userManager;
        private readonly IConfiguration _configuration;

        public RentalListingsController(ApplicationDbContext context, UserManager<IdentityUserProfile> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        // GET: RentalListings
        [Authorize]
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

            var currentUserId = GetCurrentUserId();

            foreach (var listing in viewModel)
            {
                listing.AllowModify = listing.UserId == currentUserId;
            }

            viewModel.SearchCriteria = search;

            return View(viewModel);
        }

        // GET: RentalListings/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentalListing = await _context.RentalListings.FirstOrDefaultAsync(m => m.Id == id);

            var userId = _userManager.GetUserId(User);

            rentalListing.AllowModify = (rentalListing.UserId == userId);

            if (rentalListing == null)
            {
                return NotFound();
            }

            return View(rentalListing);
        }

        // GET: RentalListings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RentalListings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(RentalListing rentalListing, IFormFile? upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null)
                {
                    rentalListing.RentalImage = upload.ConvertToModel();
                }

                rentalListing.UserId = GetCurrentUserId();

                _context.Add(rentalListing);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(rentalListing);
        }

        // GET: RentalListings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentalListing = await _context.RentalListings.FindAsync(id);
            if (rentalListing == null)
            {
                return NotFound();
            }

            return View(rentalListing);
        }

        // POST: RentalListings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RentalListing rentalListing, IFormFile? upload)
        {
            if (id != rentalListing.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (!IsListingOwner(id))
                {
                    return Forbid();
                }

                rentalListing.UserId = GetListingOwnerId(id);

                var existingImageId = _context.RentalListings.Where(c => c.Id == id).Select(c => c.RentalImageId).FirstOrDefault();

                if (existingImageId == null)
                {
                    rentalListing.RentalImageId = existingImageId;
                }

                if (upload != null)
                {
                    rentalListing.RentalImage = upload.ConvertToModel(existingImageId);
                }

                try
                {
                    _context.Update(rentalListing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentalListingExists(rentalListing.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(rentalListing);
        }

        // GET: RentalListings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentalListing = await _context.RentalListings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rentalListing == null)
            {
                return NotFound();
            }

            return View(rentalListing);
        }

        // POST: RentalListings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rentalListing = await _context.RentalListings.FindAsync(id);
            if (rentalListing != null)
            {
                _context.RentalListings.Remove(rentalListing);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool RentalListingExists(int id)
        {
            return _context.RentalListings.Any(e => e.Id == id);
        }

        private string? GetCurrentUserId()
        {
            return _userManager.GetUserId(User);
        }

        private bool IsListingOwner(int listingId)
        {
            var dbRentalUserId = GetListingOwnerId(listingId);
            var currentUserId = GetCurrentUserId();

            return dbRentalUserId == currentUserId;
        }

        private string? GetListingOwnerId(int listingId)
        {
            var dbRentalUserId = _context.RentalListings.Where(c => c.Id == listingId).Select(c => c.UserId).FirstOrDefault();

            return dbRentalUserId;
        }
    }
}