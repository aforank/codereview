namespace TicketBooking.Helpers
{
    public interface ISeatLockProvider
    {
        public bool LockSeat(string seatNo);

        public void UnLockSeat(string seatNo);
    }
}