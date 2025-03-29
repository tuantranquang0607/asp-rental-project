using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalListings.Models;
using RentalsProject.Data;
using RentalsProject.Data.Migrations;
using RentalsProject.Models;

namespace RentalsProject.Controllers
{
    [Route("api/rentalListings")]
    [ApiController]
    public class RentalListingsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RentalListingsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/RentalListingsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RentalListing>>> GetRentalListings()
        {
            var rentalListings = await _context.RentalListings.Select(r => new RentalListingSummaryDTO{
                ListingId = r.Id,
                ListingSummary = r.ListingSummary,
                City = r.City,
                Province = r.Province,
                ListingPricePerDay = r.ListingPricePerDay,
                NumberOfBedrooms = r.NumberOfBedrooms,
                NumberOfBathrooms = r.NumberOfBathrooms
            }).ToListAsync();

            return Ok(rentalListings);
        }

        // GET: api/RentalListingsApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RentalListing>> GetRentalListing(int id)
        {
            var rentalListing = await _context.RentalListings.Where(r => r.Id == id).Select(r => new RentalListingDetailsDTO{
                ListingId = r.Id,
                ListingSummary = r.ListingSummary,
                ListingDetails = r.ListingDetails,
                AddressId = int.Parse(r.ListingAddress), 
                CivicNumber = r.CivicNumber,
                StreetName = r.StreetName,
                City = r.City,
                Province = r.Province,
                PostalCode = r.PostalCode,
                ListingPricePerDay = r.ListingPricePerDay,
                NumberOfBedrooms = r.NumberOfBedrooms,
                NumberOfBathrooms = r.NumberOfBathrooms,
                SizeInSquareFeet = r.SizeInSquareFeet
            }).FirstOrDefaultAsync();

            if (rentalListing == null)
            {
                return NotFound();
            }

            return Ok(rentalListing);
        }

        // PUT: api/RentalListingsApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRentalListing(int id, RentalListing rentalListing)
        {
            if (id != rentalListing.Id)
            {
                return BadRequest();
            }

            _context.Entry(rentalListing).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RentalListingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/RentalListingsApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RentalListing>> PostRentalListing(RentalListing rentalListing)
        {
            _context.RentalListings.Add(rentalListing);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRentalListing", new { id = rentalListing.Id }, rentalListing);
        }

        // DELETE: api/RentalListingsApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRentalListing(int id)
        {
            var rentalListing = await _context.RentalListings.FindAsync(id);
            if (rentalListing == null)
            {
                return NotFound();
            }

            _context.RentalListings.Remove(rentalListing);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RentalListingExists(int id)
        {
            return _context.RentalListings.Any(e => e.Id == id);
        }
    }
}
