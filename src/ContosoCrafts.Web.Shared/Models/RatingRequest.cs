namespace ContosoCrafts.Web.Shared.Models
{
    /// <summary>
    ///  Rating Request model.
    /// </summary>
    public class RatingRequest
    {
        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        public string ProductId { get; set; }
        /// <summary>
        /// Gets or sets the rating.
        /// </summary>
        /// <value>
        /// The rating.
        /// </value>
        public int Rating { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.IEquatable&lt;ContosoCrafts.Web.Shared.Models.CheckoutResponse&gt;" />
    public record CheckoutResponse(string CheckoutSessionID);
}
