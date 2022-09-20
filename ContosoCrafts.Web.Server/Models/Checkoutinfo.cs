namespace ContosoCrafts.Web.Server.Models
{
    /// <summary>
    ///  Checkout Information model.
    /// </summary>
    public class CheckoutInfo
    {
        /// <summary>
        /// Gets or sets the amount total.
        /// </summary>
        /// <value>
        /// The amount total.
        /// </value>
        public long AmountTotal { get; set; }
        /// <summary>
        /// Gets or sets the customer email.
        /// </summary>
        /// <value>
        /// The customer email.
        /// </value>
        public string CustomerEmail { get; set; }
    }
}
