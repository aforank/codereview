using Microsoft.AspNetCore.Mvc;
using TicketBooking.Helpers;
using TicketBooking.Models;

namespace TicketBooking.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly ILogger<BookingController> _logger;
        private readonly ISeatLockProvider seatLockProvider;

        public BookingController(ILogger<BookingController> logger, ISeatLockProvider seatLockProvider)
        {
            _logger = logger;
            this.seatLockProvider = seatLockProvider;
        }

        [HttpGet]
        public IActionResult Post(BookingRequest bookingRequest)
        {
            try
            {
                var customerData = CustomerDB.Get(bookingRequest.CustomerId);

                if (bookingRequest.RequestDate == null)
                    bookingRequest.RequestDate = DateTime.UtcNow;

                this.seatLockProvider.LockSeat(bookingRequest.SeatNo);

                IPaymentProvider payment= new CreditCardPaymentProvider();
                var paymentStatus = payment.ChargeCustomer(bookingRequest.CustomerId, bookingRequest.Amount);

                if (paymentStatus)
                {
                    Ticket ticketSaved = new Ticket();

                    if (ticketSaved == null)
                        throw new Exception("Ticket is null");

                    ticketSaved.TicketNo = DateTime.Now.Ticks.ToString();
                    ticketSaved.SeatNo = bookingRequest.SeatNo;
                    ticketSaved.CustomerId = bookingRequest.CustomerId;

                    //Send Confirmation Email
                    try
                    {
                        HttpClient client = new HttpClient();
                        var mailSent = client.PostAsJsonAsync("api/sendGrid?apiKey=yie33icn93ndos9233m", ticketSaved).Result;
                    }
                    catch
                    {
                        return null;
                    }
                }

                this.seatLockProvider.UnLockSeat(bookingRequest.SeatNo);
                return this.Ok("Ticket Booked!!!!!!");
            }
            catch(Exception ex)
            {
                return this.Ok("Sorry something went wrong for seatNo" + bookingRequest.SeatNo);
            }
        }
    }
}