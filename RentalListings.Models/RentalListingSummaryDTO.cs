namespace RentalListings.Models
{
    public class RentalListingSummaryDTO
    {
        public int ListingId { get; set; }

        public string ListingSummary { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        public decimal ListingPricePerDay { get; set; }

        public int NumberOfBedrooms { get; set; }

        public int NumberOfBathrooms { get; set; }
    }
}
