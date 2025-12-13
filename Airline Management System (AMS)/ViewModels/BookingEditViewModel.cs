using Airline_Management_System__AMS_.Models;
using System.ComponentModel.DataAnnotations;

namespace Airline_Management_System__AMS_.ViewModels
{
    public class BookingEditViewModel
    {
        public int BookingId { get; set; }
        public int FlightId { get; set; }
        public int PassengerId { get; set; }
        public string PassengerName { get; set; } = string.Empty;
        public string FlightNumber { get; set; } = string.Empty;

        public string CurrentSeat { get; set; } = string.Empty;

        public decimal TicketPrice { get; set; }

        public string Class { get; set; } = string.Empty;

        public int SelectedSeatId { get; set; }

        public BookingStatus Status { get; set; }

        public List<Seat> Seats { get; set; } = new List<Seat>();
        public HashSet<string> BookedSeats { get; set; } = new HashSet<string>();
    }

}
