using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Airline_Management_System__AMS_.Data;
using Airline_Management_System__AMS_.Models;
using Airline_Management_System__AMS_.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace Airline_Management_System__AMS_.Controllers
{
    [Authorize(Roles = "User")]
    public class UserDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserDashboardController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            // Fetch passenger record for the user
            var passenger = await _context.Passengers.FirstOrDefaultAsync(p => p.UserId == user.Id);

            int totalBookings = 0;
            int upcomingTrips = 0;
            int destinationsVisited = 0;

            if (passenger != null)
            {
                var bookings = await _context.Bookings
                    .Include(b => b.Flight)
                    .Where(b => b.PassengerId == passenger.Id)
                    .ToListAsync();

                totalBookings = bookings.Count;

                upcomingTrips = bookings
                    .Where(b => b.Flight != null)
                    .Count(b => b.Flight!.DepartureTime > DateTime.Now && b.Status != BookingStatus.Cancelled);

                destinationsVisited = bookings
                    .Where(b => b.Flight != null && b.Flight.ArrivalTime < DateTime.Now && b.Status != BookingStatus.Cancelled)
                    .Select(b => b.Flight!.Destination)
                    .Distinct()
                    .Count();
            }

            var model = new UserDashboardViewModel
            {
                TotalBookings = totalBookings,
                UpcomingTrips = upcomingTrips,
                DestinationsVisited = destinationsVisited
            };

            return View(model);
        }
    }
}
