using System.Data.SqlClient;
using TicketBooking.Models;

namespace TicketBooking.Helpers
{
    internal class CustomerDB
    {
        public static Customer Get(string customerId)
        {
            SqlConnection connection = new SqlConnection();
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "Select * From CustomerId Where CustomerId=" + customerId;
            var response = command.ExecuteScalar();

            return (Customer)response;
        }
    }
}