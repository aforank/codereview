namespace ContosoPizza.Models
{
    /// <summary>
    /// Pizza class.
    /// </summary>
    public class Pizza
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is gluten free.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is gluten free; otherwise, <c>false</c>.
        /// </value>
        public bool IsGlutenFree { get; set; }
    }
}