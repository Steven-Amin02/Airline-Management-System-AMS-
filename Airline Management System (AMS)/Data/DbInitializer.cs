using Airline_Management_System__AMS_.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Airline_Management_System__AMS_.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Ensure database is migrated
            await context.Database.MigrateAsync();

            // Seed Roles
            string[] roles = { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Seed Admin User (if not exists)
            var adminEmail = "admin@ams.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "System",
                    LastName = "Admin",
                    EmailConfirmed = true,
                    Role = "Admin",
                    NationalId = "00000000000000",
                    PassportNumber = "ADMIN001",
                    PhoneNumber = "0000000000",
                    EmailConfirmationCode = "CONFIRMED"
                };
                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // Seed Flights (if none exist)
            if (!await context.Flights.AnyAsync())
            {
                var flights = new List<Flight>
                {
                    new Flight { FlightNumber = "AMS1001", Origin = "Cairo", Destination = "Dubai", DepartureTime = DateTime.Now.AddDays(7).AddHours(8), ArrivalTime = DateTime.Now.AddDays(7).AddHours(11), AircraftType = "Boeing 777", AvailableSeats = 60 },
                    new Flight { FlightNumber = "AMS1002", Origin = "Cairo", Destination = "London", DepartureTime = DateTime.Now.AddDays(10).AddHours(14), ArrivalTime = DateTime.Now.AddDays(10).AddHours(19), AircraftType = "Airbus A380", AvailableSeats = 60 },
                    new Flight { FlightNumber = "AMS1003", Origin = "Dubai", Destination = "New York", DepartureTime = DateTime.Now.AddDays(14).AddHours(22), ArrivalTime = DateTime.Now.AddDays(15).AddHours(6), AircraftType = "Boeing 787", AvailableSeats = 60 },
                    new Flight { FlightNumber = "AMS1004", Origin = "Paris", Destination = "Tokyo", DepartureTime = DateTime.Now.AddDays(21).AddHours(10), ArrivalTime = DateTime.Now.AddDays(21).AddHours(22), AircraftType = "Airbus A350", AvailableSeats = 60 },
                    new Flight { FlightNumber = "AMS1005", Origin = "Berlin", Destination = "Cairo", DepartureTime = DateTime.Now.AddDays(5).AddHours(6), ArrivalTime = DateTime.Now.AddDays(5).AddHours(10), AircraftType = "Boeing 737", AvailableSeats = 60 }
                };

                context.Flights.AddRange(flights);
                await context.SaveChangesAsync();

                // Seed Seats for each flight
                foreach (var flight in flights)
                {
                    var seats = new List<Seat>();
                    // Business Class: Rows 1-2 (12 seats)
                    for (int row = 1; row <= 2; row++)
                    {
                        foreach (char col in new[] { 'A', 'B', 'C', 'D', 'E', 'F' })
                        {
                            seats.Add(new Seat
                            {
                                SeatNumber = $"{row}{col}",
                                Class = "Business",
                                FlightId = flight.FlightId,
                                SeatPrice = 500,
                                IsAvailable = true
                            });
                        }
                    }
                    // Economy Class: Rows 3-10 (48 seats)
                    for (int row = 3; row <= 10; row++)
                    {
                        foreach (char col in new[] { 'A', 'B', 'C', 'D', 'E', 'F' })
                        {
                            seats.Add(new Seat
                            {
                                SeatNumber = $"{row}{col}",
                                Class = "Economy",
                                FlightId = flight.FlightId,
                                SeatPrice = 150,
                                IsAvailable = true
                            });
                        }
                    }
                    context.Seats.AddRange(seats);
                }
                await context.SaveChangesAsync();
            }
        }
    }
}
