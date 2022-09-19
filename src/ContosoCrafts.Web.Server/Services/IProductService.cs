using System.Collections.Generic;
using System.Threading.Tasks;
using ContosoCrafts.Web.Shared.Models;

namespace ContosoCrafts.Web.Server.Services
{
    /// <summary>
    /// Product Service Interface.
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Adds the rating.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="rating">The rating.</param>
        /// <returns></returns>
        Task AddRating(string productId, int rating);
        /// <summary>
        /// Gets the products.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Product>> GetProducts();
        /// <summary>
        /// Gets the product.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<Product> GetProduct(string id);
        /// <summary>
        /// Checks the out.
        /// </summary>
        /// <param name="Items">The items.</param>
        /// <param name="callbackRoot">The callback root.</param>
        /// <returns></returns>
        Task<CheckoutResponse> CheckOut(IEnumerable<CartItem> Items, string callbackRoot);
    }
}
