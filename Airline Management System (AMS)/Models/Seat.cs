using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Airline_Management_System__AMS_.Models
{
    public class Seat
    {
        [Key]
        public int SeatId { get; set; }

        [Required]
        [StringLength(5)]
        public string SeatNumber { get; set; } = string.Empty;

        public string Class { get; set; } = string.Empty;

        [ForeignKey("Flight")]
        public int FlightId { get; set; }
        public Flight Flight { get; set; } = null!;

        [ForeignKey("Booking")]
        public int? BookingId { get; set; } // Seat مرتبط بالـ Booking بعد الحجز
        public Booking? Booking { get; set; }

        public int SeatPrice { get; set; } = 100;

        public bool IsAvailable { get; set; } = true;
    }

}
