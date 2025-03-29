namespace RentalsProject.Models
{
    public class ReservationViewModel
    {
        public int Id { get; set; }

        public string ListingSummary { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        public DateTime CheckIn { get; set; }

        public DateTime CheckOut { get; set; }

        public string RequesterName { get; set; }

        public string RequesterPhone { get; set; }

        public string RequesterEmail { get; set; }

        public int RentalListingId { get; set; } 
    }
}
