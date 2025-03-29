using System.ComponentModel.DataAnnotations;

namespace RentalsProject.Models
{
    public class Reservation
    {
        public int Id { get; set; }


        [Required]
        public int RentalListingId { get; set; }


        [Required]
        public string UserId { get; set; }

 
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CheckIn { get; set; }


        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CheckOut { get; set; }


        public virtual RentalListing RentalListing { get; set; }


        public virtual IdentityUserProfile User { get; set; }
    }
}
