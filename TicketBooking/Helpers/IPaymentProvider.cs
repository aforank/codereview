namespace TicketBooking.Helpers
{
    internal interface IPaymentProvider
    {
        bool ChargeCustomer(string customerId, decimal Amount);
    }
}