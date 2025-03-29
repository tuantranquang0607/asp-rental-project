using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentalsProject.Data;
using RentalsProject.Data.Migrations;
using RentalsProject.Models;
using EmailService;
using IEmailSender = EmailService.IEmailSender;

namespace RentalsProject.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUserProfile> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;

        public ReservationsController(ApplicationDbContext context, UserManager<IdentityUserProfile> userManager, IEmailSender emailSender, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
            _configuration = configuration;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Reservation.Include(r => r.RentalListing).Include(r => r.User);
            return View(await applicationDbContext.ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> OwnerReservations()
        {
            var currentUserId = _userManager.GetUserId(User);

            var reservations = await _context.Reservation
                .Include(r => r.RentalListing)
                .Include(r => r.User)
                .Where(r => r.RentalListing.UserId == currentUserId) 
                .Select(r => new ReservationViewModel
                {ListingSummary = r.RentalListing.ListingSummary,
                    City = r.RentalListing.City,
                    Province = r.RentalListing.Province,
                    CheckIn = r.CheckIn,
                    CheckOut = r.CheckOut,
                    RequesterName = $"{r.User.FirstName} {r.User.LastName}",
                    RequesterPhone = r.User.PhoneNumber,
                    RequesterEmail = r.User.Email,
                    RentalListingId = r.RentalListingId
                }).ToListAsync();

            return View(reservations);
        }

        [Authorize]
        public async Task<IActionResult> MyReservations()
        {
            var currentUserId = _userManager.GetUserId(User);

            var reservations = await _context.Reservation
                .Include(r => r.RentalListing)
                .Include(r => r.User)
                .Where(r => r.UserId == currentUserId) 
                .Select(r => new ReservationViewModel{
                    Id = r.Id,
                    ListingSummary = r.RentalListing.ListingSummary,
                    City = r.RentalListing.City,
                    Province = r.RentalListing.Province,
                    CheckIn = r.CheckIn,
                    CheckOut = r.CheckOut,
                    RequesterName = $"{r.User.FirstName} {r.User.LastName}",
                    RequesterPhone = r.User.PhoneNumber,
                    RequesterEmail = r.User.Email,
                    RentalListingId = r.RentalListingId
                }).ToListAsync();

            return View(reservations);
        }

        [Authorize]
        public async Task<IActionResult> Cancel(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .Include(r => r.RentalListing)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null || reservation.UserId != _userManager.GetUserId(User))
            {
                return Forbid(); 
            }

            return View(reservation);
        }

        [HttpPost, ActionName("Cancel")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> CancelConfirmed(int id)
        {
            var reservation = await _context.Reservation.FindAsync(id);

            if (reservation == null || reservation.UserId != _userManager.GetUserId(User))
            {
                return Forbid(); 
            }

            _context.Reservation.Remove(reservation);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(MyReservations)); 
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .Include(r => r.RentalListing)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        public IActionResult Create(int? rentalListingId)
        {
            if (rentalListingId == null)
            {
                return NotFound();
            }
            var model = new RentalsProject.Models.Reservation();

            model.RentalListingId = (int)rentalListingId;
            model.UserId = _userManager.GetUserId(User);

            return View(model);
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(Reservation reservation)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Forbid(); 
            }

            if (reservation.CheckIn >= reservation.CheckOut)
            {
                ModelState.AddModelError("CheckOut", "Check-Out date must be later than Check-In date.");
            }

            var overlappingReservation = await _context.Reservation
                .Where(r => r.RentalListingId == reservation.RentalListingId &&
                            r.CheckIn < reservation.CheckOut && 
                            reservation.CheckIn < r.CheckOut)
                .FirstOrDefaultAsync();

            if (overlappingReservation != null)
            {
                ModelState.AddModelError(string.Empty, "The selected dates overlap with an existing reservation.");
            }

            List<ModelErrorCollection> errors = null;

            try
            {
                /*var message = new EmailMessage("New Reservation Request",
                    "There is a new reservation request for your property:\n\n" +
                        $"Listing Summary: {reservation.RentalListing.ListingSummary}\n" +
                        $"City: {reservation.RentalListing.City}\n" +
                        $"Province: {reservation.RentalListing.Province}\n" +
                        $"Check-In Date and Time: {reservation.CheckIn:yyyy-MM-dd HH:mm}\n" +
                        $"Check-Out Date and Time: {reservation.CheckOut:yyyy-MM-dd HH:mm}\n" +
                        $"Requester Name: {reservation.User.FirstName} {reservation.User.LastName}\n" +
                        $"Phone Number: {reservation.User.PhoneNumber}\n" +
                        $"Email Address: {reservation.User.Email}",
                    "tranquangtuan060703@gmail.com");

                this._emailSender.Send(message);*/

                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MyReservations));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Exception", ex.Message);
            }

            ViewData["RentalListingId"] = new SelectList(_context.RentalListings, "Id", "City", reservation.RentalListingId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", reservation.UserId);
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["RentalListingId"] = new SelectList(_context.RentalListings, "Id", "City", reservation.RentalListingId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", reservation.UserId);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RentalListingId,UserId,CheckIn,CheckOut")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
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
            ViewData["RentalListingId"] = new SelectList(_context.RentalListings, "Id", "City", reservation.RentalListingId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", reservation.UserId);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .Include(r => r.RentalListing)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservation.Remove(reservation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservation.Any(e => e.Id == id);
        }
    }
}
