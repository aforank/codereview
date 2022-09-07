namespace TicketBooking.Models
{
    public class BookingRequest
    {
        public string CustomerId { get; set; }

        public string SeatNo { get; set; }

        public string SeatType { get; set; }

        public decimal Amount { get; set; }

        public DateTime RequestDate { get; set; }
    }
}