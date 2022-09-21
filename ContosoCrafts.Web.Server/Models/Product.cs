namespace ContosoCrafts.Web.Server.Models
{
    /// <summary>
    /// Product model.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }
        /// <summary>
        /// Gets or sets the maker.
        /// </summary>
        /// <value>
        /// The maker.
        /// </value>
        public string Maker { get; set; }
        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        public string Image { get; set; }
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string Url { get; set; }
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the ratings.
        /// </summary>
        /// <value>
        /// The ratings.
        /// </value>
        public int[] Ratings { get; set; }
        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>
        /// The price.
        /// </value>
        public long Price { get; set; }
    }
}
