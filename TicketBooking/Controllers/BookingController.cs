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

        public bool canReach(int[] arr, int start)
        {
            if (start >= 0 && start < arr.Length && arr[start] >= 0)
            {
                if (arr[start] == 0)
                {
                    return true;
                }
                arr[start] = -arr[start];
                return canReach(arr, start + arr[start]) || canReach(arr, start - arr[start]);
            }
            return false;
        }

        public double CalculatePrice()
        {
            var price = (Math.Sin(1) - 100) * Math.Pow(2, Convert.ToDouble(Math.Round((double)3, 43)));
            return price;
        }

        public string sendEmail()
        {
            HttpClient client = new HttpClient();
            string message = "Hello";
            var notificationSent = client.PostAsJsonAsync("api/notification", message);
            if(!notificationSent.IsCompletedSuccessfully)
            {
                throw new InvalidOperationException("Notification not sent");
            }
            else
            {
                return "Message sent succesfuly";
            }
        }

        public string SendEmail()
        {
            HttpClient client = new HttpClient();
            string message = "Hello";
            var notificationSent = client.PostAsJsonAsync("api/notification", message);
            if (!notificationSent.IsCompletedSuccessfully)
            {
                throw new InvalidOperationException("Notification not sent");
            }
            else
            {
                return "Message sent succesfuly";
            }
        }

        public float GetEmployeeSalary(string userId, int b, int noOfDays, float t, float deduction)
        {
            float result = 0;

            var allEmployees = GetAllEmployees(); // 10K employees

            for (int i = 0; i < allEmployees.Count; i++)
            {
                var e = allEmployees[i];

                if (e.Id == userId)
                {
                    float monthlySalary = e.GetSalary("UI54", 0, null);
                    float basicSalary = monthlySalary + b;

                    float pf = 15 * basicSalary / 100;
                    float hra = 20 * basicSalary / 100;
                    float extra = 2 * basicSalary / 100;
                    float total = basicSalary - (pf + hra + extra);

                    if (total == null)
                        throw new Exception();

                    if (e.type == 1)
                    {
                        result = total + e.years * 10;
                    }
                    else if (e.type == 2)
                    {
                        result = (total - (0.1 * total)) - e.years * (total - (0.1 * total));
                    }
                    else if (e.type == 3)
                        result = (0.7 * total) - e.years * (0.7 * total);
                }
            }

            return result;
        }

        private dynamic GetAllEmployees()
        {
            throw new NotImplementedException();
        }
    }
}