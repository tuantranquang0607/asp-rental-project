namespace RentalListings.Models
{
    public class RentalListingDetailsDTO
    {
        public int ListingId { get; set; }

        public string ListingSummary { get; set; }

        public string ListingDetails { get; set; }

        public int AddressId { get; set; }

        public string CivicNumber { get; set; }

        public string StreetName { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        public string PostalCode { get; set; }

        public decimal ListingPricePerDay { get; set; }

        public int NumberOfBedrooms { get; set; }

        public int NumberOfBathrooms { get; set; }

        public int SizeInSquareFeet { get; set; }
    }
}
