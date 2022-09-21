namespace ContosoCrafts.Web.Server.Models
{
    /// <summary>
    /// Checkout Order Response model.
    /// </summary>
    public class CheckoutOrderResponse
    {
        /// <summary>
        /// Gets or sets the pub key.
        /// </summary>
        /// <value>
        /// The pub key.
        /// </value>
        public string PubKey { get; set; }
        /// <summary>
        /// Gets or sets the session identifier.
        /// </summary>
        /// <value>
        /// The session identifier.
        /// </value>
        public string SessionId { get; set; }
    }
}
