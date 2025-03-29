using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalsProject.Models
{
    public class RentalListing
    {
        public int Id { get; set; }


        [Required]
        [Display(Name = "Listing Summary")]
        public string? ListingSummary { get; set; }


        [Required]
        [Display(Name = "Listing Details")]
        public string? ListingDetails { get; set; }


        [Required]
        [Display(Name = "Listing Address")]
        public string? ListingAddress { get; set; }


        [Required]
        [Display(Name = "Civic Number")]
        public string? CivicNumber { get; set; }


        [Required]
        [Display(Name = "Street Name")]
        public string? StreetName { get; set; }


        [Required]
        [Display(Name = "City")]
        public string? City { get; set; }


        [Required]
        [Display(Name = "Province")]
        public string? Province { get; set; }


        [Required]
        [Display(Name = "Postal Code")]
        public string? PostalCode { get; set; }


        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Listing Price Per Day")]
        public decimal ListingPricePerDay { get; set; }


        [Required]
        [Display(Name = "Number of Bedrooms")]
        public int NumberOfBedrooms { get; set; }


        [Required]
        [Display(Name = "Number of Bathrooms")]
        public int NumberOfBathrooms { get; set; }


        [Required]
        [Display(Name = "Size (Square Feet)")]
        public int SizeInSquareFeet { get; set; }


        [NotMapped]
        public bool AllowModify { get; set; }


        public string? UserId { get; set; }


        [Display(Name = "Rental Image")]
        public int? RentalImageId { get; set; }


        [Display(Name = "Rental Image")]
        public virtual FileModel? RentalImage { get; set; }


        [NotMapped]
        public bool HasRentalImage
        {
            get
            {
                return RentalImageId != null;
            }
        }
    }
}