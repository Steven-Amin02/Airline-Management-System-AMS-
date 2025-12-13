using System.ComponentModel.DataAnnotations;

namespace Airline_Management_System__AMS_.Models
{
    public class Flight
    {
        [Key]
        public int FlightId { get; set; }

        [Required]
        [Display(Name = "Flight Number")]
        public string FlightNumber { get; set; } = string.Empty;

        [Required]
        public string Origin { get; set; } = string.Empty;

        [Required]
        public string Destination { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Departure Time")]
        public DateTime DepartureTime { get; set; }

        [Required]
        [Display(Name = "Arrival Time")]
        public DateTime ArrivalTime { get; set; }

        [Required]
        [Display(Name = "Aircraft Type")]
        public string AircraftType { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Available Seats")]
        public int AvailableSeats { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<Seat> Seats { get; set; } = new List<Seat>();

        public string FlightInfo => $"{FlightNumber} ({Origin} -> {Destination})";
    }
}
