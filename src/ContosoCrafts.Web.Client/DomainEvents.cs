namespace ContosoCrafts.Web.Client
{
    /// <summary>
    /// Domain Events class.
    /// </summary>
    public class ShoppingCartUpdated
    {
        /// <summary>
        /// Gets or sets the item count.
        /// </summary>
        /// <value>
        /// The item count.
        /// </value>
        public int ItemCount { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CheckoutStarted { }
}